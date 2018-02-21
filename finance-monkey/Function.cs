using Amazon.Lambda.Core;
using FinanceMonkey.ServiceAnalyser;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace FinanceMonkey
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(string input, ILambdaContext context)
        {
            var services = new AwsServiceAnalyser().GetApplicableServices();
            foreach (var service in services)
            {
                service.Downsize();
            }
            return input?.ToUpper();
        }
    }
}
