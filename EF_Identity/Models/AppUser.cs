using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_Identity.Models
{
    public class AppUser : IdentityUser
    {
        [Column(TypeName ="nvarchar")]
        [StringLength(400)]
        public string HomeAddress { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Birthdate { get; set; }
    }
}
