using Microsoft.Xna.Framework;
using System;

namespace _2DPuzzle
{
    public class EntityComponent
    {
        public bool canUpdate = false;
        public bool canRender = false;

        public Entity owner = null;

        public bool enabled = true;

        protected TransformComponent _transformComponent = null;

        public Type type;

        public bool isDirty = false;

        public uint uniqueID = 0;

        public bool started = false;

        public EntityComponent()
        {
            type = GetType();
            UpdateManager.GetInstance().RegisterComponent(this);
        }

        public EntityComponent(Entity inOwner)
        {
            owner = inOwner;

            type = GetType();

            UpdateManager.GetInstance().RegisterComponent(this);
        }

        public virtual void Update(GameTime inGameTime)
        {
            if(!started)
            {
                Debug.LogError("Entity " + GetType() + " is not started but you are trying to update it");
            }
        }

        public virtual void Render(GameTime inGameTime)
        {
            if (!started)
            {
                Debug.LogError("Entity " + GetType() + " is not started but you are trying to render it");
            }
        }

        public virtual void Start()
        {
            _transformComponent = owner.GetComponent<TransformComponent>();
            started = true;
        }

        public void SetCanUpdate(bool inCanUpdate)
        {
            canUpdate = inCanUpdate;
            if(canUpdate)
            {
                UpdateManager.GetInstance().RegisterComponent(this);
            }
            else
            {
                UpdateManager.GetInstance().UnregisterComponent(this);
            }
        }

        public virtual string ComponentToString()
        {
            return "";
        }

        public virtual void EditorGUI()
        {

        }

        public virtual SavedData GetSavedData()
        {
            return new SavedData();
        }

        public virtual void LoadSavedData(SavedData inSavedData)
        {

        }

        public virtual ComponentSave[] GetMoreComponentsToSave()
        {
            return null;
        }
    }
}
