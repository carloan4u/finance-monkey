using System.Collections.Generic;

namespace FinanceMonkey.ServiceAnalyser.Analysers
{
    interface IServiceAnalyser<out TService>
    {
        IEnumerable<TService> GetServices();
    }
}
