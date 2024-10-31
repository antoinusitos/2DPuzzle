using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class Inputs
    {
        public static Dictionary<string, Keys> mapping = new Dictionary<string, Keys>
        {
            { "Up", Keys.Z },
            { "Down", Keys.S },
            { "Right", Keys.D },
            { "Left", Keys.Q },
        };
    }
}
