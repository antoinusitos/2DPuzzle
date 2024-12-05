namespace _2DPuzzle
{
    public class DebugMousePosition : Entity
    {
        public DebugMousePosition(bool inInitializeNewEntity = true) : base(inInitializeNewEntity)
        {
            name = "DebugMousePosition";

            components.Add(new DebugMousePositionComponent(this, "Roboto"));
        }
    }
}
