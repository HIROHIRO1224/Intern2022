using System.Collections.Generic;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System.Net;
using HelloHandler.Model;
using System.Text;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HelloHandler;

public class Function
{

    private AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(Amazon.RegionEndpoint.APNortheast1);
    private APIGatewayProxyResponse? response = null;
    private Dictionary<string, string> headers = new Dictionary<string, string>();
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        headers.Clear();
        response = new APIGatewayProxyResponse();
        headers.Add("Access-Control-Allow-Origin", "*");
        headers.Add("Access-Control-Allow-Headers", "Content-Type");
        headers.Add("Access-Control-Allow-Methods", "POST,OPTIONS");
        response.Headers = headers;

        try
        {
            this.rekognitionClient = new AmazonRekognitionClient();
            Image imageSource = new Image();
            string[] body = request.Body.Split("base64,");
            if (body.Length < 2) return new APIGatewayProxyResponse() { StatusCode = (int)HttpStatusCode.OK, Body = "Bad Request" };
            string image = body[1];
            var decodedImage = Convert.FromBase64String(image);
            SearchFacesByImageResponse result = await SearchFace(decodedImage, "yoshikawa-hiroto");

            List<SearchFaceResponce> responceBody = new List<SearchFaceResponce>(5);
            if (result.FaceMatches.Count > 0)
            {
                foreach (var item in result.FaceMatches)
                {
                    if (item.Face.Confidence >= 0.9)
                    {
                        SearchFaceResponce obj = new SearchFaceResponce();

                        obj.Name = item.Face.ExternalImageId;
                        obj.Confidence = item.Face.Confidence;
                        responceBody.Add(obj);
                    }
                    else
                    {
                        break;
                    }
                }
                string responceBodyJson = JsonConvert.SerializeObject(responceBody);

                response.Body = responceBodyJson;

                response.StatusCode = (int)HttpStatusCode.OK;

            }
            else
            {
                response.Body = "none";
                response.StatusCode = (int)HttpStatusCode.OK;
            }

        }
        catch (Exception e)
        {
            response.Body = e.Message;
            response.StatusCode = 403;
        }

        return response;
    }
    /// <summary>
    /// collectionIdを用いて該当する人物を検索して結果を返す
    /// </summary>
    /// <param name="bytes">デコードした画像</param>
    /// <param name="collectionId">コレクション名</param>
    /// <returns>顔検索の結果</returns>
    public async Task<SearchFacesByImageResponse> SearchFace(byte[] bytes, string CollectionId)
    {
        try
        {
            Image image = new Image();
            image.Bytes = new MemoryStream(bytes);

            var searchResponce = await this.rekognitionClient.SearchFacesByImageAsync(new SearchFacesByImageRequest { CollectionId = "user-collection", Image = image });
            return searchResponce;
        }
        catch (System.Exception e)
        {
            throw e;
        }
    }
}
