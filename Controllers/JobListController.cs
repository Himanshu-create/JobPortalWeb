using JobPortalWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace JobPortalWeb.Controllers
{
    public class JobListController : Controller
    {
        IConfiguration _configuration;
        public JobListController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult JobList()
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

        public class AllJob
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Domain { get; set; }

            public DateTime DateOfReg { get; set; }


        }



        public IActionResult JobApply()
        {

            var jID = Request.Query["id"];

            ViewBag.JobID = jID;



            string conString = _configuration.GetConnectionString("AllJobDB");
            //Console.WriteLine("Envvar" + _configuration["mykey"]);
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = conString;
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM JobList WHERE JobId = 1";

            //Console.WriteLine(jID);

            SqlDataReader reader = cmd.ExecuteReader();

            
          
            AllJob jobDetails = new AllJob();
            while (reader.Read())
            {
                jobDetails.Id = (int)reader["JobID"];
                jobDetails.Name = (string)reader["JobName"];
                jobDetails.Domain = (string)reader["JobDomain"];
                jobDetails.DateOfReg = (DateTime)reader["LastDateForRegistration"];

            }
     

            ViewBag.JobDetails = jobDetails;
            conn.Close();


            return View();



        }


        [HttpPost]
        public ActionResult JobApply(JobApplicant applicant)
        {
            var jID = Request.Query["id"];
            ViewBag.JobID = jID;

            int jobID = applicant.Jobid;
            string name = applicant.name;
            string emailID = applicant.emialid;
            int phoneNo = applicant.phoneNo;
            string collegeName = applicant.collegeName;


            Console.WriteLine("INTO HTTPPOST");
            //Writing to DB

            string conString = _configuration.GetConnectionString("AllJobDB");
            //Console.WriteLine("Envvar" + _configuration["mykey"]);
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = conString;
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
  
            cmd.CommandText = $"INSERT INTO Applicants values ('{name}','{collegeName}','{emailID}',{phoneNo},{jID})";

            Console.WriteLine($"INSERT INTO Applicants values ('{name}','{collegeName}','{emailID}',{phoneNo},{jID})");
            try
            {
                int re = cmd.ExecuteNonQuery();
                Console.WriteLine("DATA UPDATED!");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message ); 
            }

            return View();
        }

        
    }
}
