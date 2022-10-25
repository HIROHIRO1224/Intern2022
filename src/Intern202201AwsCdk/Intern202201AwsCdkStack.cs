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
            Function searchFace = new SearchFaceFunction(this).function;
            Function searchUser = new SearchUserFunction(this).function;
            Function pushHistory = new PushHistoryFunction(this).function;
            Function showHistory = new ShowHistoryFunction(this).function;
            Function showUserInRoom = new ShowUserInRoomFunction(this).function;
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

                }
            );
            faceSearchMethod.AddMethod("POST", new LambdaIntegration(searchFace));

            var InRoomResource = api.Root.AddResource("InRoom");
            InRoomResource.AddCorsPreflight(
                new CorsOptions
                {
                    AllowHeaders = new string[1] { "Content-Type" },
                    AllowMethods = new string[2] { "OPTIONS", "POST" },
                    AllowOrigins = new string[1] { "*" },
                }
            );
            InRoomResource.AddMethod("POST", new LambdaIntegration(showUserInRoom));

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
            userFindResource.AddMethod("POST", new LambdaIntegration(searchUser));



            var historyResourse = api.Root.AddResource("history");
            var historyPushResource = historyResourse.AddResource("push");
            historyPushResource.AddCorsPreflight(new CorsOptions
            {
                AllowHeaders = new string[1] { "Content-Type" },
                AllowMethods = new string[2] { "OPTIONS", "POST" },
                AllowOrigins = new string[1] { "*" },
            });
            historyPushResource.AddMethod("POST", new LambdaIntegration(pushHistory));
            var historyShowResource = historyResourse.AddResource("show");
            historyShowResource.AddCorsPreflight(new CorsOptions
            {
                AllowHeaders = new string[1] { "Content-Type" },
                AllowMethods = new string[2] { "OPTIONS", "POST" },
                AllowOrigins = new string[1] { "*" },

            });
            historyShowResource.AddMethod("POST", new LambdaIntegration(showHistory));
        }
    }
}
