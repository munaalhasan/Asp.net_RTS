using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RTS.DataAccess.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Item> Item { get; set; }
        public virtual ICollection<ItemRequest> ItemRequest { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
