﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs
{
	public partial class StringExtensions_Tests
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void GetMimeFromExtension_NullOrEmpty_ReturnsOriginal(string input)
		{
			// Arrange

			// Act
			var result = input.GetMimeFromExtension();

			// Assert
			Assert.Equal(input, result);
		}

		[Theory]
		[MemberData(nameof(GetMimeTypeData))]
		public void GetMimeFromExtension_String_ReturnsMimeType(string input, MimeType expected)
		{
			// Arrange

			// Act
			var result = input.GetMimeFromExtension();

			// Assert
			Assert.Equal(expected.ToString(), result);
		}

		public static IEnumerable<object[]> GetMimeTypeData()
		{
			yield return new object[] { "file.xxx", MimeType.General };
			yield return new object[] { "file.bmp", MimeType.Bmp };
			yield return new object[] { "file.doc", MimeType.Doc };
			yield return new object[] { "file.docx", MimeType.Docx };
			yield return new object[] { "file.gif", MimeType.Gif };
			yield return new object[] { "file.jpg", MimeType.Jpg };
			yield return new object[] { "file.jpeg", MimeType.Jpg };
			yield return new object[] { "file.m4a", MimeType.M4a };
			yield return new object[] { "file.mp3", MimeType.Mp3 };
			yield return new object[] { "file.pdf", MimeType.Pdf };
			yield return new object[] { "file.png", MimeType.Png };
			yield return new object[] { "file.ppt", MimeType.Ppt };
			yield return new object[] { "file.pptx", MimeType.Pptx };
			yield return new object[] { "file.tar", MimeType.Tar };
			yield return new object[] { "file.xls", MimeType.Xls };
			yield return new object[] { "file.xlsx", MimeType.Xlsx };
			yield return new object[] { "file.zip", MimeType.Zip };
		}
	}
}
