using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class StateMachineComponent : EntityComponent
    {
        public Dictionary<string, float> parameters = new Dictionary<string, float>();

        public StateMachineState currentState = null;
        public StateMachineState startingState = null;

        public StateMachineComponent() : base()
        {

        }

        public StateMachineComponent(Entity inOwner) : base(inOwner)
        {

        }

        public void UpdateParameter(string inKey, float inValue)
        {
            if (!parameters.ContainsKey(inKey))
            {
                return;
            }

            parameters[inKey] = inValue;
        }

        public float GetParameterValue(string inKey)
        {
            if (!parameters.ContainsKey(inKey))
            {
                return -1;
            }

            return parameters[inKey];
        }

        public virtual void SetStartingState(StateMachineState inStartingState)
        {
            startingState = inStartingState;
            currentState = startingState;
        }

        public virtual void ChangeState(StateMachineState toState)
        {
            currentState.OnExit();
            currentState = toState;
            currentState.OnEnter();
        }

        public override void Update(GameTime inGameTime)
        {
            base.Update(inGameTime);

            currentState.OnUpdate();
        }

        public override SavedData GetSavedData()
        {
            SavedData savedData = new SavedData
            {
                savedFloat = new Dictionary<string, float>(),
                savedString = new Dictionary<string, string>(),
                savedInt = new Dictionary<string, int>(),
            };

            int paramNumber = 0;
            foreach (KeyValuePair<string, float> singleParam in parameters)
            {
                savedData.savedString.Add("Editor." + owner.name + ".Param" + paramNumber, singleParam.Key);
                savedData.savedFloat.Add(singleParam.Key, singleParam.Value);
                paramNumber++;
            }
            savedData.savedInt.Add("Editor." + owner.name + ".ParamNumber", paramNumber);
            return savedData;
        }

        public override void LoadSavedData(SavedData inSavedData)
        {
            int paramNumber = 0;
            if (inSavedData.savedInt.ContainsKey("Editor." + owner.name + ".ParamNumber"))
            {
                paramNumber = inSavedData.savedInt["Editor." + owner.name + ".ParamNumber"];
            }
            parameters = new Dictionary<string, float>();
            for (int paramIndex = 0; paramIndex < paramNumber; paramIndex++)
            {
                if (inSavedData.savedString.ContainsKey("Editor." + owner.name + ".Param" + paramIndex))
                {
                    string key = inSavedData.savedString["Editor." + owner.name + ".Param" + paramIndex];
                    if (inSavedData.savedFloat.ContainsKey(key))
                    {
                        parameters.Add(key, inSavedData.savedFloat[key]);
                    }
                }
            }
        }
    }
}
