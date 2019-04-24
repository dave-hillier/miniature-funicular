using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Users.Tests
{
    internal static class HttpClientHelper
    {
        private static AuthenticationHeaderValue GetAuthorizationHeader()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var header = new JwtHeader(signingCredentials);

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var payload = new JwtPayload
            {
                {"sub", "12345"},
                {"iss", "https://authority"},
                {"https://guestline.app/claims/tenant", "Tenant"},
                {"aud", "https://audience"},
                {"iat", (DateTime.UtcNow - epoch).TotalSeconds},
                {"exp", (DateTime.UtcNow.AddMinutes(1) - epoch).TotalSeconds},
                {"scope", "read:users"}
            };

            var token = new JwtSecurityToken(header, payload);
            var tokenHandler = new JwtSecurityTokenHandler();
            var bearer = tokenHandler.WriteToken(token);
            return new AuthenticationHeaderValue("bearer", bearer);
        }

        public static HttpRequestMessage CreateJsonRequest(string url, HttpMethod httpMethod, object payload)
        {
            var json = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            var request = new HttpRequestMessage(httpMethod, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                Headers =
                {
                    Authorization = GetAuthorizationHeader(),
                }
            };
            return request;
        }
    }
}