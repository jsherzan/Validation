using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Validation.Models;
using Microsoft.Extensions.Configuration;

namespace Validation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Registration()
        {
            var model = new RegistraionModel();
            model.ContactPreferences = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            model.ContactPreferences.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "Phone", Value = "1", Selected = false });
            model.ContactPreferences.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "Email", Value = "2", Selected = false });
            model.ContactPreferences.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "Postal", Value = "3", Selected = false });

            return View(model);
        }

        [HttpPost]
        public IActionResult Registration(RegistraionModel model)
        {
            string connectionString = _configuration.GetConnectionString("registrationConnection");

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("usp_insert", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("FirstName", model.FirstName);
                    cmd.Parameters.AddWithValue("Address", model.Address);
                    cmd.Parameters.AddWithValue("PhoneNumber", model.PhoneNumber);
                    cmd.Parameters.AddWithValue("Email", model.Email);
                    cmd.Parameters.AddWithValue("Age", model.Age);
                    cmd.Parameters.AddWithValue("ContactPreference", model.ContactPreference);

                    cmd.ExecuteNonQuery();
                }
            }
                return RedirectToAction("ViewFormData", model);
        }
        public IActionResult ViewFormData(RegistraionModel model)
        {
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
