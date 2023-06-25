using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RequestManager.Core.Domain.Entities
{
    public class RequestUser : IdentityUser<int>
    {
        [Required]
        [Display(Name = "User Name")]
        [MaxLength(50, ErrorMessage = "Maximum length cannot be more than 50 symbols")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }

        [MaxLength(50, ErrorMessage = "Maximum length cannot be more than 50 symbols")]
        public string Position { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}