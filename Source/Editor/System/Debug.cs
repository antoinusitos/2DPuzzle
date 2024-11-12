namespace _2DPuzzle
{
    public class Debug
    {
        public static void Log(string inMessage, Entity inEntity = null)
        {
            string name = "";
            if(inEntity != null)
            {
                name = inEntity.name;
            }
            System.Diagnostics.Debug.WriteLine(name + ":" + inMessage);
        }

        public static void LogError(string inMessage, Entity inEntity = null)
        {
            string name = "";
            if (inEntity != null)
            {
                name = inEntity.name;
            }
            System.Diagnostics.Debug.WriteLine("Error : " + name + ":" + inMessage);
        }

        public static void LogWarning(string inMessage, Entity inEntity = null)
        {
            string name = "";
            if (inEntity != null)
            {
                name = inEntity.name;
            }
            System.Diagnostics.Debug.WriteLine("Warning : " + name + ":" + inMessage);
        }
    }
}
