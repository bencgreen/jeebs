﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Jeebs.Auth;
using Jeebs.Auth.Entities;
using Jeebs.Mvc.Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Jeebs.Mvc.Auth.Controllers
{
	/// <summary>
	/// Implement this controller to add support for user authentication
	/// </summary>
	/// <typeparam name="TUser">User entity type</typeparam>
	public abstract class AuthController<TUser> : Controller
		where TUser : UserEntity
	{
		protected IDataAuthenticationProvider Auth { get; init; }

		/// <summary>
		/// Add application-specific claims to an authenticated user
		/// </summary>
		protected virtual Func<TUser, List<Claim>>? AddClaims { get; }

		protected AuthController(IDataAuthenticationProvider auth, ILog log) : base(log) =>
			Auth = auth;

		/// <summary>
		/// Display sign in page
		/// </summary>
		/// <param name="returnUrl">[Optional] Return URL</param>
		public IActionResult SignIn(string? returnUrl) =>
			View(new SignInModel(returnUrl));

		/// <summary>
		/// Perform sign in
		/// </summary>
		/// <param name="model">SignInModel</param>
		[HttpPost, AutoValidateAntiforgeryToken]
		public virtual async Task<IActionResult> SignIn(SignInModel model) =>
			(await Auth.ValidateUserAsync<TUser>(model.Email, model.Password)).Match(
				some: user =>
				{
					// Get user principal
					Log.Trace("User signed in successfully.");
					var principal = GetPrincipal(user);

					// Add SignIn to HttpContext using Cookie scheme
					return SignIn(
						principal,
						new AuthenticationProperties
						{
							ExpiresUtc = DateTime.UtcNow.AddDays(28),
							IsPersistent = model.RememberMe,
							AllowRefresh = false,
							RedirectUri = model.ReturnUrl
						},
						CookieAuthenticationDefaults.AuthenticationScheme
					);
				},
				none: () =>
				{
					// Log error and add alert for user
					Log.Trace("Unknown username or password: {Email}.", model.Email);
					TempData.AddErrorAlert("Unknown username or password.");

					// Return to sign in page
					return SignIn(model.ReturnUrl);
				}
			);

		/// <summary>
		/// Get principal for specified user with all necessary claims
		/// </summary>
		/// <param name="user">User entity</param>
		private ClaimsPrincipal GetPrincipal(TUser user)
		{
			// Create claims object
			var claims = new List<Claim>
			{
				new (JwtClaimTypes.UserId, user.Id.ValueStr, ClaimValueTypes.Integer32),
				new (ClaimTypes.GivenName, user.GivenName, ClaimValueTypes.String),
				new (ClaimTypes.Name, user.FullName, ClaimValueTypes.String),
				new (ClaimTypes.Email, user.EmailAddress, ClaimValueTypes.Email),
			};

			if (user.IsSuper)
			{
				claims.Add(new(JwtClaimTypes.IsSuper, true.ToString(), ClaimValueTypes.Boolean));
			}

			// Add custom Claims
			if (AddClaims != null)
			{
				claims.AddRange(AddClaims(user));
			}

			// Create user objects
			var userIdentity = new ClaimsIdentity(claims, "SecureSignIn");
			return new ClaimsPrincipal(userIdentity);
		}

		/// <summary>
		/// Perform sign out
		/// </summary>
		public override SignOutResult SignOut()
		{
			// Show a friendly message to the user
			TempData.AddInfoAlert("Goodbye!");

			// Build redirect URL and return SignOut view
			var redirectTo = Url.Action(nameof(SignIn), new { ReturnUrl = Request.Query["ReturnUrl"] };
			return SignOut(new AuthenticationProperties { RedirectUri = redirectTo });
		}
	}
}
