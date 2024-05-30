using System.ComponentModel.DataAnnotations;

namespace GetFit.ViewModel
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<string>();
        }

        public string Id { get; set; } = default!;

        [Required]
        public string UserName { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set;} = default!;

        public List<string> Roles { get; set;}
    }
}
