using System;
using System.Linq;
using System.Linq.Expressions;

namespace WishList.Data
{
    public class AppDbRepository : IAppDbRepository
    {
        #region Fields

        private readonly AppDbContext _appDbContext;

        #endregion Fields

        #region Constructors

        public AppDbRepository()
        {
        }

        #endregion Constructors

        #region Methods

        public IQueryable<T> Get<T>() where T : class
        {
            return _appDbContext.Set<T>();
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return _appDbContext.Set<T>().Where(predicate);
        }

        #endregion Methods
    }
}
