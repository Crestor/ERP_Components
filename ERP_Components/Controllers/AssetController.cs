using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using ERP_Component_DAL.Models;

namespace ERP_Components.Controllers
{
    public class AssetController : Controller
    {
        private readonly ILogger<AssetController> _logger;
        private readonly UserServices _userServices;
        private readonly InventoryServices inventoryServices;

        private readonly AssetServices assetServices;
        private readonly IConfiguration _configuration;

        public AssetController(ILogger<AssetController> logger, IConfiguration configuration)
        {

            _logger = logger;
            _configuration = configuration;
            _userServices = new UserServices(configuration);
            inventoryServices = new InventoryServices(configuration);
            assetServices = new AssetServices(configuration);
        }



        public IActionResult Index()
        {
            return View();
        }

        //<--------------------------------Dashboard------------------->
        public IActionResult Dashboard()
        {
            return View();
        }





        //<--------------------------------Add Asset---------------------->
        [HttpGet]
        public IActionResult AddAsset()
        {
            List<Category> categories = assetServices.getAssetCategoriesName();
            return View(categories);
           
        }

        [HttpPost]
        public IActionResult AddAsset(Asset asset)
        {
            assetServices.AddAsset(asset);
            return RedirectToAction("AddAsset");
        }

        public IActionResult AssetView()
        {
          List<Asset> asset =  assetServices.GetAssetView();
            return View(asset);
        }


        public IActionResult EditAsset(Guid assetId)
        {
         Asset asset =   assetServices.EditAsset(assetId);
            return View(asset);
        } 

        public IActionResult UpdateAsset(Asset asset)
        {
            assetServices.UpdateAsset(asset);
            return RedirectToAction("AssetView");
        }

        public IActionResult DeleteAsset(Guid itemId)
        {
            assetServices.DeleteAsset(itemId);
            return RedirectToAction("AssetView");
        }


        //<-----------------------Asset category---------->


        public IActionResult Category()
        {
            return View();
        }


        public IActionResult AddCategory(Category category)
        {
            inventoryServices.AddCategory(category);
            return RedirectToAction("Category");
        }

        public IActionResult ViewCategory()
        {
           List<Category> category =   assetServices.ViewAssetCategory();
            return View(category);
        }

        [HttpGet]
        public IActionResult EditCategory(int categoryId)
        {
          Category category =  assetServices.GetEditCategory(categoryId);
            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            inventoryServices.UpdateCategory(category);
            return RedirectToAction("ViewCategory");
            
        }
        public IActionResult DeleteCategory(int categoryId)
        {
            inventoryServices.DeleteCategory(categoryId);
            return RedirectToAction("ViewCategory");
        }


        //<----------------Asset Listing and Assignment Page --------------->


        public IActionResult AssetListing()
        {
         List<Asset> listing =   assetServices.AssetListing();
            return View(listing);
        }

        public IActionResult AssetAssign(Guid assetId)
        {
         Asset asset =   assetServices.GetAssetAssign(assetId);
            return View(asset);
        }

        public IActionResult AddAssetAssign(Asset asset)
        {
            assetServices.AddAssetAssignment(asset);
            return RedirectToAction("AssetListing");
        }

        public IActionResult ViewAssetAssign()
        {
            List<Asset> assign = assetServices.ViewAssignAsset();
                return View(assign);
        }

        //<----------------------------------------AssetMaintenance-------------------------->
        public IActionResult AssetMaintenance()
        {

            List<Asset> asset = assetServices.getAssetNames();
            return View(asset);
        }

        public IActionResult AddAssetMaintenance(Asset asset)
        {
            assetServices.AddAssetMaintenance(asset);
            return RedirectToAction("AssetMaintenance");
        }

        public IActionResult AssetMaintenanceView()
        {
            List<Asset> asset = assetServices.AssetMaintenanceView();
            return View(asset);
        }

        public IActionResult EditAssetMaintenance(Guid maintenanceId)
        {

            List<Asset> item = assetServices.getAssetNames();

            var asset = assetServices.EditMaintenance(maintenanceId);

            asset.items = item;
            return View(asset);
        }



        //<----------------------------Depreciation Tracking-------------------------->


      public IActionResult DepreciationTracking()
        {

            List<Asset> depreciation = assetServices.DepreciationTracking(); 
            return View(depreciation);
        }

        //<----------------------------Asset Transfer--------------------->

        public IActionResult AssetTransfer()
        {
           
         List<Category> category = assetServices.getAssetCategoriesName();
            return View(category);
        }

        public JsonResult AssetNamesBasedOnCategory(int categoryId)
        {
            List<Asset> names = assetServices.getAssetName(categoryId);
            return Json(names);
        }

        public IActionResult AddAssetTransfer(Asset asset)
        {
            if(asset.newOwner.ToLower() == "store")
            {
                asset.type = "AssetIn";
                assetServices.AddAssetTransfer(asset);
            }
            else
            {
                asset.type = "AssetOut";
                assetServices.AddAssetTransfer(asset);
                assetServices.AllotAssetToNewUser(asset);
            }
        

                return RedirectToAction("AssetTransfer");
        }

    }
}
