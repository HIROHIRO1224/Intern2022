using Constructs;
using Amazon.CDK.AWS.Lambda;

namespace Intern202201AwsCdk
{
    public class MyFunction
    {
        public Function function;
        public MyFunction(Construct construct, string name, string handler, string code)
        {
            function = new Function(construct, name, new FunctionProps
            {
                Runtime = Runtime.DOTNET_6,
                Code = Code.FromAsset(code),
                Handler = handler
            });
        }
    }
}
