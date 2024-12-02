using ImGuiNET;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class TransformComponent : EntityComponent
    {
        private Vector2 _position = Vector2.Zero;
        private Vector2 _relativePosition = Vector2.Zero;
        private float _rotation = 0;
        private Vector2 _scale = Vector2.One;

        public Vector2 position { get { if (owner.parent == null) return _position; else return owner.ComputePosition(); } set { _position = value; owner.isDirty = true; } }
        public Vector2 relativePosition { get => _relativePosition; set { _relativePosition = value; owner.isDirty = true; } }
        public float rotation { get => _rotation; set { _rotation = value; owner.isDirty = true; } }
        public Vector2 scale { get => _scale; set { _scale = value; owner.isDirty = true; } }

        public TransformComponent()
        {

        }

        public TransformComponent(Entity inOwner) : base(inOwner)
        {

        }

        public void PrintPosition()
        {
            Debug.Log("X:" + position.X + " | " + "Y:" + position.Y, owner);
        }

        public override string ComponentToString()
        {
            return "Unique ID:" + uniqueID + "\n" + 
                    "Position:" + position + "\n" +
                    "Relative Position:" + relativePosition + "\n" +
                    "Rotation:" + rotation + "\n" + 
                    "Scale:" + scale;
        }

        public override void EditorGUI()
        {
            ImGui.Text("Unique ID:" + uniqueID);
            float tempX = _position.X;
            if(ImGui.InputFloat("Position_X", ref tempX))
            {
                position = new Vector2(tempX, _position.Y);
            }
            float tempY = _position.Y;
            if (ImGui.InputFloat("Position_Y", ref tempY))
            {
                position = new Vector2(_position.X, tempY);
            }
            float tempRelX = _relativePosition.X;
            if (ImGui.InputFloat("RelativePosition_X", ref tempRelX))
            {
                relativePosition = new Vector2(tempRelX, _relativePosition.Y);
            }
            float tempRelY = _relativePosition.Y;
            if (ImGui.InputFloat("RelativePosition_Y", ref tempRelY))
            {
                relativePosition = new Vector2(_relativePosition.X, tempRelY);
            }
            ImGui.InputFloat("Rotation", ref _rotation);
            ImGui.InputFloat("Scale_X", ref _scale.X);
            ImGui.InputFloat("Scale_Y", ref _scale.Y);
        }

        public override SavedData GetSavedData()
        {
            SavedData savedData = new SavedData
            {
                savedFloat = new Dictionary<string, float>
                {
                    { "Editor." + owner.name + ".Position.X." + uniqueID, position.X },
                    { "Editor." + owner.name + ".Position.Y." + uniqueID, position.Y },
                    { "Editor." + owner.name + ".RelativePosition.X." + uniqueID, relativePosition.X },
                    { "Editor." + owner.name + ".RelativePosition.Y." + uniqueID, relativePosition.Y },
                    { "Editor." + owner.name + ".Rotation." + uniqueID, rotation },
                    { "Editor." + owner.name + ".Scale.X." + uniqueID, scale.X },
                    { "Editor." + owner.name + ".Scale.Y." + uniqueID, scale.Y }
                },
                savedString = new Dictionary<string, string>
                {
                    { "Editor." + owner.name + ".UniqueID", uniqueID.ToString() },
                }
            };
            return savedData;
        }

        public override void LoadSavedData(SavedData inSavedData)
        {
            if (inSavedData.savedString.ContainsKey("Editor." + owner.name + ".UniqueID"))
            {
                uniqueID = uint.Parse(inSavedData.savedString["Editor." + owner.name + ".UniqueID"]);
            }

            float x = 0;
            float y = 0;
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Position.X." + uniqueID))
            {
                x = inSavedData.savedFloat["Editor." + owner.name + ".Position.X." + uniqueID];
            }
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Position.Y." + uniqueID))
            {
                y = inSavedData.savedFloat["Editor." + owner.name + ".Position.Y." + uniqueID];
            }
            position = new Vector2(x, y);

            x = 0;
            y = 0;
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".RelativePosition.X." + uniqueID))
            {
                x = inSavedData.savedFloat["Editor." + owner.name + ".RelativePosition.X." + uniqueID];
            }
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".RelativePosition.Y." + uniqueID))
            {
                y = inSavedData.savedFloat["Editor." + owner.name + ".RelativePosition.Y." + uniqueID];
            }
            relativePosition = new Vector2(x, y);

            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Rotation." + uniqueID))
            {
                rotation = inSavedData.savedFloat["Editor." + owner.name + ".Rotation." + uniqueID];
            }

            x = 0;
            y = 0;
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Scale.X." + uniqueID))
            {
                x = inSavedData.savedFloat["Editor." + owner.name + ".Scale.X." + uniqueID];
            }
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Scale.Y." + uniqueID))
            {
                y = inSavedData.savedFloat["Editor." + owner.name + ".Scale.Y." + uniqueID];
            }
            scale = new Vector2(x, y);
        }
    }
}
