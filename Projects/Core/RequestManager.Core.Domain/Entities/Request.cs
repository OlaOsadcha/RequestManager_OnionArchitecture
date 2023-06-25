using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RequestManager.Core.Domain.Entities
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name of request")]
        [MaxLength(50, ErrorMessage = "Maximum length cannot be more than 50 symbols")]
        public string Name { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "Maximum length cannot be more than 200 symbols")]
        public string Description { get; set; }

        [MaxLength(200, ErrorMessage = "Maximum length cannot be more than 200 symbols")]
        public string Comment { get; set; }

        public int Status { get; set; }

        public int Priority { get; set; }

        // Foreigh key SubDepartment
        [Display(Name = "Sub-Department")]
        public int? SubDepartmentId { get; set; }
        public SubDepartment SubDepartment { get; set; }

        [Display(Name = "Error file")]
        public string File { get; set; }

        // Foreigh key category
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        // Foreigh key UserID
        public int? UserId { get; set; }
        // User - navigation property
        public RequestUser User { get; set; }

        // Foreigh key ExecutorId
        public int? ExecutorId { get; set; }
        // Executor - navigation property
        public RequestUser Executor { get; set; }

        // Foreigh key Life cycle of request ID
        public int LifecycleId { get; set; }
        // Lifecycle - navigation property
        public Lifecycle Lifecycle { get; set; }
    }

    // Request Status
    //RequestStatus will be used in Status property
    public enum RequestStatus
    {
        Open = 1,
        Distributed = 2,
        Proccesing = 3,
        Checking = 4,
        Closed = 5
    }
    //Request Priority
    //RequestPriority will be used in Priority property
    public enum RequestPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }
}
