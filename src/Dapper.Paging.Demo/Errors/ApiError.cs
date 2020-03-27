using Newtonsoft.Json;
using System;

namespace Dapper.Razor.Demo.Errors
{
    [Serializable]
    [JsonObject(IsReference = false)]
    public class ApiError
    {
        public ApiError(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("error")]
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
