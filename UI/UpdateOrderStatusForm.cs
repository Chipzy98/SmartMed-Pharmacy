using System;
using System.Windows.Forms;
using SmartMedPharmacy.DataAccess;
using SmartMedPharmacy.Models;

namespace SmartMedPharmacy.UI
{
    public partial class UpdateOrderStatusForm : Form
    {
        private Order _order;
        private DataManager _dataManager;

        public UpdateOrderStatusForm(Order order, DataManager dataManager)
        {
            InitializeComponent();
            _order = order;
            _dataManager = dataManager;
        }

        private void UpdateOrderStatusForm_Load(object sender, EventArgs e)
        {
            lblOrderId.Text = $"Order ID: {_order.OrderId}";
            lblCustomer.Text = $"Customer: {_order.CustomerName}";
            lblOrderDate.Text = $"Order Date: {_order.OrderDate:yyyy-MM-dd}";

            cmbStatus.Items.Add(OrderStatus.Pending.ToString());
            cmbStatus.Items.Add(OrderStatus.ReadyForPickup.ToString());
            cmbStatus.Items.Add(OrderStatus.Delivered.ToString());
            cmbStatus.Items.Add(OrderStatus.Cancelled.ToString());
            cmbStatus.SelectedItem = _order.Status.ToString();

            dgvItems.DataSource = _order.Items;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedStatus = cmbStatus.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedStatus))
                {
                    MessageBox.Show("Please select a status", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (Enum.TryParse<OrderStatus>(selectedStatus, out OrderStatus status))
                {
                    _order.Status = status;
                    _dataManager.UpdateOrder(_order);
                    MessageBox.Show("Order status updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();

        #region Designer
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblOrderId = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblOrderDate = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.SuspendLayout();

            this.lblOrderId.AutoSize = true;
            this.lblOrderId.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.lblOrderId.Location = new System.Drawing.Point(20, 20);
            this.lblOrderId.Name = "lblOrderId";
            this.lblOrderId.Size = new System.Drawing.Size(100, 20);
            this.lblOrderId.TabIndex = 0;
            this.lblOrderId.Text = "Order ID: ";

            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Location = new System.Drawing.Point(20, 50);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(100, 15);
            this.lblCustomer.TabIndex = 1;
            this.lblCustomer.Text = "Customer: ";

            this.lblOrderDate.AutoSize = true;
            this.lblOrderDate.Location = new System.Drawing.Point(20, 80);
            this.lblOrderDate.Name = "lblOrderDate";
            this.lblOrderDate.Size = new System.Drawing.Size(100, 15);
            this.lblOrderDate.TabIndex = 2;
            this.lblOrderDate.Text = "Order Date: ";

            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(20, 120);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(50, 15);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status: ";

            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(120, 120);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(250, 23);
            this.cmbStatus.TabIndex = 4;

            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Location = new System.Drawing.Point(20, 160);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.Size = new System.Drawing.Size(550, 200);
            this.dgvItems.TabIndex = 5;

            this.btnUpdate.BackColor = System.Drawing.Color.Blue;
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(200, 380);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 30);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(310, 380);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(600, 430);
            this.Controls.Add(this.lblOrderId);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.lblOrderDate);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Name = "UpdateOrderStatusForm";
            this.Text = "Update Order Status";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.UpdateOrderStatusForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblOrderId;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblOrderDate;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        #endregion
    }
}
