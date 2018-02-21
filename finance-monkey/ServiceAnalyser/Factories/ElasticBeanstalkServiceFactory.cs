using System.Collections.Generic;
using System.Linq;
using Amazon.ElasticBeanstalk;
using Amazon.ElasticBeanstalk.Model;
using FinanceMonkey.ServiceAnalyser.ServiceTypes;

namespace FinanceMonkey.ServiceAnalyser.Factories
{
    class ElasticBeanstalkServiceFactory
    {
        private readonly AmazonElasticBeanstalkClient _client;

        public ElasticBeanstalkServiceFactory()
        {
            _client = new AmazonElasticBeanstalkClient();
        }

        public ElasticBeanstalkService Create(EnvironmentDescription environmentDescription)
        {
            var service = new ElasticBeanstalkService
            {
                ServiceName = environmentDescription.EnvironmentName,
                EnvironmentId = environmentDescription.EnvironmentId,
                ApplicationName = environmentDescription.ApplicationName,
                TemplateName = environmentDescription.TemplateName
            };

            if (!HasAppropriateTags(environmentDescription.EnvironmentArn))
            {
                return service;
            }

            service.OptionSettings = GetOptionSettings(environmentDescription.EnvironmentName,
                environmentDescription.ApplicationName);

            service.InstanceType = GetCurrentInstanceSize(service.OptionSettings);
            service.AppropriateToDownsize = true;

            return service;
        }

        private List<ConfigurationOptionSetting> GetOptionSettings(string environmentName, string applicationName)
        {
            var configResponse = _client.DescribeConfigurationSettingsAsync(
                new DescribeConfigurationSettingsRequest() { EnvironmentName = environmentName, ApplicationName = applicationName }).Result;

            return configResponse.ConfigurationSettings.First().OptionSettings;
        }

        private InstanceSize GetCurrentInstanceSize(List<ConfigurationOptionSetting> currentSettings)
        {
            var instanceType = currentSettings.First(opt =>
                opt.Namespace == "aws:autoscaling:launchconfiguration" && opt.OptionName == "InstanceType").Value;
            return new InstanceSize(instanceType);
        }

        private bool HasAppropriateTags(string arn)
        {
            var tagResponse = _client
                .ListTagsForResourceAsync(new ListTagsForResourceRequest() { ResourceArn = arn })
                .Result;

            var monkeyOn = tagResponse.ResourceTags.Any(tag => tag.Key == "FinanceMonkey" && tag.Value == "On");
            var monkeyAlreadyActive = tagResponse.ResourceTags.Any(tag => tag.Key == "FinanceMonkeyInProgress");

            return !monkeyAlreadyActive && monkeyOn;

        }
    }
}
