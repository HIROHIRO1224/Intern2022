using System.Net;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using MySql.Data.MySqlClient;
using System.Text;
using System.Data;
using Newtonsoft.Json;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ShowUserInRoom;

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
        sql.Clear();
        headers.Clear();
        response = new APIGatewayProxyResponse();

        headers.Add("Access-Control-Allow-Origin", "*");
        headers.Add("Access-Control-Allow-Headers", "Content-Type");
        headers.Add("Access-Control-Allow-Methods", "POST,OPTIONS");
        response.Headers = headers;

        sql.Append(
            "select m_users.id, m_users.name from t_histories inner join m_users on t_histories.user_id=m_users.id;"
            );
        con = new MySqlConnection($"Server={server};Database={database};Uid={user};Pwd={password}");
        DataTable dataTable = new DataTable();
        try
        {
            con.Open();

            command = new MySqlCommand();
            command.CommandText = sql.ToString();
            command.Connection = con;
            var reader = command.ExecuteReader();
            dataTable.Load(reader);

            response.Body = JsonConvert.SerializeObject(dataTable).ToString();
            response.StatusCode = (int)HttpStatusCode.OK;
            con.Close();

        }
        catch (MySqlException e)
        {
            response.Body = $"Mysql Error:{e}";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        catch (Exception e)
        {
            response.Body = $"Fatal Error:{e}";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        finally
        {
            con.Dispose();
        }

        return response;
    }
}
