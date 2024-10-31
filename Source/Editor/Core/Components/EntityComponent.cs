using Microsoft.Xna.Framework;

namespace _2DPuzzle
{
    public class EntityComponent
    {
        public bool canUpdate = false;
        public bool canRender = false;

        public Entity owner = null;

        public bool enabled = true;

        protected TransformComponent _transformComponent = null;

        public EntityComponent(Entity inOwner)
        {
            owner = inOwner;

            _transformComponent = owner.GetComponent<TransformComponent>();

            UpdateManager.GetInstance().RegisterComponent(this);
        }

        public virtual void Update(GameTime inGameTime)
        {

        }

        public virtual void Render(GameTime inGameTime)
        {

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
    }
}
