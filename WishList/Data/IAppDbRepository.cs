using System;
using System.Linq;
using System.Linq.Expressions;

namespace WishList.Data
{
    public interface IAppDbRepository
    {
        #region Methods

        /// <summary>
        /// This method returns an IQueryable object representing the full contents of a table.
        /// </summary>
        /// <typeparam name="T">the name of the table model</typeparam>
        /// <returns>IQueryable<T></returns>
        IQueryable<T> Get<T>() where T : class;

        /// <summary>
        /// This method returns an IQueryable object representing the contents of a table as filtered by the predicate.
        /// </summary>
        /// <typeparam name="T">the name of the table model</typeparam>
        /// <param name="predicate">a boolean function expression, such as (x => x.column == value)</param>
        /// <returns>IQueryable<T></returns>
        IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class;

        #endregion Methods
    }
}
