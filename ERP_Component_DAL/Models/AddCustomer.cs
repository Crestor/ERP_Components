using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;




namespace ERP_Component_DAL.Models
{
    public class AddCustomer
    {
        public Guid CustomerId { get; set; }
        public Guid AccountId { get; set; }
        public Guid DocumentId { get; set; }
        public Guid ContactDetailId { get; set; }
        public int AddressId { get; set; }
        public string CustomerName { get; set; }
        public string vendorIndustry { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string series { get; set; }

        public Decimal Amount { get; set; }
        public string CompanyEmail { get; set; }
        public string ContactNo { get; set; }
        public string alternate { get; set; }
        public string GstIn { get; set; }
        public string social { get; set; }
        public string whatsapp { get; set; }
        public decimal Balance { get; set; }
        public string CustomerIndustry { get; set; }
        public int PaymentTermsDays { get; set; }
        public string CustomerCode { get; set; }
        public string PAN { get; set; }
        public DateTime OpeningDate { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string PinCode { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
     
        public string AccountNo { get; set; }
        public string AccountHolderName { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IfscCode { get; set; }
        public string Mode { get; set; }
        public IFormFile GstCertificate { get; set; }
        public IFormFile PanCard { get; set; }


        public List<AddCustomer> contact { get; set; }
    }

   
}












