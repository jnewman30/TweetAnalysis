using System.Collections.Generic;
using Microsoft.Extensions.Hosting;

namespace WebAPI.Helpers
{
    public class HostedServiceAccessor<T> : IHostedServiceAccessor<T>
        where T : IHostedService
    {
        public HostedServiceAccessor(IEnumerable<IHostedService> hostedServices)
        {
            foreach (var service in hostedServices)
            {
                if (service is not T match)
                {
                    continue;
                }

                Service = match;
                break;
            }
        }

        public T Service { get; }
    }
}