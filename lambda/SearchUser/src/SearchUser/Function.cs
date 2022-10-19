using Amazon.Lambda.Core;
using Mysql.Data.MysqlClient;
using System.Collections.Generic;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SearchUser;

public class Function
{
    ///RDSのエンドポイント
    const string server="";
    // データベース名
    const string database="";
    // ユーザー名
    const string user="";
    // パスワード
    const string password="";
    // 文字コード
    const string charset="utf8";
    

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
    {
        return new APIGatewayProxyResponse();
    }
}
