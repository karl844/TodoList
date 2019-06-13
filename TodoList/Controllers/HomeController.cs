using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return _signInManager.IsSignedIn(User) ? RedirectToAction("Index", "TodoList") : RedirectToAction("Login", "Account");
        }       

        public IActionResult Error(int? statusCode = null)
        {
            ErrorViewModel errorViewModel = new ErrorViewModel();

            if (statusCode.HasValue && statusCode.Value == 404)
            {
                errorViewModel.Title = "Opps";
                errorViewModel.Description = "The page you're looking for was not found.";               
            }
            return View(errorViewModel);
        }
    }
}
