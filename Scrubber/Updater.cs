using System.Linq;
using HtmlAgilityPack;

namespace Scrubber
{
    public static class Updater
    {
        public static string UpdateCssLinks(string html)
        {
            var document = new HtmlDocument();

            document.LoadHtml(html);

            document.DocumentNode
                .SelectNodes(".//link[@rel='stylesheet']")
                .ToList()
                .ForEach(n => { n.Remove(); });

            document.DocumentNode.SelectSingleNode(".//head")
                .AppendChild(HtmlNode.CreateNode("<link ref=\"stylesheet\" href=\"./main.css\" />"));

            return document.DocumentNode.OuterHtml;
        }
    }
}
