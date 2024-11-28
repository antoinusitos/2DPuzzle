using ImGuiNET;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class AnimatorComponent : StateMachineComponent
    {
        private AnimationState currentAnimationState = null;

        public uint startingStateID = 0;

        public int layer = 0;

        //Use this to save only
        public List<AnimationState> allStates = new List<AnimationState>();
        public List<StateMachineTransition> allTransitions = new List<StateMachineTransition>();

        public AnimatorComponent() : base()
        {
            canUpdate = true;
            canRender = true;
            RenderManager.GetInstance().RegisterRenderer(this);
        }

        public AnimatorComponent(Entity inOwner) : base(inOwner)
        {
            canUpdate = true;
            canRender = true;
            RenderManager.GetInstance().RegisterRenderer(this);
        }

        public override void Start()
        {
            base.Start();
            for (int stateIndex = 0; stateIndex < allStates.Count; stateIndex++)
            {
                allStates[stateIndex].Start();
            }
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            currentAnimationState.spriteAnimatorRenderComponent.Render(inGameTime);
        }

        public override void ChangeState(StateMachineState toState)
        {
            currentState.OnExit();
            currentState = toState;
            currentAnimationState = (AnimationState)currentState;
            currentState.OnEnter();
        }

        public override void SetStartingState(StateMachineState inStartingState)
        {
            startingState = inStartingState;
            currentState = startingState;
            currentAnimationState = (AnimationState)currentState;
        }

        public override string ComponentToString()
        {
            return "Unique ID:" + uniqueID + "\n" +
                    "Current Frame:" + (currentAnimationState != null ? currentAnimationState.spriteAnimatorRenderComponent.spriteAnimatorRender.GetCurrentIndex() : "-1") + "\n" +
                    "Layer:" + layer;
        }

        public override void EditorGUI()
        {
            ImGui.Text("Unique ID:" + uniqueID);
            ImGui.Text("Current Frame:" + (currentAnimationState != null ? currentAnimationState.spriteAnimatorRenderComponent.spriteAnimatorRender.GetCurrentIndex() : "-1"));
            ImGui.Text("Layer:" + layer);
        }

        public override SavedData GetSavedData()
        {
            SavedData toReturn = base.GetSavedData();
            if(toReturn.savedInt == null)
            {
                toReturn.savedInt = new Dictionary<string, int>();
            }
            if (toReturn.savedString == null)
            {
                toReturn.savedString = new Dictionary<string, string>();
            }

            toReturn.savedInt.Add("Editor." + owner.name + ".StatesNumber", allStates.Count);
            toReturn.savedInt.Add("Editor." + owner.name + ".TransitionsNumber", allTransitions.Count);
            toReturn.savedInt.Add("Editor." + owner.name + ".layer", layer);
            toReturn.savedString.Add("Editor." + owner.name + ".startingStateID", startingStateID.ToString());
            for (int stateIndex = 0; stateIndex < allStates.Count; stateIndex++)
            {
                toReturn.savedString.Add("Editor." + owner.name + ".StatesID" + stateIndex, allStates[stateIndex].uniqueID.ToString());
            }
            for (int transitionIndex = 0; transitionIndex < allTransitions.Count; transitionIndex++)
            {
                toReturn.savedString.Add("Editor." + owner.name + ".TransitionID" + transitionIndex, allTransitions[transitionIndex].uniqueID.ToString());
            }
            return toReturn;
        }

        public override void LoadSavedData(SavedData inSavedData)
        {
            base.LoadSavedData(inSavedData);

            if (inSavedData.savedString.ContainsKey("Editor." + owner.name + ".startingStateID"))
            {
                startingStateID = uint.Parse(inSavedData.savedString["Editor." + owner.name + ".startingStateID"]);
            }
            if (inSavedData.savedInt.ContainsKey("Editor." + owner.name + ".layer"))
            {
                layer = inSavedData.savedInt["Editor." + owner.name + ".layer"];
                RenderManager.GetInstance().SwitchLayer(0, layer, this);
            }
        }

        public override ComponentSave[] GetMoreComponentsToSave()
        {
            int totalIndex = 0;
            ComponentSave[] componentSaves = new ComponentSave[allStates.Count + allTransitions.Count]; 
            for (int stateIndex = 0; stateIndex < allStates.Count; stateIndex++)
            {
                componentSaves[totalIndex] = new ComponentSave
                {
                    componentUniqueID = allStates[stateIndex].uniqueID.ToString(),
                    componentType = allStates[stateIndex].GetType().ToString(),
                    saveData = allStates[stateIndex].GetSavedData()
                };
                totalIndex++;
            }
            for (int transitionIndex = 0; transitionIndex < allTransitions.Count; transitionIndex++)
            {
                componentSaves[totalIndex] = new ComponentSave
                {
                    componentUniqueID = allTransitions[transitionIndex].uniqueID.ToString(),
                    componentType = allTransitions[transitionIndex].GetType().ToString(),
                    saveData = allTransitions[transitionIndex].GetSavedData()
                };
                totalIndex++;
            }
            return componentSaves;
        }
    }
}
