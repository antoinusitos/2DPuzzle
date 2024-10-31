namespace _2DPuzzle
{
    public class Player : Entity
    {
        public Player()
        {
            name = "Player";

            components.Add(new SpriteRenderComponent(this, "Idle"));
            components.Add(new PlayerMovementComponent(this));
        }
    }
}
