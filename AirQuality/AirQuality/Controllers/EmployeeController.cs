using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using AirQuality.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace AirQuality.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select EmployeeId, EmployeeName, EmployeeDepartment, convert(varchar(10), DateOfJoining, 120) as DateOfJoining , PhotoFileName from
                            dbo.Employee
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LearningDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"
                            insert into dbo.Employee
                            (EmployeeName, EmployeeDepartment,DateOfJoining,PhotoFileName)
                            values (@EmployeeName, @EmployeeDepartment,@DateOfJoining,@PhotoFileName)
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LearningDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    myCommand.Parameters.AddWithValue("@EmployeeDepartment", employee.EmployeeDepartment);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Success Added to table");
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string query = @"
                            update dbo.Employee
                            set EmployeeName =  @EmployeeName, 
                                EmployeeDepartment = @EmployeeDepartment, 
                                DateOfJoining = DateOfJoining, 
                                PhotoFileName = @PhotoFileName
                                    where EmployeeId = @EmployeeId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LearningDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    myCommand.Parameters.AddWithValue("@EmployeeDepartment", employee.EmployeeDepartment);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Success Updated item");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            delete from dbo.Employee
                                where EmployeeId = @EmployeeId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LearningDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Success deleted item");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream (physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("annoymous.png");
            }
        }
    }
}
