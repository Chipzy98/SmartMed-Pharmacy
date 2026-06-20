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

        public Order()
        {
            Items = new List<OrderItem>();
            OrderDate = DateTime.Now;
            Status = OrderStatus.Pending;
            TotalAmount = 0;
        }

        public Order(int orderId, int customerId, string customerName)
        {
            OrderId = orderId;
            CustomerId = customerId;
            CustomerName = customerName;
            OrderDate = DateTime.Now;
            Status = OrderStatus.Pending;
            Items = new List<OrderItem>();
            TotalAmount = 0;
        }

        /// <summary>
        /// Adds an item to the order
        /// </summary>
        public void AddItem(OrderItem item)
        {
            if (item != null)
            {
                Items.Add(item);
                CalculateTotal();
            }
        }

        /// <summary>
        /// Removes an item from the order
        /// </summary>
        public void RemoveItem(int orderItemId)
        {
            Items.RemoveAll(x => x.OrderItemId == orderItemId);
            CalculateTotal();
        }

        /// <summary>
        /// Calculates the total amount for the entire order
        /// </summary>
        public void CalculateTotal()
        {
            TotalAmount = Items.Sum(item => item.GetTotal());
        }

        /// <summary>
        /// Gets the number of items in the order
        /// </summary>
        public int GetItemCount()
        {
            return Items.Sum(item => item.Quantity);
        }

        /// <summary>
        /// Gets the total quantity of unique medicines
        /// </summary>
        public int GetUniqueMedicineCount()
        {
            return Items.Count;
        }
    }
}
