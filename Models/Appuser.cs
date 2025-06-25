using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiWebpractice.Models
{
    public class Appuser:IdentityUser
    {
        [PersonalData]
        [Column(TypeName="nvarchar(150)")]
        public string FullName {  get; set; }

   
    }
}
