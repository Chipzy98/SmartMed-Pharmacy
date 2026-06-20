using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SmartMedPharmacy.DataAccess;
using SmartMedPharmacy.Models;

namespace SmartMedPharmacy.UI
{
    public partial class GenerateReportForm : Form
    {
        private DataManager _dataManager;

        public GenerateReportForm(DataManager dataManager)
        {
            InitializeComponent();
            _dataManager = dataManager;
        }

        private void btnGenerateSalesReport_Click(object sender, EventArgs e)
        {
            try
            {
                var orders = _dataManager.GetAllOrders().Where(o => o.Status == OrderStatus.Delivered).ToList();
                decimal totalSales = orders.Sum(o => o.TotalAmount);
                int totalOrders = orders.Count;

                string report = "=== SALES REPORT ===\n";
                report += $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\n";
                report += $"Total Sales: ${totalSales:F2}\n";
                report += $"Total Delivered Orders: {totalOrders}\n";
                report += $"Average Order Value: ${(totalOrders > 0 ? totalSales / totalOrders : 0):F2}\n\n";
                report += "=== ORDER DETAILS ===\n";

                foreach (var order in orders)
                {
                    report += $"Order ID: {order.OrderId} | Customer: {order.CustomerName} | Amount: ${order.TotalAmount:F2}\n";
                }

                rtbReport.Text = report;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerateStockReport_Click(object sender, EventArgs e)
        {
            try
            {
                var medicines = _dataManager.GetAllMedicines();
                var lowStock = medicines.Where(m => m.IsLowStock()).ToList();
                var expired = medicines.Where(m => m.IsExpired()).ToList();

                string report = "=== STOCK REPORT ===\n";
                report += $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\n";
                report += $"Total Medicines: {medicines.Count}\n";
                report += $"Low Stock Items (< 10): {lowStock.Count}\n";
                report += $"Expired Items: {expired.Count}\n\n";

                if (lowStock.Count > 0)
                {
                    report += "=== LOW STOCK MEDICINES ===\n";
                    foreach (var med in lowStock)
                    {
                        report += $"{med.MedicineName} ({med.Category}) - Stock: {med.Stock}\n";
                    }
                    report += "\n";
                }

                if (expired.Count > 0)
                {
                    report += "=== EXPIRED MEDICINES ===\n";
                    foreach (var med in expired)
                    {
                        report += $"{med.MedicineName} ({med.Category}) - Expired: {med.ExpiryDate:yyyy-MM-dd}\n";
                    }
                }

                rtbReport.Text = report;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerateCustomerReport_Click(object sender, EventArgs e)
        {
            try
            {
                var customers = _dataManager.GetAllCustomers();
                var orders = _dataManager.GetAllOrders();

                string report = "=== CUSTOMER REPORT ===\n";
                report += $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\n";
                report += $"Total Customers: {customers.Count}\n";
                report += $"Active Customers: {customers.Count(c => c.IsActive)}\n\n";
                report += "=== TOP CUSTOMERS BY ORDERS ===\n";

                var topCustomers = orders.GroupBy(o => o.CustomerId)
                    .OrderByDescending(g => g.Count())
                    .Take(10)
                    .ToList();

                foreach (var customerGroup in topCustomers)
                {
                    var customer = customers.FirstOrDefault(c => c.CustomerId == customerGroup.Key);
                    if (customer != null)
                    {
                        report += $"{customer.GetFullName()} - Orders: {customerGroup.Count()} - Total: ${customerGroup.Sum(o => o.TotalAmount):F2}\n";
                    }
                }

                rtbReport.Text = report;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportReport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                FileName = $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.File.WriteAllText(sfd.FileName, rtbReport.Text);
                    MessageBox.Show("Report exported successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #region Designer
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnGenerateSalesReport = new System.Windows.Forms.Button();
            this.btnGenerateStockReport = new System.Windows.Forms.Button();
            this.btnGenerateCustomerReport = new System.Windows.Forms.Button();
            this.rtbReport = new System.Windows.Forms.RichTextBox();
            this.btnExportReport = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.btnGenerateSalesReport.BackColor = System.Drawing.Color.Green;
            this.btnGenerateSalesReport.ForeColor = System.Drawing.Color.White;
            this.btnGenerateSalesReport.Location = new System.Drawing.Point(20, 20);
            this.btnGenerateSalesReport.Name = "btnGenerateSalesReport";
            this.btnGenerateSalesReport.Size = new System.Drawing.Size(150, 35);
            this.btnGenerateSalesReport.TabIndex = 0;
            this.btnGenerateSalesReport.Text = "Sales Report";
            this.btnGenerateSalesReport.Click += new System.EventHandler(this.btnGenerateSalesReport_Click);

            this.btnGenerateStockReport.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.btnGenerateStockReport.ForeColor = System.Drawing.Color.White;
            this.btnGenerateStockReport.Location = new System.Drawing.Point(180, 20);
            this.btnGenerateStockReport.Name = "btnGenerateStockReport";
            this.btnGenerateStockReport.Size = new System.Drawing.Size(150, 35);
            this.btnGenerateStockReport.TabIndex = 1;
            this.btnGenerateStockReport.Text = "Stock Report";
            this.btnGenerateStockReport.Click += new System.EventHandler(this.btnGenerateStockReport_Click);

            this.btnGenerateCustomerReport.BackColor = System.Drawing.Color.FromArgb(111, 66, 193);
            this.btnGenerateCustomerReport.ForeColor = System.Drawing.Color.White;
            this.btnGenerateCustomerReport.Location = new System.Drawing.Point(340, 20);
            this.btnGenerateCustomerReport.Name = "btnGenerateCustomerReport";
            this.btnGenerateCustomerReport.Size = new System.Drawing.Size(150, 35);
            this.btnGenerateCustomerReport.TabIndex = 2;
            this.btnGenerateCustomerReport.Text = "Customer Report";
            this.btnGenerateCustomerReport.Click += new System.EventHandler(this.btnGenerateCustomerReport_Click);

            this.rtbReport.Location = new System.Drawing.Point(20, 70);
            this.rtbReport.Name = "rtbReport";
            this.rtbReport.Size = new System.Drawing.Size(730, 350);
            this.rtbReport.TabIndex = 3;
            this.rtbReport.Text = "";

            this.btnExportReport.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnExportReport.ForeColor = System.Drawing.Color.White;
            this.btnExportReport.Location = new System.Drawing.Point(650, 430);
            this.btnExportReport.Name = "btnExportReport";
            this.btnExportReport.Size = new System.Drawing.Size(100, 30);
            this.btnExportReport.TabIndex = 4;
            this.btnExportReport.Text = "Export";
            this.btnExportReport.Click += new System.EventHandler(this.btnExportReport_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(770, 480);
            this.Controls.Add(this.btnGenerateSalesReport);
            this.Controls.Add(this.btnGenerateStockReport);
            this.Controls.Add(this.btnGenerateCustomerReport);
            this.Controls.Add(this.rtbReport);
            this.Controls.Add(this.btnExportReport);
            this.Name = "GenerateReportForm";
            this.Text = "Generate Reports";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnGenerateSalesReport;
        private System.Windows.Forms.Button btnGenerateStockReport;
        private System.Windows.Forms.Button btnGenerateCustomerReport;
        private System.Windows.Forms.RichTextBox rtbReport;
        private System.Windows.Forms.Button btnExportReport;
        #endregion
    }
}
