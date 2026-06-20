using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartMedPharmacy.Models
{
    /// <summary>
    /// Represents an order status in the system
    /// </summary>
    public enum OrderStatus
    {
        Pending,
        ReadyForPickup,
        Delivered,
        Cancelled
    }

    /// <summary>
    /// Represents an order item (medicine in an order)
    /// </summary>
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }

        public OrderItem()
        {
            Discount = 0;
        }

        /// <summary>
        /// Calculates the total price for this order item
        /// </summary>
        public decimal GetTotal()
        {
            decimal subtotal = Quantity * UnitPrice;
            decimal discountAmount = (subtotal * Discount) / 100;
            return subtotal - discountAmount;
        }
    }

    /// <summary>
    /// Represents an order placed by a customer
    /// </summary>
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public string SpecialNotes { get; set; }

        // Updated constructor to match the usage in CustomerDashboard
        public Order(int customerId, string customerName)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            OrderDate = DateTime.Now;
            Status = OrderStatus.Pending;
            Items = new List<OrderItem>();
            TotalAmount = 0;
        }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
            CalculateTotal();
        }

        public void RemoveItem(int orderItemId)
        {
            var item = Items.FirstOrDefault(i => i.OrderItemId == orderItemId);
            if (item != null)
            {
                Items.Remove(item);
                CalculateTotal();
            }
        }

        public void CalculateTotal()
        {
            TotalAmount = Items.Sum(i => i.Quantity * i.UnitPrice);
        }

        public int GetItemCount()
        {
            return Items.Sum(i => i.Quantity);
        }

        public int GetUniqueMedicineCount()
        {
            return Items.Select(i => i.MedicineId).Distinct().Count();
        }
    }
}
