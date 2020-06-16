﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jeebs;
using Xunit;

namespace Tests.Jeebs.Result.Audit
{
	public class Audit0_Simple
	{
		[Fact]
		public void Audit_Returns_Original_Object()
		{
			// Arrange
			var chain = R.Chain;
			static void audit<T>(R<T> _) { }

			// Act
			var result = chain.Audit(audit);

			// Assert
			Assert.StrictEqual(chain, result);
		}

		[Fact]
		public void Successful_Audit_Writes_To_Log()
		{
			// Arrange
			var chain = R.Chain;

			static R<int> l0<T>(Ok<T> r) => r.OkV(18);
			static R<T> l1<T>(OkV<T> r) => r.Error();

			var log = new List<string>();
			void audit<T>(R<T> r)
			{
				if (r is Error<T> error)
				{
					log.Add("Error!");
				}
				else if (r is OkV<T> ok)
				{
					log.Add($"Value: {ok.Val}");
				}
				else
				{
					log.Add("Unknown state.");
				}
			}

			var expected = new[] { "Unknown state.", "Value: 18", "Error!" }.ToList();

			// Act
			chain
				.Audit(audit)
				.LinkMap(l0)
				.Audit(audit)
				.LinkMap(l1)
				.Audit(audit);

			// Assert
			Assert.Equal(expected, log);
		}

		[Fact]
		public void Unsuccessful_Audit_Captures_Exception()
		{
			// Arrange
			var chain = R.Chain;
			static void audit<T>(R<T> _) => throw new Exception();

			// Act
			var result = chain.Audit(audit);

			// Assert
			Assert.Equal(1, result.Messages.Count);
			Assert.True(result.Messages.Contains<Jm.AuditException>());
		}
	}
}
