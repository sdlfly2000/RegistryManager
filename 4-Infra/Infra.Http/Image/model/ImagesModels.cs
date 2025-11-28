using System.Text.Json.Serialization;

namespace Infra.Http.Image.model
{
    public class ImagesModel
    {
        [JsonPropertyName("repositories")]
        public List<string> Repositories { get; set; }
    }
}
