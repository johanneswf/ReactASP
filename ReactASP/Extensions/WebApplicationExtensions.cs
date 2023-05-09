using ReactASP.Data;
using Microsoft.EntityFrameworkCore;

namespace ReactASP.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetRequiredService<ApplicationDbContext>();
                ArgumentNullException.ThrowIfNull(nameof(db));

                //db.Database.EnsureDeleted();
                //db.Database.Migrate();

                try
                {
                    await SeedData.InitAsync(db, serviceProvider);
                }
                catch (Exception e)
                {
                    //ToDo: Log errors when seeding!
                    throw;
                }
            }

        }
    }
}
