using RTS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RTS.Models.Roles
{
    public class CreateRole 
    {
        [Key]
        public int ID { get; set; }
        
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
