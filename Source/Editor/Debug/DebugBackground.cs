namespace _2DPuzzle
{
    public class DebugBackground : Entity
    {
        public DebugBackground()
        {
            name = "DebugBackground";

            components.Add(new DebugBackgroundComponent(this));
        }
    }
}
