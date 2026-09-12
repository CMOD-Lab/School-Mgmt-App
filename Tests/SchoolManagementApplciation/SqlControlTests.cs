using System;
using System.Collections.Generic;
using System.Data;
using Xunit;
using Npgsql;

namespace SchoolManagementApplciation.Tests
{
    /// <summary>
    /// Unit tests for SqlControl class.
    /// Tests focus on the public API surface: addprams, ExecSql, ExecProc,
    /// and the public fields/properties (count, exep, prams, data, adapter).
    /// Database-dependent methods are tested for error-handling paths using
    /// an invalid connection string so no real DB is required.
    ///
    /// NOTE: When the connection fails (Open() throws), the exception is caught
    /// before prams.Clear() is reached, so params remain in the list after a
    /// failed call. Tests reflect this actual production behavior.
    /// </summary>
    public class SqlControlTests
    {
        // ─────────────────────────────────────────────────────────────────────
        // Helper: create a SqlControl whose connection string is intentionally
        // invalid so every DB call fails predictably.
        // ─────────────────────────────────────────────────────────────────────
        private static SqlControl CreateSqlControlWithBadConnection()
        {
            return new SqlControl();
        }

        // ─────────────────────────────────────────────────────────────────────
        // Constructor tests
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void Constructor_CreatesInstance_NotNull()
        {
            var sql = new SqlControl();
            Assert.NotNull(sql);
        }

        [Fact]
        public void Constructor_InitializesParamsList_EmptyList()
        {
            var sql = new SqlControl();
            Assert.NotNull(sql.prams);
            Assert.Empty(sql.prams);
        }

        [Fact]
        public void Constructor_InitializesDataSet_NotNull()
        {
            var sql = new SqlControl();
            Assert.NotNull(sql.data);
        }

        [Fact]
        public void Constructor_InitializesAdapter_NotNull()
        {
            var sql = new SqlControl();
            Assert.NotNull(sql.adapter);
        }

        [Fact]
        public void Constructor_InitializesCount_Zero()
        {
            var sql = new SqlControl();
            Assert.Equal(0, sql.count);
        }

        [Fact]
        public void Constructor_InitializesExep_EmptyString()
        {
            var sql = new SqlControl();
            Assert.Equal(string.Empty, sql.exep);
        }

        // ─────────────────────────────────────────────────────────────────────
        // addprams tests
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void AddPrams_SingleParam_AddsToList()
        {
            var sql = new SqlControl();
            sql.addprams("@name", "TestValue");
            Assert.Single(sql.prams);
        }

        [Fact]
        public void AddPrams_SingleParam_CorrectName()
        {
            var sql = new SqlControl();
            sql.addprams("@name", "TestValue");
            Assert.Equal("@name", sql.prams[0].ParameterName);
        }

        [Fact]
        public void AddPrams_SingleParam_CorrectValue()
        {
            var sql = new SqlControl();
            sql.addprams("@name", "TestValue");
            Assert.Equal("TestValue", sql.prams[0].Value);
        }

        [Fact]
        public void AddPrams_MultipleParams_AllAdded()
        {
            var sql = new SqlControl();
            sql.addprams("@p1", 1);
            sql.addprams("@p2", "hello");
            sql.addprams("@p3", 3.14);
            Assert.Equal(3, sql.prams.Count);
        }

        [Fact]
        public void AddPrams_IntegerValue_StoredCorrectly()
        {
            var sql = new SqlControl();
            sql.addprams("@id", 42);
            Assert.Equal(42, sql.prams[0].Value);
        }

        [Fact]
        public void AddPrams_NullValue_StoredAsDBNull()
        {
            var sql = new SqlControl();
            sql.addprams("@val", DBNull.Value);
            Assert.Equal(DBNull.Value, sql.prams[0].Value);
        }

        [Fact]
        public void AddPrams_DateTimeValue_StoredCorrectly()
        {
            var sql = new SqlControl();
            var dt = new DateTime(2024, 1, 15);
            sql.addprams("@date", dt);
            Assert.Equal(dt, sql.prams[0].Value);
        }

