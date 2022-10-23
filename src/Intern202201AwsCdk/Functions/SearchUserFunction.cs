using Constructs;
using Amazon.CDK.AWS.Lambda;


namespace Intern202201AwsCdk
{
    public class SearchUserFunction
    {
        public Function function;
        const string code = "./lambda/SearchUser/src/SearchUser/bin/Debug/net6.0/publish";
        const string handler = "SearchUser::SearchUser.Function::FunctionHandler";
        public SearchUserFunction(Construct construct)
        {
            this.function=new MyFunction(construct,"SearchUser",handler,code).function;
        }
    }
}
