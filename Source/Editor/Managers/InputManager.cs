using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _2DPuzzle
{
    //TODO : Add the command pattern here
    public class InputManager
    {
        private static InputManager _instance;

        private static readonly object _lock = new object();

        public static InputManager GetInstance()
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
                        _instance = new InputManager();
                    }
                }
            }
            return _instance;
        }

        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;

        public InputManager()
        {
        }

        public void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys inKey)
        {
            return currentKeyboardState.IsKeyDown(inKey);
        }

        public bool WasKeyPressed(Keys inKey)
        {
            return previousKeyboardState.IsKeyDown(inKey) && currentKeyboardState.IsKeyUp(inKey);
        }

        public bool IsKeyDown(string inName)
        {
            if(!Inputs.mapping.ContainsKey(inName))
            {
                Debug.LogError("Mapping does not contain key : " + inName);
                return false;
            }

            return currentKeyboardState.IsKeyDown(Inputs.mapping[inName]);
        }

       /* public bool IsGamepadButtonDown(GamePadButtons inGamepadButton)
        {
            return GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed;

            return false;
        }*/

        public Vector2 GetMousePositionScreen()
        {
            return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        public Vector2 GetMousePositionWorld()
        {
            Matrix inverseTransform = Matrix.Invert(RenderManager.GetInstance().screenScaleMatrix);
            return Vector2.Transform(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), inverseTransform);
        }
    }
}
