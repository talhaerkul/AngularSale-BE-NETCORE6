using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.DTOs.Facebook
{
    public class FacebookValidationDTO
    {
        [JsonPropertyName("data")]
        public FacebookValidationDataDTO Data { get; set; }
    }
    public class FacebookValidationDataDTO
    {
        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }
}
