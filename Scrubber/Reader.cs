using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Scrubber
{
    public static class Reader
    {
        public static async Task<string> ReadHtml(Uri uri)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(uri);

            return await response.Content.ReadAsStringAsync();
        }

        public static void ExpandCss(string locator)
        {

        }

        public static string RemoveAllScripts(this string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            doc.DocumentNode.Descendants()
                .Where(n => n.Name == "script")
                .ToList()
                .ForEach(n => n.Remove());

            return doc.DocumentNode.OuterHtml;
        }
    }
}
