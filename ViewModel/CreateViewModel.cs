using System.ComponentModel.DataAnnotations;

namespace ApiApp.ViewModel
{
    public class CreateViewModel
    {
        [Required]
        public string Title { get; set; }
    }
}
