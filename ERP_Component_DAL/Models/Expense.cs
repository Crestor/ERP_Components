namespace ERP_Component_DAL.Models
{
    public class Expense
    {
        public DateOnly entrydate { get; set; }

        public string? project { get; set; }
        public string? particular { get; set; }
        public string? debit { get; set; }
        public string? credit { get; set; }
        public string? reference { get; set; }
        public string? narration { get; set; }
        public int expenseid { get; set; }

        public int generalid { get; set; }

        public int accountid { get; set; }

        public string? accountcode { get; set; }

        public string? accountname { get; set; }

        public string? groupname { get; set; }

        public string? bank { get; set; }
        public string? branch { get; set; }
        public string? Accountholdername { get; set; }
        public string? accountnumber { get; set; }
        public string? ifsc { get; set; }
        public string? balance { get; set; }
        public string? applicable { get; set; }
        public string? taxtype { get; set; }
     }
}
