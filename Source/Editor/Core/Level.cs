using System.Collections.Generic;

namespace _2DPuzzle
{
    // Spawn and remove the Entities
    public class Level
    {
        public List<Entity> entities = null;

        public Level()
        {
            entities = new List<Entity>();
        }

        public virtual void InitializeLevel()
        {

        }
    }
}
