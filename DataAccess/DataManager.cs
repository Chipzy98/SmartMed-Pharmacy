using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SmartMedPharmacy.Models;

namespace SmartMedPharmacy.DataAccess
{
    /// <summary>
    /// Handles all data persistence operations using JSON files
    /// </summary>
    public class DataManager
    {
        private readonly string _dataDirectory = "Data";
        private readonly string _medicinesFile = "medicines.json";
        private readonly string _customersFile = "customers.json";
        private readonly string _ordersFile = "orders.json";
        private readonly string _adminsFile = "admins.json";

        public DataManager()
        {
            InitializeDataDirectory();
        }

        /// <summary>
        /// Initializes the data directory and creates empty JSON files if they don't exist
        /// </summary>
        private void InitializeDataDirectory()
        {
            try
            {
                if (!Directory.Exists(_dataDirectory))
                    Directory.CreateDirectory(_dataDirectory);

                string medicinesPath = Path.Combine(_dataDirectory, _medicinesFile);
                if (!File.Exists(medicinesPath))
                    File.WriteAllText(medicinesPath, "[]");

                string customersPath = Path.Combine(_dataDirectory, _customersFile);
                if (!File.Exists(customersPath))
                    File.WriteAllText(customersPath, "[]");

                string ordersPath = Path.Combine(_dataDirectory, _ordersFile);
                if (!File.Exists(ordersPath))
                    File.WriteAllText(ordersPath, "[]");

                string adminsPath = Path.Combine(_dataDirectory, _adminsFile);
                if (!File.Exists(adminsPath))
                    File.WriteAllText(adminsPath, "[]");
            }
            catch (Exception ex)
            {
                throw new Exception("Error initializing data directory: " + ex.Message);
            }
        }

        #region Medicine Methods

        /// <summary>
        /// Gets all medicines from storage
        /// </summary>
        public List<Medicine> GetAllMedicines()
        {
            try
            {
                string path = Path.Combine(_dataDirectory, _medicinesFile);
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<List<Medicine>>(json) ?? new List<Medicine>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading medicines: " + ex.Message);
            }
        }

