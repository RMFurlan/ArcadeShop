﻿namespace meusite.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string GameKey { get; set; }
        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }
}
