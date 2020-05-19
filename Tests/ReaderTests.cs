using System;
using System.IO;
using System.Threading.Tasks;
using Scrubber;
using Xunit;

namespace Tests
{
    public class ReaderTests
    {
        private readonly string _testFilePath = $@"{Directory.GetCurrentDirectory()}/TestData/Reader.html";
        private readonly string _testSiteUrl = "http://fuckcombustion.com/";

        [Fact]
        public async Task ReturnsString()
        {
            Assert.IsType<string>(await Reader.ReadHtml(new Uri(_testSiteUrl)));
        }

        [Fact]
        public void RemovesAllScriptTags()
        {
            var html = File.ReadAllText(_testFilePath);

            var result = html.RemoveAllScripts();

            Assert.DoesNotContain("<script", result);
        }

        [Fact]
        public void CssExpanded()
        {
            // load file
            // foreach link with .css in its ref, download file contents and replace link tag with expanded script tag
        }
    }
}
