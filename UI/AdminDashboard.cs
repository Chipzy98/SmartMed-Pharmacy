using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SmartMedPharmacy.DataAccess;
using SmartMedPharmacy.Models;

namespace SmartMedPharmacy.UI
{
    public partial class AdminDashboard : Form
    {
        private Admin _admin;
        private DataManager _dataManager;

        public AdminDashboard(Admin admin, DataManager dataManager)
        {
            InitializeComponent();
            _admin = admin;
            _dataManager = dataManager;
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {_admin.GetFullName()}";
            LoadDashboardData();
            LoadMedicines();
        }

        /// <summary>
        /// Loads and displays dashboard statistics
        /// </summary>
        private void LoadDashboardData()
        {
            try
            {
                var medicines = _dataManager.GetAllMedicines();
                var orders = _dataManager.GetAllOrders();
                var customers = _dataManager.GetAllCustomers();

                decimal totalSales = orders.Where(o => o.Status == OrderStatus.Delivered).Sum(o => o.TotalAmount);
                int totalMedicines = medicines.Count;
                int activeOrders = orders.Count(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.ReadyForPickup);

                lblTotalSales.Text = $"Rs.{totalSales:F2}";
                lblTotalMedicines.Text = totalMedicines.ToString();
                lblActiveOrders.Text = activeOrders.ToString();
                lblTotalCustomers.Text = customers.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads medicines into the medicines tab
        /// </summary>
        private void LoadMedicines()
        {
            try
            {
                dgvMedicines.DataSource = _dataManager.GetAllMedicines();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading medicines: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddMedicine_Click(object sender, EventArgs e)
        {
            AddMedicineForm addForm = new AddMedicineForm(_dataManager);
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadMedicines();
                LoadDashboardData();
            }
        }

        private void btnUpdateMedicine_Click(object sender, EventArgs e)
        {
            if (dgvMedicines.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a medicine to update", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int medicineId = (int)dgvMedicines.SelectedRows[0].Cells["MedicineId"].Value;
            Medicine medicine = _dataManager.GetMedicineById(medicineId);

            UpdateMedicineForm updateForm = new UpdateMedicineForm(medicine, _dataManager);
            if (updateForm.ShowDialog() == DialogResult.OK)
            {
                LoadMedicines();
                LoadDashboardData();
            }
        }

        private void btnDeleteMedicine_Click(object sender, EventArgs e)
        {
            if (dgvMedicines.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a medicine to delete", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this medicine?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int medicineId = (int)dgvMedicines.SelectedRows[0].Cells["MedicineId"].Value;
                    _dataManager.DeleteMedicine(medicineId);
                    MessageBox.Show("Medicine deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadMedicines();
                    LoadDashboardData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSearchMedicine_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchMedicine.Text))
            {
                LoadMedicines();
                return;
            }

            try
            {
                List<Medicine> results = _dataManager.SearchMedicinesByName(txtSearchMedicine.Text);
                dgvMedicines.DataSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewOrders_Click(object sender, EventArgs e)
        {
            try
            {
                dgvOrders.DataSource = _dataManager.GetAllOrders().Select(o => new
                {
                    o.OrderId,
                    o.CustomerId,
                    o.CustomerName,
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

        private void btnUpdateOrderStatus_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to update", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int orderId = (int)dgvOrders.SelectedRows[0].Cells["OrderId"].Value;
            Order order = _dataManager.GetOrderById(orderId);

            UpdateOrderStatusForm updateForm = new UpdateOrderStatusForm(order, _dataManager);
            if (updateForm.ShowDialog() == DialogResult.OK)
            {
                btnViewOrders_Click(null, null);
                LoadDashboardData();
            }
        }

        private void btnViewCustomers_Click(object sender, EventArgs e)
        {
            try
            {
                dgvCustomers.DataSource = _dataManager.GetAllCustomers().Select(c => new
                {
                    c.CustomerId,
                    c.FirstName,
                    c.LastName,
                    c.Email,
                    c.PhoneNumber,
                    c.City
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens the Generate Report Form
        /// Reports available: Sales, Stock, Customer
        /// </summary>
        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateReportForm reportForm = new GenerateReportForm(_dataManager);
                reportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening report form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region Designer Generated Code
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDashboard = new System.Windows.Forms.TabPage();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblSalesLabel = new System.Windows.Forms.Label();
            this.lblTotalSales = new System.Windows.Forms.Label();
            this.lblMedicinesLabel = new System.Windows.Forms.Label();
            this.lblTotalMedicines = new System.Windows.Forms.Label();
            this.lblOrdersLabel = new System.Windows.Forms.Label();
            this.lblActiveOrders = new System.Windows.Forms.Label();
            this.lblCustomersLabel = new System.Windows.Forms.Label();
            this.lblTotalCustomers = new System.Windows.Forms.Label();
            this.tabMedicines = new System.Windows.Forms.TabPage();
            this.dgvMedicines = new System.Windows.Forms.DataGridView();
            this.btnAddMedicine = new System.Windows.Forms.Button();
            this.btnUpdateMedicine = new System.Windows.Forms.Button();
            this.btnDeleteMedicine = new System.Windows.Forms.Button();
            this.btnSearchMedicine = new System.Windows.Forms.Button();
            this.txtSearchMedicine = new System.Windows.Forms.TextBox();
            this.tabOrders = new System.Windows.Forms.TabPage();
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.btnViewOrders = new System.Windows.Forms.Button();
            this.btnUpdateOrderStatus = new System.Windows.Forms.Button();
            this.tabCustomers = new System.Windows.Forms.TabPage();
            this.dgvCustomers = new System.Windows.Forms.DataGridView();
            this.btnViewCustomers = new System.Windows.Forms.Button();
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnGenerateReport = new System.Windows.Forms.Button();

            this.tabControl.SuspendLayout();
            this.tabDashboard.SuspendLayout();
            this.tabMedicines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicines)).BeginInit();
            this.tabOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            this.tabCustomers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            this.pnlTopBar.SuspendLayout();
            this.SuspendLayout();

            // Top Bar Panel
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(33, 37, 41);
            this.pnlTopBar.Controls.Add(this.btnGenerateReport);
            this.pnlTopBar.Controls.Add(this.btnLogout);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Height = 50;
            this.pnlTopBar.Name = "pnlTopBar";

            // tabControl
            this.tabControl.Controls.Add(this.tabDashboard);
            this.tabControl.Controls.Add(this.tabMedicines);
            this.tabControl.Controls.Add(this.tabOrders);
            this.tabControl.Controls.Add(this.tabCustomers);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 50);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1143, 643);
            this.tabControl.TabIndex = 0;

            // tabDashboard
            this.tabDashboard.Controls.Add(this.lblWelcome);
            this.tabDashboard.Controls.Add(this.lblSalesLabel);
            this.tabDashboard.Controls.Add(this.lblTotalSales);
            this.tabDashboard.Controls.Add(this.lblMedicinesLabel);
            this.tabDashboard.Controls.Add(this.lblTotalMedicines);
            this.tabDashboard.Controls.Add(this.lblOrdersLabel);
            this.tabDashboard.Controls.Add(this.lblActiveOrders);
            this.tabDashboard.Controls.Add(this.lblCustomersLabel);
            this.tabDashboard.Controls.Add(this.lblTotalCustomers);
            this.tabDashboard.Location = new System.Drawing.Point(4, 25);
            this.tabDashboard.Name = "tabDashboard";
            this.tabDashboard.Size = new System.Drawing.Size(1135, 614);
            this.tabDashboard.TabIndex = 0;
            this.tabDashboard.Text = "Dashboard";
            this.tabDashboard.UseVisualStyleBackColor = true;

            // lblWelcome
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.Location = new System.Drawing.Point(34, 32);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(118, 29);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome";

            // lblSalesLabel
            this.lblSalesLabel.AutoSize = true;
            this.lblSalesLabel.Location = new System.Drawing.Point(57, 107);
            this.lblSalesLabel.Name = "lblSalesLabel";
            this.lblSalesLabel.Size = new System.Drawing.Size(79, 16);
            this.lblSalesLabel.TabIndex = 1;
            this.lblSalesLabel.Text = "Total Sales:";

            // lblTotalSales
            this.lblTotalSales.AutoSize = true;
            this.lblTotalSales.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalSales.ForeColor = System.Drawing.Color.Green;
            this.lblTotalSales.Location = new System.Drawing.Point(171, 107);
            this.lblTotalSales.Name = "lblTotalSales";
            this.lblTotalSales.Size = new System.Drawing.Size(80, 24);
            this.lblTotalSales.TabIndex = 2;
            this.lblTotalSales.Text = "Rs.0.00";

            // lblMedicinesLabel
            this.lblMedicinesLabel.AutoSize = true;
            this.lblMedicinesLabel.Location = new System.Drawing.Point(57, 160);
            this.lblMedicinesLabel.Name = "lblMedicinesLabel";
            this.lblMedicinesLabel.Size = new System.Drawing.Size(106, 16);
            this.lblMedicinesLabel.TabIndex = 3;
            this.lblMedicinesLabel.Text = "Total Medicines:";

            // lblTotalMedicines
            this.lblTotalMedicines.AutoSize = true;
            this.lblTotalMedicines.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalMedicines.ForeColor = System.Drawing.Color.Blue;
            this.lblTotalMedicines.Location = new System.Drawing.Point(171, 160);
            this.lblTotalMedicines.Name = "lblTotalMedicines";
            this.lblTotalMedicines.Size = new System.Drawing.Size(21, 24);
            this.lblTotalMedicines.TabIndex = 4;
            this.lblTotalMedicines.Text = "0";

            // lblOrdersLabel
            this.lblOrdersLabel.AutoSize = true;
            this.lblOrdersLabel.Location = new System.Drawing.Point(57, 213);
            this.lblOrdersLabel.Name = "lblOrdersLabel";
            this.lblOrdersLabel.Size = new System.Drawing.Size(91, 16);
            this.lblOrdersLabel.TabIndex = 5;
            this.lblOrdersLabel.Text = "Active Orders:";

            // lblActiveOrders
            this.lblActiveOrders.AutoSize = true;
            this.lblActiveOrders.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblActiveOrders.ForeColor = System.Drawing.Color.Orange;
            this.lblActiveOrders.Location = new System.Drawing.Point(171, 213);
            this.lblActiveOrders.Name = "lblActiveOrders";
            this.lblActiveOrders.Size = new System.Drawing.Size(21, 24);
            this.lblActiveOrders.TabIndex = 6;
            this.lblActiveOrders.Text = "0";

            // lblCustomersLabel
            this.lblCustomersLabel.AutoSize = true;
            this.lblCustomersLabel.Location = new System.Drawing.Point(57, 267);
            this.lblCustomersLabel.Name = "lblCustomersLabel";
            this.lblCustomersLabel.Size = new System.Drawing.Size(108, 16);
            this.lblCustomersLabel.TabIndex = 7;
            this.lblCustomersLabel.Text = "Total Customers:";

            // lblTotalCustomers
            this.lblTotalCustomers.AutoSize = true;
            this.lblTotalCustomers.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalCustomers.ForeColor = System.Drawing.Color.Purple;
            this.lblTotalCustomers.Location = new System.Drawing.Point(171, 267);
            this.lblTotalCustomers.Name = "lblTotalCustomers";
            this.lblTotalCustomers.Size = new System.Drawing.Size(21, 24);
            this.lblTotalCustomers.TabIndex = 8;
            this.lblTotalCustomers.Text = "0";

            // tabMedicines
            this.tabMedicines.Controls.Add(this.dgvMedicines);
            this.tabMedicines.Controls.Add(this.btnAddMedicine);
            this.tabMedicines.Controls.Add(this.btnUpdateMedicine);
            this.tabMedicines.Controls.Add(this.btnDeleteMedicine);
            this.tabMedicines.Controls.Add(this.btnSearchMedicine);
            this.tabMedicines.Controls.Add(this.txtSearchMedicine);
            this.tabMedicines.Location = new System.Drawing.Point(4, 25);
            this.tabMedicines.Name = "tabMedicines";
            this.tabMedicines.Size = new System.Drawing.Size(1135, 614);
            this.tabMedicines.TabIndex = 1;
            this.tabMedicines.Text = "Medicines";
            this.tabMedicines.UseVisualStyleBackColor = true;

            // dgvMedicines
            this.dgvMedicines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMedicines.Location = new System.Drawing.Point(23, 75);
            this.dgvMedicines.Name = "dgvMedicines";
            this.dgvMedicines.RowHeadersWidth = 51;
            this.dgvMedicines.Size = new System.Drawing.Size(1086, 427);
            this.dgvMedicines.TabIndex = 0;

            // btnAddMedicine
            this.btnAddMedicine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddMedicine.ForeColor = System.Drawing.Color.White;
            this.btnAddMedicine.Location = new System.Drawing.Point(23, 523);
            this.btnAddMedicine.Name = "btnAddMedicine";
            this.btnAddMedicine.Size = new System.Drawing.Size(114, 37);
            this.btnAddMedicine.TabIndex = 1;
            this.btnAddMedicine.Text = "Add";
            this.btnAddMedicine.UseVisualStyleBackColor = false;
            this.btnAddMedicine.Click += new System.EventHandler(this.btnAddMedicine_Click);

            // btnUpdateMedicine
            this.btnUpdateMedicine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnUpdateMedicine.ForeColor = System.Drawing.Color.White;
            this.btnUpdateMedicine.Location = new System.Drawing.Point(160, 523);
            this.btnUpdateMedicine.Name = "btnUpdateMedicine";
            this.btnUpdateMedicine.Size = new System.Drawing.Size(114, 37);
            this.btnUpdateMedicine.TabIndex = 2;
            this.btnUpdateMedicine.Text = "Update";
            this.btnUpdateMedicine.UseVisualStyleBackColor = false;
            this.btnUpdateMedicine.Click += new System.EventHandler(this.btnUpdateMedicine_Click);

            // btnDeleteMedicine
            this.btnDeleteMedicine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDeleteMedicine.ForeColor = System.Drawing.Color.White;
            this.btnDeleteMedicine.Location = new System.Drawing.Point(297, 523);
            this.btnDeleteMedicine.Name = "btnDeleteMedicine";
            this.btnDeleteMedicine.Size = new System.Drawing.Size(114, 37);
            this.btnDeleteMedicine.TabIndex = 3;
            this.btnDeleteMedicine.Text = "Delete";
            this.btnDeleteMedicine.UseVisualStyleBackColor = false;
            this.btnDeleteMedicine.Click += new System.EventHandler(this.btnDeleteMedicine_Click);

            // btnSearchMedicine
            this.btnSearchMedicine.Location = new System.Drawing.Point(971, 32);
            this.btnSearchMedicine.Name = "btnSearchMedicine";
            this.btnSearchMedicine.Size = new System.Drawing.Size(137, 27);
            this.btnSearchMedicine.TabIndex = 5;
            this.btnSearchMedicine.Text = "Search";
            this.btnSearchMedicine.Click += new System.EventHandler(this.btnSearchMedicine_Click);

            // txtSearchMedicine
            this.txtSearchMedicine.Location = new System.Drawing.Point(23, 32);
            this.txtSearchMedicine.Name = "txtSearchMedicine";
            this.txtSearchMedicine.Size = new System.Drawing.Size(937, 22);
            this.txtSearchMedicine.TabIndex = 4;

            // tabOrders
            this.tabOrders.Controls.Add(this.dgvOrders);
            this.tabOrders.Controls.Add(this.btnViewOrders);
            this.tabOrders.Controls.Add(this.btnUpdateOrderStatus);
            this.tabOrders.Location = new System.Drawing.Point(4, 25);
            this.tabOrders.Name = "tabOrders";
            this.tabOrders.Size = new System.Drawing.Size(1135, 614);
            this.tabOrders.TabIndex = 2;
            this.tabOrders.Text = "Orders";
            this.tabOrders.UseVisualStyleBackColor = true;

            // dgvOrders
            this.dgvOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrders.Location = new System.Drawing.Point(23, 64);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.RowHeadersWidth = 51;
            this.dgvOrders.Size = new System.Drawing.Size(1086, 427);
            this.dgvOrders.TabIndex = 0;

            // btnViewOrders
            this.btnViewOrders.Location = new System.Drawing.Point(23, 32);
            this.btnViewOrders.Name = "btnViewOrders";
            this.btnViewOrders.Size = new System.Drawing.Size(114, 27);
            this.btnViewOrders.TabIndex = 1;
            this.btnViewOrders.Text = "Load Orders";
            this.btnViewOrders.Click += new System.EventHandler(this.btnViewOrders_Click);

            // btnUpdateOrderStatus
            this.btnUpdateOrderStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnUpdateOrderStatus.ForeColor = System.Drawing.Color.White;
            this.btnUpdateOrderStatus.Location = new System.Drawing.Point(23, 523);
            this.btnUpdateOrderStatus.Name = "btnUpdateOrderStatus";
            this.btnUpdateOrderStatus.Size = new System.Drawing.Size(171, 37);
            this.btnUpdateOrderStatus.TabIndex = 2;
            this.btnUpdateOrderStatus.Text = "Update Status";
            this.btnUpdateOrderStatus.UseVisualStyleBackColor = false;
            this.btnUpdateOrderStatus.Click += new System.EventHandler(this.btnUpdateOrderStatus_Click);

            // tabCustomers
            this.tabCustomers.Controls.Add(this.dgvCustomers);
            this.tabCustomers.Controls.Add(this.btnViewCustomers);
            this.tabCustomers.Location = new System.Drawing.Point(4, 25);
            this.tabCustomers.Name = "tabCustomers";
            this.tabCustomers.Size = new System.Drawing.Size(1135, 614);
            this.tabCustomers.TabIndex = 3;
            this.tabCustomers.Text = "Customers";
            this.tabCustomers.UseVisualStyleBackColor = true;

            // dgvCustomers
            this.dgvCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomers.Location = new System.Drawing.Point(23, 64);
            this.dgvCustomers.Name = "dgvCustomers";
            this.dgvCustomers.RowHeadersWidth = 51;
            this.dgvCustomers.Size = new System.Drawing.Size(1086, 480);
            this.dgvCustomers.TabIndex = 0;

            // btnViewCustomers
            this.btnViewCustomers.Location = new System.Drawing.Point(23, 32);
            this.btnViewCustomers.Name = "btnViewCustomers";
            this.btnViewCustomers.Size = new System.Drawing.Size(137, 27);
            this.btnViewCustomers.TabIndex = 1;
            this.btnViewCustomers.Text = "Load Customers";
            this.btnViewCustomers.Click += new System.EventHandler(this.btnViewCustomers_Click);

            // btnGenerateReport - IMPROVED POSITIONING
            this.btnGenerateReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(66)))), ((int)(((byte)(193)))));
            this.btnGenerateReport.ForeColor = System.Drawing.Color.White;
            this.btnGenerateReport.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnGenerateReport.Location = new System.Drawing.Point(20, 10);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(180, 35);
            this.btnGenerateReport.TabIndex = 1;
            this.btnGenerateReport.Text = "📊 Generate Reports";
            this.btnGenerateReport.UseVisualStyleBackColor = false;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);

            // btnLogout
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.Location = new System.Drawing.Point(1020, 10);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(110, 35);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "🚪 Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // AdminDashboard
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 693);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlTopBar);
            this.Name = "AdminDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SmartMed Pharmacy - Admin Dashboard";
            this.Load += new System.EventHandler(this.AdminDashboard_Load);

            this.tabControl.ResumeLayout(false);
            this.tabDashboard.ResumeLayout(false);
            this.tabDashboard.PerformLayout();
            this.tabMedicines.ResumeLayout(false);
            this.tabMedicines.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicines)).EndInit();
            this.tabOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            this.tabCustomers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            this.pnlTopBar.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabDashboard;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblSalesLabel;
        private System.Windows.Forms.Label lblTotalSales;
        private System.Windows.Forms.Label lblMedicinesLabel;
        private System.Windows.Forms.Label lblTotalMedicines;
        private System.Windows.Forms.Label lblOrdersLabel;
        private System.Windows.Forms.Label lblActiveOrders;
        private System.Windows.Forms.Label lblCustomersLabel;
        private System.Windows.Forms.Label lblTotalCustomers;
        private System.Windows.Forms.TabPage tabMedicines;
        private System.Windows.Forms.DataGridView dgvMedicines;
        private System.Windows.Forms.Button btnAddMedicine;
        private System.Windows.Forms.Button btnUpdateMedicine;
        private System.Windows.Forms.Button btnDeleteMedicine;
        private System.Windows.Forms.Button btnSearchMedicine;
        private System.Windows.Forms.TextBox txtSearchMedicine;
        private System.Windows.Forms.TabPage tabOrders;
        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.Button btnViewOrders;
        private System.Windows.Forms.Button btnUpdateOrderStatus;
        private System.Windows.Forms.TabPage tabCustomers;
        private System.Windows.Forms.DataGridView dgvCustomers;
        private System.Windows.Forms.Button btnViewCustomers;
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnGenerateReport;

        #endregion
    }
}