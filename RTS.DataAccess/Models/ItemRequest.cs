using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RTS.DataAccess.Models
{
    public class ItemRequest
    {

        [Key]
        public int Id { get; set; }        

        public int RequestEmployeeID { get; set; }
        [ForeignKey("RequestEmployeeID")]
        public  Employee Employee { get; set; }
        

        public int ItemID { get; set; }
        [ForeignKey("ItemID")]
        public Item Item { get; set; }


        public int StatusID { get; set; }
        [ForeignKey("StatusID")]
        public Status Status { get; set; }


        //public DateTime RequesttDate { get; set; }       

    }
}
