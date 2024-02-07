using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Domain.ViewModels.Account
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationMinutes { get; set; }
        public int RefreshTokenLength { get; set; }

        public JwtSettings(string secret, string issuer, string audience, int expirationMinutes, int refreshTokenLength)
        {
            Secret = secret;
            Issuer = issuer;
            Audience = audience;
            ExpirationMinutes = expirationMinutes;
            RefreshTokenLength = refreshTokenLength;
        }

        public JwtSettings()
        {

        }
    }
}