        /// <summary>
        /// Adds a new medicine to storage
        /// </summary>
        public void AddMedicine(Medicine medicine)
        {
            try
            {
                List<Medicine> medicines = GetAllMedicines();
                medicine.MedicineId = medicines.Count > 0 ? medicines.Max(m => m.MedicineId) + 1 : 1;
                medicines.Add(medicine);
                SaveMedicines(medicines);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding medicine: " + ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing medicine
        /// </summary>
        public void UpdateMedicine(Medicine medicine)
        {
            try
            {
                List<Medicine> medicines = GetAllMedicines();
                Medicine existing = medicines.FirstOrDefault(m => m.MedicineId == medicine.MedicineId);
                if (existing != null)
                {
                    medicines.Remove(existing);
                    medicines.Add(medicine);
                    SaveMedicines(medicines);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating medicine: " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes a medicine from storage
        /// </summary>
        public void DeleteMedicine(int medicineId)
        {
            try
            {
                List<Medicine> medicines = GetAllMedicines();
                medicines.RemoveAll(m => m.MedicineId == medicineId);
                SaveMedicines(medicines);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting medicine: " + ex.Message);
            }
        }

        /// <summary>
        /// Gets a medicine by ID
        /// </summary>
        public Medicine GetMedicineById(int medicineId)
        {
            try
            {
                List<Medicine> medicines = GetAllMedicines();
                return medicines.FirstOrDefault(m => m.MedicineId == medicineId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting medicine: " + ex.Message);
            }
        }

        /// <summary>
        /// Searches medicines by name (linear search)
        /// </summary>
        public List<Medicine> SearchMedicinesByName(string name)
        {
            try
            {
                List<Medicine> medicines = GetAllMedicines();
                return medicines.Where(m => m.MedicineName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching medicines: " + ex.Message);
            }
        }

        /// <summary>
        /// Searches medicines by category
        /// </summary>
        public List<Medicine> SearchMedicinesByCategory(string category)
        {
            try
            {
                List<Medicine> medicines = GetAllMedicines();
                return medicines.Where(m => m.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching by category: " + ex.Message);
            }
        }

        /// <summary>
        /// Searches medicines by price range
        /// </summary>
        public List<Medicine> SearchMedicinesByPriceRange(decimal minPrice, decimal maxPrice)
        {
            try
            {
                List<Medicine> medicines = GetAllMedicines();
                return medicines.Where(m => m.Price >= minPrice && m.Price <= maxPrice).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching by price range: " + ex.Message);
            }
        }

        private void SaveMedicines(List<Medicine> medicines)
        {
            try
            {
                string path = Path.Combine(_dataDirectory, _medicinesFile);
                string json = JsonConvert.SerializeObject(medicines, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving medicines: " + ex.Message);
            }
        }

        #endregion

        #region Customer Methods

        /// <summary>
        /// Gets all customers from storage
        /// </summary>
        public List<Customer> GetAllCustomers()
        {
            try
            {
                string path = Path.Combine(_dataDirectory, _customersFile);
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<List<Customer>>(json) ?? new List<Customer>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading customers: " + ex.Message);
            }
        }

        /// <summary>
        /// Registers a new customer
        /// </summary>
        public void AddCustomer(Customer customer)
        {
            try
            {
                List<Customer> customers = GetAllCustomers();
                if (customers.Any(c => c.Username == customer.Username))
                    throw new Exception("Username already exists");

                customer.CustomerId = customers.Count > 0 ? customers.Max(c => c.CustomerId) + 1 : 1;
                customers.Add(customer);
                SaveCustomers(customers);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding customer: " + ex.Message);
            }
        }

        /// <summary>
        /// Updates customer information
        /// </summary>
        public void UpdateCustomer(Customer customer)
        {
            try
            {
                List<Customer> customers = GetAllCustomers();
                Customer existing = customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
                if (existing != null)
                {
                    customers.Remove(existing);
                    customers.Add(customer);
                    SaveCustomers(customers);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating customer: " + ex.Message);
            }
        }

        /// <summary>
        /// Gets a customer by ID
        /// </summary>
        public Customer GetCustomerById(int customerId)
        {
            try
            {
                List<Customer> customers = GetAllCustomers();
                return customers.FirstOrDefault(c => c.CustomerId == customerId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting customer: " + ex.Message);
            }
        }

        /// <summary>
        /// Authenticates a customer (login)
        /// </summary>
        public Customer AuthenticateCustomer(string username, string password)
        {
            try
            {
                List<Customer> customers = GetAllCustomers();
                return customers.FirstOrDefault(c => c.Username == username && c.Password == password && c.IsActive);
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating customer: " + ex.Message);
            }
        }

        private void SaveCustomers(List<Customer> customers)
        {
            try
            {
                string path = Path.Combine(_dataDirectory, _customersFile);
                string json = JsonConvert.SerializeObject(customers, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving customers: " + ex.Message);
            }
        }

        #endregion

        #region Order Methods

        /// <summary>
        /// Gets all orders from storage
        /// </summary>
        public List<Order> GetAllOrders()
        {
            try
            {
                string path = Path.Combine(_dataDirectory, _ordersFile);
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<List<Order>>(json) ?? new List<Order>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading orders: " + ex.Message);
            }
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        public void AddOrder(Order order)
        {
            try
            {
                List<Order> orders = GetAllOrders();
                order.OrderId = orders.Count > 0 ? orders.Max(o => o.OrderId) + 1 : 1;
                orders.Add(order);
                SaveOrders(orders);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding order: " + ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing order
        /// </summary>
        public void UpdateOrder(Order order)
        {
            try
            {
                List<Order> orders = GetAllOrders();
                Order existing = orders.FirstOrDefault(o => o.OrderId == order.OrderId);
                if (existing != null)
                {
                    orders.Remove(existing);
                    orders.Add(order);
                    SaveOrders(orders);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating order: " + ex.Message);
            }
        }

        /// <summary>
        /// Gets orders for a specific customer
        /// </summary>
        public List<Order> GetCustomerOrders(int customerId)
        {
            try
            {
                List<Order> orders = GetAllOrders();
                return orders.Where(o => o.CustomerId == customerId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting customer orders: " + ex.Message);
            }
        }

        /// <summary>
        /// Gets a specific order by ID
        /// </summary>
        public Order GetOrderById(int orderId)
        {
            try
            {
                List<Order> orders = GetAllOrders();
                return orders.FirstOrDefault(o => o.OrderId == orderId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting order: " + ex.Message);
            }
        }

        private void SaveOrders(List<Order> orders)
        {
            try
            {
                string path = Path.Combine(_dataDirectory, _ordersFile);
                string json = JsonConvert.SerializeObject(orders, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving orders: " + ex.Message);
            }
        }

        #endregion

        #region Admin Methods

        /// <summary>
        /// Gets all admins from storage
        /// </summary>
        public List<Admin> GetAllAdmins()
        {
            try
            {
                string path = Path.Combine(_dataDirectory, _adminsFile);
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<List<Admin>>(json) ?? new List<Admin>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading admins: " + ex.Message);
            }
        }

        /// <summary>
        /// Adds a new admin
        /// </summary>
        public void AddAdmin(Admin admin)
        {
            try
            {
                List<Admin> admins = GetAllAdmins();
                if (admins.Any(a => a.Username == admin.Username))
                    throw new Exception("Username already exists");

                admin.AdminId = admins.Count > 0 ? admins.Max(a => a.AdminId) + 1 : 1;
                admin.CreatedDate = DateTime.Now;
                admins.Add(admin);
                SaveAdmins(admins);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding admin: " + ex.Message);
            }
        }

        /// <summary>
        /// Gets an admin by ID
        /// </summary>
        public Admin GetAdminById(int adminId)
        {
            try
            {
                List<Admin> admins = GetAllAdmins();
                return admins.FirstOrDefault(a => a.AdminId == adminId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting admin: " + ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing admin
        /// </summary>
        public void UpdateAdmin(Admin admin)
        {
            try
            {
                List<Admin> admins = GetAllAdmins();
                Admin existing = admins.FirstOrDefault(a => a.AdminId == admin.AdminId);
                if (existing != null)
                {
                    admins.Remove(existing);
                    admins.Add(admin);
                    SaveAdmins(admins);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating admin: " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes an admin from storage
        /// </summary>
        public void DeleteAdmin(int adminId)
        {
            try
            {
                List<Admin> admins = GetAllAdmins();
                admins.RemoveAll(a => a.AdminId == adminId);
                SaveAdmins(admins);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting admin: " + ex.Message);
            }
        }

        /// <summary>
        /// Authenticates an admin (login)
        /// </summary>
        public Admin AuthenticateAdmin(string username, string password)
        {
            try
            {
                List<Admin> admins = GetAllAdmins();
                return admins.FirstOrDefault(a => a.Username == username && a.Password == password && a.IsActive);
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating admin: " + ex.Message);
            }
        }

        /// <summary>
        /// Searches admins by role
        /// </summary>
        public List<Admin> GetAdminsByRole(string role)
        {
            try
            {
                List<Admin> admins = GetAllAdmins();
                return admins.Where(a => a.Role.Equals(role, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching admins by role: " + ex.Message);
            }
        }

        /// <summary>
        /// Gets active admins only
        /// </summary>
        public List<Admin> GetActiveAdmins()
        {
            try
            {
                List<Admin> admins = GetAllAdmins();
                return admins.Where(a => a.IsActive).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting active admins: " + ex.Message);
            }
        }

        private void SaveAdmins(List<Admin> admins)
        {
            try
            {
                string path = Path.Combine(_dataDirectory, _adminsFile);
                string json = JsonConvert.SerializeObject(admins, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving admins: " + ex.Message);
            }
        }

        #endregion
    }
}