
using ERP_Component_DAL.Models;
using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace ERP_Components.Controllers
{
    public class SalesController : Controller
    {
        private readonly ILogger<SalesController> _logger;
        private readonly SalesServices salesServices;
        private readonly CenterlizedService centerlizedService;
        private readonly IConfiguration _configuration;
        private List<QuotationModel> quotationModels = new List<QuotationModel>();


        public SalesController(ILogger<SalesController> logger, IConfiguration configuration, CenterlizedService centerlizedService)
        {
            _logger = logger;
            _configuration = configuration;
            salesServices = new SalesServices(configuration);
            this.centerlizedService = centerlizedService;
        }
        public IActionResult Dashboard()
        {
            return View();
        }




        //Quotation  
        public IActionResult ManageQuotation()
        {
            List<QuotationModel> QL = salesServices.ListQuotation();

            return View(QL);

        }
        //edit Quotaion 
        public IActionResult EditQuotation(Guid QuotationID)//int QuotationID
        {


            List<QuotationModel> EQ = salesServices.EditQuota(QuotationID); //remaining2
            return View(EQ);
        }
        public IActionResult EditQuotationDetails(QuotationModel q)
        {

            salesServices.updateEditPro(q);


            return View();
        }
        public IActionResult EditQuotationform(QuotationModel e)
        {
            salesServices.finalEditQuota(e);
            salesServices.editAmount(e);
            return RedirectToAction("ManageQuotation");
        }


        public IActionResult AddQuotation()
        {
            HttpContext.Session.SetString("QuotationAdded", "False");
            List<QuotationModel> aq = salesServices.GetProductDetails();
            List<QuotationViewModel.Customer> Customers = salesServices.GetCustomersList();

            var model = new QuotationViewModel
            {
                ItemNames = aq,
                Customers = Customers
            };

            return View(model);
        }






        [HttpPost]
        public IActionResult SetQuotation(QuotationModel quotation,List<QuotationModel> ItemLists)
        {
            salesServices.AddQuotation(quotation, ItemLists);
            return RedirectToAction("AddQuotation");
        }



        //delete Quotation

        public IActionResult DeleteQuota(QuotationModel q)
        {
            salesServices.DeleteQuotation(q);
            return RedirectToAction("ManageQuotation");
        }


        public IActionResult ViewQuotation(Guid QuotationID)
        {
            //salesServices.ViewQuotationDoc(vq, QuotationID);
            //List<QuotationModel> pl = salesServices.ShowProductLi(QuotationID);
            List<QuotationModel> pl = salesServices.ViewQuotationDoc(QuotationID);
            return View(pl);
        }



        ////Product
        //public IActionResult ProductView()
        //{
        //    List<Products> pl = salesServices.AddCategory();
        //    return View(pl);
        //}
        //public JsonResult ItemName(int CategoryID)
        //{

        //    List<Products> pl = salesServices.AddItemName(CategoryID);
        //    return Json(pl);
        //}



        //invoice 
        public IActionResult ManageInvoice()
        {
            List<Invoice> IL = salesServices.ListInvoice();
            return View(IL);
        }

        //create invoice
        public IActionResult AddInvoice(Guid QuotationID)
        {
            //HttpContext.Session.SetString("invoiceAdded", "False");

            //List<Invoice> In = salesServices.InvoiceQvalue(QuotationID);

            InvoiceForm? invoice = salesServices.GetInvoiceFromQuotation(QuotationID);
            return View(invoice);
        }
        public IActionResult SetAddInvoice(Invoice q)
        {
            //TODO: Fix this controller
            Console.WriteLine("We are in SetAdd Invoice");
            q.InvoiceID = salesServices.AddInvoiceDetails(q);
            salesServices.SaveShipingDetails(q);
            //salesServices.TransportDetail(q);
            //salesServices.DispatchDetail(q);
            //salesServices.CourierDetail(q);
            salesServices.Updatestatus(q);




            return RedirectToAction("ManageInvoice");
        }

        public IActionResult DispatchInvoice(Guid InvoiceID)
        {
            return View(InvoiceID);
        }

        public IActionResult AddDispatchDetails(Dispatch dispatch)
        {    
            salesServices.SaveDispatchDetails(dispatch);
            return RedirectToAction("ManageInvoice");
        }

        public IActionResult CancelInvoice(Invoice I, Guid InvoiceID)
        {
            salesServices.CancelInvoice(I, InvoiceID);
            return RedirectToAction("ManageInvoice");
        }


        public IActionResult EditInvoice(Guid InvoiceID)  
        {
            List<Invoice> li = new List<Invoice>();
            li.Add(salesServices.EditInvoice(InvoiceID));
            li.Add(salesServices.ViewShippedTo(InvoiceID));
            li.Add(salesServices.ViewTransportDetails(InvoiceID));
            li.Add(salesServices.ViewDispatchDetails(InvoiceID));
            li.Add(salesServices.ViewCourierDetails(InvoiceID));
            return View(li);
        }
        [HttpPost]
        public IActionResult UpdateEditInvoice(Invoice i)
        {
            salesServices.updateInvoice(i);

            return RedirectToAction("ManageInvoice");
        }




        public IActionResult ViewInvoice(Guid InvoiceID, Guid QuotationID)
        {
            InvoiceView iv = new InvoiceView();
            iv.ProductView = salesServices.ViewInvoiceProd(QuotationID);
            iv.ListView = new List<Invoice>
        {

            salesServices.EditInvoice(InvoiceID),
            salesServices.ViewShippedTo(InvoiceID),
            salesServices.ViewTransportDetails(InvoiceID),
            salesServices.ViewDispatchDetails(InvoiceID),
            salesServices.ViewCourierDetails(InvoiceID),

        };





            return View(iv);
        }





        //ProformaInvoice
        public IActionResult ManageProformaInvoice()
        {
            List<ProformaInvoice> IL = salesServices.ListProformaInvoice();
            return View(IL);
        }
        public IActionResult ProformaInvoice()
        {
            return View();
        }
        public IActionResult SetProformaInvoice(ProformaInvoice Pi)
        {
            salesServices.Proformainvoice(Pi);
            return RedirectToAction("ManageProformaInvoice");
        }

        public IActionResult DeleteProformaInvoice(ProformaInvoice P, Guid ProformaInvoiceID)
        {
            salesServices.DeleteProforma(P, ProformaInvoiceID);
            return RedirectToAction("ManageProformaInvoice");
        }
        public IActionResult EditProforma(ProformaInvoice p, Guid ProformaInvoiceID)
        {
            var x = salesServices.EditProformaInvoice(p, ProformaInvoiceID);
            return View(x);
        }
        public IActionResult UpdateEdit(ProformaInvoice p)
        {
            salesServices.UpdateProfoma(p);
            return RedirectToAction("ManageProformaInvoice");
        }
        //CreditNote

        public IActionResult ManageCreditNote()
        {
            List<CreditNote> CL = salesServices.ListCreditNote();
            return View(CL);
        }
        public IActionResult CreditNote()
        {
            return View();
        }
        public IActionResult SetCreditNote(CreditNote c)
        {
            salesServices.CreditNote(c);
            return RedirectToAction("ManageCreditNote");
        }
        public IActionResult EditCreditNote(CreditNote c, Guid CreditNoteId)
        {
            var x = salesServices.EditCreditNote(c, CreditNoteId);
            return View(x);
        }
        public IActionResult UpdateCreditNote(CreditNote c)
        {
            salesServices.UpdateCreditNote(c);
            return RedirectToAction("ManageCreditNote");
        }
        public IActionResult DeleteCreditNote(CreditNote c, Guid CreditNoteId)
        {
            salesServices.DeleteCreditNote(c, CreditNoteId);
            return RedirectToAction("ManageCreditNote");
        }

        //ReturnNote
        public IActionResult ReturnNote(bool y)
        {
            ViewBag.loaded = y;
            List<ReturnNote> lr = salesServices.ListReturnNote();
            return View(lr);
        }
        public IActionResult SetReturnNote(ReturnNote r)
        {
            salesServices.CreateReturnNote(r);
            bool x = true;
            return RedirectToAction("ReturnNote", new { y = x });
        }
        public IActionResult DeleteReturnNote(ReturnNote r, Guid ReturnNoteId)
        {
            salesServices.DeleteReturnNote(r, ReturnNoteId);
            bool x = true;
            return RedirectToAction("ReturnNote", new { y = x });
        }
        public IActionResult EditReturnNote(ReturnNote r, Guid ReturnNoteId)
        {
            var x = salesServices.EditReturnNote(r, ReturnNoteId);
            return View(x);
        }
        public IActionResult UpdateReturnNote(ReturnNote r)
        {
            salesServices.UpdateReturnNote(r);
            bool x = true;
            return RedirectToAction("ReturnNote", new { y = x });
        }

        //DeliveryChallan
        public IActionResult ManageDeliveryChallan()
        {
            List<DeliveryChallan> D = salesServices.ListDeliveryChallan();
            return View(D);
        }

        public IActionResult DeliveryChallan()
        {
            return View();
        }

        public IActionResult SetDeliveryChallan(DeliveryChallan D)
        {
            salesServices.DeliveryChallan(D);
            return RedirectToAction("DeliveryChallan");
        }
        public IActionResult DeleteDeliveryChallan(DeliveryChallan D, Guid DeliveryChallanID)
        {
            salesServices.DeleteDeliveryChallan(D, DeliveryChallanID);
            return RedirectToAction("ManageDeliveryChallan");
        }
        public IActionResult EditDeliveryChallan(DeliveryChallan D, Guid DeliveryChallanId)
        {
            var x = salesServices.EditDeliveryChallan(D, DeliveryChallanId);
            return View(x);
        }
        public IActionResult UpdateDeliveryChallan(DeliveryChallan D)
        {
            salesServices.UpdateDeliveryChallan(D);
            return RedirectToAction("ManageDeliveryChallan");
        }

        public IActionResult CustomerHistory()
        {
            List<CustomerHistory> CL = salesServices.ListCoustomer();
            return View(CL);
        }
        //sales forecasting

        public IActionResult Salesforecasting()
        {
            HttpContext.Session.SetString("SalesforecastingADD", "False");



            List<QuotationModel> aq = salesServices.AddSFItemName();

            var model = new QuotationViewModel
            {
                ItemNames = aq,


            };

            return View(model);
        }



        //public IActionResult SetSalesForCasting(QuotationModel quotation, List<QuotationModel> ItemLists)
        //{

        //    salesServices.SalesForCasting(quotation, ItemLists); 
        //    return RedirectToAction("Salesforecasting");

        //}






        //[HttpPost]
        //Harsh sir ye wala remove kar dena iski bhi koi zaruurat nahi hia
        //public JsonResult SetSalesforecasting(QuotationModel O)
        //{
        //    var QuotationCreated = HttpContext.Session.GetString("SalesforecastingADD");
        //    if (QuotationCreated == "False")
        //    {
        //        O.RequisitionID = salesServices.AddSFDetails(O);// get QuotationID 
        //        HttpContext.Session.SetString("SalesforecastingADD", "True");
        //        HttpContext.Session.SetString("RequisitionID", O.RequisitionID.ToString());
        //        var x = salesServices.AddSFItems(O);

        //    }
        //    else
        //    {
        //        O.RequisitionID = Guid.Parse(HttpContext.Session.GetString("RequisitionID"));

        //        var x = salesServices.AddSFItems(O);



        //    }

        //    List<QuotationModel> ol = salesServices.OrderTable(O.RequisitionID);


        //    var model = new QuotationViewModel
        //    {
        //        OrderTable = ol,

        //    };



        //    return Json(model);
        //}


       







        // update details  
        // Harsh sir ye wala Action remove kar dena iski zaruurat nahi hai
        //public IActionResult FinalSalesforecasting(QuotationModel O)
        //{
        //    O.RequisitionID = Guid.Parse(HttpContext.Session.GetString("RequisitionID"));
        //    Guid CenterID = Guid.Parse(HttpContext.Session.GetString("CenterID"));
        //    centerlizedService.updateSalesForecastDetails(O, CenterID, RequisitionTypes.SALES_FORCASTING);  //reamining1
        //    return RedirectToAction("Salesforecasting");
        //}

        //Ye wala Action main data send karna or Requisition model ko dekhla usme send kar dena data
        //Same problem Retails sales main bhi hai vaha bhi check kar lena
        public IActionResult SetSalesForecast(Requisition salesForecast)
        {
            Guid CenterID = Guid.Parse(HttpContext.Session.GetString("CenterID"));
            centerlizedService.SaveRequisition(salesForecast, CenterID, RequisitionTypes.SALES_FORCASTING);
            return RedirectToAction("Salesforecasting");
        }


        public IActionResult ViewInventory()
        {
            Guid CenterID = Guid.Parse(HttpContext.Session.GetString("CenterID"));
            List<Items> items = centerlizedService.ViewInventory(CenterID);
            return View(items);
        }


        public IActionResult BillsReceivable()
        {
            return View();
        }
        public IActionResult SalePurchaseSummary()
        {
            return View();
        }
        public IActionResult SalePerson()
        {
            return View();
        }
        public IActionResult PaymentBreakUp()
        {
            return View();
        }


    }
}