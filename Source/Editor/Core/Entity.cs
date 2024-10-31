using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class Entity
    {
        public List<EntityComponent> components = null;

        public bool isActive = true;

        public Entity parent = null;

        public List<Entity> children = null;

        public string name = "";

        public Entity()
        {
            children = new List<Entity>();
            components = new List<EntityComponent>();

            TransformComponent transformComponent = new TransformComponent(this);

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
    }
}
