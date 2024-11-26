using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Principal;

namespace _2DPuzzle
{
    public class Entity
    {
        public string name = "";
        public bool isActive = true;
        public List<EntityComponent> components = null;

        [JsonIgnore]
        public Entity parent = null;

        public List<Entity> children = null;

        [JsonIgnore]
        public TransformComponent transformComponent = null;

        public uint entityID = 0;
        [JsonIgnore]
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
            transformComponent = new TransformComponent(this);
            components.Add(transformComponent);

            entityID = EditorManager.GetInstance().GetNewEntityID();
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
                entitySave.componentsSaved.Add(componentSave);
            }

            return entitySave;
        }
    }
}
