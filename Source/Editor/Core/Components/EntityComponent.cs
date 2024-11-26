using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace _2DPuzzle
{
    public class EntityComponent
    {
        public bool canUpdate = false;
        public bool canRender = false;

        [JsonIgnore]
        public Entity owner = null;

        public bool enabled = true;

        [JsonIgnore]
        protected TransformComponent _transformComponent = null;

        public Type type;

        public bool isDirty = false;

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

        }

        public virtual void Render(GameTime inGameTime)
        {

        }

        public virtual void Start()
        {
            _transformComponent = owner.GetComponent<TransformComponent>();
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

        public virtual SavedData GetSavedData()
        {
            return new SavedData();
        }

        public virtual void LoadSavedData(SavedData inSavedData)
        {

        }
    }
}
