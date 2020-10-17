using Dapper.SqlGenerator.Tests.Connections;
using Dapper.SqlGenerator.Tests.TestClasses;
using NUnit.Framework;

namespace Dapper.SqlGenerator.Tests
{
    [TestFixture]
    public class SqlGenerateTests
    {
        [SetUp]
        public void Init()
        {
            ProductOrderInit.Init();
        }
        
        [Test]
        public void TestGetColumnsProductPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var cols = pgConnection.Sql().GetColumns<TestProduct>(ColumnSelection.Select);
            Assert.AreEqual("\"id\" AS \"Id\",\"Type\" AS \"Kind\",\"Content\",\"Id\" + 1 AS \"Value\",\"Enum\",\"MaybeDate\",\"Date\",\"MaybeGuid\",\"Guid\",\"Duration\",\"Last\"", cols);
            cols = pgConnection.Sql().GetColumns<TestProduct>(ColumnSelection.Keys);
            Assert.AreEqual("\"id\" AS \"Id\"", cols, "Keys");
            cols = pgConnection.Sql().GetColumns<TestProduct>(ColumnSelection.NonKeys | ColumnSelection.Computed);
            Assert.AreEqual("\"Type\" AS \"Kind\",\"Content\",\"Id\" + 1 AS \"Value\",\"Enum\",\"MaybeDate\",\"Date\",\"MaybeGuid\",\"Guid\",\"Duration\",\"Last\"", cols, "NonKeys");
        }

        [Test]
        public void TestGetColumnsProductSqlServer()
        {
            var sqlConnection = new SqlConnection();
            var cols = sqlConnection.Sql().GetColumns<TestProduct>(ColumnSelection.Select);
            Assert.AreEqual("[id] AS [Id],[Type] AS [Kind],[Content],[Id] + 1 AS [Value],[Enum],[MaybeDate],[Date],[MaybeGuid],[Guid],[Duration],[Last]", cols);
            cols = sqlConnection.Sql().GetColumns<TestProduct>(ColumnSelection.Keys);
            Assert.AreEqual("[id] AS [Id]", cols, "Keys");
            cols = sqlConnection.Sql().GetColumns<TestProduct>(ColumnSelection.NonKeys | ColumnSelection.Computed);
            Assert.AreEqual("[Type] AS [Kind],[Content],[Id] + 1 AS [Value],[Enum],[MaybeDate],[Date],[MaybeGuid],[Guid],[Duration],[Last]", cols, "NonKeys");
        }

