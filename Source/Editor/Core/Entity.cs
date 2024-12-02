﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
    }
}
