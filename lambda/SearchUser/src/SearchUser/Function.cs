using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using System.Text;
using System.Data;
using Newtonsoft.Json;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SearchUser;

public class Function
{
    /// <summary>
    /// RDSのエンドポイント
    /// </summary>
    const string server = "yoshikawa-test-instance-1.ctl8p031cpd0.ap-northeast-1.rds.amazonaws.com";

    /// <summary>
    /// データベース名
    /// </summary>
    const string database = "yoshikawa_db";

    /// <summary>
    /// ユーザー名
    /// </summary>
    const string user = "admin";
    
    /// <summary>
    /// パスワード
    /// </summary>
    const string password = "dp3245TNT";
    // 文字コード
    const string charset = "utf8";

    StringBuilder sql = new StringBuilder(100);
    MySqlConnection? con = null;
    MySqlCommand? command = null;
    private APIGatewayProxyResponse? response = null;
    private Dictionary<string, string> headers = new Dictionary<string, string>(5);
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        headers.Clear();
        sql.Clear();
        
        response = new APIGatewayProxyResponse();
        sql.Append($"select id,name from m_users where image_name = '{request.Body}';");

        headers.Add("Access-Control-Allow-Origin", "*");
        headers.Add("Access-Control-Allow-Headers", "Content-Type");
        headers.Add("Access-Control-Allow-Methods", "POST,OPTIONS");
        response.Headers = headers;

        con = new MySqlConnection($"Server={server};Database={database};Uid={user};Pwd={password}");
        try
        {
            con.Open();
            // response.Body = "接続できますた";
            command = new MySqlCommand(sql.ToString(), con);
            var reader = command.ExecuteReader();
            reader.Read();
            MyResponseBody responseBody=new MyResponseBody(reader.GetInt32("id"),reader.GetString("name"));
            response.Body = JsonConvert.SerializeObject(responseBody).ToString();

            response.StatusCode = (int)HttpStatusCode.OK;
            con.Close();

        }
        catch (MySqlException)
        {
            response.Body = "";
            response.StatusCode = (int)HttpStatusCode.BadGateway;
            con.Close();

        }
        catch (SystemException e)
        {
            response.Body = $"致命的な失敗をしますた:{e.Message}";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            con.Close();

        }
        finally
        {
            con.Dispose();
        }

        return response;
    }

}
