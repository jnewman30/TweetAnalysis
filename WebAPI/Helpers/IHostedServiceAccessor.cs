using Microsoft.Extensions.Hosting;

namespace WebAPI.Helpers
{
    public interface IHostedServiceAccessor<T> where T : IHostedService
    {
        T Service { get; }
    }
}