using System;
using Microsoft.EntityFrameworkCore;
using ProductManagementApp.Infrastructure.Persistence;
using webapi.Domain.Entities;

namespace webapi.Infrastructure.Persistance;

public class SeedData
{
    public static void Seed(AppDbContext context)
    {
        context.Database.Migrate();

        if (!context.ProductTypes.Any())
        {
            var productTypes = File.ReadAllLines("./Infrastructure/Persistance/Data/product-type.txt");
            foreach (var productType in productTypes)
            {
                context.ProductTypes.Add(new ProductType { Name = productType });
            }
            context.SaveChanges();
        }

        if (!context.Colours.Any())
        {
            var colours = File.ReadAllLines("./Infrastructure/Persistance/Data/colour.txt");
            foreach (var colour in colours)
            {
                context.Colours.Add(new Colour { Name = colour });
            }
            context.SaveChanges();
        }
    }
}
