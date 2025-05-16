using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShareIt.Client.Service
{
    public class PexelsImageService
    {
        private readonly HttpClient _httpClient;
        
        //Hinweis: Der API Key an dieser Stelle muss ein eigener sein. Der für dieses Projekt genutzte API-Key war ein privater API-Key von Pexels, daher ist hier kein API Key hinterlegt.
        //Anbei ein Beispiel für das Einfügen eines API-Keys:
        //private readonly string _apiKey = "HIER API-KEY EINFÜGEN (z. B. von https://www.pexels.com/api/)";
        private readonly string _apiKey = "";

        public PexelsImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> GetImageUrlsAsync(string query, int count = 6)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                // Kein API-Key -> gib Standardbild zurück
                return new List<string>
        {
            "https://i.ibb.co/nLNpg7H/blank-profile-picture-gb0d3bbf79-1280.png"
        };
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.pexels.com/v1/search?query={query}&per_page={count}");
                request.Headers.Add("Authorization", _apiKey);

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode) return new List<string>();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<PexelsResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result?.Photos?.Select(p => p.Src.Medium).ToList() ?? new List<string>();
            }
            catch
            {
                // Im Fehlerfall (z. B. kein Internet, Fehlerhafte API) Fallback-Bild zurückgeben
                return new List<string>
        {
            "https://i.ibb.co/nLNpg7H/blank-profile-picture-gb0d3bbf79-1280.png"
        };
            }
        }

        private class PexelsResult
        {
            public List<Photo> Photos { get; set; }
        }

        private class Photo
        {
            public Src Src { get; set; }
        }

        private class Src
        {
            public string Medium { get; set; }
        }
    }
}
