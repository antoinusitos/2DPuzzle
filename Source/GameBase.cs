using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _2DPuzzle
{
    public class GameBase : Game
    {
        //ENGINE
        protected GraphicsDeviceManager _graphics = null;

        //GAME
        protected UpdateManager _updateManager = null;
        protected RenderManager _renderManager = null;
        protected ContentManager _contentManager = null;
        protected LevelManager _levelManager = null;
        protected CollisionManager _collisionManager = null;
        protected WorldManager _worldManager = null;
        protected SoundManager _soundManager = null;
        protected SaveManager _saveManager = null;
        protected EditorManager _editorManager = null;
        protected InputManager _inputManager = null;

        protected bool _isResizing = false;

        protected bool started = false;

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
            _collisionManager = CollisionManager.GetInstance();
            _worldManager = WorldManager.GetInstance();
            _soundManager = SoundManager.GetInstance();
            _soundManager.InitializeManager();
            _saveManager = SaveManager.GetInstance();
            _saveManager.InitializeManager();
            _editorManager = EditorManager.GetInstance();
            _inputManager = InputManager.GetInstance();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _contentManager.LoadContent();
            _editorManager.InitializeManager(this);
        }

        protected override void Update(GameTime inGameTime)
        {
            if (!EditorManager.GetInstance().isPlaying)
            {
                return;
            }

            if (!_updateManager.Update(inGameTime))
            {
                Exit();
            }

            _collisionManager.ProcessPhysics();

            base.Update(inGameTime);
        }

        protected override void Draw(GameTime inGameTime)
        {
            _inputManager.Update();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _renderManager.totalBatch = 0;
            _renderManager.Render(inGameTime);

            base.Draw(inGameTime);

            EditorManager.GetInstance().RenderWindows(inGameTime);
        }
    }
}
