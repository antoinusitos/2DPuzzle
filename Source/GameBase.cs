using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _2DPuzzle
{
    public class GameBase : Microsoft.Xna.Framework.Game
    {
        //ENGINE
        protected GraphicsDeviceManager _graphics = null;

        //GAME
        protected UpdateManager _updateManager = null;
        protected RenderManager _renderManager = null;
        protected ContentManager _contentManager = null;
        protected LevelManager _levelManager = null;

        protected bool _isResizing = false;

        public GameBase()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnClientSizeChanged;
        }

        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            if(!_isResizing && Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0)
            {
                _isResizing = true;
                _renderManager.UpdateScreenScaleMatrix();
                _isResizing = false;
            }
        }

        protected override void Initialize()
        {
            _updateManager = UpdateManager.GetInstance();
            _renderManager = RenderManager.GetInstance();
            _renderManager.InitializeManager(GraphicsDevice, Content, _graphics);
            _contentManager = ContentManager.GetInstance();
            _levelManager = LevelManager.GetInstance();
            _levelManager.InitializeManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _contentManager.LoadContent();
        }

        protected override void Update(GameTime inGameTime)
        {
            if(!_updateManager.Update(inGameTime))
            {
                Exit();
            }

            base.Update(inGameTime);
        }

        protected override void Draw(GameTime inGameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _renderManager.totalBatch = 0;
            _renderManager.Render(inGameTime);

            base.Draw(inGameTime);
        }
    }
}
