using Newtonsoft.Json;
using System.Collections.Generic;

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

        public Entity()
        {
            children = new List<Entity>();
            components = new List<EntityComponent>();

            transformComponent = new TransformComponent(this);

            components.Add(transformComponent);

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

        public string Save()
        {
            string toReturn = "{\n" + name + "\n[";

            string json = "";
            for (int componentIndex = 0; componentIndex < components.Count; componentIndex++)
            {
                json += Newtonsoft.Json.JsonConvert.SerializeObject(components[componentIndex]);
            }

            toReturn += "\n}\n]";
            return toReturn;
        }
    }
}
