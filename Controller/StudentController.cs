using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student.Api.Model;
using Student.Api.Services.Interfaces;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Student.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IStudents students;
        public StudentController(IConfiguration config, IStudents students)
        {
            _config = config;
            this.students = students;
        }
        [HttpGet]
        public ActionResult<List<StudentDto>> Get()
        {
            return Ok(this.students.Get());
        }

        [HttpGet]
        [Route("GetOne")]
        public ActionResult<StudentDto> GetOne(int id)
        {
            return Ok(this.students.GetOne(id));
        }

        [HttpPost]
        [Route("SaveOne")]
        public ActionResult<StudentDto> SaveOne(StudentQuery student)
        {
            return Ok(this.students.SaveOne(student));
        }

        [HttpPost]
        [Route("SaveBulk")]
        public ActionResult<StudentDto> SaveBulk(List<StudentQuery> student)
        {
            return Ok(this.students.SaveBulk(student));
        }

        [HttpGet]
        [Route("ExportExcel")]
        public ActionResult ExporttoExcel()
        {
            var arraylist = this.students.Get(); 
            var writer = new System.Xml.Serialization.XmlSerializer(arraylist.GetType());
            var stream = new MemoryStream();
            writer.Serialize(stream, arraylist);

            Random rng = new Random();
            int value = rng.Next(1000);
            string text = value.ToString("000");
            var fileName = string.Format("Student-{0}.xls", text);

            return File(stream.ToArray(), "application/octet-stream", fileName);
        }
    }
}
