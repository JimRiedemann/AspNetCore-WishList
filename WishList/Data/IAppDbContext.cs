using Microsoft.EntityFrameworkCore;
using WishList.Models;

namespace WishList.Data
{
    public interface IAppDbContext
    {
        #region Properties

        DbSet<Wisher> Wishers { get; set; }

        DbSet<Wish> Wishes { get; set; }

        #endregion Properties
    }
}
