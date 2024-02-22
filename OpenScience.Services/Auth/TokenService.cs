using Microsoft.IdentityModel.Tokens;
using OpenScience.Data.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenScience.Services.Auth
{
	public class TokenService
	{
		public string CreateLoginToken(
			string username,
			string userId,
			IJwtConfiguration jwtConfiguration,
			params KeyValuePair<string, string>[] additionalClaims
		)
		{
			var claims = new List<Claim> {
				new Claim(JwtRegisteredClaimNames.Sub, username),
				new Claim(JwtRegisteredClaimNames.Jti, userId)
			};

			foreach (var additionalClaim in additionalClaims)
			{
				var claim = new Claim(additionalClaim.Key, additionalClaim.Value);
				claims.Add(claim);
			}

			return GenerateToken(claims.ToArray(), jwtConfiguration);
		}

		public string GenerateToken(
			Claim[] claims,
			IJwtConfiguration jwtConfiguration
			)
		{
			var expires = DateTime.Now.AddHours(jwtConfiguration.ValidHours);

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				jwtConfiguration.Issuer,
				jwtConfiguration.Audience,
				claims,
				expires: expires,
				signingCredentials: creds
			);

			string tokenString = new JwtSecurityTokenHandler()
				.WriteToken(token);

			return tokenString;
		}
	}
}
