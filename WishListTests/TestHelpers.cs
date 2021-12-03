using System;
using System.Linq;

namespace WishListTests
{
    public static class TestHelpers
    {
        #region Fields

        private static readonly string _projectName = "WishList";

        #endregion Fields

        #region Methods

        public static Type GetUserType(string fullName)
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    where assembly.FullName.StartsWith(_projectName)
                    from type in assembly.GetTypes()
                    where type.FullName == fullName
                    select type).FirstOrDefault();
        }

        #endregion Methods
    }
}
