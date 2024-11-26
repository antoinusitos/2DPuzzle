using System.Collections.Generic;

namespace _2DPuzzle
{
    public class AnimationState : StateMachineState
    {
        public SpriteAnimatorRender spriteAnimatorRender = null;

        public string spritePath = "";
        public int frameNumber = 0;
        public bool loop = false;

        public AnimatorComponent parentAnimatorComponent = null;

        public void SetAnimation(string inSpritePath, int inFrameNumber, bool inLoop)
        {
            spritePath = inSpritePath;
            frameNumber = inFrameNumber;
            loop = inLoop;
            LoadSpriteAnimatorRender();
        }

        private void LoadSpriteAnimatorRender()
        {
            spriteAnimatorRender = new SpriteAnimatorRender(spritePath, frameNumber, loop);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            spriteAnimatorRender.UpdateAnimator();
        }

        public override SavedData GetSavedData()
        {
            SavedData savedData = new SavedData
            {
                savedString = new Dictionary<string, string>
                {
                    { "Editor." + parentAnimatorComponent.owner.name + ".SpritePath", spritePath },
                },
                savedBool = new Dictionary<string, bool>
                {
                    { "Editor." + parentAnimatorComponent.owner.name + ".Loop", loop },
                },
                savedInt = new Dictionary<string, int>
                {
                    { "Editor." + parentAnimatorComponent.owner.name + ".FrameNumber", frameNumber },
                }
            };
            return savedData;
        }

        public override void LoadSavedData(SavedData inSavedData)
        {
            if (inSavedData.savedString.ContainsKey("Editor." + parentAnimatorComponent.owner.name + ".SpritePath"))
            {
                spritePath = inSavedData.savedString["Editor." + parentAnimatorComponent.owner.name + ".SpritePath"];
            }

            if (inSavedData.savedBool.ContainsKey("Editor." + parentAnimatorComponent.owner.name + ".Loop"))
            {
                loop = inSavedData.savedBool["Editor." + parentAnimatorComponent.owner.name + ".Loop"];
            }

            if (inSavedData.savedInt.ContainsKey("Editor." + parentAnimatorComponent.owner.name + ".FrameNumber"))
            {
                frameNumber = inSavedData.savedInt["Editor." + parentAnimatorComponent.owner.name + ".FrameNumber"];
            }

            LoadSpriteAnimatorRender();
        }
    }
}
