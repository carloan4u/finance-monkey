using System;
using System.Collections.Generic;
using Amazon.ElasticBeanstalk.Model;
using FinanceMonkey.Downsizer.Downsizers;

namespace FinanceMonkey.ServiceAnalyser.ServiceTypes
{
    class ElasticBeanstalkService : BaseService
    {
        public string EnvironmentId { get; set; }
        public string ApplicationName { get; set; }
        public string TemplateName { get; set; }

        public List<ConfigurationOptionSetting> OptionSettings { get; set; }

        public override bool Downsize()
        {
            Console.WriteLine($"[{ServiceName}]: Downsizing");
            new ElasticBeanstalkDownsizer().Downsize(this);
            Console.WriteLine($"[{ServiceName}]: Done.. I think?");
            return true;
        }
    }
}
