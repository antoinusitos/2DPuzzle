using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace _2DPuzzle
{
    // Spawn and remove the Entities
    public class Level
    {
        public string name = "";
        public List<Entity> entities = null;

        public Level()
        {
            entities = new List<Entity>();
        }

        public virtual void InitializeLevel()
        {

        }

        public void Save()
        {
            string toSave = Newtonsoft.Json.JsonConvert.SerializeObject(this, Formatting.Indented,
            new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });


            File.WriteAllText(name + ".json", toSave);
        }
    }
}
