using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace _2DPuzzle
{
    public class Entity
    {
        public string name = "";
        public bool isActive = true;
        public List<EntityComponent> components = null;

        public Entity parent = null;

        public List<Entity> children = null;

        public TransformComponent transformComponent = null;

        public uint uniqueID = 0;
        public bool isDirty = false;

        public bool differFromPrefab = false;

        public Entity(bool inInitializeNewEntity = false)
        {
            children = new List<Entity>();
            components = new List<EntityComponent>();

            if(inInitializeNewEntity)
            {
                InitializeNewEntity();
            }
        }

        public void InitializeNewEntity()
        {
            transformComponent = new TransformComponent(this)
            {
                uniqueID = EditorManager.GetInstance().GetNewUniqueID()
            };
            components.Add(transformComponent);
        }

        public virtual void Start()
        {
            for(int componentIndex = 0;  componentIndex < components.Count; componentIndex++)
            {
                components[componentIndex].Start();
            }
        }

        public T GetComponent<T>() where T : EntityComponent
        {
            if(components == null)
            {
                return null;
            }

            for (int componentIndex = 0; componentIndex < components.Count; componentIndex++)
            {
                if (components[componentIndex].GetType() == typeof(T))
                {
                    return (T)components[componentIndex];
                }
            }

            return null;
        }

        public EntitySave GetSaveData()
        {
            EntitySave entitySave = new EntitySave
            {
                name = name,
                componentsSaved = new List<ComponentSave>()
            };
            for (int componentIndex = 0; componentIndex < components.Count; componentIndex++)
            {
                ComponentSave componentSave = new ComponentSave();
                componentSave.componentType = components[componentIndex].GetType().ToString();
                componentSave.saveData = components[componentIndex].GetSavedData();
                componentSave.componentUniqueID = components[componentIndex].uniqueID.ToString();
                entitySave.componentsSaved.Add(componentSave);
                ComponentSave[] componentSaves = components[componentIndex].GetMoreComponentsToSave();
                if(componentSaves != null)
                {
                    for (int moreComponentIndex = 0; moreComponentIndex < componentSaves.Length; moreComponentIndex++)
                    {
                        entitySave.componentsSaved.Add(componentSaves[moreComponentIndex]);
                    }
                }
            }

            return entitySave;
        }

        public void AttachTo(Entity inEntity)
        {
            if(inEntity == null)
            {
                if(parent != null)
                {
                    parent.children.Remove(this);
                    parent = null;
                }
                return;
            }

            inEntity.children.Add(this);
            parent = inEntity;
        }

        public Vector2 ComputePosition()
        {
            if(parent == null)
            {
                return transformComponent.position;
            }
            else
            {
                return transformComponent.relativePosition + parent.ComputePosition();
            }
        }

        public Entity Clone()
        {
            Entity entity = new Entity()
            {
                name = name,
                uniqueID = EditorManager.GetInstance().GetNewUniqueID()
            };

            for (int componentIndex = 0; componentIndex < components.Count; componentIndex++)
            {
                Type type = components[componentIndex].GetType();
                object o = Activator.CreateInstance(type);
                if (type.IsSubclassOf(typeof(EntityComponent)))
                {
                    EntityComponent entityComponent = (EntityComponent)o;
                    entityComponent.uniqueID = EditorManager.GetInstance().GetNewUniqueID();
                    entityComponent.owner = entity;
                    components[componentIndex].CloneComponent(ref entityComponent);
                    entity.components.Add(entityComponent);
                }
                else if (type == typeof(AnimationState))
                {
                    AnimationState animationState = (AnimationState)o;
                    animationState.uniqueID = components[componentIndex].uniqueID;
                    AnimatorComponent animatorComponent = entity.GetComponent<AnimatorComponent>();
                    animationState.parentAnimatorComponent = animatorComponent;
                    animationState.parentStateMachine = animatorComponent;
                    animationState.LoadSavedData(components[componentIndex].GetSavedData());
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
                    stateMachineTransition.uniqueID = components[componentIndex].uniqueID;
                    AnimatorComponent animatorComponent = entity.GetComponent<AnimatorComponent>();
                    stateMachineTransition.parentStateMachine = animatorComponent;
                    stateMachineTransition.LoadSavedData(components[componentIndex].GetSavedData());
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
            entity.Start();

            return entity;
        }
    }
}
