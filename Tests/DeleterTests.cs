using System.IO;
using Scrubber;
using Xunit;

namespace Tests
{
    public class DeleterTests
    {
        private readonly string _testFilePath = $@"{Directory.GetCurrentDirectory()}/TestData/Reader.html";

        [Fact]
        public void RemovesAllScriptTags()
        {
            var html = File.ReadAllText(_testFilePath);

            var sut = Deleter.RemoveAllUnwanted(html);

            Assert.DoesNotContain("<script", sut);
        }
    }
}
