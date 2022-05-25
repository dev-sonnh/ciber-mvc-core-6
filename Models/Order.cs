namespace Ciber.Models
{
    public class Order
    {
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Id { get; set; }
        public int Amount { get; set; }
        public DateTime OrderDate { get; set; }

        public Customer? Customer { get; set; }

        public  Product? Product { get; set; }

    }
}
