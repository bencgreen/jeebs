﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System.Data;

namespace Jeebs.Data
{
	/// <summary>
	/// Database client
	/// </summary>
	public interface IDbClient
	{
		/// <summary>
		/// Return a new database connection
		/// </summary>
		/// <param name="connectionString">Database connection string</param>
		IDbConnection Connect(string connectionString);

		#region General Queries

		#endregion

		#region CRUD Queries

		/// <summary>
		/// Return a query to create an entity
		/// </summary>
		/// <typeparam name="TEntity">Entity type</typeparam>
		Option<string> GetCreateQuery<TEntity>()
			where TEntity : IEntity;

		/// <summary>
		/// Return a query to retrieve a single entity by ID
		/// </summary>
		/// <typeparam name="TEntity">Entity type</typeparam>
		/// <typeparam name="TModel">Return model type</typeparam>
		Option<string> GetRetrieveQuery<TEntity, TModel>()
			where TEntity : IEntity;

		/// <summary>
		/// Return a query to update a single entity
		/// </summary>
		/// <typeparam name="TEntity">Entity type</typeparam>
		/// <typeparam name="TModel">Return model type</typeparam>
		Option<string> GetUpdateQuery<TEntity, TModel>()
			where TEntity : IEntity;

		/// <summary>
		/// Return a query to delete a single entity by ID
		/// </summary>
		/// <typeparam name="TEntity">Entity type</typeparam>
		Option<string> GetDeleteQuery<TEntity>()
			where TEntity : IEntity;

		#endregion
	}
}
