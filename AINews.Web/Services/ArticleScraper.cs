using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace AINews.Web.Services
{
    public class NewsScraperServices
    {
        public string ArticleScraper(string newsUrl)
        {
            var html = GetNewsHtml(newsUrl);
            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);


            foreach (var node in document.QuerySelectorAll("style, script, nav, header, footer, aside"))
            {
                node.Remove();
            }

            var selectors = new[] { ".post-content", ".entry-content", ".article-content", ".main-content", "article" };

            foreach (var selector in selectors)
            {
                var element = document.QuerySelector(selector);
                if (element != null)
                {
                    var text = element.TextContent.Trim();
                    if (!string.IsNullOrWhiteSpace(text) && text.Length > 200)
                    {
                        return text;
                    }
                }
            }

            var candidates = document.Body.QuerySelectorAll("div, section, p")
                .Select(e => new { Element = e, Text = e.TextContent?.Trim() ?? "" })
                .Where(e => e.Text.Length > 100)
                .OrderByDescending(e => e.Text.Length);

            return candidates.FirstOrDefault()?.Text ?? string.Empty;
        }

        private string GetNewsHtml(string newsUrl)
        {
            using var client = new HttpClient();
            return client.GetStringAsync($"{newsUrl}").Result;
        }

    }
}