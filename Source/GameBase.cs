using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.ImGuiNet;
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

        protected bool _isResizing = false;

        public static ImGuiRenderer GuiRenderer;
        bool _toolActive;
        System.Numerics.Vector4 _colorV4;

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

            GuiRenderer = new ImGuiRenderer(this);
            _toolActive = true;
            Vector4 vec = Color.CornflowerBlue.ToVector4();
            _colorV4 = new System.Numerics.Vector4(vec.X, vec.Y, vec.Z, vec.W);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _contentManager.LoadContent();

            GuiRenderer.RebuildFontAtlas();
        }

        protected override void Update(GameTime inGameTime)
        {
            if(!_updateManager.Update(inGameTime))
            {
                Exit();
            }

            _collisionManager.ProcessPhysics();

            base.Update(inGameTime);
        }

        protected override void Draw(GameTime inGameTime)
        {
            GraphicsDevice.Clear(new Color(_colorV4.X, _colorV4.Y, _colorV4.Z, _colorV4.W));

            _renderManager.totalBatch = 0;
            _renderManager.Render(inGameTime);

            base.Draw(inGameTime);

            GuiRenderer.BeginLayout(inGameTime);
            if (_toolActive)
            {
                ImGui.Begin("My First Tool", ref _toolActive, ImGuiWindowFlags.MenuBar);
                if (ImGui.BeginMenuBar())
                {
                    if (ImGui.BeginMenu("File"))
                    {
                        if (ImGui.MenuItem("Open..", "Ctrl+O")) { /* Do stuff */ }
                        if (ImGui.MenuItem("Save", "Ctrl+S")) { /* Do stuff */ }
                        if (ImGui.MenuItem("Close", "Ctrl+W")) { _toolActive = false; }
                        ImGui.EndMenu();
                    }
                    ImGui.EndMenuBar();
                }

                // Edit a color stored as 4 floats
                ImGui.ColorEdit4("Color", ref _colorV4);

                // Generate samples and plot them
                var samples = new float[100];
                for (var n = 0; n < samples.Length; n++)
                    samples[n] = (float)Math.Sin(n * 0.2f + ImGui.GetTime() * 1.5f);
                ImGui.PlotLines("Samples", ref samples[0], 100);

                // Display contents in a scrolling region
                ImGui.TextColored(new System.Numerics.Vector4(1, 1, 0, 1), "Important Stuff");
                ImGui.BeginChild("Scrolling", new System.Numerics.Vector2(0));
                for (var n = 0; n < 50; n++)
                    ImGui.Text($"{n:0000}: Some text");
                ImGui.EndChild();
                ImGui.End();
            }
            GuiRenderer.EndLayout();
        }
    }
}
