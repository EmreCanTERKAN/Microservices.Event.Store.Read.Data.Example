using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Product.Application.Models.ViewModels;
using Shared.Events;
using Shared.Services.Abstractions;

namespace Product.Application.Controllers
{
    public class ProductsController(IEventStoreService eventStoreService, IMongoDbService mongoDbService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var productsCollection = mongoDbService.GetCollection<Shared.Models.Product>("products");
            var products = await (await productsCollection.FindAsync(_ => true)).ToListAsync();
            return View(products);
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
                IsAvailable = model.IsAvailable,
                ProductName = model.ProductName
            };

            await eventStoreService.AppendToStreamAsync("products-stream", new[]
            {
                eventStoreService.GenerateEventData(newProductAddedEvent)
            });
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string productId)
        {
            var productCollection = mongoDbService.GetCollection<Shared.Models.Product>("products");
            var product = await (await productCollection.FindAsync(p => p.Id == productId)).FirstOrDefaultAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> CountUpdate()
        {
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> PriceUpdate()
        {
            return null;
        }
        [HttpPost]
        public async Task<IActionResult> AvailableUpdate()
        {
            return null;
        }
    }
}
