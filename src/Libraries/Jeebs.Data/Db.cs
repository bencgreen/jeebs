﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Jeebs.Config;
using Jeebs.Data.TypeHandlers;
using Microsoft.Extensions.Options;
using static F.OptionF;

namespace Jeebs.Data
{
	/// <inheritdoc cref="IDb"/>
	public abstract class Db : IDb, IDisposable
	{
		/// <inheritdoc/>
		public IDbClient Client { get; private init; }

		/// <summary>
		/// Configuration for this database connection
		/// </summary>
		public DbConnectionConfig Config { get; private init; }

		private IDbConnection? Connection { get; init; }

		/// <summary>
		/// ILog (should be given a context of the implementing class)
		/// </summary>
		protected ILog Log { get; private init; }

		/// <summary>
		/// Inject database connection and connect to client
		/// </summary>
		/// <param name="config">Database configuration</param>
		/// <param name="log">ILog (should be given a context of the implementing class)</param>
		/// <param name="client">Database client</param>
		/// <param name="name">Connection name</param>
		protected Db(IOptions<DbConfig> config, ILog log, IDbClient client, string name)
		{
			Client = client;
			Config = config.Value.GetConnection(name);
			Log = log;

			try
			{
				Connection = client.Connect(Config.ConnectionString);
			}
			catch (Exception e)
			{
				Log.Fatal(e, "Unable to connect to database {Name}", name);
			}
		}

		/// <inheritdoc/>
		public Task<Option<IEnumerable<TModel>>> QueryAsync<TModel>(string query, object? parameters, CommandType type) =>
			ReturnAsync(() =>
				Connection.QueryAsync<TModel>(
					sql: query,
					param: parameters ?? new object(),
					commandType: type
				),
				e => new Msg.QueryExceptionMsg(e)
			)
			.BindAsync(
				x => x.Count() switch
				{
					> 0 =>
						Return(x),

					_ =>
						None<IEnumerable<TModel>, Msg.QueryNotFoundMsg>()
				}
			);

		/// <inheritdoc/>
		public Task<Option<TModel>> QuerySingleAsync<TModel>(string query, object? parameters, CommandType type) =>
			ReturnAsync<TModel?>(() =>
				Connection.QuerySingleOrDefaultAsync<TModel?>(
					sql: query,
					param: parameters ?? new object(),
					commandType: type
				),
				true,
				e => new Msg.QuerySingleExceptionMsg(e)
			)
			.BindAsync(
				x => x switch
				{
					TModel model =>
						Return(model),

					_ =>
						None<TModel, Msg.QuerySingleNotFoundMsg>()
				}
			);

		/// <inheritdoc/>
		public Task<Option<bool>> ExecuteAsync(string query, object? parameters, CommandType type) =>
			ReturnAsync(() =>
				Connection.ExecuteAsync(
					sql: query,
					param: parameters ?? new object(),
					commandType: type
				),
				e => new Msg.ExecuteExceptionMsg(e)
			)
			.MapAsync(
				x => x != 0,
				DefaultHandler
			);

		/// <inheritdoc/>
		public Task<Option<TReturn>> ExecuteAsync<TReturn>(string query, object? parameters, CommandType type) =>
			ReturnAsync(() =>
				Connection.ExecuteScalarAsync<TReturn>(
					sql: query,
					param: parameters ?? new object(),
					commandType: type
				),
				e => new Msg.ExecuteScalarExceptionMsg(e)
			);

		#region Dispose

		/// <summary>
		/// Set to true if the object has been disposed
		/// </summary>
		private bool disposed = false;

		/// <summary>
		/// Suppress garbage collection and call <see cref="Dispose(bool)"/>
		/// https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose managed resources
		/// </summary>
		/// <param name="disposing">True if disposing</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}

			if (disposing)
			{
				Connection?.Dispose();
			}

			disposed = true;
		}

		#endregion

		#region Static

		/// <summary>
		/// Add default type handlers
		/// </summary>
		static Db() =>
			SqlMapper.AddTypeHandler(new GuidTypeHandler());

		/// <summary>
		/// Persist an EnumList to the database by encoding it as JSON
		/// </summary>
		/// <typeparam name="T">Type to handle</typeparam>
		protected static void AddEnumeratedListTypeHandler<T>()
			where T : Enumerated =>
			SqlMapper.AddTypeHandler(new EnumeratedListTypeHandler<T>());

		/// <summary>
		/// Persist a type to the database by encoding it as JSON
		/// </summary>
		/// <typeparam name="T">Type to handle</typeparam>
		protected static void AddJsonTypeHandler<T>() =>
			SqlMapper.AddTypeHandler(new JsonTypeHandler<T>());

		/// <summary>
		/// Persist a StrongId to the database
		/// </summary>
		/// <typeparam name="T">StrongId itype</typeparam>
		protected static void AddStrongIdTypeHandler<T>()
			where T : StrongId, new() =>
			SqlMapper.AddTypeHandler(new StrongIdTypeHandler<T>());

		#endregion

		/// <summary>Messages</summary>
		public static class Msg
		{
			/// <summary>Error running QueryAsync</summary>
			/// <param name="Exception">Exception object</param>
			public sealed record QueryExceptionMsg(Exception Exception) : ExceptionMsg(Exception) { }

			/// <summary>The query returned any empty list</summary>
			public sealed record QueryNotFoundMsg : NotFoundMsg { }

			/// <summary>Error running QuerySingleAsync</summary>
			/// <param name="Exception">Exception object</param>
			public sealed record QuerySingleExceptionMsg(Exception Exception) : ExceptionMsg(Exception) { }

			/// <summary>The query returned no items, or more than one</summary>
			public sealed record QuerySingleNotFoundMsg : NotFoundMsg { }

			/// <summary>Error running ExecuteAsync</summary>
			/// <param name="Exception">Exception object</param>
			public sealed record ExecuteExceptionMsg(Exception Exception) : ExceptionMsg(Exception) { }

			/// <summary>Error running ExecuteScalarAsync</summary>
			/// <param name="Exception">Exception object</param>
			public sealed record ExecuteScalarExceptionMsg(Exception Exception) : ExceptionMsg(Exception) { }
		}
	}
}
