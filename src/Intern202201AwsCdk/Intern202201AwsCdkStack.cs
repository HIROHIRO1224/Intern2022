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
            api.Root.AddCorsPreflight(new CorsOptions(){AllowOrigins=new string[1]{"*"} });
            var faceDetectMethod = api.Root.AddResource("faceSearch");
            faceDetectMethod.AddMethod("OPTIONS", new MockIntegration());
            faceDetectMethod.AddMethod("POST", new LambdaIntegration(searchFace));
        }
    }
}
