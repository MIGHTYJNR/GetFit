using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GetFit.ViewModel
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        public string Id { get; set; } = default!;

        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; } = default!;

        public List<string> Users { get; set; } = default!;
    }
}
