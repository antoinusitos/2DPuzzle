namespace _2DPuzzle
{
    public class AnimationState : StateMachineState
    {
        public SpriteAnimatorRender spriteAnimatorRender = null;

        public void SetAnimation(string inSpritePath, int inFrameNumber, bool inLoop)
        {
            spriteAnimatorRender = new SpriteAnimatorRender(inSpritePath, inFrameNumber, inLoop);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            spriteAnimatorRender.UpdateAnimator();
        }
    }
}
