using Souq.Models;

namespace Souq.ViewModels
{
    public class AllOrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public string Filter { get; set; }
        public int PageNumber { get; set; }
        public int Size { get; set; }

    }
}
