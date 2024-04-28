using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System;

namespace Project.Controllers
{
    public class LoginController : Controller
    {

        public readonly LoginContext _context;
        //private readonly LoginContext _context;

        public LoginController(LoginContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        //public IActionResult Login(LoginModel model)
        //{
        //    var myUser = _context.dblog.FromSqlRaw("SELECT * FROM [AUTH].[dbo].[LoginModel] WHERE [Id] = {0}", model.Id)
        //                               .FirstOrDefault();

        //    if (myUser != null && myUser.Password == model.Password)
        //    {
        //        return RedirectToAction("Dashboard", "Login");
        //    }

        //    ModelState.AddModelError(string.Empty, "Invalid user ID or password");
        //    return View(model);
        //}

        //public IActionResult Login(LoginModel model)
        //{
        //    var myUser = _context.dblog.FromSqlRaw("SELECT * FROM [AUTH].[dbo].[LoginModel] WHERE [Id] = {0}", model.Id)
        //                               .FirstOrDefault();

        //    if (myUser != null)
        //    {
        //        if (myUser.Password == model.Password)
        //        {
        //            return RedirectToAction("Dashboard", "Login");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Password is incorrect");
        //            // Clear only the password field in the model
        //            model.Password = string.Empty;
        //        }
        //        //ViewData["LoginFlag"] = "Invalid UserName and Password";
        //        //return View();
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "Username is invalid");
        //    }

        //    // Check if there are any model errors, if not, add a general error
        //    if (!ModelState.IsValid)
        //    {
        //        ViewData["LoginFlag"] = "User ID or password is wrong";
        //    }

        //    return View(model);
        //}

        public IActionResult Login(LoginModel model)
        {
            var myUser = _context.dblog.FromSqlRaw("SELECT * FROM [AUTH].[dbo].[LoginModel] WHERE [Id] = {0}", model.Id)
                                       .FirstOrDefault();

            if (myUser != null)
            {
                if (myUser.Password == model.Password)
                {
                    return RedirectToAction("Dashboard", "Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Password is invalid");
                    ViewData["LoginFlag"] = "Password is invalid";
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Username is invalid");
                ViewData["LoginFlag"] = "Username is invalid";
            }

            return View(model);
        }


        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        //public IActionResult Registration(RegistrationModel model)
        //    {
        //    if (ModelState.IsValid)
        //    {
        //        // Check if the user already exists
        //        var existingUser = _context.dblog.FromSqlRaw("SELECT * FROM [AUTH].[dbo].[LoginModel] WHERE [Id] = {0}", model.Id)
        //                                        .FirstOrDefault();

        //        if (existingUser != null)
        //        {
        //            ModelState.AddModelError("Id", "User ID already exists");
        //            return View(model);
        //        }
        //        try
        //        {
        //           var myuser= _context.dblog.FromSqlRaw("Insert Into LoginModel(Id,password) Values ({0}, {1})", model.Id, model.Password);
        //            if (myuser != null)
        //            {
        //                return RedirectToAction("Login");
        //            }

        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        // Save the new user to the database
        //       // return RedirectToAction("Login");
        //    }

        //    return View(model);
        //}
        public IActionResult Registration(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the user already exists
                var existingUser = _context.dblog
                    .FromSqlRaw("SELECT * FROM [AUTH].[dbo].[LoginModel] WHERE [Id] = {0}", model.Id)
                    .FirstOrDefault();

                if (existingUser != null)
                {
                    ModelState.AddModelError("Id", "User ID already exists");
                    return View(model);
                }

                try
                {
                    // Insert the new user into the database
                    _context.Database.ExecuteSqlRaw("INSERT INTO [AUTH].[dbo].[LoginModel] (Id, Password) VALUES ({0}, {1})", model.Id, model.Password);

                    // Redirect to the login page after successful registration
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately (log, display an error message, etc.)
                    ModelState.AddModelError("", "An error occurred while registering. Please try again.");
                    return View(model);
                }
            }

            return View(model);
        }

        public IActionResult Dashboard()
        {
            var data = _context.Persons.FromSqlRaw("SELECT [id],[firstname],[lastname],[age] FROM[AUTH].[dbo].[PersonModel]").ToList();
            ViewBag.data = data;
            return View();
        }



