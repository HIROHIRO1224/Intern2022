using System.Collections.Generic;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System.Net;
using HelloHandler.Model;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HelloHandler;

public class Function
{

    private AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(Amazon.RegionEndpoint.APNortheast1);
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            this.rekognitionClient = new AmazonRekognitionClient();
            Image imageSource = new Image();
            string[] body = request.Body.Split("base64,");
            if (body.Length < 2) return new APIGatewayProxyResponse() { StatusCode = (int)HttpStatusCode.OK, Body = "Bad Request" };
            string image = body[1];
            var decodedImage = Convert.FromBase64String(image);
            SearchFacesByImageResponse result = await SearchFace(decodedImage, "yoshikawa-hiroto");
            APIGatewayProxyResponse responce = new APIGatewayProxyResponse();

            List<SearchFaceResponce> responceBody = new List<SearchFaceResponce>(5);

            foreach (var item in result.FaceMatches)
            {
                if (item.Face.Confidence >= 0.9)
                {
                    SearchFaceResponce obj = new SearchFaceResponce();

                    obj.Name.Append(item.Face.ExternalImageId);
                    obj.Confidence = item.Face.Confidence;
                    responceBody.Add(obj);
                }
            }
            string responceBodyJson = JsonConvert.SerializeObject(responceBody);

            responce.Body = responceBodyJson;
            var headers = new Dictionary<string, string>();
            headers.Add("Access-Control-Allow-Origin", "*");
            responce.Headers = headers;

            return responce;


        }
        catch (System.Exception e)
        {
            throw e;
        }
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
