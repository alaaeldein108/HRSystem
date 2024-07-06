using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.DAL.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModificationTime { get; set; } = DateTime.Now;
        public ApplicationUser CreationUsers { get; set; }

        [ForeignKey("CreationUsers")]
        public string? CreatorId { get; set; }
        public ApplicationUser ModificarionUsers { get; set; }
        [ForeignKey("ModificarionUsers")]
        public string? ModifiedBy { get; set; }

    }
}
