using System.Collections.Generic;

namespace Dapper.SqlGenerator
{
    public interface ISqlAdapter : IBaseSqlAdapter
    {
        /// <summary>
        /// Returns a query checking if table @table exists in the information schema
        /// </summary>
        /// <returns>Table exists query</returns>
        string TableExists();
        
        /// <summary>
        /// Prepares a SELECT query
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="table"></param>
        /// <param name="columnSet"></param>
        /// <param name="columnSelection"></param>
        /// <param name="whereSet"></param>
        /// <param name="whereSelection"></param>
        /// <param name="alias"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        string Select<TEntity>(ModelBuilder modelBuilder, EntityTypeBuilder<TEntity> table, string columnSet, ColumnSelection columnSelection, string whereSet, ColumnSelection whereSelection, string alias);

        /// <summary>
        /// Inserts a row
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="table"></param>
        /// <param name="insertKeys"></param>
        /// <param name="columnSet"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        string Insert<TEntity>(ModelBuilder modelBuilder, EntityTypeBuilder<TEntity> table, bool insertKeys, string columnSet);

        /// <summary>
        /// Inserts a row and returns keys 
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="table"></param>
        /// <param name="insertKeys"></param>
        /// <param name="columnSet"></param>
        string InsertReturn<TEntity>(ModelBuilder modelBuilder, EntityTypeBuilder<TEntity> table, bool insertKeys, string columnSet);

        /// <summary>
        /// Updates a row
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="table"></param>
        /// <param name="columnSet"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        string Update<TEntity>(ModelBuilder modelBuilder, EntityTypeBuilder<TEntity> table, string columnSet);

        /// <summary>
        /// Deletes a row
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="table"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        string Delete<TEntity>(ModelBuilder modelBuilder, EntityTypeBuilder<TEntity> table);

        /// <summary>
        /// Prepares a merge (upsert - update or insert) operation on the entity 
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="table"></param>
        /// <param name="mergeSet"></param>
        /// <param name="insertKeys"></param>
        /// <param name="columnSet"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        string Merge<TEntity>(ModelBuilder modelBuilder, EntityTypeBuilder<TEntity> table, string mergeSet, bool insertKeys, string columnSet);
    }
}