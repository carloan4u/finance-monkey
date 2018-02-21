using FinanceMonkey.Downsizer;

namespace FinanceMonkey.ServiceAnalyser.ServiceTypes
{
    abstract class BaseService
    {
        public string ServiceName { get; set; }
        public InstanceSize InstanceType { get; set; }
        public bool AppropriateToDownsize { get; set; }

        public abstract bool Downsize();

    }
}
