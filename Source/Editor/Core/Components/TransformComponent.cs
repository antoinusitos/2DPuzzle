using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class TransformComponent : EntityComponent
    {
        private Vector2 _position = Vector2.Zero;
        private float _rotation = 0;
        private Vector2 _scale = Vector2.One;

        public Vector2 position { get => _position; set { _position = value; owner.isDirty = true; } }
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
            return "Position:" + position + "\n" + 
                    "Rotation:" + rotation + "\n" + 
                    "Scale:" + scale;
        }

        public override SavedData GetSavedData()
        {
            SavedData savedData = new SavedData
            {
                savedFloat = new Dictionary<string, float>
                {
                    { "Editor." + owner.name + ".Position.X", position.X },
                    { "Editor." + owner.name + ".Position.Y", position.Y },
                    { "Editor." + owner.name + ".Rotation", rotation },
                    { "Editor." + owner.name + ".Scale.X", scale.X },
                    { "Editor." + owner.name + ".Scale.Y", scale.Y }
                }
            };
            return savedData;
        }

        public override void LoadSavedData(SavedData inSavedData)
        {
            float x = 0;
            float y = 0;
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Position.X"))
            {
                x = inSavedData.savedFloat["Editor." + owner.name + ".Position.X"];
            }
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Position.Y"))
            {
                y = inSavedData.savedFloat["Editor." + owner.name + ".Position.Y"];
            }
            position = new Vector2(x, y);

            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Rotation"))
            {
                rotation = inSavedData.savedFloat["Editor." + owner.name + ".Rotation"];
            }

            x = 0;
            y = 0;
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Scale.X"))
            {
                x = inSavedData.savedFloat["Editor." + owner.name + ".Scale.X"];
            }
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Scale.Y"))
            {
                y = inSavedData.savedFloat["Editor." + owner.name + ".Scale.Y"];
            }
            scale = new Vector2(x, y);
        }
    }
}
