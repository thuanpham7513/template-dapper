using Dapper;
using DapperTemplate.Abstracts;
using DapperTemplate.Helper;
using DapperTemplate.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                conn.Open();
                affectedRows = await conn.ExecuteAsync(
                    "spCreateUser",
                    new
                    {
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

        public async Task<IEnumerable<User>> GetAll(int pageIndex, int pageSize)
        {
            IEnumerable<User> result;
            using (var conn = new SqlConnection(_appData.DefaultConnection))
            {
                conn.Open();
                result = await conn.QueryAsync<User>(
                    "spGetUsers",
                    new 
                    { 
                        MaxRow = pageIndex * pageSize, 
                        MinRow = (pageSize - 1) * pageSize
                    },
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
