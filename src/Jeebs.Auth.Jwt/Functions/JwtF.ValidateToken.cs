﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Jeebs;
using Jeebs.Auth;
using Jeebs.Config;
using Microsoft.IdentityModel.Tokens;
using static F.OptionF;

namespace F
{
	public static partial class JwtF
	{
		/// <summary>
		/// Validate tokens
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

				// Validate token and return principal
				return handler.ValidateToken(token, parameters, out var validatedToken);
			}
			catch (SecurityTokenNotYetValidException)
			{
				return None<ClaimsPrincipal, Msg.TokenIsNotValidYetMsg>();
			}
			catch (Exception e) when (e.Message.Contains("IDX10223"))
			{
				return None<ClaimsPrincipal, Msg.TokenHasExpiredMsg>();
			}
			catch (Exception e)
			{
				return None<ClaimsPrincipal>(new Msg.ValidatingTokenExceptionMsg(e));
			}
		}

		/// <summary>Messages</summary>
		public static partial class Msg
		{
			/// <summary>The token has expired</summary>
			public sealed record TokenHasExpiredMsg : IMsg { }

			/// <summary>The token is not valid yet</summary>
			public sealed record TokenIsNotValidYetMsg : IMsg { }

			/// <summary>Exception while validating token</summary>
			public sealed record ValidatingTokenExceptionMsg(Exception Exception) : ExceptionMsg(Exception) { }
		}
	}
}
