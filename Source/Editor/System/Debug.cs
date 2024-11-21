using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _2DPuzzle
{
    public class ConsoleDebug
    {
        public int severity = -1; //0:normal, 1:warning, 2:error
        public string text = "";
    }

    public class Debug
    {
        public static List<ConsoleDebug> allDebug = new List<ConsoleDebug>();

        public static void Log(string inMessage, Entity inEntity = null)
        {
            string name = "";
            if(inEntity != null)
            {
                name = inEntity.name;
            }
            System.Diagnostics.Debug.WriteLine("Log : " + name + " : " + inMessage);
            allDebug.Add(new ConsoleDebug() { severity = 0, text = "Log : " + name + " : " + inMessage });
        }

        public static void LogError(string inMessage, Entity inEntity = null)
        {
            string name = "";
            if (inEntity != null)
            {
                name = inEntity.name;
            }
            System.Diagnostics.Debug.WriteLine("Error : " + name + " : " + inMessage);
            allDebug.Add(new ConsoleDebug() { severity = 2, text = "Error : " + name + " : " + inMessage });
        }

        public static void LogWarning(string inMessage, Entity inEntity = null)
        {
            string name = "";
            if (inEntity != null)
            {
                name = inEntity.name;
            }
            System.Diagnostics.Debug.WriteLine("Warning : " + name + " : " + inMessage);
            allDebug.Add(new ConsoleDebug() { severity = 1, text = "Warning : " + name + " : " + inMessage });
        }

        public static List<LineDrawing> linesToDraw = new List<LineDrawing>();

        public struct LineDrawing
        {
            public Vector2 point;
            public float length;
            public float angle;
            public Color color;
            public float thickness;
            public Vector2 origin;
            public Vector2 scale;
        }

        private static Texture2D _lineTexture;
        private static Texture2D GetLineTexture()
        {
            if (_lineTexture == null)
            {
                _lineTexture = new Texture2D(RenderManager.GetInstance().spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _lineTexture.SetData(new[] { Color.White });
            }

            return _lineTexture;
        }

        public static void DrawLine(Vector2 inPoint1, Vector2 inPoint2, Color inColor, float inThickness = 1f)
        {
            var distance = Vector2.Distance(inPoint1, inPoint2);
            var angle = (float)Math.Atan2(inPoint2.Y - inPoint1.Y, inPoint2.X - inPoint1.X);
            DrawLine(inPoint1, distance, angle, inColor, inThickness);
        }

        public static void DrawLine(Vector2 inPoint, float inLength, float inAngle, Color inColor, float inThickness = 1f)
        {
            var inOrigin = new Vector2(0f, 0.5f);
            var inScale = new Vector2(inLength, inThickness);

            LineDrawing lineDrawing = new LineDrawing()
            {
                point = inPoint,
                length = inLength,
                angle = inAngle,
                color = inColor,
                thickness = inThickness,
                origin = inOrigin,
                scale = inScale
            };

            linesToDraw.Add(lineDrawing);
        }

        public static void RenderLines()
        {
            for(int lineIndex = 0; lineIndex < linesToDraw.Count; lineIndex++)
            {
                RenderManager.GetInstance().spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: RenderManager.GetInstance().screenScaleMatrix);
                RenderManager.GetInstance().spriteBatch.Draw(GetLineTexture(), linesToDraw[lineIndex].point, null, linesToDraw[lineIndex].color, linesToDraw[lineIndex].angle, linesToDraw[lineIndex].origin, linesToDraw[lineIndex].scale, SpriteEffects.None, 0);
                RenderManager.GetInstance().spriteBatch.End();
            }

            linesToDraw.Clear();
        }
    }
}
