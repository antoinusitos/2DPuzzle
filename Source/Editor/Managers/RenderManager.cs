using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class RenderManager
    {
        private static RenderManager _instance;

        private static readonly object _lock = new object();

        public static RenderManager GetInstance()
        {
            // This conditional is needed to prevent threads stumbling over the
            // lock once the instance is ready.
            if (_instance == null)
            {
                // Now, imagine that the program has just been launched. Since
                // there's no Singleton instance yet, multiple threads can
                // simultaneously pass the previous conditional and reach this
                // point almost at the same time. The first of them will acquire
                // lock and will proceed further, while the rest will wait here.
                lock (_lock)
                {
                    // The first thread to acquire the lock, reaches this
                    // conditional, goes inside and creates the Singleton
                    // instance. Once it leaves the lock block, a thread that
                    // might have been waiting for the lock release may then
                    // enter this section. But since the Singleton field is
                    // already initialized, the thread won't create a new
                    // object.
                    if (_instance == null)
                    {
                        _instance = new RenderManager();
                    }
                }
            }
            return _instance;
        }

        public SpriteBatch spriteBatch = null;

        public Microsoft.Xna.Framework.Content.ContentManager content = null;

        public int totalBatch = 0;

        private Dictionary<int, List<RenderComponent>> LayerToRenderComponents = null;

        private const int minLayer = -3;
        private const int maxLayer = 3;

        public void InitializeManager(GraphicsDevice inGraphicsDevice, Microsoft.Xna.Framework.Content.ContentManager inContent)
        {
            spriteBatch = new SpriteBatch(inGraphicsDevice);
            content = inContent;

            LayerToRenderComponents = new Dictionary<int, List<RenderComponent>>
            {
                { -3, new List<RenderComponent>() },
                { -2, new List<RenderComponent>() },
                { -1, new List<RenderComponent>() },
                { 0, new List<RenderComponent>() },
                { 1, new List<RenderComponent>() },
                { 2, new List<RenderComponent>() },
                { 3, new List<RenderComponent>() }
            };
        }

        public void Render(GameTime inGameTime)
        {
            totalBatch = 0;

            for(int currentLayer = minLayer; currentLayer <= maxLayer; currentLayer++)
            {
                for (int currentRenderer = 0; currentRenderer < LayerToRenderComponents[currentLayer].Count; currentRenderer++)
                {
                    LayerToRenderComponents[currentLayer][currentRenderer].Render(inGameTime);
                }
            }

            //Debug.Log("Batch :" + totalBatch);
        }

        public void RegisterRenderer(RenderComponent inRenderComponent)
        {
            LayerToRenderComponents[0].Add(inRenderComponent);
        }

        public void SwitchLayer(int oldLayer, int inLayer, RenderComponent inRenderComponent)
        {
            LayerToRenderComponents[oldLayer].Remove(inRenderComponent);
            LayerToRenderComponents[inLayer].Add(inRenderComponent);
        }
    }
}
