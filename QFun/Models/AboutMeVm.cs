using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QFun.Models
{
    public class AboutMeVm
    {
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Required]
        public string AboutMe { get; set; }

    }
}
