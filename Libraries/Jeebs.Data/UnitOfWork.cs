﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using Jeebs.Util;

namespace Jeebs.Data
{
	/// <summary>
	/// Database Unit of Work
	/// </summary>
	public sealed class UnitOfWork : IDisposable
	{
		/// <summary>
		/// Provides thread-safe locking
		/// </summary>
		private static readonly object _ = new object();

		/// <summary>
		/// IDbTransaction
		/// </summary>
		private readonly IDbTransaction transaction;

		/// <summary>
		/// IDbConnection
		/// </summary>
		private IDbConnection Connection => transaction.Connection;

		/// <summary>
		/// IAdapter
		/// </summary>
		private readonly IAdapter adapter;

		/// <summary>
		/// ILog
		/// </summary>
		private readonly ILog log;

		/// <summary>
		/// Create object
		/// </summary>
		/// <param name="connection">IDbConnection</param>
		/// <param name="adapter">IAdapter</param>
		/// <param name="log">ILog</param>
		internal UnitOfWork(in IDbConnection connection, in IAdapter adapter, in ILog log)
		{
			transaction = connection.BeginTransaction();
			this.adapter = adapter;
			this.log = log;
		}

		/// <summary>
		/// Shorthand for IAdapter.SplitAndEscape
		/// </summary>
		/// <param name="element"></param>
		public string E(in object element) => adapter.SplitAndEscape(element.ToString());

		/// <summary>
		/// Commit all queries - should normally be called as part of Dispose()
		/// </summary>
		public void Commit()
		{
			try
			{
				transaction.Commit();
			}
			catch (Exception ex)
			{
				log.Error(ex, "Error committing transaction.");
			}
		}

		/// <summary>
		/// Rollback all queries
		/// </summary>
		public void Rollback()
		{
			try
			{
				transaction.Rollback();
			}
			catch (Exception ex)
			{
				log.Error(ex, "Error rolling back transaction.");
			}
		}

		/// <summary>
		/// Commit transaction and close connection
		/// </summary>
		public void Dispose()
		{
			Commit();
			transaction.Dispose();
			Connection.Dispose();
		}

		#region Logging

		/// <summary>
		/// Log a query
		/// </summary>
		/// <param name="method">Calling method</param>
		/// <param name="query">SQL query</param>
		private void LogQuery(string method, string query)
		{
			log.Debug("Method: UnitOfWork.{0}()", method);
			log.Debug("Query: {0}", query);
		}

		/// <summary>
		/// Log a query
		/// </summary>
		/// <typeparam name="T">Parameter object type</typeparam>
		/// <param name="method">Calling method</param>
		/// <param name="query">SQL query</param>
		/// <param name="parameters">Parameters</param>
		private void LogQuery<T>(string method, string query, T parameters)
		{
			log.Debug("Method: UnitOfWork.{0}()", method);
			log.Debug("Query: {0}", query);
			log.Debug("Parameters: {0}", Json.Serialise(parameters));
		}

		/// <summary>
		/// Query failure -
		///		Rollback
		///		Log Error
		///		Return Failure Result
		/// </summary>
		/// <param name="error">Error message</param>
		/// <param name="args">Error message arguments</param>
		private DbResult.DbFailure Fail(string error, params object[] args)
		{
			// Rollback transaction
			Rollback();

			// Log error
			log.Error(error, args);

			// Return failure object
			return DbResult.Failure(error);
		}

		/// <summary>
		/// Query failure -
		///		Rollback
		///		Log Error
		///		Return Failure Result
		/// </summary>
		/// <param name="ex">Exception</param>
		/// <param name="error">Error message</param>
		/// <param name="args">Error message arguments</param>
		private DbResult.DbFailure Fail(Exception ex, string error, params object[] args)
		{
			// Rollback transaction
			Rollback();

			// Log exception
			log.Error(ex, error, args);

			// Return failure object
			return DbResult.Failure(error);
		}

