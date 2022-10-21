using System.Net;
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
            // 関数のインスタンス
            Function searchFace = new Function(this, "HelloHandler", new FunctionProps
            {
                Runtime = Runtime.DOTNET_6,
                Code = Code.FromAsset("./lambda/HelloHandler/src/HelloHandler/bin/Release/net6.0/publish"),
                Handler = "HelloHandler::HelloHandler.Function::FunctionHandler",
            });
            // Function searchFace=new SearchFaceFunction(this).function;

            Function findUser = new Function(this, "SearchUser", new FunctionProps
            {
                Runtime = Runtime.DOTNET_6,
                Code = Code.FromAsset("./lambda/SearchUser/src/SearchUser/bin/Release/net6.0/publish"),
                Handler = "SearchUser::SearchUser.Function::FunctionHandler",
            });

            Function pushHistory = new Function(this, "PushHistory", new FunctionProps
            {
                Runtime = Runtime.DOTNET_6,
                Code = Code.FromAsset("./lambda/PushHistory/src/PushHistory/bin/Release/net6.0/publish"),
                Handler = "PushHistory::PushHistory.Function::FunctionHandler",
            });


            // APIのインスタンス生成
            var api = new RestApi(this, "EAEMS");

            // searchFaceByImageを実行するLambdaの関数の定義
            var faceSearchMethod = api.Root.AddResource("faceSearch");
            faceSearchMethod.AddCorsPreflight(
                new CorsOptions
                {
                    AllowHeaders = new string[1] { "Content-Type" },
                    AllowMethods = new string[2] { "OPTIONS", "POST" },
                    AllowOrigins = new string[1] { "*" },
                    StatusCode=200
                });
            faceSearchMethod.AddMethod("POST", new LambdaIntegration(searchFace));

            //ユーザーデータを取得したりする関数の定義
            var userResource = api.Root.AddResource("user");
            var userFindResource = userResource.AddResource("find");
            userFindResource.AddCorsPreflight(
                new CorsOptions
                {
                    AllowHeaders = new string[1] { "Content-Type" },
                    AllowMethods = new string[2] { "OPTIONS", "POST" },
                    AllowOrigins = new string[1] { "*" },

                });
            userFindResource.AddMethod("POST", new LambdaIntegration(findUser));


        }
    }
}
