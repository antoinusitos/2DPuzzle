using System.Collections.Generic;

namespace _2DPuzzle
{
    public class AnimationState : StateMachineState
    {
        public SpriteAnimatorRenderComponent spriteAnimatorRenderComponent = null;

        public string spritePath = "";
        public int frameNumber = 0;
        public bool loop = false;

        public AnimatorComponent parentAnimatorComponent = null;

        public override void Start()
        {
            spriteAnimatorRenderComponent.Start();
        }

        public void SetAnimation(string inSpritePath, int inFrameNumber, bool inLoop, bool inMustRegister = true)
        {
            spritePath = inSpritePath;
            frameNumber = inFrameNumber;
            loop = inLoop;
            LoadSpriteAnimatorRender(inMustRegister);
        }

        private void LoadSpriteAnimatorRender(bool inMustRegister)
        {
            spriteAnimatorRenderComponent = new SpriteAnimatorRenderComponent(parentAnimatorComponent.owner, spritePath, frameNumber, loop, inMustRegister);
        }

        public override SavedData GetSavedData()
        {
            SavedData savedData = new SavedData
            {
                savedString = new Dictionary<string, string>
                {
                    { "Editor." + parentAnimatorComponent.owner.name + "." + uniqueID + ".SpritePath", spritePath },
                    { "Editor." + parentAnimatorComponent.owner.name + "." + uniqueID + ".AnimationStateName", animationStateName },
                },
                savedBool = new Dictionary<string, bool>
                {
                    { "Editor." + parentAnimatorComponent.owner.name + "." + uniqueID +  ".Loop", loop },
                },
                savedInt = new Dictionary<string, int>
                {
                    { "Editor." + parentAnimatorComponent.owner.name + "." + uniqueID +  ".FrameNumber", frameNumber },
                }
            };
            return savedData;
        }

        public override void LoadSavedData(SavedData inSavedData)
        {
            if (inSavedData.savedString.ContainsKey("Editor." + parentAnimatorComponent.owner.name + "." + uniqueID + ".SpritePath"))
            {
                spritePath = inSavedData.savedString["Editor." + parentAnimatorComponent.owner.name + "." + uniqueID + ".SpritePath"];
            }
            if (inSavedData.savedString.ContainsKey("Editor." + parentAnimatorComponent.owner.name + "." + uniqueID + ".AnimationStateName"))
            {
                animationStateName = inSavedData.savedString["Editor." + parentAnimatorComponent.owner.name + "." + uniqueID + ".AnimationStateName"];
            }

            if (inSavedData.savedBool.ContainsKey("Editor." + parentAnimatorComponent.owner.name + "." + uniqueID + ".Loop"))
            {
                loop = inSavedData.savedBool["Editor." + parentAnimatorComponent.owner.name + "." + uniqueID + ".Loop"];
            }

            if (inSavedData.savedInt.ContainsKey("Editor." + parentAnimatorComponent.owner.name + "." + uniqueID + ".FrameNumber"))
            {
                frameNumber = inSavedData.savedInt["Editor." + parentAnimatorComponent.owner.name + "." + uniqueID + ".FrameNumber"];
            }

            if(parentAnimatorComponent.startingStateID == uniqueID)
            {
                parentAnimatorComponent.SetStartingState(this);
            }

            LoadSpriteAnimatorRender(false);
        }
    }
}