        [Test]
        public void TestGetColumnsOrderPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var cols = pgConnection.Sql().GetColumns<TestOrder>(ColumnSelection.Select);
            Assert.AreEqual("\"Id\" AS \"OrderId\",\"ProductId\",\"Count\"", cols);
            cols = pgConnection.Sql().GetColumns<TestOrder>(ColumnSelection.Keys);
            Assert.AreEqual("\"Id\" AS \"OrderId\"", cols, "Keys");
            cols = pgConnection.Sql().GetColumns<TestOrder>(ColumnSelection.NonKeys | ColumnSelection.Computed);
            Assert.AreEqual("\"ProductId\",\"Count\"", cols, "NonKeys");
        }

        [Test]
        public void TestGetColumnsOrderSqlServer()
        {
            var pgConnection = new SqlConnection();
            var cols = pgConnection.Sql().GetColumns<TestOrder>(ColumnSelection.Select);
            Assert.AreEqual("[Id] AS [OrderId],[ProductId],[Count]", cols);
            cols = pgConnection.Sql().GetColumns<TestOrder>(ColumnSelection.Keys);
            Assert.AreEqual("[Id] AS [OrderId]", cols, "Keys");
            cols = pgConnection.Sql().GetColumns<TestOrder>(ColumnSelection.NonKeys | ColumnSelection.Computed);
            Assert.AreEqual("[ProductId],[Count]", cols, "NonKeys");
        }

        [Test]
        public void TestGetColumnEqualParamProductPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var cols = pgConnection.Sql().GetColumnEqualParams<TestProduct>(ColumnSelection.Keys | ColumnSelection.NonKeys);
            Assert.AreEqual("\"id\"=@Id,\"Type\"=@Kind,\"Content\"=CAST(@Content AS json),\"Enum\"=@Enum,\"MaybeDate\"=@MaybeDate,\"Date\"=@Date,\"MaybeGuid\"=@MaybeGuid,\"Guid\"=@Guid,\"Duration\"=@Duration,\"Last\"=@Last", cols);
            cols = pgConnection.Sql().GetColumnEqualParams<TestProduct>(ColumnSelection.Keys);
            Assert.AreEqual("\"id\"=@Id", cols, "Keys");
            cols = pgConnection.Sql().GetColumnEqualParams<TestProduct>(ColumnSelection.NonKeys);
            Assert.AreEqual("\"Type\"=@Kind,\"Content\"=CAST(@Content AS json),\"Enum\"=@Enum,\"MaybeDate\"=@MaybeDate,\"Date\"=@Date,\"MaybeGuid\"=@MaybeGuid,\"Guid\"=@Guid,\"Duration\"=@Duration,\"Last\"=@Last", cols, "NonKeys");
        }

        [Test]
        public void TestGetColumnEqualParamSqlServer()
        {
            var sqlConnection = new SqlConnection();
            var cols = sqlConnection.Sql().GetColumnEqualParams<TestProduct>(ColumnSelection.Keys | ColumnSelection.NonKeys);
            Assert.AreEqual("[id]=@Id,[Type]=@Kind,[Content]=@Content,[Enum]=@Enum,[MaybeDate]=@MaybeDate,[Date]=@Date,[MaybeGuid]=@MaybeGuid,[Guid]=@Guid,[Duration]=@Duration,[Last]=@Last", cols);
            cols = sqlConnection.Sql().GetColumnEqualParams<TestProduct>(ColumnSelection.Keys);
            Assert.AreEqual("[id]=@Id", cols, "Keys");
            cols = sqlConnection.Sql().GetColumnEqualParams<TestProduct>(ColumnSelection.NonKeys);
            Assert.AreEqual("[Type]=@Kind,[Content]=@Content,[Enum]=@Enum,[MaybeDate]=@MaybeDate,[Date]=@Date,[MaybeGuid]=@MaybeGuid,[Guid]=@Guid,[Duration]=@Duration,[Last]=@Last", cols, "NonKeys");
        }

        [Test]
        public void TestInsertPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var insert = pgConnection.Sql().Insert<TestProduct>();
            Assert.AreEqual("INSERT INTO \"TestProducts\" (\"Type\",\"Content\",\"Enum\",\"MaybeDate\",\"Date\",\"MaybeGuid\",\"Guid\",\"Duration\",\"Last\") VALUES (@Kind,CAST(@Content AS json),@Enum,@MaybeDate,@Date,@MaybeGuid,@Guid,@Duration,@Last)", insert);
        }

        [Test]
        public void TestInsertKeysPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var insert = pgConnection.Sql().Insert<TestProduct>(true);
            Assert.AreEqual("INSERT INTO \"TestProducts\" (\"id\",\"Type\",\"Content\",\"Enum\",\"MaybeDate\",\"Date\",\"MaybeGuid\",\"Guid\",\"Duration\",\"Last\") VALUES (@Id,@Kind,CAST(@Content AS json),@Enum,@MaybeDate,@Date,@MaybeGuid,@Guid,@Duration,@Last)", insert);
        }

        [Test]
        public void TestInsertOrdersPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var insert = pgConnection.Sql().Insert<TestOrder>();
            Assert.AreEqual("INSERT INTO \"orders\" (\"ProductId\",\"Count\") VALUES (@ProductId,@Count)", insert);
        }

        [Test]
        public void TestInsertOrdersKeysPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var insert = pgConnection.Sql().Insert<TestOrder>(true);
            Assert.AreEqual("INSERT INTO \"orders\" (\"Id\",\"ProductId\",\"Count\") VALUES (@OrderId,@ProductId,@Count)", insert);
        }

        [Test]
        public void TestInsertSqlServer()
        {
            var sqlConnection = new SqlConnection();
            var insert = sqlConnection.Sql().Insert<TestProduct>();
            Assert.AreEqual("INSERT INTO [TestProducts] ([Type],[Content],[Enum],[MaybeDate],[Date],[MaybeGuid],[Guid],[Duration],[Last]) VALUES (@Kind,@Content,@Enum,@MaybeDate,@Date,@MaybeGuid,@Guid,@Duration,@Last)", insert);
        }

        [Test]
        public void TestInsertKeysSqlServer()
        {
            var sqlConnection = new SqlConnection();
            var insert = sqlConnection.Sql().Insert<TestProduct>(true);
            Assert.AreEqual("INSERT INTO [TestProducts] ([id],[Type],[Content],[Enum],[MaybeDate],[Date],[MaybeGuid],[Guid],[Duration],[Last]) VALUES (@Id,@Kind,@Content,@Enum,@MaybeDate,@Date,@MaybeGuid,@Guid,@Duration,@Last)", insert);
        }

        [Test]
        public void TestInsertReturnPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var cols = pgConnection.Sql().InsertReturn<TestProduct>();
            Assert.AreEqual("INSERT INTO \"TestProducts\" (\"Type\",\"Content\",\"Enum\",\"MaybeDate\",\"Date\",\"MaybeGuid\",\"Guid\",\"Duration\",\"Last\") VALUES (@Kind,CAST(@Content AS json),@Enum,@MaybeDate,@Date,@MaybeGuid,@Guid,@Duration,@Last) RETURNING \"id\" AS \"Id\"", cols);
        }

        [Test]
        public void TestInsertKeysReturnPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var cols = pgConnection.Sql().InsertReturn<TestProduct>(true);
            Assert.AreEqual("INSERT INTO \"TestProducts\" (\"id\",\"Type\",\"Content\",\"Enum\",\"MaybeDate\",\"Date\",\"MaybeGuid\",\"Guid\",\"Duration\",\"Last\") VALUES (@Id,@Kind,CAST(@Content AS json),@Enum,@MaybeDate,@Date,@MaybeGuid,@Guid,@Duration,@Last) RETURNING \"id\" AS \"Id\"", cols);
        }

        [Test]
        public void TestInsertReturnSqlServer()
        {
            var sqlConnection = new SqlConnection();
            var cols = sqlConnection.Sql().InsertReturn<TestProduct>();
            Assert.AreEqual("INSERT INTO [TestProducts] ([Type],[Content],[Enum],[MaybeDate],[Date],[MaybeGuid],[Guid],[Duration],[Last]) OUTPUT INSERTED.[id] AS [Id] VALUES (@Kind,@Content,@Enum,@MaybeDate,@Date,@MaybeGuid,@Guid,@Duration,@Last)", cols);
        }

        [Test]
        public void TestInsertKeysReturnSqlServer()
        {
            var sqlConnection = new SqlConnection();
            var cols = sqlConnection.Sql().InsertReturn<TestProduct>(true);
            Assert.AreEqual("INSERT INTO [TestProducts] ([id],[Type],[Content],[Enum],[MaybeDate],[Date],[MaybeGuid],[Guid],[Duration],[Last]) OUTPUT INSERTED.[id] AS [Id] VALUES (@Id,@Kind,@Content,@Enum,@MaybeDate,@Date,@MaybeGuid,@Guid,@Duration,@Last)", cols);
        }

        [Test]
        public void TestUpdatePostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var cols = pgConnection.Sql().Update<TestProduct>();
            Assert.AreEqual("UPDATE \"TestProducts\" SET \"Type\"=@Kind,\"Content\"=CAST(@Content AS json),\"Enum\"=@Enum,\"MaybeDate\"=@MaybeDate,\"Date\"=@Date,\"MaybeGuid\"=@MaybeGuid,\"Guid\"=@Guid,\"Duration\"=@Duration,\"Last\"=@Last WHERE \"id\"=@Id", cols);
        }

        [Test]
        public void TestUpdateOrdersPostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var cols = pgConnection.Sql().Update<TestOrder>();
            Assert.AreEqual("UPDATE \"orders\" SET \"ProductId\"=@ProductId,\"Count\"=@Count WHERE \"Id\"=@OrderId", cols);
        }

        [Test]
        public void TestUpdateSqlServer()
        {
            var sqlConnection = new SqlConnection();
            var cols = sqlConnection.Sql().Update<TestProduct>();
            Assert.AreEqual("UPDATE [TestProducts] SET [Type]=@Kind,[Content]=@Content,[Enum]=@Enum,[MaybeDate]=@MaybeDate,[Date]=@Date,[MaybeGuid]=@MaybeGuid,[Guid]=@Guid,[Duration]=@Duration,[Last]=@Last WHERE [id]=@Id", cols);
        }

        [Test]
        public void TestUpdateOrdersSqlServer()
        {
            var pgConnection = new SqlConnection();
            var cols = pgConnection.Sql().Update<TestOrder>();
            Assert.AreEqual("UPDATE [orders] SET [ProductId]=@ProductId,[Count]=@Count WHERE [Id]=@OrderId", cols);
        }

        [Test]
        public void TestDeletePostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var cols = pgConnection.Sql().Delete<TestProduct>();
            Assert.AreEqual("DELETE FROM \"TestProducts\" WHERE \"id\"=@Id", cols);
        }

        [Test]
        public void TestDeleteSqlServer()
        {
            var sqlConnection = new SqlConnection();
            var cols = sqlConnection.Sql().Delete<TestProduct>();
            Assert.AreEqual("DELETE FROM [TestProducts] WHERE [id]=@Id", cols);
        }
        
        [Test]
        public void TestMergePostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var cols = pgConnection.Sql().Merge<TestOrder>("unique_order");
            Assert.AreEqual("INSERT INTO \"orders\" (\"ProductId\",\"Count\") VALUES (@ProductId,@Count) ON CONFLICT(\"Id\",\"ProductId\") DO UPDATE \"orders\" SET \"ProductId\"=@ProductId,\"Count\"=@Count WHERE \"Id\"=@OrderId AND \"ProductId\"=@ProductId", cols);
        }

        [Test]
        public void TestMergeSqlServer()
        {
            var sqlConnection = new SqlConnection();
            var cols = sqlConnection.Sql().Merge<TestOrder>("unique_order");
            Assert.AreEqual("SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;BEGIN TRAN;IF EXISTS (SELECT * FROM [orders] WITH (UPDLOCK) WHERE [Id]=@OrderId AND [ProductId]=@ProductId) UPDATE [orders] SET [ProductId]=@ProductId,[Count]=@Count WHERE [Id]=@OrderId; ELSE INSERT INTO [orders] ([ProductId],[Count]) VALUES (@ProductId,@Count); COMMIT", cols);
        }

        [Test]
        public void TestOrdersTablePostgres()
        {
            var pgConnection = new NpgsqlConnection();
            var table = pgConnection.Sql().Table<TestOrder>();
            Assert.AreEqual("\"orders\"", table);
        }
        
        [Test]
        public void TestOrdersTableSqlServer()
        {
            var sqlConnection = new SqlConnection();
            var table = sqlConnection.Sql().Table<TestOrder>();
            Assert.AreEqual("[orders]", table);
        }
    }
}