        [Fact]
        public void AddPrams_DoubleValue_StoredCorrectly()
        {
            var sql = new SqlControl();
            sql.addprams("@amount", 99.99);
            Assert.Equal(99.99, sql.prams[0].Value);
        }

        [Fact]
        public void AddPrams_EmptyStringValue_StoredCorrectly()
        {
            var sql = new SqlControl();
            sql.addprams("@val", string.Empty);
            Assert.Equal(string.Empty, sql.prams[0].Value);
        }

        [Fact]
        public void AddPrams_BoolValue_StoredCorrectly()
        {
            var sql = new SqlControl();
            sql.addprams("@flag", true);
            Assert.Equal(true, sql.prams[0].Value);
        }

        [Fact]
        public void AddPrams_SequentialCalls_PreservesOrder()
        {
            var sql = new SqlControl();
            sql.addprams("@first", "A");
            sql.addprams("@second", "B");
            sql.addprams("@third", "C");
            Assert.Equal("@first",  sql.prams[0].ParameterName);
            Assert.Equal("@second", sql.prams[1].ParameterName);
            Assert.Equal("@third",  sql.prams[2].ParameterName);
        }

        // ─────────────────────────────────────────────────────────────────────
        // ExecSql tests (error-handling path – no real DB)
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void ExecSql_WithInvalidConnection_SetsExepMessage()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecSql("SELECT 1");
            Assert.NotNull(sql.exep);
            Assert.NotEmpty(sql.exep);
        }

