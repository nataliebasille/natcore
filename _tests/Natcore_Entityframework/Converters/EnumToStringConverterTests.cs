using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Natcore_Entityframework.Converters
{
    [TestClass]
    public class EnumToStringConverterTests
    {
        [TestMethod]
        public async Task Can_read_and_write_enum_that_is_converted_to_a_string()
        {
            using (var context = TestContext.CreateContext())
            {
                var entity = new TestEntity
                {
                    EnumProp = TestEnum.Three
                };

                context.Add(entity);

                await context.SaveChangesAsync();

                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SELECT EnumProp From TestEntity";
                    context.Database.OpenConnection();

                    var result = command.ExecuteScalar();

                    result.Should().BeOfType<string>();
                    result.Should().Be(entity.EnumProp.ToString());
                }

                (await context.Set<TestEntity>().FirstOrDefaultAsync())
                    .Should()
                    .BeEquivalentTo(entity);
            }
        }
    }
}
