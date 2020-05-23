using System.Linq;
using HtmlAgilityPack;

namespace Scrubber
{
    public static class Deleter
    {
        public static string RemoveAllUnwanted(string html)
        {
            var document = new HtmlDocument();

            document.LoadHtml(html);

            document.DocumentNode.Descendants()
                .Where(n => n.Name == "script")
                .ToList()
                .ForEach(n => n.Remove());

            document.DocumentNode.Descendants()
                .Where(n => n.Name == "base")
                .ToList()
                .ForEach(n => n.Remove());

            return document.DocumentNode.OuterHtml;
        }
    }
}
