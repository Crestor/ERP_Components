using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using ERP_Component_DAL.Models;

namespace ERP_Components.Controllers
{
    public class AssetController : Controller
    {
        private readonly ILogger<AssetController> _logger;
        private readonly UserServices _userServices;

        private readonly AssetServices assetServices;
        private readonly IConfiguration _configuration;

        public AssetController(ILogger<AssetController> logger, IConfiguration configuration)
        {

            _logger = logger;
            _configuration = configuration;
            _userServices = new UserServices(configuration);
            assetServices = new AssetServices(configuration);
        }



        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddAsset()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAsset(Asset asset)
        {
            assetServices.AddAsset(asset);
            return View();
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

    }
}
