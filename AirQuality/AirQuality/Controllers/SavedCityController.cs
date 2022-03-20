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

namespace AirQuality.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavedCityController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public SavedCityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select CityId, CityName, CountryName, LastUpdated, Parameters from
                            dbo.SavedCities
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
        public JsonResult Post(SavedCity city)
        {
            string query = @"
                            insert into dbo.SavedCities
                            (CityName, CountryName,LastUpdated,Parameters)
                            values (@CityName,@CountryName,@LastUpdated,@Parameters)
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LearningDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@CityName", city.CityName);
                    myCommand.Parameters.AddWithValue("@CountryName", city.CountryName);
                    myCommand.Parameters.AddWithValue("@LastUpdated", city.LastUpdated);
                    myCommand.Parameters.AddWithValue("@Parameters", city.Parameters);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Success Added to table");
        }

        [HttpPut]
        public JsonResult Put(SavedCity city)
        {
            string query = @"
                            update dbo.Employee
                            set CityName =  @CityName, 
                                CountryName = @CountryName, 
                                LastUpdated = LastUpdated, 
                                Parameters = @Parameters
                                    where CityId = @CityId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LearningDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@CityId", city.CityId);
                    myCommand.Parameters.AddWithValue("@CityName", city.CityName);
                    myCommand.Parameters.AddWithValue("@CountryName", city.CountryName);
                    myCommand.Parameters.AddWithValue("@LastUpdated", city.LastUpdated);
                    myCommand.Parameters.AddWithValue("@Parameters", city.Parameters);
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
                            delete from dbo.SavedCities
                                where CityId = @CityId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LearningDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@CityId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Success deleted item");
        }
    }
}