        //public IActionResult Index()
        //{
        //    IEnumerable<PersonModel> objCatlist = _context.Persons;
        //    return View(objCatlist);
        //}

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PersonModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Persons.Add(model);
                // _context.SaveChanges();
                //_context.Database.ExecuteSqlRaw("INSERT INTO [AUTH].[dbo].[PersonModel] (Id,Firstname,LastName,Age) VALUES ({0}, {1})", model.Id, model.FirstName, model.LastName, model.Age);
                _context.Database.ExecuteSqlRaw("INSERT INTO [AUTH].[dbo].[PersonModel] (Id, FirstName, LastName, Age) VALUES ({0}, {1}, {2}, {3})", model.Id, model.FirstName, model.LastName, model.Age);

                TempData["ResultOk"] = "Record Added Successfully !";
                return RedirectToAction("Dashboard");
            }

            return View(model);
        }
        public IActionResult Edit(int id)
        {
            // Fetch the PersonModel from the database based on the provided id
            var person = _context.Persons.FromSqlRaw("SELECT * FROM [AUTH].[dbo].[PersonModel] WHERE [Id] = {0}", id).FirstOrDefault();

            if (person == null)
            {
                return NotFound(); // Return a 404 Not Found response if the person is not found
            }

            return View(person); // Return the Edit view with the fetched person data
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PersonModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Persons.Add(model);
                    // _context.SaveChanges();
                    //_context.Database.ExecuteSqlRaw("INSERT INTO [AUTH].[dbo].[PersonModel] (Id,Firstname,LastName,Age) VALUES ({0}, {1})", model.Id, model.FirstName, model.LastName, model.Age);
                    //_context.Database.ExecuteSqlRaw("INSERT INTO [AUTH].[dbo].[PersonModel] (Id, FirstName, LastName, Age) VALUES ({0}, {1}, {2}, {3})", model.Id, model.FirstName, model.LastName, model.Age);

                    // Update the PersonModel in the database
                    _context.Database.ExecuteSqlRaw("UPDATE [AUTH].[dbo].[PersonModel] SET FirstName = {0}, LastName = {1}, Age = {2} WHERE Id = {3}",
                     model.FirstName, model.LastName, model.Age, model.Id);


                    TempData["ResultOk"] = "Record Updated Successfully !";
                    return RedirectToAction("Dashboard");
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately (log, display an error message, etc.)
                    ModelState.AddModelError("", "An error occurred while updating. Please try again.");
                }
            }

            return View(model);
        }

        // Delete Option work 

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var person = _context.Persons
        //        .FirstOrDefault(m => m.Id == id);
        //    if (person == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(person);
        //}

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Ensure the table name and schema are correct
            var person = _context.Persons
                .FromSqlRaw("SELECT * FROM [AUTH].[dbo].[PersonModel] WHERE [Id] = {0}", id)
                .FirstOrDefault();

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    var person = _context.Persons
        //       .FromSqlRaw("SELECT * FROM [AUTH].[dbo].[PersonModel] WHERE [Id] = {0}", id)
        //       .FirstOrDefault();
        //    if (person == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Persons.Remove(person);
        //    _context.SaveChanges();

        //    TempData["ResultOk"] = "Record Deleted Successfully!";
        //    return RedirectToAction(nameof(Dashboard));
        //}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var person = _context.Persons.FromSqlRaw("SELECT * FROM [AUTH].[dbo].[PersonModel] WHERE [Id] = {0}", id).FirstOrDefault();

                if (person == null)
                {
                    return NotFound();
                }

                // _context.Persons.Remove(person);
                //_context.SaveChanges();
                // _context.Database.ExecuteSqlRaw("DELETED INTO [AUTH].[dbo].[PersonModel] (Id, FirstName, LastName, Age) VALUES ({0}, {1}, {2}, {3})", model.Id, model.FirstName, model.LastName, model.Age);
                //team query 
                 _context.Database.ExecuteSqlRaw("DELETE FROM [AUTH].[dbo].[PersonModel] WHERE [Id] = {0}", id);
                TempData["ResultOk"] = "Record Deleted Successfully!";
                return RedirectToAction(nameof(Dashboard));
            }
            catch (DbUpdateException ex)
            {
                // Log or inspect the inner exception for more details
                // You can access the inner exception using ex.InnerException
                // Example: Log.Error(ex.InnerException, "An error occurred while saving changes");

                // You can also display an error message to the user
                ModelState.AddModelError("", "An error occurred while saving changes. Please try again.");

                return View(); // Return to the same view or redirect as appropriate
            }
        }



    }
}
