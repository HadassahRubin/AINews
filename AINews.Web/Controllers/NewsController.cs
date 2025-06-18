using AINews.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AINews.Web.Controllers
{
    public class OllamaResponse
    {
        [JsonPropertyName("response")]
        public string Response { get; set; }
    }
    public class SummaryRequest
    {
        public string Url { get; set; }
    }

    public class SummaryResponse
    {
        public string Summary { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        [HttpPost("generate")]
        public SummaryResponse Generate(SummaryRequest request)
        {
            var scraperService = new NewsScraperServices();

            string text = scraperService.ArticleScraper(request.Url);
            var prompt = $"You are a freindly news reporter ,who is giving a short summery of {request.Url} .You just want to give over the main idea of the article as your audience doesnt have a lot of time .";

            var ollamaRequest = new
            {
                model = "llava",
                prompt = prompt,
                stream = false
            };

            var json = JsonSerializer.Serialize(ollamaRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = client.PostAsync("https://api.lit-ai-demo.com/api/generate", content).Result;

            var result = response.Content.ReadFromJsonAsync<OllamaResponse>().Result;

            return new SummaryResponse { Summary = result.Response };
        }
    }
}
