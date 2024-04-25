namespace Souq.DTOs.Order
{
    public class OrderStatisticsDto
    {
        public int Confirmed { get; set; }
        public int Shipping { get; set; }
        public int Delivered { get; set; }
    }
}