		/// <summary>
		/// Query failure -
		///		Rollback
		///		Log Error
		///		Return Failure Result
		/// </summary>
		/// <typeparam name="T">Return value type</typeparam>
		/// <param name="error">Error message</param>
		/// <param name="args">Error message arguments</param>
		private DbResult.DbFailure<T> Fail<T>(string error, params object[] args)
		{
			// Rollback transaction
			Rollback();

			// Log error
			log.Error(error, args);

			// Return failure object
			return DbResult.Failure<T>(error);
		}

		/// <summary>
		/// Query failure -
		///		Rollback
		///		Log Error
		///		Return Failure Result
		/// </summary>
		/// <typeparam name="T">Return value type</typeparam>
		/// <param name="ex">Exception</param>
		/// <param name="error">Error message</param>
		/// <param name="args">Error message arguments</param>
		private DbResult.DbFailure<T> Fail<T>(Exception ex, string error, params object[] args)
		{
			// Rollback transaction
			Rollback();

			// Log exception
			log.Error(ex, error, args);

			// Return failure object
			return DbResult.Failure<T>(error);
		}

		#endregion

		#region C

		/// <summary>
		/// Insert an entity
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <param name="poco">Entity object</param>
		/// <returns>Entity (complete with new ID)</returns>
		public IDbResult<T> Insert<T>(T poco)
			where T : class, IEntity
		{
			// Declare here so accessible outside try...catch
			int newId;

			try
			{
				// Create query
				var query = adapter.CreateSingleAndReturnId<T>();
				LogQuery(nameof(Insert), query, poco);

				// Insert and capture new ID
				newId = Connection.ExecuteScalar<int>(query, poco, transaction);
			}
			catch (Exception ex)
			{
				return Fail<T>(ex, $"Unable to insert {typeof(T)}.");
			}

			// If newId is still 0, rollback changes - something went wrong
			if (newId == 0)
			{
				return Fail<T>($"Unable to retrieve ID of inserted {typeof(T)}.");
			}

			// Retrieve fresh POCO with inserted ID
			return Single<T>(newId);
		}

		/// <summary>
		/// Insert an entity
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <param name="poco">Entity object</param>
		/// <returns>Entity (complete with new ID)</returns>
		public async Task<IDbResult<T>> InsertAsync<T>(T poco)
			where T : class, IEntity
		{
			// Declare here so accessible outside try...catch
			int newId;

			try
			{
				// Create query
				var query = adapter.CreateSingleAndReturnId<T>();
				LogQuery(nameof(InsertAsync), query, poco);

				// Insert and capture new ID
				newId = await Connection.ExecuteScalarAsync<int>(query, poco, transaction);
			}
			catch (Exception ex)
			{
				return Fail<T>(ex, $"Unable to insert {typeof(T)}.");
			}

			// If newId is still 0, rollback changes - something went wrong
			if (newId == 0)
			{
				return Fail<T>($"Unable to retrieve ID of inserted {typeof(T)}.");
			}

			return await SingleAsync<T>(newId);
		}

		#endregion

		#region R

		/// <summary>
		/// Perform a query, returning a dynamic object
		/// </summary>
		/// <param name="query">Query string</param>
		/// <param name="parameters">Parameters</param>
		/// <returns>IEnumerable</returns>
		public IDbResult<IEnumerable<dynamic>> Query(in string query, in object? parameters = null)
		{
			try
			{
				LogQuery(nameof(Query), query, parameters);

				var result = Connection.Query<dynamic>(query, param: parameters, transaction: transaction);
				return DbResult.Success(result);
			}
			catch (Exception ex)
			{
				return Fail<IEnumerable<dynamic>>(
					new Jx.Data.QueryException(query, parameters, ex),
					$"An error occurred while executing the query."
				);
			}
		}

		/// <summary>
		/// Perform a query, returning a dynamic object
		/// </summary>
		/// <param name="query">Query string</param>
		/// <param name="parameters">Parameters</param>
		/// <returns>IEnumerable</returns>
		public async Task<IDbResult<IEnumerable<dynamic>>> QueryAsync(string query, object? parameters = null)
		{
			try
			{
				LogQuery(nameof(QueryAsync), query, parameters);

				var result = await Connection.QueryAsync<dynamic>(query, param: parameters, transaction: transaction);
				return DbResult.Success(result);
			}
			catch (Exception ex)
			{
				return Fail<IEnumerable<dynamic>>(
					new Jx.Data.QueryException(query, parameters, ex),
					$"An error occurred while executing the query."
				);
			}
		}

