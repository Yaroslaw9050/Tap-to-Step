using System.Threading;
using Cysharp.Threading.Tasks;

namespace Runtime.Builders.Location
{
    public interface ILocationGenerator
    {
        public UniTask GenerateNewLocationAsync(CancellationToken token);
    }
}