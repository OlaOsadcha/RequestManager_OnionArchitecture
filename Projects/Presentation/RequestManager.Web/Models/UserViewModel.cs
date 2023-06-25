using RequestManager.Core.Domain.Entities;

namespace RequestManager.Web.Models
{
    public class UserViewModel
    {
        public RequestUser User { get; set; }

        public string Role { get; set; }
    }
}