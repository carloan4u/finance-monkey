using System.Collections.Generic;
using System.Linq;
using Amazon.ElasticBeanstalk;
using FinanceMonkey.ServiceAnalyser.Factories;
using FinanceMonkey.ServiceAnalyser.ServiceTypes;

namespace FinanceMonkey.ServiceAnalyser.Analysers
{
    class ElasticBeanstalkAnalyser : IServiceAnalyser<ElasticBeanstalkService>
    {
        private readonly ElasticBeanstalkServiceFactory _serviceFactory = new ElasticBeanstalkServiceFactory();

        public IEnumerable<ElasticBeanstalkService> GetServices()
        {
            var beanstalkClient = new AmazonElasticBeanstalkClient();
            var response = beanstalkClient.DescribeEnvironmentsAsync().Result;
            return response.Environments.Select(e => _serviceFactory.Create(e));
        }
    }
}
