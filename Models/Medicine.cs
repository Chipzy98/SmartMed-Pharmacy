using System;

namespace SmartMedPharmacy.Models
{
    /// <summary>
    /// Represents a medicine/drug in the pharmacy inventory
    /// </summary>
    public class Medicine
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string Category { get; set; }
        public string Dosage { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Supplier { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool RequiresPrescription { get; set; }
        public DateTime DateAdded { get; set; }

        public Medicine()
        {
            DateAdded = DateTime.Now;
            RequiresPrescription = false;
        }

        public Medicine(int id, string name, string category, string dosage, 
                        decimal price, int stock, string supplier, DateTime expiryDate)
        {
            MedicineId = id;
            MedicineName = name;
            Category = category;
            Dosage = dosage;
            Price = price;
            Stock = stock;
            Supplier = supplier;
            ExpiryDate = expiryDate;
            DateAdded = DateTime.Now;
            RequiresPrescription = false;
        }

        /// <summary>
        /// Validates if the medicine details are valid
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(MedicineName))
                return false;
            if (string.IsNullOrWhiteSpace(Category))
                return false;
            if (Price < 0)
                return false;
            if (Stock < 0)
                return false;
            if (ExpiryDate < DateTime.Now)
                return false;

            return true;
        }

        /// <summary>
        /// Checks if medicine is expired
        /// </summary>
        public bool IsExpired()
        {
            return ExpiryDate <= DateTime.Now;
        }

        /// <summary>
        /// Checks if stock is low (less than 10 units)
        /// </summary>
        public bool IsLowStock()
        {
            return Stock < 10;
        }
    }
}
