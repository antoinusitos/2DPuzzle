namespace _2DPuzzle
{
    public class ParallaxAsset : Entity
    {
        protected string assetNAme = "";

        public ParallaxAsset(string inAssetNAme, bool inInitializeNewEntity = true) : base(inInitializeNewEntity)
        {
            name = "Parallax-" + inAssetNAme;
            assetNAme = inAssetNAme;
            components.Add(new SpriteRenderComponent(this, assetNAme));
        }
    }
}