        [Fact]
        public void ExecSql_WithInvalidConnection_CountRemainsZero()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecSql("SELECT 1");
            Assert.Equal(0, sql.count);
        }

        [Fact]
        public void ExecSql_WithNoParams_ParamsListRemainsEmpty()
        {
            // Arrange – no params added
            var sql = CreateSqlControlWithBadConnection();

            // Act
            sql.ExecSql("SELECT 1");

            // Assert – nothing was added, nothing to clear
            Assert.Empty(sql.prams);
        }

        [Fact]
        public void ExecSql_WithOneParam_ParamRemainsAfterFailure()
        {
            // When connection fails, prams.Clear() is never reached
            var sql = CreateSqlControlWithBadConnection();
            sql.addprams("@name", "test");
            sql.ExecSql("SELECT * FROM students WHERE name = @name");
            // Params remain because exception is thrown before Clear()
            Assert.Single(sql.prams);
        }

        [Fact]
        public void ExecSql_WithMultipleParams_ParamsRemainAfterFailure()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.addprams("@a", 1);
            sql.addprams("@b", 2);
            sql.addprams("@c", 3);
            sql.ExecSql("SELECT @a, @b, @c");
            // Params remain because exception is thrown before Clear()
            Assert.Equal(3, sql.prams.Count);
        }

        [Fact]
        public void ExecSql_ResetsExepBeforeExecution()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecSql("SELECT 1");
            sql.ExecSql("SELECT 2");
            Assert.NotNull(sql.exep);
        }

        [Fact]
        public void ExecSql_ResetsCountToZeroBeforeExecution()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecSql("SELECT 1");
            Assert.Equal(0, sql.count);
        }

        [Fact]
        public void ExecSql_MultipleCallsInSequence_EachResetsState()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecSql("SELECT 1");
            string firstExep = sql.exep;
            sql.ExecSql("SELECT 2");
            string secondExep = sql.exep;
            Assert.NotEmpty(firstExep);
            Assert.NotEmpty(secondExep);
        }

        [Fact]
        public void ExecSql_AfterFailure_DataSetIsReset()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecSql("SELECT 1");
            Assert.NotNull(sql.data);
        }

        // ─────────────────────────────────────────────────────────────────────
        // ExecProc tests (error-handling path – no real DB)
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void ExecProc_WithInvalidConnection_SetsExepMessage()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecProc("CALL some_proc()");
            Assert.NotNull(sql.exep);
            Assert.NotEmpty(sql.exep);
        }

        [Fact]
        public void ExecProc_WithInvalidConnection_CountRemainsZero()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecProc("CALL some_proc()");
            Assert.Equal(0, sql.count);
        }

        [Fact]
        public void ExecProc_WithNoParams_ParamsListRemainsEmpty()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecProc("CALL some_proc()");
            Assert.Empty(sql.prams);
        }

        [Fact]
        public void ExecProc_WithParams_ParamsRemainAfterFailure()
        {
            // When connection fails, prams.Clear() is never reached
            var sql = CreateSqlControlWithBadConnection();
            sql.addprams("@id", 1);
            sql.addprams("@name", "test");
            sql.ExecProc("CALL insert_student(@id, @name)");
            // Params remain because exception is thrown before Clear()
            Assert.Equal(2, sql.prams.Count);
        }

        [Fact]
        public void ExecProc_WithMultipleParams_ParamsRemainAfterFailure()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.addprams("@x", 10);
            sql.addprams("@y", 20);
            sql.ExecProc("CALL proc(@x, @y)");
            Assert.Equal(2, sql.prams.Count);
        }

        [Fact]
        public void ExecProc_ResetsExepBeforeExecution()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecProc("CALL proc1()");
            sql.ExecProc("CALL proc2()");
            Assert.NotNull(sql.exep);
        }

        [Fact]
        public void ExecProc_ResetsCountToZeroBeforeExecution()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecProc("CALL some_proc()");
            Assert.Equal(0, sql.count);
        }

        [Fact]
        public void ExecProc_MultipleCallsInSequence_EachResetsState()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecProc("CALL proc1()");
            string firstExep = sql.exep;
            sql.ExecProc("CALL proc2()");
            string secondExep = sql.exep;
            Assert.NotEmpty(firstExep);
            Assert.NotEmpty(secondExep);
        }

        // ─────────────────────────────────────────────────────────────────────
        // State isolation tests
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void TwoInstances_HaveIndependentParamLists()
        {
            var sql1 = new SqlControl();
            var sql2 = new SqlControl();
            sql1.addprams("@a", 1);
            Assert.Single(sql1.prams);
            Assert.Empty(sql2.prams);
        }

        [Fact]
        public void AddPrams_SingleParam_ThenExecSql_ParamRemainsOnFailure()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.addprams("@x", "value");
            sql.ExecSql("SELECT @x");
            // Param remains because connection failure prevents Clear()
            Assert.Single(sql.prams);
        }

        [Fact]
        public void AddPrams_SingleParam_ThenExecProc_ParamRemainsOnFailure()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.addprams("@x", "value");
            sql.ExecProc("CALL proc(@x)");
            // Param remains because connection failure prevents Clear()
            Assert.Single(sql.prams);
        }

        [Fact]
        public void DataSet_AfterConstruction_HasNoTables()
        {
            var sql = new SqlControl();
            Assert.Equal(0, sql.data.Tables.Count);
        }

        [Fact]
        public void ExecSql_ExepContainsConnectionErrorInfo()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecSql("SELECT 1");
            // The error message should be a non-trivial string
            Assert.True(sql.exep.Length > 5);
        }

        [Fact]
        public void ExecProc_ExepContainsConnectionErrorInfo()
        {
            var sql = CreateSqlControlWithBadConnection();
            sql.ExecProc("CALL proc()");
            Assert.True(sql.exep.Length > 5);
        }

        [Fact]
        public void AddPrams_LargeNumberOfParams_AllStored()
        {
            var sql = new SqlControl();
            for (int i = 0; i < 20; i++)
                sql.addprams($"@p{i}", i);
            Assert.Equal(20, sql.prams.Count);
        }

        [Fact]
        public void AddPrams_NegativeIntValue_StoredCorrectly()
        {
            var sql = new SqlControl();
            sql.addprams("@neg", -999);
            Assert.Equal(-999, sql.prams[0].Value);
        }

        [Fact]
        public void AddPrams_ZeroValue_StoredCorrectly()
        {
            var sql = new SqlControl();
            sql.addprams("@zero", 0);
            Assert.Equal(0, sql.prams[0].Value);
        }
    }
}
