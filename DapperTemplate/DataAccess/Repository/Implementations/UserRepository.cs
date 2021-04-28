using Dapper;
using DapperTemplate.Abstracts;
using DapperTemplate.Helper;
using DapperTemplate.Models;
using Microsoft.Extensions.Options;
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
        private readonly AppData _appData;
        public UserRepository(IOptions<AppData> options)
        {
            _appData = options.Value;
        }
        public async Task<bool> Create(User user)
        {
            int affectedRows;
            using (var conn = new SqlConnection(_appData.DefaultConnection))
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
            }

            return affectedRows > 0;
        }

        public async Task<IEnumerable<User>> GetAll(
            int? pageIndex = null, 
            int? pageSize = null, 
            string search = null,
            bool? desc = null)
        {
            IEnumerable<User> result;
            using (var conn = new SqlConnection(_appData.DefaultConnection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@PageIndex", pageIndex);
                parameters.Add("@SearchText", search);
                parameters.Add("@SearchColumn", "FullName");
                parameters.Add("@SelectColumns", " Id, FullName, HomeAddress, Age, Md5Password "); 
                if (desc.HasValue && desc.Value) 
                {
                    parameters.Add("@SortOrder", "DESC");
                }
                else if(desc.HasValue && !desc.Value)
                {
                    parameters.Add("@SortOrder", "ASC");
                }
               
                conn.Open();
                result = await conn.QueryAsync<User>(
                    "spGetUsers",
                    parameters,
                    commandType: CommandType.StoredProcedure);


            }

            return result;
        }

        public Task<User> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            int affectedRows;
            using (var conn = new SqlConnection(_appData.DefaultConnection))
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
            using (var conn = new SqlConnection(_appData.DefaultConnection))
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
