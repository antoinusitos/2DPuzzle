using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGuiNet;
using System;

namespace _2DPuzzle
{
    public  class EditorManager
    {
        private static EditorManager _instance;

        private static readonly object _lock = new object();

        public static EditorManager GetInstance()
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
                        _instance = new EditorManager();
                    }
                }
            }
            return _instance;
        }

        private GameBase gameBase = null;

        private ImGuiRenderer guiRenderer;

        private bool consoleActive = false;

        private bool openLevelActive = false;

        public bool isPlaying = false;

        private Entity inspectedEntity = null;

        private DebugMousePosition debugMousePosition = null;

        public void InitializeManager(GameBase inGameBase)
        {
            gameBase = inGameBase;

            guiRenderer = new ImGuiRenderer(gameBase);
            guiRenderer.RebuildFontAtlas();

            debugMousePosition = new DebugMousePosition();
            debugMousePosition.transformComponent.position = new Vector2(0, RenderManager.GetInstance().GetScreenHeight() - 20);
        }

        public void RenderWindows(GameTime inGameTime)
        {
            guiRenderer.BeginLayout(inGameTime);

            RenderHeader();

            RenderHierarchy();

            RenderInspector();

            RenderConsole();

            RenderOpenMenu();

            guiRenderer.EndLayout();

            debugMousePosition.components[1].Update(inGameTime);
        }

        private void RenderHeader()
        {
            ImGui.SetNextWindowPos(System.Numerics.Vector2.Zero);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(RenderManager.GetInstance().GetScreenWidth(), 0));
            ImGui.Begin(" ", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground);
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Open", "Ctrl+O")) { openLevelActive = true; }
                    if (ImGui.MenuItem("Save", "Ctrl+S")) { LevelManager.GetInstance().SaveLevel(); }
                    if (ImGui.MenuItem("Close", "Ctrl+W")) { gameBase.Exit(); }
                    ImGui.EndMenu();
                }
                if (ImGui.MenuItem("Console"))
                {
                    consoleActive = true;
                    Debug.Log("Opening Console");
                    Debug.LogWarning("Opening Console");
                    Debug.LogError("Opening Console");
                }
                if(!isPlaying)
                {
                    if (ImGui.MenuItem("Play"))
                    {
                        isPlaying = true;
                    }
                }
                else
                {
                    if (ImGui.MenuItem("Stop"))
                    {
                        isPlaying = false;
                    }
                }
                ImGui.EndMenuBar();
            }
            ImGui.End();
        }

        private void RenderHierarchy()
        {
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, 20));
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, RenderManager.GetInstance().GetScreenHeight() - 40));
            ImGui.Begin("Hierarchy", ImGuiWindowFlags.NoMove);
            ImGui.BeginChild("Scrolling", new System.Numerics.Vector2(0), ImGuiChildFlags.None);
            if (LevelManager.GetInstance().currentLevel != null)
            {
                for (int textIndex = 0; textIndex < LevelManager.GetInstance().currentLevel.entities.Count; textIndex++)
                {
                    string current = "";
                    if(inspectedEntity == LevelManager.GetInstance().currentLevel.entities[textIndex])
                    {
                        current = "         (Inspected)";
                    }
                    if (ImGui.MenuItem(LevelManager.GetInstance().currentLevel.entities[textIndex].name + current))
                    {
                        inspectedEntity = LevelManager.GetInstance().currentLevel.entities[textIndex];
                        Debug.Log("Clicked on " + LevelManager.GetInstance().currentLevel.entities[textIndex].name);
                    }
                }
            }
            ImGui.EndChild();
            ImGui.End();
        }

        private void RenderInspector()
        {
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(RenderManager.GetInstance().GetScreenWidth() - 300, 20));
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, RenderManager.GetInstance().GetScreenHeight() - 40));
            ImGui.Begin("Inspector", ImGuiWindowFlags.NoMove);
            if(inspectedEntity != null)
            {
                ImGui.Text(inspectedEntity.name);
                for(int componentIndex = 0; componentIndex < inspectedEntity.components.Count; componentIndex++)
                {
                    string componentName = inspectedEntity.components[componentIndex].GetType().ToString();
                    componentName = componentName.Remove(0, 10);
                    ImGui.Text("");
                    ImGui.BulletText(componentName);
                    ImGui.Text(inspectedEntity.components[componentIndex].ComponentToString());
                }
            }
            ImGui.End();
        }

        private void RenderConsole()
        {
            if(!consoleActive)
            {
                return;
            }

            ImGui.Begin("Console", ref consoleActive, ImGuiWindowFlags.None);
            ImGui.BeginChild("Scrolling", new System.Numerics.Vector2(0), ImGuiChildFlags.None);
            for (int textIndex = 0; textIndex < Debug.allDebug.Count; textIndex++)
            {
                System.Numerics.Vector4 color = new System.Numerics.Vector4(0, 0, 0, 1);
                if(Debug.allDebug[textIndex].severity == 2)
                {
                    color.X = 255;
                }
                else if (Debug.allDebug[textIndex].severity == 1)
                {
                    color.X = 255;
                    color.Y = 255;
                }
                else
                {
                    color = System.Numerics.Vector4.One;
                }
                ImGui.TextColored(color, Debug.allDebug[textIndex].text);
            }
            ImGui.EndChild();
            ImGui.End();
        }

        private void RenderOpenMenu()
        {
            if (!openLevelActive)
            {
                return;
            }

            ImGui.Begin("Open Level", ref openLevelActive, ImGuiWindowFlags.None);
            if (ImGui.MenuItem("Level 1")) { openLevelActive = false; LevelManager.GetInstance().AddLevel(new LevelTest()); }
            ImGui.End();
        }
    }
}
