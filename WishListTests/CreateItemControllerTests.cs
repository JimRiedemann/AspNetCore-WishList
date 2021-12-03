using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace WishListTests
{
    public class CreateItemControllerTests
    {
        #region Methods

        [Fact(DisplayName = "Create WishController @create-itemcontroller")]
        public void CreateItemControllerTest()
        {
            // Get appropriate path to file for the current operating system
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "WishController.cs";
            // Assert Index.cshtml is in the Views/Home folder
            Assert.True(File.Exists(filePath), "`WishController.cs` was not found in the `Controllers` folder.");

            var controllerType = TestHelpers.GetUserType("WishList.Controllers.WishController");

            Assert.True(controllerType != null, "`WishController.cs` was found, but it appears it does not contain a `public` class `WishController`.");
            Assert.True(controllerType.BaseType == typeof(Controller), "`WishController` was found, but does not appear to inherit the `Controller` class from the `Microsoft.AspNetCore.Mvc` namespace.");

            var applicationDbContextType = TestHelpers.GetUserType("WishList.Data.ApplicationDbContext");

            Assert.True(applicationDbContextType != null, "class `ApplicationDbContext` was not found, this class should already exist in the `Data` folder, if you receive this you may have accidentally deleted or renamed it.");

            // Verify WishController contains a private property _context of type ApplicationDbContext
            var contextField = controllerType.GetField("_context", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.True(contextField != null, "`WishController` was found, but does not appear to contain a `private` `readonly` field `_context` of type `ApplicationDbConetext`.");

            // Verify WishController contains a constructor has a parameter of type ApplicationDbContext
            var constructor = controllerType.GetConstructor(new Type[] { applicationDbContextType });
            Assert.True(constructor != null, "`WishController` was found, but did not contain a constructor accepting a parameter of type `ApplicationDbContext`.");

            // Verify _context is set by the constructor's parameter
            string file;
            using (var streamReader = new StreamReader(filePath))
            {
                file = streamReader.ReadToEnd();
            }
            var pattern = @"public\s*WishController\s*?[(]\s*?ApplicationDbContext\s*context\s*?[)]\s*?{\s*?_context\s*?=\s*?context\s*?;\s*?}";
            var rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), "`WishController`'s constructor did not set the `_context` property to the provided `ApplicationDbContext` parameter.");
        }

        [Fact(DisplayName = "Create Item Create HttpGet Action @create-item-create-httpget-action")]
        public void CreateItemCreateHttpGetActionTest()
        {
            // Get appropriate path to file for the current operating system
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "WishController.cs";
            // Assert Index.cshtml is in the Views/Home folder
            Assert.True(File.Exists(filePath), "`WishController.cs` was not found in the `Controllers` folder.");

            var controllerType = TestHelpers.GetUserType("WishList.Controllers.WishController");

            Assert.True(controllerType != null, "`WishController.cs` was found, but it appears it does not contain a `public` class `WishController`.");

            // Verify Create Action Exists
            var method = controllerType.GetMethod("Create", new Type[] { });
            Assert.True(method != null, "`WishController` was found, but does not appear to contain an action `Create` with a return type of `IActionResult`.");

            // Verify Create has the correct return type
            Assert.True(method.ReturnType == typeof(IActionResult), "`WishController`'s `Create` action was found, but didn't have a return type of `IActionResult`.");

            // Verify Create has the HttpGet attribute
            Assert.True(method.CustomAttributes.Where(e => e.AttributeType == typeof(HttpGetAttribute)) != null, "`WishController`'s `Create` action was found, but does not appear to have the `HttpGet` attribute.");

            // Verify Create returns the "Create" view
            string file;
            using (var streamReader = new StreamReader(filePath))
            {
                file = streamReader.ReadToEnd();
            }
            var pattern = @"[[]HttpGet[\]]\s*?public\s*IActionResult\s*Create\s*?[(]\s*?[)]\s*?{\s*?return\s*View[(]\s*?(""Create"")?\s*?[)];\s*?}";
            var rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), "`WishController`'s `Create` (Get) action does not appear to be returning the 'Create' view.");
        }

        [Fact(DisplayName = "Create Item Create HttpPost Action @create-item-create-httppost-action")]
        public void CreateItemCreateHttpPostActionTest()
        {
            // Get appropriate path to file for the current operating system
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "WishController.cs";
            // Assert Index.cshtml is in the Views/Home folder
            Assert.True(File.Exists(filePath), "`WishController.cs` was not found in the `Controllers` folder.");

            var controllerType = TestHelpers.GetUserType("WishList.Controllers.WishController");

            Assert.True(controllerType != null, "`WishController.cs` was found, but it appears it does not contain a `public` class `WishController`.");

            var itemType = TestHelpers.GetUserType("WishList.Models.Item");

            Assert.True(itemType != null, "`item` was not found, `Item` should have been created in a previous step, have you accidentally deleted or renamed it?");

            // Verify Create Action Exists
            var method = controllerType.GetMethod("Create", new Type[] { itemType });
            Assert.True(method != null, "`WishController` was found, but does not appear to contain an action `Create` that accepts a parameter of type `Item` with a return type of `IActionResult`.");

            // Verify Create has the correct return type
            Assert.True(method.ReturnType == typeof(IActionResult), "`WishController`'s `Create` action was found, but didn't have a return type of `IActionResult`.");

            // Verify Create adds the provided Item to dbContext.Items
            Assert.True(method.CustomAttributes.Where(e => e.AttributeType == typeof(HttpPostAttribute)) != null, "`WishController`'s `Create` action was found, but does not appear to have the `HttpPost` attribute.");

            // Verify Create redirects to action to the Index action
            string file;
            using (var streamReader = new StreamReader(filePath))
            {
                file = streamReader.ReadToEnd();
            }
            var pattern = @"[[]HttpPost[\]]\s*?public\s* IActionResult\s* Create\s*?[(]\s*?((Wishlist[.])?Models[.]?)?Item\s* item\s*?[)]\s*?{\s*?_context[.](Items[.])?Add[(]\s*?item\s*?[)];\s*?_context[.]SaveChanges[(]\s*?[)];\s*?return\s* RedirectToAction[(]\s*?""Index""\s*?(,""Item"")?[)];\s*?}";
            var rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), "`WishController`'s `Create` (Post) action does not appear to be adding the provided `item` to `_context.Items`, `SaveChanges`, and then redirecting to the `Item`'s `Index` action.");
        }

        [Fact(DisplayName = "Create Item Delete Action @create-item-delete-action")]
        public void CreateItemDeleteActionTest()
        {
            // Get appropriate path to file for the current operating system
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "WishController.cs";
            // Assert Index.cshtml is in the Views/Home folder
            Assert.True(File.Exists(filePath), "`WishController.cs` was not found in the `Controllers` folder.");

            var controllerType = TestHelpers.GetUserType("WishList.Controllers.WishController");

            Assert.True(controllerType != null, "`WishController.cs` was found, but it appears it does not contain a `public` class `WishController`.");

            // Verify Delete Action Exists
            var method = controllerType.GetMethod("Delete");
            Assert.True(method != null, "`WishController` was found, but does not appear to contain an action `Delete` that accepts a parameter of type `int` with a return type of `IActionResult`.");

            // Verify Delete has the correct return type
            Assert.True(method.ReturnType == typeof(IActionResult), "`WishController`'s `Delete` action was found, but didn't have a return type of `IActionResult`.");

            // Verify Delete has the HttpPost attribute
            Assert.True(method.CustomAttributes.Where(e => e.AttributeType == typeof(HttpPostAttribute)) != null, "`WishController`'s `Create` action was found, but does not appear to have the `HttpPost` attribute.");

            // Verify Delete removes the matching item from dbContext.Items
            // Verify Delete redirects to action to the Index action

            string file;
            using (var streamReader = new StreamReader(filePath))
            {
                file = streamReader.ReadToEnd();
            }
            var pattern = @"public\s*IActionResult\s*Delete\s*?[(]\s*?int\s*id\s*?[)]\s*?{\s*?.*_context.Items.FirstOrDefault[(].*[.]Id\s*?==\s*?id\s*?[)];\s*?_context([.]Items)?[.]Remove[(]\s*?.*\s*?[)];\s*?_context[.]SaveChanges[(]\s*?[)];\s*?return\s*RedirectToAction[(]""Index""(,\s*?""Item"")?[)];\s*?}";
            var rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), "`WishController`'s `Delete` action does not appear to be removing the `Item` with the matching `Id` to the one provided from `_context.Items`, `SaveChanges`, and then redirecting to the `Item`'s `Index` action.");
        }

        [Fact(DisplayName = "Create Item Index Action @create-item-index-action")]
        public void CreateItemIndexActionTest()
        {
            // Get appropriate path to file for the current operating system
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "WishController.cs";
            // Assert Index.cshtml is in the Views/Home folder
            Assert.True(File.Exists(filePath), "`WishController.cs` was not found in the `Controllers` folder.");

            var controllerType = TestHelpers.GetUserType("WishList.Controllers.WishController");

            Assert.True(controllerType != null, "`WishController.cs` was found, but it appears it does not contain a `public` class `WishController`.");

            // Verify Index Action Exists
            var method = controllerType.GetMethod("Index");
            Assert.True(method != null, "`WishController` was found, but does not appear to contain an action `Index` with a return type of `IActionResult`.");

            // Verify Index has the correct return type
            Assert.True(method.ReturnType == typeof(IActionResult), "`WishController`'s `Index` action was found, but didn't have a return type of `IActionResult`.");

            string file;
            using (var streamReader = new StreamReader(filePath))
            {
                file = streamReader.ReadToEnd();
            }
            var pattern = @"public\s*IActionResult\s*Index\s*?[(]\s*?[)]\s*?{\s*?((var|List<Item>).*=\s*?_context.Items(;\s*?return\s*View[(](""Index"",)?.*[.]ToList[(]\s*?[)]\s*?[)];|[.]ToList[(]\s*?[)]\s*?;\s*?return\s*View\s*?[(]\s*?(""Index"",)?.*[)];)|return\s*View\s*?[(]\s*?(""Index"",)?\s*?_context[.]Items[.]ToList[(]\s*?[)]\s*?[)]\s*?[;])\s*?}";
            var rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), "`WishController`'s `Index` action does not appear to be getting all `Item`s from `_context.Items` converting it to type `List<Item>` and returning it as the model for the 'Index' view.");
        }

        #endregion Methods
    }
}
