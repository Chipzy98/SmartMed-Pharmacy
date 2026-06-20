using System;
using System.Windows.Forms;
using SmartMedPharmacy.DataAccess;
using SmartMedPharmacy.Models;

namespace SmartMedPharmacy.UI
{
    public partial class AddMedicineForm : Form
    {
        private DataManager _dataManager;

        public AddMedicineForm(DataManager dataManager)
        {
            InitializeComponent();
            _dataManager = dataManager;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                try
                {
                    Medicine medicine = new Medicine
                    {
                        MedicineName = txtName.Text.Trim(),
                        Category = txtCategory.Text.Trim(),
                        Dosage = txtDosage.Text.Trim(),
                        Price = decimal.Parse(txtPrice.Text),
                        Stock = int.Parse(txtStock.Text),
                        Supplier = txtSupplier.Text.Trim(),
                        ExpiryDate = dtpExpiry.Value,
                        RequiresPrescription = chkPrescription.Checked
                    };

                    if (!medicine.Validate())
                    {
                        MessageBox.Show("Invalid medicine data", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _dataManager.AddMedicine(medicine);
                    MessageBox.Show("Medicine added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) return false;
            if (string.IsNullOrWhiteSpace(txtCategory.Text)) return false;
            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0) return false;
            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0) return false;
            if (dtpExpiry.Value <= DateTime.Now) return false;
            return true;
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.lblDosage = new System.Windows.Forms.Label();
            this.txtDosage = new System.Windows.Forms.TextBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.lblStock = new System.Windows.Forms.Label();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.txtSupplier = new System.Windows.Forms.TextBox();
            this.lblExpiry = new System.Windows.Forms.Label();
            this.dtpExpiry = new System.Windows.Forms.DateTimePicker();
            this.chkPrescription = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(20, 20);
            this.lblName.Text = "Medicine Name:";
            this.txtName.Location = new System.Drawing.Point(120, 20);
            this.txtName.Size = new System.Drawing.Size(250, 20);

            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(20, 50);
            this.lblCategory.Text = "Category:";
            this.txtCategory.Location = new System.Drawing.Point(120, 50);
            this.txtCategory.Size = new System.Drawing.Size(250, 20);

            this.lblDosage.AutoSize = true;
            this.lblDosage.Location = new System.Drawing.Point(20, 80);
            this.lblDosage.Text = "Dosage:";
            this.txtDosage.Location = new System.Drawing.Point(120, 80);
            this.txtDosage.Size = new System.Drawing.Size(250, 20);

            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(20, 110);
            this.lblPrice.Text = "Price:";
            this.txtPrice.Location = new System.Drawing.Point(120, 110);
            this.txtPrice.Size = new System.Drawing.Size(250, 20);

            this.lblStock.AutoSize = true;
            this.lblStock.Location = new System.Drawing.Point(20, 140);
            this.lblStock.Text = "Stock:";
            this.txtStock.Location = new System.Drawing.Point(120, 140);
            this.txtStock.Size = new System.Drawing.Size(250, 20);

            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Location = new System.Drawing.Point(20, 170);
            this.lblSupplier.Text = "Supplier:";
            this.txtSupplier.Location = new System.Drawing.Point(120, 170);
            this.txtSupplier.Size = new System.Drawing.Size(250, 20);

            this.lblExpiry.AutoSize = true;
            this.lblExpiry.Location = new System.Drawing.Point(20, 200);
            this.lblExpiry.Text = "Expiry Date:";
            this.dtpExpiry.Location = new System.Drawing.Point(120, 200);
            this.dtpExpiry.Size = new System.Drawing.Size(250, 20);

            this.chkPrescription.AutoSize = true;
            this.chkPrescription.Location = new System.Drawing.Point(120, 230);
            this.chkPrescription.Text = "Requires Prescription";
            this.chkPrescription.Size = new System.Drawing.Size(150, 17);

            this.btnSave.BackColor = System.Drawing.Color.Green;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(120, 270);
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(230, 270);
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(420, 320);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.lblDosage);
            this.Controls.Add(this.txtDosage);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.txtStock);
            this.Controls.Add(this.lblSupplier);
            this.Controls.Add(this.txtSupplier);
            this.Controls.Add(this.lblExpiry);
            this.Controls.Add(this.dtpExpiry);
            this.Controls.Add(this.chkPrescription);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Name = "AddMedicineForm";
            this.Text = "Add Medicine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Label lblDosage;
        private System.Windows.Forms.TextBox txtDosage;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.TextBox txtSupplier;
        private System.Windows.Forms.Label lblExpiry;
        private System.Windows.Forms.DateTimePicker dtpExpiry;
        private System.Windows.Forms.CheckBox chkPrescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        #endregion
    }
}
