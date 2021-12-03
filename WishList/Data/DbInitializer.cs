using System;
using System.Linq;
using WishList.Models;
using WishList.Data;

namespace WishList.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any wish makers.
            if (context.Wishers.Any())
            {
                return;   // DB has been seeded
            }

            var wishers = new Wisher[]
            {
                new Wisher{WisherId = 1, Name = "Fisherman & His Wife", BirthDate=DateTime.Parse("1812-12-20")},
                new Wisher{WisherId = 2, Name = "Mr. & Mrs. White"    , BirthDate=DateTime.Parse("1902-09-01")},
                new Wisher{WisherId = 3, Name = "Aladdin"             , BirthDate=DateTime.Parse("0100-09-01")},
                new Wisher{WisherId = 4, Name = "Major Tony Nelson"   , BirthDate=DateTime.Parse("1931-09-21")},
            };
            foreach (var w in wishers)
            {
                context.Wishers.Add(w);
            }

            context.SaveChangesWithIdentityInsert<Wisher>();

            var wishes = new Wish[]
            {
                new Wish { WishId =  1, WisherId = 4, WishOrder = 1, Description = "I wish you could speak English." },
                new Wish { WishId =  2, WisherId = 1, WishOrder = 4, Description = "I wish my wife was Bishop" },
                new Wish { WishId =  3, WisherId = 1, WishOrder = 3, Description = "I wish my wife was Burgher" },
                new Wish { WishId =  4, WisherId = 3, WishOrder = 2, Description = "I wish to be rescued from the bottom of the ocean." },
                new Wish { WishId =  5, WisherId = 3, WishOrder = 1, Description = "I wish to be a Prince." },
                new Wish { WishId =  6, WisherId = 2, WishOrder = 2, Description = "I wish my son alive again." },
                new Wish { WishId =  7, WisherId = 3, WishOrder = 3, Description = "I wish Genie was free." },
                new Wish { WishId =  8, WisherId = 1, WishOrder = 5, Description = "I wish my wife was King" },
                new Wish { WishId =  9, WisherId = 1, WishOrder = 2, Description = "I wish for a castle for my wife." },
                new Wish { WishId = 10, WisherId = 1, WishOrder = 1, Description = "I wish for a nice little cottage for my wife." },
                new Wish { WishId = 11, WisherId = 2, WishOrder = 1, Description = "I wish for £200." },
                new Wish { WishId = 12, WisherId = 1, WishOrder = 6, Description = "I wish my wife was Emperor" },
                new Wish { WishId = 13, WisherId = 2, WishOrder = 3, Description = "I wish my son dead again and back in his grave." },
            };
            foreach (var w in wishes)
            {
                context.Wishes.Add(w);
            }

            context.SaveChangesWithIdentityInsert<Wish>();
        }
    }
}
