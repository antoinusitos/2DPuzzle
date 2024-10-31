
namespace _2DPuzzle
{
    public class LevelTest : Level
    {
        public override void InitializeLevel()
        {
            base.InitializeLevel();

            Player player = new Player();
            entities.Add(player);
            player.GetComponent<TransformComponent>().position.Y = 100;
        }
    }
}
