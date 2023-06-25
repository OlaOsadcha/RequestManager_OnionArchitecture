using System.ComponentModel.DataAnnotations;

namespace RequestManager.Web.Models.Enitities
{
    public class LifecycleDTO
    {
        public int Id { get; set; }

        public DateTime Opened { get; set; }
      
        public DateTime? Distributed { get; set; }

        public DateTime? Proccesing { get; set; }

        public DateTime? Checking { get; set; }

        public DateTime? Closed { get; set; }
    }
}
