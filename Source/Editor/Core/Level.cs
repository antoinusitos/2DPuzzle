using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;

namespace _2DPuzzle
{
    // Spawn and remove the Entities
    public class Level
    {
        public string name = "";
        public List<Entity> entities = null;

        public Level()
        {
            entities = new List<Entity>();
        }

        public virtual void InitializeLevel()
        {

        }

        public virtual void UninitializeLevel()
        {
            for (int entityIndex = 0; entityIndex < entities.Count; entityIndex++)
            {
                entities[entityIndex] = null;
            }

            entities.Clear();
        }

        public virtual void Start()
        {
            for (int entityIndex = 0; entityIndex < entities.Count; entityIndex++)
            {
                entities[entityIndex].Start();
            }
        }

        public void Save()
        {
            if(name == string.Empty)
            {
                Debug.LogWarning("Saving a level with an empty name");
            }
            LevelSave levelSave = new LevelSave()
            {
                name = name,
                entitiesCleanSaved = new List<EntityCleanSave>(),
                entitiesSaved = new List<LevelEntitySave>()
            };
            List<EntityCleanSave> notDifferedEntities = new List<EntityCleanSave>();
            for (int entityIndex = 0; entityIndex < entities.Count; entityIndex++)
            {
                if(entities[entityIndex].differFromPrefab)
                {
                    EntitySave entitySave = entities[entityIndex].GetSaveData();
                    LevelEntitySave levelEntitySave = new LevelEntitySave()
                    {
                        name = entitySave.name,
                        componentsSaved = entitySave.componentsSaved,
                        uniqueID = entities[entityIndex].uniqueID
                    };
                    levelSave.entitiesSaved.Add(levelEntitySave);
                }
                else
                {
                    levelSave.entitiesCleanSaved.Add(new EntityCleanSave() { name = entities[entityIndex].name, uniqueID = entities[entityIndex].uniqueID });
                }
            }
            string toSave = JsonConvert.SerializeObject(levelSave, Formatting.Indented);
            File.WriteAllText("Levels/" + name + ".json", toSave);
        }

        public void Load(string inLevelName)
        {
            string jsonString = File.ReadAllText("Levels/" + inLevelName + ".json");
            LevelSave levelSave = JsonConvert.DeserializeObject<LevelSave>(jsonString);
            name = levelSave.name;
            //LOADING PREFABS THAT ARE NOT MODIFIED
            for (int entityIndex = 0; entityIndex < levelSave.entitiesCleanSaved.Count; entityIndex++)
            {
                Entity entity = new Entity()
                {
                    uniqueID = levelSave.entitiesCleanSaved[entityIndex].uniqueID
                };
                SaveManager.GetInstance().LoadEntity(ref entity, levelSave.entitiesCleanSaved[entityIndex].name);
                entities.Add(entity);
            }
            //LOADING PREFABS THAT ARE MODIFIED OR NOT PREFAB
            for (int entityIndex = 0; entityIndex < levelSave.entitiesSaved.Count; entityIndex++)
            {
                Entity entity = new Entity()
                {
                    uniqueID = levelSave.entitiesSaved[entityIndex].uniqueID,
                    name = levelSave.entitiesSaved[entityIndex].name
                };
                List<ComponentSave> componentsSaved = levelSave.entitiesSaved[entityIndex].componentsSaved;
                for (int componentIndex = 0; componentIndex < componentsSaved.Count; componentIndex++)
                {
                    Type type = Type.GetType(componentsSaved[componentIndex].componentType);
                    object o = Activator.CreateInstance(type);
                    if (type.IsSubclassOf(typeof(EntityComponent)))
                    {
                        EntityComponent entityComponent = (EntityComponent)o;
                        entityComponent.uniqueID = uint.Parse(componentsSaved[componentIndex].componentUniqueID);
                        entityComponent.owner = entity;
                        entityComponent.LoadSavedData(componentsSaved[componentIndex].saveData);
                        entity.components.Add(entityComponent);
                    }
                    else if (type == typeof(AnimationState))
                    {
                        AnimationState animationState = (AnimationState)o;
                        animationState.uniqueID = uint.Parse(componentsSaved[componentIndex].componentUniqueID);
                        AnimatorComponent animatorComponent = entity.GetComponent<AnimatorComponent>();
                        animationState.parentAnimatorComponent = animatorComponent;
                        animationState.parentStateMachine = animatorComponent;
                        animationState.LoadSavedData(componentsSaved[componentIndex].saveData);
                        if (animatorComponent.currentState == null)
                        {
                            animatorComponent.SetStartingState(animationState);
                        }
                        animatorComponent.Start();
                        animatorComponent.allStates.Add(animationState);
                    }
                    else if (type == typeof(StateMachineTransition))
                    {
                        StateMachineTransition stateMachineTransition = (StateMachineTransition)o;
                        stateMachineTransition.uniqueID = uint.Parse(componentsSaved[componentIndex].componentUniqueID);
                        AnimatorComponent animatorComponent = entity.GetComponent<AnimatorComponent>();
                        stateMachineTransition.parentStateMachine = animatorComponent;
                        stateMachineTransition.LoadSavedData(componentsSaved[componentIndex].saveData);
                        animatorComponent.allTransitions.Add(stateMachineTransition);
                        for (int animationStateIndex = 0; animationStateIndex < animatorComponent.allStates.Count; animationStateIndex++)
                        {
                            if (animatorComponent.allStates[animationStateIndex] == stateMachineTransition.fromState)
                            {
                                animatorComponent.allStates[animationStateIndex].transitions.Add(stateMachineTransition);
                            }
                        }
                    }
                }
                entity.transformComponent = entity.GetComponent<TransformComponent>();

                entities.Add(entity);
            }
        }
    }
}
