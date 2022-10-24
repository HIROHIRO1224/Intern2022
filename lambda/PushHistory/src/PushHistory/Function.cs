using System.ComponentModel;
using System.Net;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using MySql.Data.MySqlClient;
using System.Text;
using TimeZoneConverter;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PushHistory;

public class Function
{
    /// <summary>
    /// データベースのエンドポイント名
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
    /// <summary>
    /// 文字コード
    /// </summary>
    const string charset = "utf8";


    private APIGatewayProxyResponse? responce = null;
    private StringBuilder sql = new StringBuilder(100);
    private Dictionary<string, string> headers = new Dictionary<string, string>(3);


    MySqlConnection? con = null;
    MySqlCommand? command = null;

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
        responce = new APIGatewayProxyResponse();

        string id = Guid.NewGuid().ToString();

        string[] req = request.Body.Split(",");
        THistory tHistory = new THistory(id, int.Parse(req[0]), int.Parse(req[1]), req[2]);
        sql.Append($"insert into t_histories (id,user_id,room_id,in_or_out,stamp_date) values (@id,@userId,@roomId,@inOrOut,@stampDate);");

        headers.Add("Access-Control-Allow-Origin", "*");
        headers.Add("Access-Control-Allow-Headers", "Content-Type");
        headers.Add("Access-Control-Allow-Methods", "POST,OPTIONS");

        responce.Headers = headers;

        con = new MySqlConnection($"Server={server};Database={database};Uid={user};Pwd={password}");
        try
        {
            TimeZoneInfo TIMEZONE_JST =TZConvert.GetTimeZoneInfo("Asia/Tokyo");

            DateTime jst = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TIMEZONE_JST);
            // DateTime jst = DateTime.UtcNow;
            con.Open();

            command = new MySqlCommand();
            command.CommandText = sql.ToString();
            command.Connection = con;

            command.Parameters.AddWithValue("@id", tHistory.Id);
            command.Parameters.AddWithValue("@userId", tHistory.UserId);
            command.Parameters.AddWithValue("@roomId", tHistory.RoomId);
            command.Parameters.AddWithValue("@inOrOut", tHistory.InOrOut);
            command.Parameters.AddWithValue("@stampDate", jst);
            int result = command.ExecuteNonQuery();
            if (result < 1)
            {
                responce.Body = "追加できませんでした";
                responce.StatusCode = (int)HttpStatusCode.NoContent;
            }
            else
            {
                responce.Body = "追加完了";
                responce.StatusCode = (int)HttpStatusCode.OK;
            }

            con.Close();
        }
        catch (MySqlException e)
        {
            responce.Body = $"失敗しますた:{e.Message},requestCount:{req.Length}";
            responce.StatusCode = (int)HttpStatusCode.BadRequest;
            con.Close();
        }
        catch (Exception e)
        {
            responce.Body = $"よくわからない失敗しました{e.Message}";
            responce.StatusCode = (int)HttpStatusCode.BadGateway;
        }
        finally
        {
            con.Dispose();
        }

        return responce;
    }
}
