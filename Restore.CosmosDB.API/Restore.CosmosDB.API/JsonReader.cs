using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Restore.CosmosDB.API
{
    public class JsonReader
    {
        public static List<ObservationsModel> ReadJsonFile(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<ObservationsModel>>(jsonString);
        }
    }
}
