using System.Data;
using Dapper.SqlGenerator.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dapper.SqlGenerator.Tests
{
    [TestClass]
    public class DapperSqlGeneratorTests
    {
        [TestInitialize]
        public void Init()
        {
            DapperSqlGenerator
                .Configure()
                .HasDefaultKeyColumn("Id", o => o.HasColumnName("id"))
                .Entity<Product>(e =>
                {
                    e.ToTable("Products");
                    e.Property(x => x.Kind)
                        .HasColumnName("Type");
                    e.Property(x => x.Name)
                        .Ignore();
                    e.Property(x => x.Content, typeof(PostgresAdapter))
                        .HasColumnType("json");
                    e.Property(x => x.Value, typeof(PostgresAdapter))
                        .HasComputedColumnSql("\"Id\" + 1");
                    e.Property(x => x.Value, typeof(SqlServerAdapter))
                        .HasComputedColumnSql("[Id] + 1");
                })
                .Entity<Order>(e =>
                {
                    e.ToTable("Orders");
                    e.HasKey(c => c.OrderId);
                    e.Property(c => c.OrderId)
                        .HasColumnName("Id");
                });
        }
        
        [TestMethod]
        public void TestGetColumnsProductPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var cols = pgConnection.SqlBuilder().GetColumns<Product>(ColumnSelection.Select);
            Assert.AreEqual("\"id\" AS \"Id\",\"Type\" AS \"Kind\",\"Content\",\"Id\" + 1 AS \"Value\"", cols);
            cols = pgConnection.SqlBuilder().GetColumns<Product>(ColumnSelection.Keys);
            Assert.AreEqual("\"id\" AS \"Id\"", cols, "Keys");
            cols = pgConnection.SqlBuilder().GetColumns<Product>(ColumnSelection.NonKeys | ColumnSelection.Computed);
            Assert.AreEqual("\"Type\" AS \"Kind\",\"Content\",\"Id\" + 1 AS \"Value\"", cols, "NonKeys");
        }

        [TestMethod]
        public void TestGetColumnsProductSqlServer()
        {
            var sqlConnection = new SqlConnection();
            var cols = sqlConnection.SqlBuilder().GetColumns<Product>(ColumnSelection.Select);
            Assert.AreEqual("[id] AS [Id],[Type] AS [Kind],[Content],[Id] + 1 AS [Value]", cols);
            cols = sqlConnection.SqlBuilder().GetColumns<Product>(ColumnSelection.Keys);
            Assert.AreEqual("[id] AS [Id]", cols, "Keys");
            cols = sqlConnection.SqlBuilder().GetColumns<Product>(ColumnSelection.NonKeys | ColumnSelection.Computed);
            Assert.AreEqual("[Type] AS [Kind],[Content],[Id] + 1 AS [Value]", cols, "NonKeys");
        }

        [TestMethod]
        public void TestGetColumnsOrderPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var cols = pgConnection.SqlBuilder().GetColumns<Order>(ColumnSelection.Select);
            Assert.AreEqual("\"Id\" AS \"OrderId\",\"ProductId\",\"Count\"", cols);
            cols = pgConnection.SqlBuilder().GetColumns<Order>(ColumnSelection.Keys);
            Assert.AreEqual("\"Id\" AS \"OrderId\"", cols, "Keys");
            cols = pgConnection.SqlBuilder().GetColumns<Order>(ColumnSelection.NonKeys | ColumnSelection.Computed);
            Assert.AreEqual("\"ProductId\",\"Count\"", cols, "NonKeys");
        }

        [TestMethod]
        public void TestGetColumnsOrderSqlServer()
        {
            var pgConnection = new SqlConnection();
            var cols = pgConnection.SqlBuilder().GetColumns<Order>(ColumnSelection.Select);
            Assert.AreEqual("[Id] AS [OrderId],[ProductId],[Count]", cols);
            cols = pgConnection.SqlBuilder().GetColumns<Order>(ColumnSelection.Keys);
            Assert.AreEqual("[Id] AS [OrderId]", cols, "Keys");
            cols = pgConnection.SqlBuilder().GetColumns<Order>(ColumnSelection.NonKeys | ColumnSelection.Computed);
            Assert.AreEqual("[ProductId],[Count]", cols, "NonKeys");
        }

        [TestMethod]
        public void TestGetColumnEqualParamProductPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var cols = pgConnection.SqlBuilder().GetColumnEqualParams<Product>(ColumnSelection.Keys | ColumnSelection.NonKeys);
            Assert.AreEqual("\"id\"=@Id,\"Type\"=@Kind,\"Content\"=CAST(@Content AS json)", cols);
            cols = pgConnection.SqlBuilder().GetColumnEqualParams<Product>(ColumnSelection.Keys);
            Assert.AreEqual("\"id\"=@Id", cols, "Keys");
            cols = pgConnection.SqlBuilder().GetColumnEqualParams<Product>(ColumnSelection.NonKeys);
            Assert.AreEqual("\"Type\"=@Kind,\"Content\"=CAST(@Content AS json)", cols, "NonKeys");
        }

        [TestMethod]
        public void TestGetColumnEqualParamSqlServer()
        {
            var sqlConnection = new SqlConnection();
            var cols = sqlConnection.SqlBuilder().GetColumnEqualParams<Product>(ColumnSelection.Keys | ColumnSelection.NonKeys);
            Assert.AreEqual("[id]=@Id,[Type]=@Kind,[Content]=@Content", cols);
            cols = sqlConnection.SqlBuilder().GetColumnEqualParams<Product>(ColumnSelection.Keys);
            Assert.AreEqual("[id]=@Id", cols, "Keys");
            cols = sqlConnection.SqlBuilder().GetColumnEqualParams<Product>(ColumnSelection.NonKeys);
            Assert.AreEqual("[Type]=@Kind,[Content]=@Content", cols, "NonKeys");
        }

        [TestMethod]
        public void TestInsertPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var cols = pgConnection.SqlBuilder().Insert<Product>();
            Assert.AreEqual("INSERT INTO \"Products\" (\"Type\",\"Content\") VALUES (@Kind,CAST(@Content AS json))", cols);
        }

        [TestMethod]
        public void TestInsertSqlServer()
        {
            var sqlConnection = new SqlConnection();
            var cols = sqlConnection.SqlBuilder().Insert<Product>();
            Assert.AreEqual("INSERT INTO [Products] ([Type],[Content]) VALUES (@Kind,@Content)", cols);
        }

        private class Product
        {
            public int Id { get; set; }
            
            public int Kind { get; set; }
            
            public string Name { get; set; }
            
            public string Content { get; set; }
            
            public int Value { get; set; }
        }

        private class Order
        {
            public int OrderId { get; set; }
            
            public int ProductId { get; set; }
            
            public int Count { get; set; }
        }

        private class SqlConnection : IDbConnection
        {
            public void Dispose()
            {
                throw new System.NotImplementedException();
            }

            public IDbTransaction BeginTransaction()
            {
                throw new System.NotImplementedException();
            }

            public IDbTransaction BeginTransaction(IsolationLevel il)
            {
                throw new System.NotImplementedException();
            }

            public void ChangeDatabase(string databaseName)
            {
                throw new System.NotImplementedException();
            }

            public void Close()
            {
                throw new System.NotImplementedException();
            }

            public IDbCommand CreateCommand()
            {
                throw new System.NotImplementedException();
            }

            public void Open()
            {
                throw new System.NotImplementedException();
            }

            public string ConnectionString { get; set; } = "SqlServer CS";
            
            public int ConnectionTimeout { get; } = 1;

            public string Database { get; } = string.Empty;
            
            public ConnectionState State { get; } = ConnectionState.Broken;
        }

        private class NpgsqlConnection : IDbConnection
        {
            public void Dispose()
            {
                throw new System.NotImplementedException();
            }

            public IDbTransaction BeginTransaction()
            {
                throw new System.NotImplementedException();
            }

            public IDbTransaction BeginTransaction(IsolationLevel il)
            {
                throw new System.NotImplementedException();
            }

            public void ChangeDatabase(string databaseName)
            {
                throw new System.NotImplementedException();
            }

            public void Close()
            {
                throw new System.NotImplementedException();
            }

            public IDbCommand CreateCommand()
            {
                throw new System.NotImplementedException();
            }

            public void Open()
            {
                throw new System.NotImplementedException();
            }

            public string ConnectionString { get; set; } = "Postgres CS";
            
            public int ConnectionTimeout { get; } = 1;

            public string Database { get; } = string.Empty;
            
            public ConnectionState State { get; } = ConnectionState.Broken;
        } 
    }
}