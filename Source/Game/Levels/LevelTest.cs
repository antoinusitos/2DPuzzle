using Microsoft.Xna.Framework;

namespace _2DPuzzle
{
    public class LevelTest : Level
    {
        public override void InitializeLevel()
        {
            base.InitializeLevel();

            Player player = new Player();
            entities.Add(player);
            player.GetComponent<TransformComponent>().position = new Vector2(100, 250);

            ParallaxAsset parallaxAssetFar = new ParallaxAsset("Far");
            parallaxAssetFar.GetComponent<SpriteRenderComponent>().SwitchLayer(-3);
            entities.Add(parallaxAssetFar);

            ParallaxAsset parallaxAssetMid = new ParallaxAsset("Mid");
            parallaxAssetMid.GetComponent<SpriteRenderComponent>().SwitchLayer(-2);
            entities.Add(parallaxAssetMid);

            ParallaxAsset parallaxAssetClose = new ParallaxAsset("Close");
            parallaxAssetClose.GetComponent<SpriteRenderComponent>().SwitchLayer(-1);
            entities.Add(parallaxAssetClose);

            DebugMousePosition debugMousePosition = new DebugMousePosition();
            debugMousePosition.transformComponent.position = new Vector2(RenderManager.GetInstance().GetScreenWidth() - 150, 10);

            Floor floor = new Floor();
            entities.Add(floor);
            floor.GetComponent<TransformComponent>().position = new Vector2(0, 300);

            /*Floor floor = new Floor();
            entities.Add(floor);
            floor.GetComponent<TransformComponent>().position.Y = 100;

            DebugBackground debugBackground = new DebugBackground();
            entities.Add(debugBackground);*/
        }
    }
}
