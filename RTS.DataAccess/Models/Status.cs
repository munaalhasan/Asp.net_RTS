using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RTS.DataAccess.Models
{
    public class Status
    {

        [Key]
        public int Id { get; set; }
        public string StatusType { get; set; }

        public virtual ICollection<ItemRequest> ItemRequest { get; set; }
        
    }
}
