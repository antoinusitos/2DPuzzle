namespace _2DPuzzle
{
    public class Floor : Entity
    {
        public Floor()
        {
            name = "Floor";

            SpriteRenderComponent spriteRenderComponent = new SpriteRenderComponent(this, "TileTest");
            components.Add(spriteRenderComponent);
        }
    }
}
