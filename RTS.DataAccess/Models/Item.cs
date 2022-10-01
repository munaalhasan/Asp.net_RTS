using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RTS.DataAccess.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        public int AssignedToEmployeeID { get; set; }
        [ForeignKey("AssignedToEmployeeID"),DisallowNull]
        public Employee Employee { get; set; }

        
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId"), DisallowNull]
        public ItemCategory ItemCategory { get; set; }

        public virtual ICollection<ItemRequest> ItemRequest { get; set; }        


        [DefaultValue("false")]
        public bool isDeleted { get; set; }
        

        public string Name { get; set; }

        public string Description { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        [DataType(DataType.Date)]
        public Nullable<DateTime> PurchaseDate { get; set; }

        public Nullable<int> SerialNumber { get; set; }


    }
}
