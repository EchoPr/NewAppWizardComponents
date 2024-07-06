using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;

namespace NewAppWizardComponents;

public class Method
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("level")]
    public int Level { get; set; }

    [JsonProperty("desc_ru")]
    public string DescRu { get; set; }

    [JsonProperty("desc_en")]
    public string DescEn { get; set; }

    [JsonProperty("group")]
    public List<string> Group { get; set; }

    [JsonProperty("input")]
    public List<Parameters> Input { get; set; }

    [JsonProperty("output")]
    public List<Parameters> Output { get; set; }
}

public class Parameters
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("desc_ru")]
    public string DescRu { get; set; }

    [JsonProperty("desc_en")]
    public string DescEn { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}

public class AllMethods
{
    

    [JsonProperty("functions")]
    public List<Method> Functions { get; set; }
}
