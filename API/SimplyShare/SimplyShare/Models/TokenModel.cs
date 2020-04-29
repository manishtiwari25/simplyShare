﻿using Newtonsoft.Json;

namespace SimplyShare.Models
{
    [JsonObject]
    public class TokenModel
    {
        [JsonProperty("aud")]
        public string Aud { get; set; }

        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("exp")]
        public string Exp { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public string EmailVerified { get; set; }

        [JsonProperty("access_type")]
        public string AccessType { get; set; }
    }
}
