
using Dapper;
using DapperTemplate.Abstracts;
using DapperTemplate.Helper;
using DapperTemplate.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTemplate.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbHelper _dbHelper;
        public UserRepository(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public async Task<bool> Create(User user)
        {
            int affectedRows;
            using (var conn = new SqlConnection(_dbHelper.GetDatabaseConnection()))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Name", user.Name);
                parameters.Add("@Username", user.Username);
                parameters.Add("@Password", user.Password);
                parameters.Add("@Address", user.Address);
                parameters.Add("@Age", user.Age);
                parameters.Add("@CreatedDate", DateTime.UtcNow);
                parameters.Add("@Email", user.Email);        

                conn.Open();
                affectedRows = await conn
                    .ExecuteAsync(
                    sql: "spCreateUser", 
                    param: parameters, 
                    commandType: CommandType.StoredProcedure);

                var total = parameters.Get<int>("Total");
            }

            return affectedRows > 0;
        }

        public async Task<IEnumerable<User>> GetAll(
            int? pageIndex = null, 
            int? pageSize = null, 
            string search = null,
            bool? desc = null)
        {
            IEnumerable<User> users;
            using (var conn = new SqlConnection(_dbHelper.GetDatabaseConnection()))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@PageIndex", pageIndex);
                parameters.Add("@SearchText", search);
                parameters.Add("@SearchColumn", "FullName");
                parameters.Add("@SelectColumns", " Id, FullName, HomeAddress, Age, Md5Password ");
                parameters.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@ResultCommand", dbType: DbType.String, direction: ParameterDirection.Output, size: 1000);

                if (desc.HasValue && desc.Value) 
                {
                    parameters.Add("@SortOrder", "DESC");
                }
                else if(desc.HasValue && !desc.Value)
                {
                    parameters.Add("@SortOrder", "ASC");
                }

                conn.Open();
                users = await conn.QueryAsync<User>("spGetUsers",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if(users.Count() > 0)
                {
                    users.First().Total = parameters.Get<int>("Total");
                }

                var command = parameters.Get<string>("ResultCommand");
            }

            return users;
        }

        public Task<User> GetById(int id)
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
                    "spDeleleteUser",
                    id,
                    commandType: CommandType.StoredProcedure);
            }

            return affectedRows > 0;
        }

        public async Task<bool> Update(User user)
        {
            int affectedRows;
            using (var conn = new SqlConnection(_dbHelper.GetDatabaseConnection()))
            {
                conn.Open();
                affectedRows = await conn.ExecuteAsync(
                    "spUpdateUser",
                    new
                    {
                        Id = user.Id,
                        Address = user.Address,
                        Age = user.Age,
                        CreatedDate = DateTime.UtcNow,
                        Email = user.Email,
                        Username = user.Username,
                        Password = user.Password,
                        Name = user.Name
                    },
                    commandType: CommandType.StoredProcedure);
            }


            return affectedRows > 0;
        }
    }
}