		/// <summary>
		/// Run a query against the database
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="query">Query string</param>
		/// <param name="parameters">Parameters</param>
		/// <returns>IEnumerable</returns>
		public IDbResult<IEnumerable<T>> Query<T>(string query, object? parameters = null)
		{
			try
			{
				LogQuery(nameof(Query), query, parameters);

				var result = Connection.Query<T>(query, param: parameters, transaction: transaction);
				return DbResult.Success(result);
			}
			catch (Exception ex)
			{
				return Fail<IEnumerable<T>>(
					new Jx.Data.QueryException(query, parameters, ex),
					$"An error occurred while executing the query."
				);
			}
		}

		/// <summary>
		/// Run a query against the database
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="query">Query string</param>
		/// <param name="parameters">Parameters</param>
		/// <returns>IEnumerable</returns>
		public async Task<IDbResult<IEnumerable<T>>> QueryAsync<T>(string query, object? parameters = null)
		{
			try
			{
				LogQuery(nameof(QueryAsync), query, parameters);

				var result = await Connection.QueryAsync<T>(query, param: parameters, transaction: transaction);
				return DbResult.Success(result);
			}
			catch (Exception ex)
			{
				return Fail<IEnumerable<T>>(
					new Jx.Data.QueryException(query, parameters, ex),
					$"An error occurred while executing the query."
				);
			}
		}

		/// <summary>
		/// Get an entity from the database by ID
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <param name="id">Entity ID</param>
		/// <returns>Entity (or null if not found)</returns>
		public IDbResult<T> Single<T>(int id)
			where T : class, IEntity
		{
			try
			{
				var query = adapter.RetrieveSingleById<T>(id);

				LogQuery(nameof(Single), query);

				var result = Connection.QuerySingle<T>(query, transaction: transaction);
				return DbResult.Success(result);
			}
			catch (Exception ex)
			{
				return Fail<T>(ex, $"An error occured while retrieving {typeof(T)} with ID '{id}'.");
			}
		}

		/// <summary>
		/// Get an entity from the database by ID
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <param name="id">Entity ID</param>
		/// <returns>Entity (or null if not found)</returns>
		private async Task<IDbResult<T>> SingleAsync<T>(int id)
			where T : class, IEntity
		{
			try
			{
				var query = adapter.RetrieveSingleById<T>(id);

				LogQuery(nameof(SingleAsync), query);

				var result = await Connection.QuerySingleAsync<T>(query, transaction: transaction);
				return DbResult.Success(result);
			}
			catch (Exception ex)
			{
				return Fail<T>(ex, $"An error occured while retrieving {typeof(T)} with ID '{id}'.");
			}
		}

		/// <summary>
		/// Return a single object by query, or default value if the object cannot be found
		/// </summary>
		/// <param name="query">Query string</param>
		/// <param name="parameters">Parameters</param>
		/// <returns>Object or default value</returns>
		public IDbResult<T> Single<T>(string query, object parameters)
		{
			try
			{
				LogQuery(nameof(Single), query, parameters);

				var result = Connection.QuerySingle<T>(query, parameters, transaction);
				return DbResult.Success(result);
			}
			catch (Exception ex)
			{
				return Fail<T>(
					new Jx.Data.QueryException(query, parameters, ex),
					$"An error occurred while retrieving {typeof(T)}."
				);
			}
		}

		/// <summary>
		/// Return a single object by query, or default value if the object cannot be found
		/// </summary>
		/// <param name="query">Query string</param>
		/// <param name="parameters">Parameters</param>
		/// <returns>Object or default value</returns>
		public async Task<IDbResult<T>> SingleAsync<T>(string query, object parameters)
		{
			try
			{
				LogQuery(nameof(SingleAsync), query, parameters);

				var result = await Connection.QuerySingleAsync<T>(query, parameters, transaction);
				return DbResult.Success(result);
			}
			catch (Exception ex)
			{
				return Fail<T>(
					new Jx.Data.QueryException(query, parameters, ex),
					$"An error occurred while retrieving {typeof(T)}."
				);
			}
		}

