using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UnderRun
{
    public class Saves
    {
        public static void SaveSettings(GameSettings settings, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(filePath, json);
        }

        public static GameSettings LoadSettings(string filePath)
        {
            if (!File.Exists(filePath))
                return new GameSettings(); // Return defaults if no file

            string json = File.ReadAllText(filePath);
            try
            {
                if (JsonSerializer.Deserialize<GameSettings>(json) != null) return JsonSerializer.Deserialize<GameSettings>(json);
                else return new GameSettings();
            }
            catch
            {
                return new GameSettings();
            }
        }

    }


    public class GameSettings
    {
        public int ResolutionID { get; set; } = 1;
        public List<ConfigFlags> WindowFlags { get; set; } = new List<ConfigFlags>();
    }
}
