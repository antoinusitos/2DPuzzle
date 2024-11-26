using Microsoft.Xna.Framework;

namespace _2DPuzzle
{
    public class LevelTest : Level
    {
        public LevelTest()
        {
            name = "LevelTest";
        }

        public override void InitializeLevel()
        {
            base.InitializeLevel();

            Player player = new Player();
            player.InitializeNewEntity();
            entities.Add(player);
            player.GetComponent<TransformComponent>().position = new Vector2(100, 100);

            ParallaxAsset parallaxAssetFar = new ParallaxAsset("Far");
            parallaxAssetFar.GetComponent<SpriteRenderComponent>().SwitchLayer(-3);
            entities.Add(parallaxAssetFar);

            ParallaxAsset parallaxAssetMid = new ParallaxAsset("Mid");
            parallaxAssetMid.GetComponent<SpriteRenderComponent>().SwitchLayer(-2);
            entities.Add(parallaxAssetMid);

            ParallaxAsset parallaxAssetClose = new ParallaxAsset("Close");
            parallaxAssetClose.GetComponent<SpriteRenderComponent>().SwitchLayer(-1);
            entities.Add(parallaxAssetClose);

            Floor floor = new Floor();
            entities.Add(floor);
            floor.GetComponent<TransformComponent>().position = new Vector2(75 , 300);

            DebugRectangle debugRectangle = new DebugRectangle(new Vector2(67, 288), new Vector2(65, 34));
            entities.Add(debugRectangle);

            SoundManager.GetInstance().PlaySound("");

            //SaveManager.GetInstance().LoadAll();

            /*SaveManager.GetInstance().SaveBool("test bool", false);
            SaveManager.GetInstance().SaveBool("testBool2", true);
            SaveManager.GetInstance().SaveInt("testInt", 9);
            SaveManager.GetInstance().SaveFloat("testFloat", 3.33f);
            SaveManager.GetInstance().SaveAll();*/
        }
    }
}
