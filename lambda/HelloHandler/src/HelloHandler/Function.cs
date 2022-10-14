using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HelloHandler;

public class Function
{
    
    /// <summary>N
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public APIGatewayProxyResponce FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var res=new APIGatewayProxyResponce();
        res.Body="Hello World!";
        res.StatusCode=(int)HttpStatusCode.OK;
        return res;
    }
}
