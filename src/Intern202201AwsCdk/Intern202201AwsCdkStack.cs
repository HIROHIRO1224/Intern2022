using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.APIGateway;
using Constructs;

namespace Intern202201AwsCdk
{
    public class Intern202201AwsCdkStack : Stack
    {
        internal Intern202201AwsCdkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var hello =new Function(this,"HelloHandler",new FunctionProps
            {
                Runtime=Runtime.DOTNET_6,
                Code=Code.FromAsset("./lambda/HelloHandler/src/HelloHandler/bin/Debug/net6.0/publish"),
                Handler="HelloHandler::HelloHandler.Function::FunctionHandler"
            });
            
            new LambdaRestApi(this,"Endpoint",new LambdaRestApiProps{Handler = hello}){};
        }
    }
}