		#endregion

		#region U

		/// <summary>
		/// Update an object
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <param name="poco">Entity object</param>
		/// <returns>IDbResult - Whether or not the update was successful</returns>
		public IDbResult Update<T>(in T poco)
			where T : class, IEntity
		{
			lock (_)
			{
				if (poco is IEntityWithVersion pocoWithVersion)
				{
					return UpdateWithVersion(pocoWithVersion);
				}
				else
				{
					return UpdateWithoutVersion(poco);
				}
			}
		}

		/// <summary>
		/// Update using versioning
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <param name="poco">Object</param>
		/// <returns>IDbResult - Whether or not the update was successful</returns>
		private IDbResult UpdateWithVersion<T>(in T poco)
			where T : class, IEntityWithVersion
		{
			var currentVersion = poco.Version;
			var error = $"Unable to update {typeof(T)} '{poco.Id}'.";

			try
			{
				// Build the query
				var query = adapter.UpdateSingle<T>(poco.Id, poco.Version);

				// Now increase the row version and execute query
				poco.Version++;

				LogQuery(nameof(UpdateWithVersion), query, poco);

				var rowsAffected = Connection.Execute(query, poco, transaction);
				if (rowsAffected == 1)
				{
					return new DbSuccess();
				}
			}
			catch (Exception ex)
			{
				return Fail(ex, error);
			}

			// Build the query to get a fresh poco
			var selectSql = adapter.RetrieveSingleById<T>(poco.Id);

			// Get the fresh poco
			var freshPoco = Connection.QuerySingle<T>(selectSql);
			if (freshPoco.Version > currentVersion)
			{
				Rollback();
				log.Error(error + " Concurrency check failed.");
				return DbResult.ConcurrencyFailure<T>();
			}
			else
			{
				return Fail(error);
			}
		}

		/// <summary>
		/// Update without using versioning
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <param name="poco">Object</param>
		/// <returns>IDbResult - Whether or not the update was successful</returns>
		private IDbResult UpdateWithoutVersion<T>(in T poco)
			where T : class, IEntity
		{
			var error = $"Unable to update {typeof(T)} '{poco.Id}'.";

			try
			{
				// Build the query
				var query = adapter.UpdateSingle<T>(poco.Id);
				LogQuery(nameof(UpdateWithoutVersion), query, poco);

				// Now execute query
				var rowsAffected = Connection.Execute(query, poco, transaction);
				if (rowsAffected == 1)
				{
					return new DbSuccess();
				}
				else
				{
					return Fail(error);
				}
			}
			catch (Exception ex)
			{
				return Fail(ex, error);
			}
		}

		#endregion

		#region D

		/// <summary>
		/// Update an object
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <param name="poco">Entity object</param>
		/// <returns>IDbResult - Whether or not the update was successful</returns>
		public IDbResult Delete<T>(in T poco)
			where T : class, IEntity
		{
			lock (_)
			{
				if (poco is IEntityWithVersion pocoWithVersion)
				{
					return DeleteWithVersion(pocoWithVersion);
				}
				else
				{
					return DeleteWithoutVersion(poco);
				}
			}
		}

		/// <summary>
		/// Delete an entity
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <param name="poco">Entity to delete</param>
		/// <result>IDbResult - Whether or not the delete was successful</result>
		private IDbResult DeleteWithVersion<T>(in T poco)
			where T : class, IEntityWithVersion
		{
			var error = $"Unable to delete {typeof(T)} '{poco.Id}'.";

			try
			{
				// Build the query
				var query = adapter.DeleteSingle<T>(poco.Id, poco.Version);
				LogQuery(nameof(Delete), query);

				// Now execute query
				var rowsAffected = Connection.Execute(query, transaction: transaction);
				if (rowsAffected == 1)
				{
					return DbResult.Success();
				}
				else
				{
					return Fail(error);
				}
			}
			catch (Exception ex)
			{
				return Fail(ex, error);
			}
		}

