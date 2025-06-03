
using Microsoft.AspNetCore.Http;

namespace ERP_Component_DAL.Models
{
    public class AddVendor
    {
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public string vendorIndustry { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string GST { get; set; }
        public decimal Balance { get; set; }
        public string? VendorIndustry { get; set; }
        public int PaymentTermsDays { get; set; }
        public string? VendorCode { get; set; }
        public string? PAN { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Pincode { get; set; }
        public string? District { get; set; }
        public string? AddressLine { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountHolderName { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? IFSCCode { get; set; }
        public IFormFile GSTCertificate { get; set; }
        public IFormFile PANCard { get; set; }
    }


}

