using System;
using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.Lambda;


namespace Intern202201AwsCdk
{
    public class SearchFaceFunction
    {
        public Function function;
        const string code = "./lambda/HelloHandler/src/HelloHandler/bin/Debug/net6.0/publish";
        const string handler = "HelloHandler::HelloHandler.Function::FunctionHandler";
        public SearchFaceFunction(Construct construct)
        {
            this.function=new MyFunction(construct,"SearchFace",handler,code).function;
        }
    }
}
