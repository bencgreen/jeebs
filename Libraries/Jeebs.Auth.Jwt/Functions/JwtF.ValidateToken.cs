﻿// Copyright (c) bcg|design.
// Licensed under https://mit.bcgdesign.com/2013.

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Jeebs;
using Jeebs.Auth;
using Jeebs.Config;
using Jm.Functions.JwtF.ValidateToken;
using Microsoft.IdentityModel.Tokens;

namespace F
{
	public static partial class JwtF
	{
		/// <summary>
		/// Validate token
		/// </summary>
		/// <param name="config">JwtConfig</param>
		/// <param name="token">Token value</param>
		public static Option<ClaimsPrincipal> ValidateToken(JwtConfig config, string token)
		{
			try
			{
				// Create validation parameters
				var parameters = new TokenValidationParameters
				{
					RequireExpirationTime = true,
					ValidIssuer = config.Issuer,
					ValidAudience = config.Audience ?? config.Issuer,
					IssuerSigningKey = config.GetSigningKey()
				};

				if (config.GetEncryptingKey() is Some<SecurityKey> encryptingKey)
				{
					parameters.TokenDecryptionKey = encryptingKey.Value;
				}

				// Create handler to validate token
				var handler = new JwtSecurityTokenHandler();
				var principal = handler.ValidateToken(token, parameters, out var validatedToken);

				// Check date values
				if (validatedToken.ValidTo < DateTime.Now)
				{
					return Option.None<ClaimsPrincipal>().AddReason<TokenHasExpiredMsg>();
				}

				// Return valid principal
				return principal;
			}
			catch (SecurityTokenNotYetValidException)
			{
				return Option.None<ClaimsPrincipal>().AddReason<TokenIsNotValidYetMsg>();
			}
			catch (Exception e)
			{
				return Option.None<ClaimsPrincipal>().AddReason<ErrorValidatingTokenMsg>(e);
			}
		}
	}
}
