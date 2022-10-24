using Amazon.CDK.AWS.Lambda;
using Constructs;
namespace Intern202201AwsCdk
{
    class ShowHistoryFunction
    {
        public Function function{get;}

        const string code = "./lambda/ShowHistory/src/ShowHistory/bin/Debug/net6.0/publish";
        const string handler = "ShowHistory::ShowHistory.Function::FunctionHandler";
        

        public ShowHistoryFunction(Construct construct){
            function=new MyFunction(construct,"showHistory",handler,code).function;
        }
    }
}