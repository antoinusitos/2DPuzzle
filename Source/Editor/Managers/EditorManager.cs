using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGuiNet;

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

        private uint currentUniqueID = 1;

        private AnimatorComponent inspectedAnimatorComponent = null;
        private bool animatorToolActive = false;

        public void InitializeManager(GameBase inGameBase)
        {
            gameBase = inGameBase;

            guiRenderer = new ImGuiRenderer(gameBase);
            guiRenderer.RebuildFontAtlas();

            debugMousePosition = new DebugMousePosition();
            debugMousePosition.transformComponent.position = new Vector2(0, RenderManager.GetInstance().GetScreenHeight() - 20);

            LevelManager.GetInstance().AddLevel(new LevelTest());
        }

        public uint GetNewUniqueID()
        {
            uint toReturn = currentUniqueID;
            currentUniqueID++;
            return toReturn;
        }

        public void RenderWindows(GameTime inGameTime)
        {
            guiRenderer.BeginLayout(inGameTime);

            RenderHeader();

            RenderHierarchy();

            RenderInspector();

            RenderConsole();

            RenderOpenMenu();

            RenderAnimatorTool();

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
            ImGui.Separator();
            if (ImGui.MenuItem("Create Entity", "ctrl+N"))
            {
                Entity newEntity = new Entity()
                {
                    name = "NewEntity",
                    isDirty = true,
                };
                newEntity.uniqueID = EditorManager.GetInstance().GetNewUniqueID();
                newEntity.InitializeNewEntity();
                LevelManager.GetInstance().currentLevel.entities.Add(newEntity);
                inspectedEntity = newEntity;
            }
            ImGui.Separator();
            ImGui.BeginChild("Scrolling", new System.Numerics.Vector2(0), ImGuiChildFlags.None);
            if (LevelManager.GetInstance().currentLevel != null)
            {
                for (int textIndex = 0; textIndex < LevelManager.GetInstance().currentLevel.entities.Count; textIndex++)
                {
                    string current = "";
                    bool selected = false;
                    if(inspectedEntity == LevelManager.GetInstance().currentLevel.entities[textIndex])
                    {
                        current = "         (Inspected)";
                        selected = true;
                    }
                    if (ImGui.MenuItem(LevelManager.GetInstance().currentLevel.entities[textIndex].name + current, "", selected))
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
            ImGui.Begin("Inspector", ImGuiWindowFlags.None);
            if(inspectedEntity != null)
            {
                if (inspectedEntity.isDirty && ImGui.MenuItem("Save Entity"))
                {
                    SaveManager.GetInstance().SaveEntity(inspectedEntity);
                }
                else if(!inspectedEntity.isDirty)
                {
                    ImGui.Text("Entity is Saved");
                }
                if (ImGui.MenuItem("FORCE Save Entity"))
                {
                    SaveManager.GetInstance().SaveEntity(inspectedEntity);
                }
                ImGui.Separator();
                ImGui.InputText("name", ref inspectedEntity.name, 32);
                ImGui.Text("uniqueID:" + inspectedEntity.uniqueID.ToString());
                for (int componentIndex = 0; componentIndex < inspectedEntity.components.Count; componentIndex++)
                {
                    ImGui.Separator();
                    string componentName = inspectedEntity.components[componentIndex].GetType().ToString();
                    componentName = componentName.Remove(0, 10);
                    ImGui.BulletText(componentName);
                    ImGui.Text(inspectedEntity.components[componentIndex].ComponentToString());
                    if (inspectedEntity.components[componentIndex].GetType() == typeof(AnimatorComponent))
                    {
                        if (ImGui.MenuItem("Animator Tool", "ctrl+A"))
                        {
                            inspectedAnimatorComponent = (AnimatorComponent)inspectedEntity.components[componentIndex];
                            animatorToolActive = true;
                        }
                    }
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
            Level[] levels = LevelManager.GetInstance().GetLevelsArray();
            for (int levelIndex = 0; levelIndex < levels.Length; levelIndex++)
            {
                if (ImGui.MenuItem(levels[levelIndex].name)) 
                {
                    inspectedEntity = null;
                    openLevelActive = false; 
                    LevelManager.GetInstance().LoadLevel(levels[levelIndex].name); 
                }
            }
            ImGui.End();
        }

        private void RenderAnimatorTool()
        {
            if (!animatorToolActive)
            {
                return;
            }

            ImGui.Begin("Animator Tool", ref animatorToolActive, ImGuiWindowFlags.None);
            ImGui.BeginChild("Scrolling", new System.Numerics.Vector2(0), ImGuiChildFlags.None);
            AnimationState[] animationStates = inspectedAnimatorComponent.allStates.ToArray();
            for (int stateIndex = 0; stateIndex < animationStates.Length; stateIndex++)
            {
                ImGui.TextColored(new System.Numerics.Vector4(1, 0, 0, 1), "Name:" + animationStates[stateIndex].animationStateName);
                ImGui.Text("Sprite Path:" + animationStates[stateIndex].spritePath);
                ImGui.Separator();
                if (ImGui.CollapsingHeader("Transitions" + stateIndex))
                {
                    for (int transitionIndex = 0; transitionIndex < animationStates[stateIndex].transitions.Count; transitionIndex++)
                    {
                        ImGui.Text("From:" + animationStates[stateIndex].transitions[transitionIndex].fromState.animationStateName);
                        ImGui.Text("To:" + animationStates[stateIndex].transitions[transitionIndex].toState.animationStateName);
                        ImGui.Text("Condition Data");
                        ImGui.Text("Parameter:" + animationStates[stateIndex].transitions[transitionIndex].transitionCondition.parameter);
                        ImGui.Text("Condition:" + animationStates[stateIndex].transitions[transitionIndex].transitionCondition.condition);
                        ImGui.Text("Value:" + animationStates[stateIndex].transitions[transitionIndex].transitionCondition.value);
                    }
                }
                ImGui.Separator();
                ImGui.Separator();
            }
            ImGui.EndChild();
            ImGui.End();
        }
    }
}
