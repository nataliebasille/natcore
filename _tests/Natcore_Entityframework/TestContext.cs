using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Natcore.EntityFramework.Converters;
using System;

namespace Natcore_Entityframework
{
    public class TestContext : DbContext, IDisposable
    {
        private readonly SqliteConnection _connection;
        private TestContext(SqliteConnection connection, DbContextOptions<TestContext> options)
            : base(options) 
        {
            _connection = connection;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var testEntityBuilder = modelBuilder.Entity<TestEntity>();
            testEntityBuilder.HasKey(x => x.Key);

            testEntityBuilder.Property(x => x.EnumProp)
                .HasConversion(ValueConverters.EnumToString<TestEnum>());
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_connection.State == System.Data.ConnectionState.Open)
                _connection.Close();

            _connection.Dispose();
        }

        public static TestContext CreateContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<TestContext>()
                .UseSqlite(connection)
                .Options;

            var context = new TestContext(connection, options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
