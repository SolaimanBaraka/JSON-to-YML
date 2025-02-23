using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace JsonToYamlConverter
{
    public class Program
    {
        public static void Main()
        {
            Console.Write("Nombre del archivo JSON: ");
            string nombreArchivo = Console.ReadLine();
            string jsonFilePath = $@"C:\Users\Solaiman Baraka\Desktop\JSON\{nombreArchivo}.json";
            string yamlFilePath = $@"C:\Users\Solaiman Baraka\Desktop\YAML\{nombreArchivo}.yml";

            string jsonContent = File.ReadAllText(jsonFilePath);
            var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonContent);
            var nativeObject = ConvertJTokenToObject(jsonObject);

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            string yamlContent = serializer.Serialize(nativeObject);
            File.WriteAllText(yamlFilePath, yamlContent);

            Console.WriteLine($"Archivo YAML guardado como: {yamlFilePath}");
        }

        private static object ConvertJTokenToObject(JToken token)
        {
            if (token is JObject obj)
            {
                var result = new Dictionary<string, object>();
                foreach (var prop in obj)
                {
                    result[prop.Key] = ConvertJTokenToObject(prop.Value);
                }
                return result;
            }
            else if (token is JArray array)
            {
                var list = new List<object>();
                foreach (var item in array)
                {
                    list.Add(ConvertJTokenToObject(item));
                }
                return list;
            }
            else if (token is JValue value)
            {
                return value.Value;
            }
            return null;
        }
    }
}