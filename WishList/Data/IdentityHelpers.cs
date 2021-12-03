using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WishList.Data
{
    public static class IdentityHelpers
    { // Code found at https://stackoverflow.com/questions/40896047/how-to-turn-on-identity-insert-in-net-core
        #region Methods

        public static void DisableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, false);

        public static async Task DisableIdentityInsertAsync<T>(this DbContext context) => await SetIdentityInsertAsync<T>(context, false);

        public static void EnableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, true);

        public static async Task EnableIdentityInsertAsync<T>(this DbContext context) => await SetIdentityInsertAsync<T>(context, true);

        public static void SaveChangesWithIdentityInsert<T>([NotNull] this DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            using var transaction = context.Database.BeginTransaction();
            context.EnableIdentityInsert<T>();
            context.SaveChanges();
            context.DisableIdentityInsert<T>();
            transaction.Commit();
        }

        public static async Task SaveChangesWithIdentityInsertAsync<T>([NotNull] this DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            await using var transaction = await context.Database.BeginTransactionAsync();
            await context.EnableIdentityInsertAsync<T>();
            await context.SaveChangesAsync();
            await context.DisableIdentityInsertAsync<T>();
            await transaction.CommitAsync();
        }

        #endregion Methods

        #region Internal Methods

        private static void SetIdentityInsert<T>([NotNull] DbContext context, bool enable)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var entityType = context.Model.FindEntityType(typeof(T));
            var value = enable ? "ON" : "OFF";
            context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} {value}");
        }

        private static async Task SetIdentityInsertAsync<T>([NotNull] DbContext context, bool enable)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var entityType = context.Model.FindEntityType(typeof(T));
            var value = enable ? "ON" : "OFF";
            await context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} {value}");
        }

        #endregion Internal Methods
    }
}
