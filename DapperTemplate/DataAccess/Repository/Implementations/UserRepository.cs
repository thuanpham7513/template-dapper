
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
            DateTime? startDate = null,
            DateTime? endDate = null,
            string searchEmail = null,
            string searchName = null,
            string searchAddress = null,
            bool? desc = null)
        {
            IEnumerable<User> users;
            using (var conn = new SqlConnection(_dbHelper.GetDatabaseConnection()))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@PageIndex", pageIndex);
                parameters.Add("@DateTo", endDate);
                parameters.Add("@DateFrom", startDate);

                parameters.Add("@SearchEmail", searchEmail);
                parameters.Add("@SearchAddress", searchAddress);
                parameters.Add("@searchName", searchName);

                conn.Open();
                users = await conn.QueryAsync<User>("spGetUsers",
                    parameters,
                    commandType: CommandType.StoredProcedure);
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
