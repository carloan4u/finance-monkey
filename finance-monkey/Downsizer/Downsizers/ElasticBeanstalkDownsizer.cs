using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.ElasticBeanstalk;
using Amazon.ElasticBeanstalk.Model;
using FinanceMonkey.ServiceAnalyser.ServiceTypes;

namespace FinanceMonkey.Downsizer.Downsizers
{
    class ElasticBeanstalkDownsizer
    {
        private readonly AmazonElasticBeanstalkClient _client;

        private const string DownsizedTemplateName = "finance-monkey-downsized";
        private const string BackupTemplateName = "finance-monkey-original-backup";

        public ElasticBeanstalkDownsizer()
        {
            _client = new AmazonElasticBeanstalkClient();
        }

        public bool Downsize(ElasticBeanstalkService service)
        {
            Task.WaitAll(
                BackupCurrentConfig(service),
                CreateDownsizedConfig(service)
            );
            ApplyConfiguration(service.ServiceName).Wait();
            return true;
        }

        private async Task BackupCurrentConfig(ElasticBeanstalkService service)
        {
            await ForceCreateNewTemplate(service.ApplicationName, service.EnvironmentId, BackupTemplateName,
                "A backup of the original deployed configuration for when the finance monkey is finished with this service",
                service.InstanceType.ToString());
        }

        private async Task CreateDownsizedConfig(ElasticBeanstalkService service)
        {

            await ForceCreateNewTemplate(service.ApplicationName, service.EnvironmentId, DownsizedTemplateName,
                "A copy of the originally deployed config but with a smaller instance type",
                service.InstanceType.Downsize().ToString());
        }

        private async Task ForceCreateNewTemplate(string applicationName, string environmentId, string templateName,
            string description, string instanceType)
        {
            _client.DeleteConfigurationTemplateAsync(new DeleteConfigurationTemplateRequest
            {
                ApplicationName = applicationName,
                TemplateName = templateName
            }).Wait();

            await _client.CreateConfigurationTemplateAsync(new CreateConfigurationTemplateRequest
            {
                ApplicationName = applicationName,
                EnvironmentId = environmentId,
                TemplateName = templateName,
                Description = description,
                OptionSettings = new List<ConfigurationOptionSetting>
                {
                    new ConfigurationOptionSetting("aws:autoscaling:launchconfiguration", "InstanceType", instanceType)
                }
            });
        }

        private async Task ApplyConfiguration(string environmentName)
        {
            await _client.UpdateEnvironmentAsync(new UpdateEnvironmentRequest
            {
                EnvironmentName = environmentName,
                TemplateName = DownsizedTemplateName
            });
        }
    }
}
