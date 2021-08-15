using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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

        async private Task<Seller> GetSellerByIdAsync(int? id)
        {
            var seller = await _sellerService.FindByIdAsync(id.Value);

            return seller;
        }

        private RedirectToActionResult RedirectToError(string message = "Id not found")
        {
            return RedirectToAction(nameof(Error), new { message });
        }

        async public Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();

            return View(list);
        }

        async public Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        async public Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

                return View(viewModel);
            }

            await _sellerService.InsertAsync(seller);

            return RedirectToAction(nameof(Index));
        }

        async public Task<IActionResult> Delete(int? id)
        {
            var seller = await GetSellerByIdAsync(id);

            if (seller == null)
            {
                return this.RedirectToError();
            }

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        async public Task<IActionResult> Delete(int id)
        {
            await _sellerService.RemoveAsync(id);

            return RedirectToAction(nameof(Index));
        }

        async public Task<IActionResult> Details(int? id)
        {
            var seller = await GetSellerByIdAsync(id);

            if (seller == null)
            {
                return this.RedirectToError();
            }

            return View(seller);
        }

        async public Task<IActionResult> Edit(int? id)
        {
            var seller = await GetSellerByIdAsync(id);

            if (seller == null)
            {
                return this.RedirectToError();
            }

            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        async public Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

                return View(viewModel);
            }

            if (id != seller.Id) return this.RedirectToError();

            try
            {

                await _sellerService.UpdateAsync(seller);

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