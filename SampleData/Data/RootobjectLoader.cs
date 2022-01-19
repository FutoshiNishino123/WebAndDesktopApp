using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleData.Data
{
    internal static class RootobjectLoader
    {
        public static Rootobject? Load()
        {
            var json = File.ReadAllText("Data/parameter.json");
            var obj = JsonSerializer.Deserialize<Rootobject>(json);
            return obj;
        }
    }
}