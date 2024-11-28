using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

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

        public virtual void UninitializeLevel()
        {
            for (int entityIndex = 0; entityIndex < entities.Count; entityIndex++)
            {
                entities[entityIndex] = null;
            }

            entities.Clear();
        }

        public virtual void Start()
        {
            for (int entityIndex = 0; entityIndex < entities.Count; entityIndex++)
            {
                entities[entityIndex].Start();
            }
        }

        public void Save()
        {
            if(name == string.Empty)
            {
                Debug.LogWarning("Saving a level with an empty name");
            }
            LevelSave levelSave = new LevelSave()
            {
                name = name,
                entitiesCleanSaved = new List<EntityCleanSave>(),
                entitiesSaved = new List<LevelEntitySave>()
            };
            List<EntityCleanSave> notDifferedEntities = new List<EntityCleanSave>();
            for (int entityIndex = 0; entityIndex < entities.Count; entityIndex++)
            {
                if(entities[entityIndex].differFromPrefab)
                {
                    EntitySave entitySave = entities[entityIndex].GetSaveData();
                    LevelEntitySave levelEntitySave = new LevelEntitySave()
                    {
                        name = entitySave.name,
                        componentsSaved = entitySave.componentsSaved,
                        uniqueID = entities[entityIndex].uniqueID
                    };
                    levelSave.entitiesSaved.Add(levelEntitySave);
                }
                else
                {
                    levelSave.entitiesCleanSaved.Add(new EntityCleanSave() { name = entities[entityIndex].name, uniqueID = entities[entityIndex].uniqueID });
                }
            }
            string toSave = JsonConvert.SerializeObject(levelSave, Formatting.Indented);
            File.WriteAllText("Levels/" + name + ".json", toSave);
        }

        public void Load(string inLevelName)
        {
            string jsonString = File.ReadAllText("Levels/" + inLevelName + ".json");
            LevelSave levelSave = JsonConvert.DeserializeObject<LevelSave>(jsonString);
            name = levelSave.name;
            for (int entityIndex = 0; entityIndex < levelSave.entitiesCleanSaved.Count; entityIndex++)
            {
                Entity entity = new Entity()
                {
                    uniqueID = levelSave.entitiesCleanSaved[entityIndex].uniqueID
                };
                SaveManager.GetInstance().LoadEntity(ref entity, levelSave.entitiesCleanSaved[entityIndex].name);
                entities.Add(entity);
            }
        }
    }
}
