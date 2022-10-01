using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RTS.DataAccess.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
       
        public int RequestID { get; set; }
        [ForeignKey("RequestID")]
        public ItemRequest ItemRequest { get; set; }
        

        public int RequestToEmployeeID { get; set; }
        [ForeignKey("RequestToEmployeeID")]
        public Employee Employee { get; set; }


        public int changedStatus { get; set; }
        [ForeignKey("changedStatus")]
        public Status Status { get; set; }


        public int changedAssigndToEmpID { get; set; }
        [ForeignKey("changedAssigndToEmpID")]
        public Item Item { get; set; }



        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

    }
}
