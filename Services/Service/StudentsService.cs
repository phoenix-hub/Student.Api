using Dapper;
using Student.Api.Model;
using Student.Api.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Student.Api.Services.Service
{
    public class StudentsService : IStudents
    {
        private readonly IConfiguration _config;
        public StudentsService(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<StudentDto> Get()
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("SqlConnection")))
            {
                if (connection.State == ConnectionState.Closed) connection.Open();
                return connection.Query<StudentDto>("select * from Students");
            }
        }

        public StudentDto GetOne(int id)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("SqlConnection")))
            {
                string strQuery = "select * from Students where id= @id";

                if (connection.State == ConnectionState.Closed) connection.Open();
                var stud = connection.QueryFirstOrDefault<StudentDto>(strQuery,
                    new { id = id });
                if (stud is null)
                {
                    throw new Exception("No Record found!");
                }
                return stud;
            }
        }
        public StudentDto SaveOne(StudentQuery student)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("SqlConnection")))
            {
                string query = "insert into Students ([DOB],[Mname],[Mnumber],[fName],[stuName]) values (@DOB, @Mname, @Mnumber, @fName, @stuName); SELECT CAST(SCOPE_IDENTITY() as int);";

                if (connection.State == ConnectionState.Closed) connection.Open();
                var stud = connection.ExecuteScalar(query, student);

                return new StudentDto()
                {
                    Id = (int)stud,
                    DOB = student.DOB,
                    fName = student.fName,
                    Mname = student.Mname,
                    Mnumber = student.Mnumber,
                    stuName = student.stuName
                };
            }
        }

        public bool SaveBulk(List<StudentQuery> students)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("SqlConnection")))
            {
                if (connection.State == ConnectionState.Closed) connection.Open();
                var trans = connection.BeginTransaction();
                try
                {
                    var stud = connection.Execute("insert into Students ([DOB],[Mname],[Mnumber],[fName],[stuName]) values (@DOB, @Mname, @Mnumber, @fName, @stuName)", students, trans);

                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return false;
                }
            }

        }

    }
}
