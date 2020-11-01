using System.Data;
using System.Threading.Tasks;

namespace Dapper.SqlGenerator.Async
{
    public static class QueryExtensions
    {
        /// <summary>
        /// Execute the INSERT expression on the given table
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="connection">The connection to query on.</param>
        /// <param name="entityToInsert">The object to INSERT.</param>
        /// <param name="columnSet">Set of columns to update</param>
        /// <param name="insertKeys">If true, keys will also be inserted.</param>
        /// <param name="transaction">The transaction to use, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <returns>Task executing UPDATE returning number of rows affected</returns>
        public static Task<int> InsertAsync<T>(this IDbConnection connection, T entityToInsert, string columnSet = null, bool insertKeys = false, IDbTransaction transaction = null, int? commandTimeout = null)
            =>  connection.ExecuteAsync(connection.Sql().Insert<T>(columnSet, insertKeys), entityToInsert, transaction, commandTimeout);
        
        /// <summary>
        /// Execute the INSERT expression on the given table
        /// RETURNING values of the keys defined for this table
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="connection">The connection to query on.</param>
        /// <param name="entityToInsert">The object to INSERT.</param>
        /// <param name="columnSet">Set of columns to update</param>
        /// <param name="insertKeys">If true, keys will also be inserted.</param>
        /// <param name="transaction">The transaction to use, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <returns>Task executing INSERT returning object with inserted keys</returns>
        public static Task<T> InsertReturnAsync<T>(this IDbConnection connection, T entityToInsert, string columnSet = null, bool insertKeys = false, IDbTransaction transaction = null, int? commandTimeout = null)
            =>  connection.QuerySingleOrDefaultAsync<T>(connection.Sql().InsertReturn<T>(columnSet, insertKeys), entityToInsert, transaction, commandTimeout);
        
        /// <summary>
        /// Execute the UPDATE expression from the given table of records identified by keys 
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="connection">The connection to query on.</param>
        /// <param name="entityToUpdate">The object to UPDATE containing all keys to filter on.</param>
        /// <param name="columnSet">Set of columns to update</param>
        /// <param name="transaction">The transaction to use, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <returns>Task executing UPDATE returning number of rows affected</returns>
        public static Task<int> UpdateAsync<T>(this IDbConnection connection, T entityToUpdate, string columnSet = null, IDbTransaction transaction = null, int? commandTimeout = null)
            =>  connection.ExecuteAsync(connection.Sql().Update<T>(columnSet), entityToUpdate, transaction, commandTimeout);
        
        /// <summary>
        /// Execute the DELETE expression from the given table of records identified by keys 
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="connection">The connection to query on.</param>
        /// <param name="entityToDelete">The object to DELETE containing all keys to filter on.</param>
        /// <param name="transaction">The transaction to use, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <returns>Task executing DELETE returning number of rows affected</returns>
        public static Task<int> DeleteAsync<T>(this IDbConnection connection, T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null)
            =>  connection.ExecuteAsync(connection.Sql().Delete<T>(), entityToDelete, transaction, commandTimeout);
    }
}