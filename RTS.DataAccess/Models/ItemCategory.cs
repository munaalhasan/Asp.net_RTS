using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RTS.DataAccess.Models
{
    public class ItemCategory
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public virtual ICollection<Item> Item { get; set; }
       
    }
}
