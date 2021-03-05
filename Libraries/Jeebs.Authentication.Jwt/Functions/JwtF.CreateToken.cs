﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Jeebs;
using Jeebs.Authentication;
using Defaults = Jeebs.Authentication.Defaults;
using Jeebs.Config;
using Jm.Functions.JwtF.CreateToken;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;

namespace F
{
	/// <summary>
	/// JSON Web Tokens function
	/// </summary>
	public static partial class JwtF
	{
		/// <inheritdoc cref="CreateToken(JwtConfig, IPrincipal, string, string)"/>
		public static Option<string> CreateToken(JwtConfig config, IPrincipal principal) =>
			CreateToken(config, principal, Defaults.Algorithms.Signing, Defaults.Algorithms.Encrypting);

		/// <inheritdoc cref="CreateToken(JwtConfig, IPrincipal, string, string)"/>
		public static Option<string> CreateToken(JwtConfig config, IPrincipal principal, string signingAlgorithm) =>
			CreateToken(config, principal, signingAlgorithm, Defaults.Algorithms.Encrypting);

		/// <summary>
		/// <para>Generate a new JSON Web Token for the specified user</para>
		/// <para>See <see cref="Defaults.Algorithms"/> for default signing and encrypting algorithms</para>
		/// </summary>
		/// <param name="config">JwtConfig</param>
		/// <param name="principal">IPrincipal</param>
		/// <param name="signingAlgorithm">Signing algorithm</param>
		/// <param name="encryptingAlgorithm">Encrypting algorithm</param>
		public static Option<string> CreateToken(JwtConfig config, IPrincipal principal, string signingAlgorithm, string encryptingAlgorithm) =>
			CreateToken(
				config,
				principal,
				signingAlgorithm,
				encryptingAlgorithm,
				DateTime.UtcNow,
				DateTime.UtcNow.AddHours(config.ValidForHours)
		);

		/// <summary>
		/// <para>Generate a new JSON Web Token for the specified user</para>
		/// <para>See <see cref="Defaults.Algorithms"/> for default signing and encrypting algorithms</para>
		/// </summary>
		/// <param name="config">JwtConfig</param>
		/// <param name="principal">IPrincipal</param>
		/// <param name="signingAlgorithm">Signing algorithm</param>
		/// <param name="encryptingAlgorithm">Encrypting algorithm</param>
		internal static Option<string> CreateToken(
			JwtConfig config,
			IPrincipal principal,
			string signingAlgorithm,
			string encryptingAlgorithm,
			DateTime notBefore,
			DateTime expires
		)
		{
			// Ensure there is a current user
			if (principal.Identity == null)
			{
				return Option.None<string>().AddReason<NullIdentityMsg>();
			}

			var identity = principal.Identity;

			// Ensure the current user is authenticated
			if (!identity.IsAuthenticated)
			{
				return Option.None<string>().AddReason<IdentityNotAuthenticatedMsg>();
			}

			// Ensure the JwtConfig is valid
			if (!config.IsValid)
			{
				return Option.None<string>().AddReason<JwtConfigInvalidMsg>();
			}

			try
			{
				// Create token values
				var descriptor = new SecurityTokenDescriptor
				{
					Issuer = config.Issuer,
					Audience = config.Audience ?? config.Issuer,
					Subject = new ClaimsIdentity(identity),
					NotBefore = notBefore,
					Expires = expires,
					IssuedAt = DateTime.UtcNow,
					SigningCredentials = new SigningCredentials(config.GetSigningKey(), signingAlgorithm)
				};

				if (config.GetEncryptingKey() is Some<SecurityKey> encryptingKey)
				{
					descriptor.EncryptingCredentials = new EncryptingCredentials(
						encryptingKey.Value,
						JwtConstants.DirectKeyUseAlg,
						encryptingAlgorithm
					);
				}

				// Create handler to create and write token
				var handler = new JwtSecurityTokenHandler();
				var token = handler.CreateJwtSecurityToken(descriptor);
				return handler.WriteToken(token);
			}
			catch (ArgumentException e) when (e.Message.Contains("IDX10703"))
			{
				return Option.None<string>().AddReason<SigningKeyNotLongEnoughMsg>();
			}
			catch (ArgumentOutOfRangeException e) when (e.Message.Contains("IDX10653"))
			{
				return Option.None<string>().AddReason<EncryptingKeyNotLongEnoughMsg>();
			}
			catch (Exception e)
			{
				return Option.None<string>().AddReason<ErrorCreatingJwtSecurityTokenMsg>(e);
			}
		}
	}
}
