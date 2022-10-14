using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
namespace Intern202201AwsCdk
{
    public class Intern202201AwsCdkStack : Stack
    {
        internal Intern202201AwsCdkStack(Construct scope, string id, Amazon.CDK.IStackProps props = null) : base(scope, id, props)
        {
            var hello =new Function(this,"HelloHandler",new FunctionProps
            {
                Runtime=Runtime.DOTNET_6,
                Code=Code.FromAsset("./lambda/HelloHandler/src/HelloHandler/bin/Debug/net6.0/publish"),
                Handler="HelloHandler::HelloHandler.Function::FunctionHandler"
            });
        }
    }
}
