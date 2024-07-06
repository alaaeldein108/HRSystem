using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.DAL.Entities
{
    public class ApplicationRole:IdentityRole
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModificationTime { get; set; }= DateTime.Now;

        public ApplicationUser CreationRoles { get; set; }

        [ForeignKey("CreationRoles")]
        public string? CreatorId { get; set; }
        public ApplicationUser ModificarionRoles { get; set; }
        [ForeignKey("ModificarionRoles")]
        public string? ModifiedBy { get; set; }


    }
}
