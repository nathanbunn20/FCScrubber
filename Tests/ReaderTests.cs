using System.Threading.Tasks;
using FakeItEasy;
using Scrubber;
using Xunit;

namespace Tests
{
    public class ReaderTests
    {
        private readonly IReader _sut;

        public ReaderTests()
        {
            _sut = A.Fake<IReader>();
        }

        [Fact]
        public async Task ReturnsString()
        {
            A.CallTo(() => _sut.Read()).Returns("");

            Assert.IsType<string>(await _sut.Read());
        }
    }
}
