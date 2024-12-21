using Microsoft.AspNetCore.Mvc;
using Product.Application.Models.ViewModels;
using Shared.Events;
using Shared.Services.Abstractions;

namespace Product.Application.Controllers
{
    public class ProductsController(IEventStoreService eventStoreService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductVM model)
        {
            NewProductAddedEvent newProductAddedEvent = new NewProductAddedEvent()
            {
                ProductId = Guid.NewGuid().ToString(),
                InitialCount = model.Count,
                InitialPrice = model.Price,
                IsAvailable = model.IsAvailabe,
                ProductName = model.ProductName
            };

            await eventStoreService.AppendToStreamAsync("products-stream", new[]
            {
                eventStoreService.GenerateEventData(newProductAddedEvent)
            });
            return RedirectToAction("Index");
        }
    }
}
