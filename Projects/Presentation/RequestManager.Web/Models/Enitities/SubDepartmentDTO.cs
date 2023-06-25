using RequestManager.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace RequestManager.Web.Models.Enitities
{
    public class SubDepartmentDTO
    {
        public int Id { get; set; }       
        public string CabNumber { get; set; }          
        public int? DepartmentId { get; set; }     
        public DepartmentDTO Department { get; set; }
    }
}