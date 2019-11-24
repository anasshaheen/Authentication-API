using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace ApiAuth.API.ViewModels
{
    public partial class GoogleUserData
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("profile")]
        public string Profile { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public string EmailVerified { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("hd")]
        public string Hd { get; set; }
    }

    internal class GoogleAppAccessToken
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("id_token")]
        public string TokenId { get; set; }
        [JsonProperty("expires_in")]
        public string Expires { get; set; }


    }
    public partial class GoogleUserData
    {
        public static GoogleUserData FromJson(string json) => JsonConvert.DeserializeObject<GoogleUserData>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GoogleUserData self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}