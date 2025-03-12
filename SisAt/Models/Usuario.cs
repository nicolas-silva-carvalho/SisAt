using Microsoft.AspNetCore.Identity;
using SisAt.Helper;

namespace SisAt.Models
{
    public class Usuario : IdentityUser<long>
    {
        public List<IdentityRole<long>>? Roles { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
