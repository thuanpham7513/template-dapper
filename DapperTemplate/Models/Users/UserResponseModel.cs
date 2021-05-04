using System;

namespace DapperTemplate.Models
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Username { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalCount { get; set; }
    }
}
