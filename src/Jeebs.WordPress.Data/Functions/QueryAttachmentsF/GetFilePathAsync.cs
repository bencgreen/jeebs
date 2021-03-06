﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using System.Threading.Tasks;
using Jeebs;
using Jeebs.Data;
using Jeebs.WordPress.Data;
using Jeebs.WordPress.Data.Entities;

namespace F.WordPressF.DataF
{
	public static partial class QueryAttachmentsF
	{
		/// <summary>
		/// Get the filesystem path to the specified Attachment
		/// </summary>
		/// <param name="db">IWpDb</param>
		/// <param name="w">IUnitOfWork</param>
		/// <param name="fileId">File (Post) ID</param>
		internal static Task<Option<string>> GetFilePathAsync(IWpDb db, IUnitOfWork w, WpPostId fileId)
		{
			return
				ExecuteAsync<Attachment>(
					db, w, opt => opt with { Ids = ImmutableList.Create(fileId) }
				)
				.UnwrapAsync(
					x => x.Single<Attachment>(
						noItems: () => new Msg.AttachmentNotFoundMsg(fileId.Value),
						tooMany: () => new Msg.MultipleAttachmentsFoundMsg(fileId.Value)
					)
				)
				.MapAsync(
					x => x.GetFilePath(db.WpConfig.UploadsPath),
					e => new Msg.ErrorGettingAttachmentFilePathMsg(e, fileId.Value)
				);
		}

		internal record Attachment : WpAttachmentEntity;

		public static partial class Msg
		{
			/// <summary>Attachment not found</summary>
			/// <param name="FileId">File (Post) ID</param>
			public sealed record AttachmentNotFoundMsg(ulong FileId) : IMsg { }

			/// <summary>Multiple Attachments found</summary>
			/// <param name="FileId">File (Post) ID</param>
			public sealed record MultipleAttachmentsFoundMsg(ulong FileId) : IMsg { }

			/// <summary>Unable to get Attachment file path</summary>
			/// <param name="Exception">Exception object</param>
			/// <param name="FileId">File (Post) ID</param>
			public sealed record ErrorGettingAttachmentFilePathMsg(Exception Exception, ulong FileId) : ExceptionMsg(Exception);
		}
	}
}
