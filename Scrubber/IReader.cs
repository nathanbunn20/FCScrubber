using System.Threading.Tasks;

namespace Scrubber
{
    public interface IReader
    {
        Task<string> Read();
    }
}
