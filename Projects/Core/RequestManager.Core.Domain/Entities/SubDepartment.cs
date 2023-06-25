using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RequestManager.Core.Domain.Entities
{
    public class SubDepartment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // SubDepartment number
        [Required]
        [Display(Name = "Sub-Department number")]
        [MaxLength(50, ErrorMessage = "Maximum length cannot be more than 50 symbols")]
        public string CabNumber { get; set; }

        // Foreigh key Department Id
        [Required]
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }
        // Navigation property Department
        public Department Department { get; set; }
    }
}