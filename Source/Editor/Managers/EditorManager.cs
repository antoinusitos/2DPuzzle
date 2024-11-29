using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGuiNet;
using Newtonsoft.Json;
using System.IO;

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

        private bool saveLevelActive = false;

        private bool addComponentActive = false;

        private bool spawnPrefabActive = false;

        public void InitializeManager(GameBase inGameBase)
        {
            gameBase = inGameBase;

            string jsonString = File.ReadAllText("Editor/EditorSettings.json");
            EditorSettingsSave editorSettings = JsonConvert.DeserializeObject<EditorSettingsSave>(jsonString);

            if(editorSettings != null)
            {
                currentUniqueID = editorSettings.uniqueIDReached;
            }

            guiRenderer = new ImGuiRenderer(gameBase);
            guiRenderer.RebuildFontAtlas();

            debugMousePosition = new DebugMousePosition();
            debugMousePosition.transformComponent.position = new Vector2(0, RenderManager.GetInstance().GetScreenHeight() - 20);
            debugMousePosition.Start();

            LevelManager.GetInstance().AddLevel(new LevelTest());
        }

        public uint GetNewUniqueID()
        {
            uint toReturn = currentUniqueID;
            currentUniqueID++;
            EditorSettingsSave editorSettings = new EditorSettingsSave()
            {
                uniqueIDReached = currentUniqueID
            };

            string settingsToSave = JsonConvert.SerializeObject(editorSettings, Formatting.Indented);
            File.WriteAllText("Editor/EditorSettings.json", settingsToSave);
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

            RenderSaveMenu();

            RenderAnimatorTool();

            RenderAddComponent();

            RenderSpawnPrefab();

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
                    if (ImGui.MenuItem("New", "Ctrl+N")) { LevelManager.GetInstance().NewLevel(); }
                    if (ImGui.MenuItem("Open", "Ctrl+O")) { openLevelActive = true; }
                    if (ImGui.MenuItem("Save", "Ctrl+S")) { if (LevelManager.GetInstance().currentLevel.name == string.Empty) { saveLevelActive = true; } else { LevelManager.GetInstance().SaveLevel(); } }
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
            if (ImGui.MenuItem("Spawn Prefab", "ctrl+P"))
            {
                spawnPrefabActive = true;
            }
            if (ImGui.MenuItem("Create Entity", "ctrl+N"))
            {
                Entity newEntity = new Entity()
                {
                    name = "NewEntity",
                    isDirty = true,
                };
                newEntity.uniqueID = GetNewUniqueID();
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
                    inspectedEntity.components[componentIndex].EditorGUI();
                    if (inspectedEntity.components[componentIndex].GetType() == typeof(AnimatorComponent))
                    {
                        if (ImGui.MenuItem("Animator Tool", "ctrl+A"))
                        {
                            inspectedAnimatorComponent = (AnimatorComponent)inspectedEntity.components[componentIndex];
                            animatorToolActive = true;
                        }
                    }
                }
                ImGui.Separator();
                if (ImGui.MenuItem("Add Component", ""))
                {
                    addComponentActive = true;
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

        private void RenderSaveMenu()
        {
            if (!saveLevelActive)
            {
                return;
            }

            ImGui.Begin("Save Level", ref saveLevelActive, ImGuiWindowFlags.None);
            ImGui.InputText("Level Name", ref LevelManager.GetInstance().currentLevel.name, 32);
            if(ImGui.Button("Save"))
            {
                LevelManager.GetInstance().SaveLevel();
                saveLevelActive = false;
            }
            ImGui.End();
        }

        private void RenderOpenMenu()
        {
            if (!openLevelActive)
            {
                return;
            }

            ImGui.Begin("Open Level", ref openLevelActive, ImGuiWindowFlags.None);
            string[] availableLevels = Directory.GetFiles("Levels/");
            for (int levelIndex = 0; levelIndex < availableLevels.Length; levelIndex++)
            {
                string level = availableLevels[levelIndex].Replace("Levels/", "").Replace(".json", "");
                if (ImGui.MenuItem(level))
                {
                    inspectedEntity = null;
                    openLevelActive = false;
                    LevelManager.GetInstance().LoadLevel(level);
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

        private void RenderAddComponent()
        {
            if (!addComponentActive)
            {
                return;
            }

            ImGui.Begin("Add Component", ref addComponentActive, ImGuiWindowFlags.None);
            if (ImGui.MenuItem("Sprite Render Component", ""))
            {
                addComponentActive = false;
                SpriteRenderComponent spriteRenderComponent = new SpriteRenderComponent()
                {
                    uniqueID = GetNewUniqueID(),
                    owner = inspectedEntity,
                };
                inspectedEntity.components.Add(spriteRenderComponent);
                spriteRenderComponent.Start();
                inspectedEntity.differFromPrefab = true;
            }
            if (ImGui.MenuItem("Physics Component", ""))
            {
                addComponentActive = false;
                PhysicsComponent physicsComponent = new PhysicsComponent()
                {
                    uniqueID = GetNewUniqueID(),
                    owner = inspectedEntity,
                };
                inspectedEntity.components.Add(physicsComponent);
                physicsComponent.Start();
                inspectedEntity.differFromPrefab = true;
            }
            if (ImGui.MenuItem("Animator Component", ""))
            {
                addComponentActive = false;
                AnimatorComponent animatorComponent = new AnimatorComponent()
                {
                    uniqueID = GetNewUniqueID(),
                    owner = inspectedEntity,
                };
                inspectedEntity.components.Add(animatorComponent);
                animatorComponent.Start();
                inspectedEntity.differFromPrefab = true;
            }
            ImGui.End();
        }

        private void RenderSpawnPrefab()
        {
            if (!spawnPrefabActive)
            {
                return;
            }

            ImGui.Begin("Spawn Prefab", ref spawnPrefabActive, ImGuiWindowFlags.None);
            string [] prefabs = Directory.GetFiles("Prefabs/");
            for(int prefabIndex = 0;  prefabIndex < prefabs.Length; prefabIndex++)
            {
                string prefab = prefabs[prefabIndex].Replace("Prefabs/", "").Replace(".json", "");
                if (ImGui.MenuItem(prefab, ""))
                {
                    spawnPrefabActive = false;
                    uint newID = GetNewUniqueID();
                    Entity newEntity = new Entity()
                    {
                        name = prefab + newID,
                        differFromPrefab = true,
                    };
                    SaveManager.GetInstance().LoadEntity(ref newEntity, prefab);
                    newEntity.uniqueID = newID;
                    LevelManager.GetInstance().currentLevel.entities.Add(newEntity);
                    inspectedEntity = newEntity;
                    newEntity.Start();
                }
            }
            ImGui.End();
        }
    }
}
