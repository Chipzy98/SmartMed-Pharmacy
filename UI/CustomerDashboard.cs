using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SmartMedPharmacy.DataAccess;
using SmartMedPharmacy.Models;

namespace SmartMedPharmacy.UI
{
    public partial class CustomerDashboard : Form
    {
        private Customer _customer;
        private DataManager _dataManager;
        private Order _currentOrder;
        private List<Medicine> _cartMedicines;

        public CustomerDashboard(Customer customer, DataManager dataManager)
        {
            InitializeComponent();
            _customer = customer;
            _dataManager = dataManager;
            _cartMedicines = new List<Medicine>();
            _currentOrder = new Order(_customer.CustomerId, _customer.GetFullName());
        }

        private void CustomerDashboard_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {_customer.GetFullName()}";
            LoadMedicines();
        }

        private void LoadMedicines()
        {
            try
            {
                dgvMedicines.DataSource = _dataManager.GetAllMedicines().Where(m => !m.IsExpired()).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading medicines: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearchMedicine_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSearchMedicine.Text))
                {
                    LoadMedicines();
                    return;
                }

                List<Medicine> results = _dataManager.SearchMedicinesByName(txtSearchMedicine.Text);
                dgvMedicines.DataSource = results.Where(m => !m.IsExpired()).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (dgvMedicines.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a medicine", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int medicineId = (int)dgvMedicines.SelectedRows[0].Cells["MedicineId"].Value;
                Medicine medicine = _dataManager.GetMedicineById(medicineId);

                if (medicine.RequiresPrescription)
                {
                    MessageBox.Show("This medicine requires a prescription. Please upload prescription to proceed.", 
                        "Prescription Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string quantityStr = PromptQuantity();
                if (!int.TryParse(quantityStr, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Invalid quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (quantity > medicine.Stock)
                {
                    MessageBox.Show("Not enough stock available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                OrderItem item = new OrderItem
                {
                    OrderItemId = _currentOrder.Items.Count + 1,
                    MedicineId = medicineId,
                    MedicineName = medicine.MedicineName,
                    Quantity = quantity,
                    UnitPrice = medicine.Price
                };

                _currentOrder.AddItem(item);
                UpdateCart();
                MessageBox.Show("Added to cart successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string PromptQuantity()
        {
            Form promptForm = new Form()
            {
                Text = "Enter Quantity",
                Width = 300,
                Height = 150,
                StartPosition = FormStartPosition.CenterParent
            };

            Label label = new Label() { Left = 20, Top = 20, Text = "Quantity:", Width = 250 };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 250 };
            Button okButton = new Button() { Text = "OK", Left = 120, Width = 80, Top = 80, DialogResult = DialogResult.OK };

            promptForm.Controls.Add(label);
            promptForm.Controls.Add(textBox);
            promptForm.Controls.Add(okButton);
            promptForm.AcceptButton = okButton;

            return promptForm.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void UpdateCart()
        {
            dgvCart.DataSource = _currentOrder.Items.Select(i => new { i.OrderItemId, i.MedicineName, i.Quantity, i.UnitPrice }).ToList();
            lblCartTotal.Text = $"Total: ${_currentOrder.TotalAmount:F2}";
        }

        private void btnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to remove", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int itemId = (int)dgvCart.SelectedRows[0].Cells["OrderItemId"].Value;
            _currentOrder.RemoveItem(itemId);
            UpdateCart();
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (_currentOrder.Items.Count == 0)
            {
                MessageBox.Show("Cart is empty. Add medicines to place an order.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _dataManager.AddOrder(_currentOrder);
                MessageBox.Show($"Order placed successfully. Order ID: {_currentOrder.OrderId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _currentOrder = new Order(_customer.CustomerId, _customer.GetFullName());
                UpdateCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTrackOrders_Click(object sender, EventArgs e)
        {
            try
            {
                var orders = _dataManager.GetCustomerOrders(_customer.CustomerId);
                dgvOrders.DataSource = orders.Select(o => new
                {
                    o.OrderId,
                    o.OrderDate,
                    Status = o.Status.ToString(),
                    o.TotalAmount
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }

        #region Designer
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabBrowseMedicines = new System.Windows.Forms.TabPage();
            this.dgvMedicines = new System.Windows.Forms.DataGridView();
            this.btnAddToCart = new System.Windows.Forms.Button();
            this.txtSearchMedicine = new System.Windows.Forms.TextBox();
            this.btnSearchMedicine = new System.Windows.Forms.Button();
            this.tabCart = new System.Windows.Forms.TabPage();
            this.dgvCart = new System.Windows.Forms.DataGridView();
            this.lblCartTotal = new System.Windows.Forms.Label();
            this.btnRemoveFromCart = new System.Windows.Forms.Button();
            this.btnPlaceOrder = new System.Windows.Forms.Button();
            this.tabOrders = new System.Windows.Forms.TabPage();
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.btnTrackOrders = new System.Windows.Forms.Button();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabBrowseMedicines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicines)).BeginInit();
            this.tabCart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).BeginInit();
            this.tabOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            this.SuspendLayout();

            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Arial", 12F);
            this.lblWelcome.Location = new System.Drawing.Point(20, 10);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(100, 19);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome";

            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(850, 10);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 25);
            this.btnLogout.TabIndex = 1;
            this.btnLogout.Text = "Logout";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            this.tabControl.Controls.Add(this.tabBrowseMedicines);
            this.tabControl.Controls.Add(this.tabCart);
            this.tabControl.Controls.Add(this.tabOrders);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 40);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1000, 560);
            this.tabControl.TabIndex = 0;

            this.tabBrowseMedicines.Controls.Add(this.dgvMedicines);
            this.tabBrowseMedicines.Controls.Add(this.btnAddToCart);
            this.tabBrowseMedicines.Controls.Add(this.txtSearchMedicine);
            this.tabBrowseMedicines.Controls.Add(this.btnSearchMedicine);
            this.tabBrowseMedicines.Location = new System.Drawing.Point(4, 22);
            this.tabBrowseMedicines.Name = "tabBrowseMedicines";
            this.tabBrowseMedicines.Size = new System.Drawing.Size(992, 534);
            this.tabBrowseMedicines.Text = "Browse Medicines";

            this.dgvMedicines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMedicines.Location = new System.Drawing.Point(20, 70);
            this.dgvMedicines.Name = "dgvMedicines";
            this.dgvMedicines.Size = new System.Drawing.Size(950, 400);
            this.dgvMedicines.TabIndex = 0;

            this.txtSearchMedicine.Location = new System.Drawing.Point(20, 30);
            this.txtSearchMedicine.Name = "txtSearchMedicine";
            this.txtSearchMedicine.Size = new System.Drawing.Size(820, 20);
            this.txtSearchMedicine.TabIndex = 1;

            this.btnSearchMedicine.Location = new System.Drawing.Point(850, 30);
            this.btnSearchMedicine.Name = "btnSearchMedicine";
            this.btnSearchMedicine.Size = new System.Drawing.Size(120, 25);
            this.btnSearchMedicine.TabIndex = 2;
            this.btnSearchMedicine.Text = "Search";
            this.btnSearchMedicine.Click += new System.EventHandler(this.btnSearchMedicine_Click);

            this.btnAddToCart.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnAddToCart.ForeColor = System.Drawing.Color.White;
            this.btnAddToCart.Location = new System.Drawing.Point(20, 490);
            this.btnAddToCart.Name = "btnAddToCart";
            this.btnAddToCart.Size = new System.Drawing.Size(150, 35);
            this.btnAddToCart.TabIndex = 3;
            this.btnAddToCart.Text = "Add to Cart";
            this.btnAddToCart.Click += new System.EventHandler(this.btnAddToCart_Click);

            this.tabCart.Controls.Add(this.dgvCart);
            this.tabCart.Controls.Add(this.lblCartTotal);
            this.tabCart.Controls.Add(this.btnRemoveFromCart);
            this.tabCart.Controls.Add(this.btnPlaceOrder);
            this.tabCart.Location = new System.Drawing.Point(4, 22);
            this.tabCart.Name = "tabCart";
            this.tabCart.Size = new System.Drawing.Size(992, 534);
            this.tabCart.Text = "Shopping Cart";

            this.dgvCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCart.Location = new System.Drawing.Point(20, 20);
            this.dgvCart.Name = "dgvCart";
            this.dgvCart.Size = new System.Drawing.Size(950, 400);
            this.dgvCart.TabIndex = 0;

            this.lblCartTotal.AutoSize = true;
            this.lblCartTotal.Font = new System.Drawing.Font("Arial", 12F);
            this.lblCartTotal.Location = new System.Drawing.Point(20, 430);
            this.lblCartTotal.Name = "lblCartTotal";
            this.lblCartTotal.Size = new System.Drawing.Size(80, 19);
            this.lblCartTotal.TabIndex = 1;
            this.lblCartTotal.Text = "Total: $0.00";

            this.btnRemoveFromCart.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnRemoveFromCart.ForeColor = System.Drawing.Color.White;
            this.btnRemoveFromCart.Location = new System.Drawing.Point(20, 470);
            this.btnRemoveFromCart.Name = "btnRemoveFromCart";
            this.btnRemoveFromCart.Size = new System.Drawing.Size(120, 35);
            this.btnRemoveFromCart.TabIndex = 2;
            this.btnRemoveFromCart.Text = "Remove";
            this.btnRemoveFromCart.Click += new System.EventHandler(this.btnRemoveFromCart_Click);

            this.btnPlaceOrder.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnPlaceOrder.ForeColor = System.Drawing.Color.White;
            this.btnPlaceOrder.Location = new System.Drawing.Point(850, 470);
            this.btnPlaceOrder.Name = "btnPlaceOrder";
            this.btnPlaceOrder.Size = new System.Drawing.Size(120, 35);
            this.btnPlaceOrder.TabIndex = 3;
            this.btnPlaceOrder.Text = "Place Order";
            this.btnPlaceOrder.Click += new System.EventHandler(this.btnPlaceOrder_Click);

            this.tabOrders.Controls.Add(this.dgvOrders);
            this.tabOrders.Controls.Add(this.btnTrackOrders);
            this.tabOrders.Location = new System.Drawing.Point(4, 22);
            this.tabOrders.Name = "tabOrders";
            this.tabOrders.Size = new System.Drawing.Size(992, 534);
            this.tabOrders.Text = "Track Orders";

            this.dgvOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrders.Location = new System.Drawing.Point(20, 50);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.Size = new System.Drawing.Size(950, 450);
            this.dgvOrders.TabIndex = 0;

            this.btnTrackOrders.Location = new System.Drawing.Point(20, 20);
            this.btnTrackOrders.Name = "btnTrackOrders";
            this.btnTrackOrders.Size = new System.Drawing.Size(120, 25);
            this.btnTrackOrders.TabIndex = 1;
            this.btnTrackOrders.Text = "Load Orders";
            this.btnTrackOrders.Click += new System.EventHandler(this.btnTrackOrders_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.btnLogout);
            this.Name = "CustomerDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SmartMed Pharmacy - Customer Dashboard";
            this.Load += new System.EventHandler(this.CustomerDashboard_Load);
            this.tabControl.ResumeLayout(false);
            this.tabBrowseMedicines.ResumeLayout(false);
            this.tabBrowseMedicines.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicines)).EndInit();
            this.tabCart.ResumeLayout(false);
            this.tabCart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).EndInit();
            this.tabOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabBrowseMedicines;
        private System.Windows.Forms.DataGridView dgvMedicines;
        private System.Windows.Forms.Button btnAddToCart;
        private System.Windows.Forms.TextBox txtSearchMedicine;
        private System.Windows.Forms.Button btnSearchMedicine;
        private System.Windows.Forms.TabPage tabCart;
        private System.Windows.Forms.DataGridView dgvCart;
        private System.Windows.Forms.Label lblCartTotal;
        private System.Windows.Forms.Button btnRemoveFromCart;
        private System.Windows.Forms.Button btnPlaceOrder;
        private System.Windows.Forms.TabPage tabOrders;
        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.Button btnTrackOrders;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnLogout;
        #endregion
    }
}
