using Dapper;
using DapperTemplate.DataAccess.Repository.Abstraction;
using DapperTemplate.Helper;
using DapperTemplate.Models.Classes;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTemplate.DataAccess.Repository.Implementations
{
    public class ClassRepository : IClassRepository
    {

        private readonly DbHelper _dbHelper;
        public ClassRepository(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public async Task<bool> Create(Class cls)
        {
            int affectedRows;
            using (var conn = new SqlConnection(_dbHelper.GetDatabaseConnection()))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Name", cls.Name);
                parameters.Add("@Code", cls.Code);

                conn.Open();
                affectedRows = await conn
                    .ExecuteAsync(
                    sql: "spCreateClass",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);

            }

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Class>> GetAll(
            int? pageIndex = null,
            int? pageSize = null,
            string search = null,
            bool? desc = null)
        {
            IEnumerable<Class> users;
            using (var conn = new SqlConnection(_dbHelper.GetDatabaseConnection()))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@PageSize", pageSize);
                parameters.Add("@PageIndex", pageIndex);
                parameters.Add("@SearchText", search);
                parameters.Add("@SearchColumn", "Name");
                parameters.Add("@SelectColumns", " Id, Name, Code ");
                parameters.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Command", dbType: DbType.String, direction: ParameterDirection.Output, size: 1000);

                if (desc.HasValue && desc.Value)
                {
                    parameters.Add("@SortOrder", "DESC");
                }
                else if (desc.HasValue && !desc.Value)
                {
                    parameters.Add("@SortOrder", "ASC");
                }

                conn.Open();
                users = await conn.QueryAsync<Class>("spGetClass",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (users.Count() > 0)
                {
                    users.First().Total = parameters.Get<int>("Total");
                }

                var command = parameters.Get<string>("Command");

            }

            return users;
        }

        public Task<Class> GetById(int id)
        {
            throw new System.NotImplementedException();  
        }

        public async Task<bool> Delete(int id)
        {
            int affectedRows;
            using (var conn = new SqlConnection(_dbHelper.GetDatabaseConnection()))
            {
                conn.Open();
                affectedRows = await conn.ExecuteAsync(
                    "spDeleleteClass",
                    id,
                    commandType: CommandType.StoredProcedure);
            }

            return affectedRows > 0;
        }

        public async Task<bool> Update(Class cls)
        {
            int affectedRows;
            using (var conn = new SqlConnection(_dbHelper.GetDatabaseConnection()))
            {
                conn.Open();
                affectedRows = await conn.ExecuteAsync(
                    "spUpdateClass",
                    new
                    {
                        Id = cls.Id,
                        Name = cls.Name,
                        Address = cls.Code,
                    },
                    commandType: CommandType.StoredProcedure);
            }


            return affectedRows > 0;
        }
    }
}
