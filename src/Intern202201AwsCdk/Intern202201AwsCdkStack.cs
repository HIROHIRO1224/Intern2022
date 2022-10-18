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
            var searchFace = new Function(this, "HelloHandler", new FunctionProps
            {
                Runtime = Runtime.DOTNET_6,
                Code = Code.FromAsset("./lambda/HelloHandler/src/HelloHandler/bin/Debug/net6.0/publish"),
                Handler = "HelloHandler::HelloHandler.Function::FunctionHandler",
            });

            var api = new RestApi(this, "api");
            var faceDetectMethod = api.Root.AddResource("faceSearch");
            faceDetectMethod.AddCorsPreflight(
                new CorsOptions{
                    AllowHeaders=new string[1]{"Content-Type"},
                    AllowMethods=new string[2]{"OPTIONS","POST"},
                    AllowOrigins=new string[1]{"*"},
            });
            faceDetectMethod.AddMethod("POST", new LambdaIntegration(searchFace));
        }
    }
}
