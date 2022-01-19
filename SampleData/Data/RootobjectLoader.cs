using SampleData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

internal static class RootobjectLoader
{
    public static Rootobject? Load()
    {
        var json = File.ReadAllText("Resources/parameter.json");
        var obj = JsonSerializer.Deserialize<Rootobject>(json);
        return obj;
    }
}
