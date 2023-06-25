using RequestManager.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RequestManager.Web.Models.Enitities
{
    public class RequestDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }

        public int Status { get; set; }

        public int Priority { get; set; }

        public int? SubDepartmentId { get; set; }
        public SubDepartmentDTO SubDepartment { get; set; }

        public string File { get; set; }

        public int? CategoryId { get; set; }
        public CategoryDTO Category { get; set; }

        public int? UserId { get; set; }
        public UserDTO User { get; set; }

        public int? ExecutorId { get; set; }
        public UserDTO Executor { get; set; }

        public int LifecycleId { get; set; }
        public LifecycleDTO Lifecycle { get; set; }
    }
}
