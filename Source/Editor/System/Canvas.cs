using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _2DPuzzle
{
    public class Canvas
    {
        private GraphicsDevice _graphicDevice = null;
        private RenderTarget2D _target = null;
        private Rectangle _destinationRectangle ;

        public Canvas(GraphicsDevice inGraphicsDevice, int inWidth, int inHeight) 
        {
            _graphicDevice = inGraphicsDevice;
            _target = new RenderTarget2D(_graphicDevice, inWidth, inHeight);
        }

        public void SetDestinationRectangle()
        {
            var screenSize = _graphicDevice.PresentationParameters.Bounds;

            float scaleX = (float)screenSize.Width / _target.Width;
            float scaleY = (float)screenSize.Height / _target.Height;
            float scale = Math.Min(scaleX, scaleY);

            int newWidth = (int)(_target.Width * scale);
            int newHeight = (int)(_target.Height * scale);

            int posX = (screenSize.Width - newWidth) / 2;
            int posY = (screenSize.Height - newHeight) / 2;

            _destinationRectangle = new Rectangle(posX, posY, newWidth, newHeight);
        }

        public void Activate()
        {
            _graphicDevice.SetRenderTarget(_target);
        }

        public void Draw(SpriteBatch inSpriteBatch)
        {
            _graphicDevice.SetRenderTarget(null);
            _graphicDevice.Clear(Color.Black);
            inSpriteBatch.Begin();
            inSpriteBatch.Draw(_target, _destinationRectangle, Color.White);
            inSpriteBatch.End();
        }
    }
}
