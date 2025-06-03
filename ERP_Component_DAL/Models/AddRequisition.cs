namespace ERP_Component_DAL.Models
{
    public class AddRequisition
    {

      public  int Sno { get; set; }

       public int ReqNo { get; set; }

        public string  Raisedby { get; set; }

        public DateTime RaisedDate {get; set;}

        public decimal TAmount { get; set; }
        


    }
}
