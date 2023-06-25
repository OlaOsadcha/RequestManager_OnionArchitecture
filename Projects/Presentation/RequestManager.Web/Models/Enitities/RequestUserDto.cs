using RequestManager.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace RequestManager.Web.Models.Enitities
{
    public class RequestUserDto
    {
        public int Id { get; set; }      
        public  string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }      
        public int? DepartmentId { get; set; }
        public DepartmentDTO Department { get; set; }
    }
}
