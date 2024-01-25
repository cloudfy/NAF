using System.Net.Http;
using System.Text;

namespace Naf.Net.Json;

public class JsonContent(object value) : StringContent(Serialize(value), Encoding.UTF8, "application/json")
{
    private static string Serialize(object value)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(value);
    }
}
