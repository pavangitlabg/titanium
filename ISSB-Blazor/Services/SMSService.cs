using System.Net.Http.Headers;
using System.Text;
using Data.Models;
using Newtonsoft.Json;

//using Twilio;
//using Twilio.Rest.Api.V2010.Account;


namespace Services;

public class SMSService
{


    public async Task<ResponseModel> SendSMS(SMSRequestModel requestModel)
    {
        try
        {
            var base_url = "https://rest.clicksend.com/v3/sms/send";
            var user = "";
            var token = "";

            var jsonObject = JsonConvert.SerializeObject(requestModel);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var authenticationString = $"{user}:{token}";
            var base64EncodedAuthenticationString =
                Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, base_url);
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = content;

            //make the request
            var task = client.SendAsync(requestMessage);
            var response = task.Result;
            var json = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<ResponseModel>(json);
            return responseModel;
        }
        catch (Exception ex)
        {
            var error = ex.Message;
            var errorModel = new ResponseModel
            {
                response_msg = error
            };
            return errorModel;
        }
    }
}