		/// <summary>
		/// Delete an entity
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <param name="poco">Entity to delete</param>
		/// <result>IDbResult - Whether or not the delete was successful</result>
		private IDbResult DeleteWithoutVersion<T>(in T poco)
			where T : class, IEntity
		{
			var error = $"Unable to delete {typeof(T)} '{poco.Id}'.";

			try
			{
				// Build the query
				var query = adapter.DeleteSingle<T>(poco.Id);
				LogQuery(nameof(Delete), query);

				// Now execute query
				var rowsAffected = Connection.Execute(query, transaction: transaction);
				if (rowsAffected == 1)
				{
					return DbResult.Success();
				}
				else
				{
					return Fail(error);
				}
			}
			catch (Exception ex)
			{
				return Fail(ex, error);
			}
		}

		/// <summary>
		/// Delete an entity
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <param name="poco">Entity to delete</param>
		/// <result>IDbResult - Whether or not the delete was successful</result>
		public async Task<IDbResult> DeleteAsync<T>(T poco)
			where T : class, IEntity
		{
			var error = $"Unable to delete {typeof(T)} '{poco.Id}'.";

			try
			{
				// Build the query
				var query = adapter.DeleteSingle<T>(poco.Id);
				LogQuery(nameof(DeleteAsync), query);

				// Now execute query
				var rowsAffected = await Connection.ExecuteAsync(query, transaction: transaction);
				if (rowsAffected == 1)
				{
					return DbResult.Success();
				}
				else
				{
					return Fail(error);
				}
			}
			catch (Exception ex)
			{
				return Fail(ex, error);
			}
		}

		#endregion

		#region Direct

		/// <summary>
		/// Execute a query on the database
		/// </summary>
		/// <param name="query">SQL qyery</param>
		/// <param name="parameters">Parameters</param>
		public IDbResult Execute(in string query, in object? parameters = null)
		{
			try
			{
				LogQuery(nameof(Execute), query, parameters);
				Connection.Execute(query, param: parameters, transaction: transaction);
				return DbResult.Success();
			}
			catch (Exception ex)
			{
				return Fail(new Jx.Data.QueryException(query, parameters, ex), "Error executing query.");
			}
		}

		/// <summary>
		/// Execute a query on the database
		/// </summary>
		/// <param name="query">SQL qyery</param>
		/// <param name="parameters">Parameters</param>
		public async Task<IDbResult> ExecuteAsync(string query, object? parameters = null)
		{
			try
			{
				LogQuery(nameof(ExecuteAsync), query, parameters);
				await Connection.ExecuteAsync(query, param: parameters, transaction: transaction);
				return DbResult.Success();
			}
			catch (Exception ex)
			{
				return Fail(new Jx.Data.QueryException(query, parameters, ex), "Error executing query.");
			}
		}

		/// <summary>
		/// Execute a query and return a scalar value
		/// </summary>
		/// <typeparam name="T">Return type</typeparam>
		/// <param name="query">SQL qyery</param>
		/// <param name="parameters">Parameters</param>
		/// <returns>IDbResult - on success, Value is Scalar value T</returns>
		public IDbResult<T> ExecuteScalar<T>(in string query, in object? parameters = null)
		{
			try
			{
				LogQuery(nameof(ExecuteScalar), query, parameters);
				var result = Connection.ExecuteScalar<T>(query, param: parameters, transaction: transaction);
				return DbResult.Success(result);
			}
			catch (Exception ex)
			{
				return Fail<T>(new Jx.Data.QueryException(query, parameters, ex), "Error executing query.");
			}
		}

		/// <summary>
		/// Execute a query and return a scalar value
		/// </summary>
		/// <typeparam name="T">Return type</typeparam>
		/// <param name="query">SQL qyery</param>
		/// <param name="parameters">Parameters</param>
		/// <returns>IDbResult - on success, Value is Scalar value T</returns>
		public async Task<IDbResult<T>> ExecuteScalarAsync<T>(string query, object? parameters = null)
		{
			try
			{
				LogQuery(nameof(ExecuteScalarAsync), query, parameters);
				var result = await Connection.ExecuteScalarAsync<T>(query, param: parameters, transaction: transaction);
				return DbResult.Success(result);
			}
			catch (Exception ex)
			{
				return Fail<T>(new Jx.Data.QueryException(query, parameters, ex), "Error executing query.");
			}
		}

		#endregion
	}
}
