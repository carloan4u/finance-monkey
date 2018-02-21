using System.Collections.Generic;
using System.Linq;
using FinanceMonkey.ServiceAnalyser.Analysers;
using FinanceMonkey.ServiceAnalyser.ServiceTypes;

namespace FinanceMonkey.ServiceAnalyser
{
    class AwsServiceAnalyser
    {
        public IEnumerable<BaseService> GetApplicableServices()
        {
            var services = new List<BaseService>();
            services.AddRange(new ElasticBeanstalkAnalyser().GetServices());
            foreach (var service in services)
            {
                if (service.AppropriateToDownsize && service.InstanceType.IsSmallest)
                {
                    service.AppropriateToDownsize = false;
                }
            }
            return services.Where(service => service.AppropriateToDownsize);
        }
    }
}
