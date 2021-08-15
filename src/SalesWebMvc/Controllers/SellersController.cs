using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using System;
using System.Diagnostics;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly ILogger<SellersController> _logger;
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(ILogger<SellersController> logger, SellerService sellerService, DepartmentService departmentService)
        {
            _logger = logger;
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        private Seller GetSellerById(int? id)
        {
            var seller = _sellerService.FindById(id.Value);

            return seller;
        }

        private RedirectToActionResult RedirectToError(string message = "Id not found")
        {
            return RedirectToAction(nameof(Error), new { message });
        }

        public IActionResult Index()
        {
            var list = _sellerService.FindAll();

            return View(list);
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

                return View(viewModel);
            }

            _sellerService.Insert(seller);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            var seller = GetSellerById(id);

            if (seller == null)
            {
                return this.RedirectToError();
            }

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            var seller = GetSellerById(id);

            if (seller == null)
            {
                return this.RedirectToError();
            }

            return View(seller);
        }

        public IActionResult Edit(int? id)
        {
            var seller = GetSellerById(id);

            if (seller == null)
            {
                return this.RedirectToError();
            }

            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

                return View(viewModel);
            }

            if (id != seller.Id) return this.RedirectToError();

            try
            {

                _sellerService.Update(seller);

                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return this.RedirectToError(e.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }

    }
}