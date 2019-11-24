using ApiAuth.API.Infrastructure.Abstraction.IServices;
using ApiAuth.API.Infrastructure.Constants;
using ApiAuth.API.Infrastructure.Options;
using ApiAuth.API.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Services
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<AccessTokenViewModel> GenerateEncodedToken(string userName, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name, userName),
                 new Claim(ClaimTypes.NameIdentifier, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
             };
            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtClaimIdentifiers.Roles, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = _jwtOptions.Expiration,
                SigningCredentials = _jwtOptions.SigningCredentials,
                NotBefore = _jwtOptions.NotBefore,
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                IssuedAt = _jwtOptions.IssuedAt
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AccessTokenViewModel(tokenHandler.WriteToken(token), _jwtOptions.ValidFor.TotalSeconds);
        }

        #region Helpers

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() -
                                 new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        #endregion
    }
}
