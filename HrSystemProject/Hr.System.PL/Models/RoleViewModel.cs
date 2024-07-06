using HrSystem.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Hr.System.PL.Models.ValidationAttributes;

namespace HrSystem.PL.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Creator { get; set; }
        public DateTime ModificationTime { get; set; }
        public string Modifier { get; set; }
        public List<string> Permissions { get; set; }
    }
}
