namespace _2DPuzzle
{
    public class ParallaxAsset : Entity
    {
        public ParallaxAsset(string inAssetNAme)
        {
            name = "Parallax-" + inAssetNAme;

            components.Add(new SpriteRenderComponent(this, inAssetNAme));
        }
    }
}
