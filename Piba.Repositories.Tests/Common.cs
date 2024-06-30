using Microsoft.EntityFrameworkCore;
using Piba.Data;

namespace Piba.Repositories.Tests
{
    public class Common
    {
        public static PibaDbContext GenerateInMemoryDatabase(string databaseName)
        {
            var dbContextBuilder = new DbContextOptionsBuilder<PibaDbContext>()
                 .UseInMemoryDatabase(databaseName);

            var context =  new PibaDbContext(dbContextBuilder.Options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
