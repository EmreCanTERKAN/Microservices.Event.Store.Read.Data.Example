namespace Product.Application.Models.ViewModels
{
    public class ListProductVM
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }
        public bool IsAvailabe { get; set; }
        public decimal Price { get; set; }
    }
}
