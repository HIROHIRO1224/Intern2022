
using Constructs;
using Amazon.CDK.AWS.Lambda;


namespace Intern202201AwsCdk
{
    public class PushHistoryFunction
    {
        public Function function;
        const string code = "./lambda/PushHistory/src/PushHistory/bin/Debug/net6.0/publish";
        const string handler = "PushHistory::PushHistory.Function::FunctionHandler";
        public PushHistoryFunction(Construct construct)
        {
            this.function=new MyFunction(construct,"pushHistory",handler,code).function;
        }
    }
}
