using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatShop.Data
{
public static class MockShopData
{
    public static Category[] categories = {
        new Category {
            //ID = 1,
            Name = "Baseball Hats",
            Description = "Fun Baseball Hats",
            BannerImageUrl = "/image/baseballBanner.jpg", Products =
            {
                new Product{
                    //ID = 1,
                    Name = "Duck Baseball Hat",
                    Price = 11.99m,
                    Description = "This is a duck Hat",
                    ProductColors =
                    {
                        new ProductColor
                        {
                            //ID = 1,
                            Color = "Blue/White"
                        }
                    },
                    ProductSizes =
                    {
                        new ProductSize {
                            //ID = 1,
                            Size = "Small" },
                        new ProductSize {
                            //ID = 2, 
                            Size = "Medium" },
                        new ProductSize {
                            //ID = 3, 
                            Size = "Large" }
                    }, Reviews =
                    {
                        new Review{
                            //ID = 1,
                            Rating = 5, Text = "I love this hat"},
                        new Review{
                            //ID = 2,
                            Rating = 1, Text = "I hate this hat"}
                    },
                    ProductImages =
                    {
                        new ProductImage{
                            //ID = 1, 
                            Url = "/image/hat1.png", AltText="Hat 1"}
                    }
                }
            }
        },
        new Category {
            //ID = 2,
            Name = "Stetson Hats",
            Description = "These are old-timey!",
            BannerImageUrl = "/image/stetsonBanner.jpg",
            Products = { new Product {
                //ID = 2, 
                Name = "Old Hat" } }
        },
        new Category {
            //ID = 3,
            Name = "Knit Hats",
            Description = "Keep Your Head Warm",
            BannerImageUrl = "/image/knitBanner.jpg"}
    };
}
}
