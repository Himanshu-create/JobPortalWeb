using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using static JobPortalWeb.Controllers.JobListController;

namespace JobPortalWeb.Controllers
{

   
    public class AdminController : Controller
    {

        IConfiguration _configuration;
        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {

            string conString = _configuration.GetConnectionString("AllJobDB");
            //Console.WriteLine("Envvar" + _configuration["mykey"]);
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = conString;
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM JobList";
            SqlDataReader reader = cmd.ExecuteReader();

            List<AllJob> allJobList = new();
            while (reader.Read())
            {
                AllJob alljob = new AllJob();
                alljob.Id = (int)reader["JobID"];
                alljob.Name = (string)reader["JobName"];
                alljob.Domain = (string)reader["JobDomain"];
                alljob.DateOfReg = (DateTime)reader["LastDateForRegistration"];


                allJobList.Add(alljob);
            }
            ViewBag.allJobList = allJobList;
            conn.Close();
            return View();
        }

        public IActionResult ViewAllApplicant()
        {

            var jID = Request.Query["id"];
            ViewBag.JobID = jID;

            string conString = _configuration.GetConnectionString("AllJobDB");
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = conString;
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM Applicants WHERE Jobid = {jID}";
            SqlDataReader reader = cmd.ExecuteReader();

            List<ApplicantsDetails> allApplicant = new();
            while (reader.Read())
            {
                ApplicantsDetails ad = new ApplicantsDetails();
                ad.Id = (int)reader["ApplicantID"];
                ad.Name = (string)reader["name"];
                ad.PhoneNo = (int)reader["phoneNo"];
                //ad.email = (string)reader["email"];
                ad.CollegeName = (string)reader["collegeName"];

                allApplicant.Add(ad);

                //Console.WriteLine(reader["emailid"]);

            }
            
            ViewBag.allApplicants = allApplicant;
            conn.Close();

            return View();
        }

        public class ApplicantsDetails
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string CollegeName { get; set; }
            public int PhoneNo { get; set; }
            public string email = "EMAIl ID";
                
        }
    }
}
