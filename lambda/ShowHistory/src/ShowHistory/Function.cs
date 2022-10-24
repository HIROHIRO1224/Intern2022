using System.Data;
using System.Net;
using System.Text;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ShowHistory;

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
            "select t_histories.id,m_users.name,m_rooms.name,t_histories.in_or_out,t_histories.stamp_date from t_histories inner join m_users on t_histories.user_id=m_users.id inner join m_rooms on t_histories.room_id=m_rooms.id order by t_histories.stamp_date desc;"
            );
        con = new MySqlConnection($"Server={server};Database={database};Uid={user};Pwd={password}");
        DataTable dataTable=new DataTable();
        try
        {
            con.Open();

            command=new MySqlCommand();
            command.CommandText=sql.ToString();
            command.Connection=con;
            var reader= command.ExecuteReader();
            dataTable.Load(reader);

            response.Body= JsonConvert.SerializeObject(dataTable).ToString();
            response.StatusCode=(int) HttpStatusCode.OK;
            con.Close();
        }
        catch(MySqlException e)
        {
            response.Body=e.Message;
            response.StatusCode=(int)HttpStatusCode.BadRequest;
        }
        catch(Exception e)
        {
            response.Body=e.Message;
            response.StatusCode=(int)HttpStatusCode.InternalServerError;
        }
        finally
        {
            con.Dispose();
        }

        return response;
    }
}
