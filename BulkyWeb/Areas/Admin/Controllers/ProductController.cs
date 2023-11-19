
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitofwork, IWebHostEnvironment webHost)
        {
            _unitOfWork = unitofwork;
            _webHostEnvironment = webHost;
        }
        public IActionResult Index()
        {
            List<Product> objectProducts = _unitOfWork.Product.GetAll().ToList();

            return View(objectProducts);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVm = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            if (id == 0 | id == null)
            {
                return View(productVm);
            }
            else
            {
                productVm.Product = _unitOfWork.Product.Get(u => u.Id == id);

                return View(productVm);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVm, IFormFile? image)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if(image != null)
            {
                if (productVm.Product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, productVm.Product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

                productVm.Product.ImageUrl = @"\images\product\" + fileName;
            }
            if(productVm.Product.Id == 0)
            {
                TempData["success"] = "Product created succesfully";
                _unitOfWork.Product.Add(productVm.Product);
            }
            else
            {
                TempData["success"] = "Product edited succesfully";
                _unitOfWork.Product.Update(productVm.Product);
            }
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Product oldProduct = _unitOfWork.Product.Get(u => u.Id == id);
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem()
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ViewBag.CategoryList = categoryList;
            return View(oldProduct);
        }

        [HttpPost]
        public IActionResult Delete(Product obj)
        {
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";

            return RedirectToAction("Index");
        }

    }
}
