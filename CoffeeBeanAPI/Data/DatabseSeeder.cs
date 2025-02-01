using CoffeeBeanAPI.Models;
using Newtonsoft.Json;
using System;

namespace CoffeeBeanAPI.Data
{


    public class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Ensure database is created
                await context.Database.EnsureCreatedAsync();

                // Check if there are existing beans
                if (!context.Beans.Any())
                {
                    string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "AllTheBeans.json");

                    if (File.Exists(jsonFilePath))
                    {
                        var jsonData = await File.ReadAllTextAsync(jsonFilePath);
                        var beans = JsonConvert.DeserializeObject<List<Bean>>(jsonData);


                        // Add to database
                        await context.Beans.AddRangeAsync(beans);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }

}
