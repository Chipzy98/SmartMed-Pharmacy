using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartMedPharmacy.DataAccess;
using SmartMedPharmacy.Models;

namespace SmartMedPharmacy.UI
{
    public partial class AdminManagementForm : Form
    {
        private DataManager _dataManager;

        private DataGridView dgvAdmins;
        private Button btnAddAdmin;
        private Button btnEditAdmin;
        private Button btnDeleteAdmin;
        private Button btnRefresh;
        private Label lblTitle;

        public AdminManagementForm()
        {
            InitializeComponent();
            _dataManager = new DataManager();
        }

        private void AdminManagementForm_Load(object sender, EventArgs e)
        {
            LoadAdmins();
        }

        private void LoadAdmins()
        {
            try
            {
                dgvAdmins.Rows.Clear();

                List<Admin> admins = _dataManager.GetAllAdmins();

                foreach (Admin admin in admins)
                {
                    dgvAdmins.Rows.Add(
                        admin.AdminId,
                        admin.FullName,
                        admin.Email,
                        admin.Username,
                        admin.Role,
                        admin.IsActive ? "Active" : "Inactive",
                        admin.PhoneNumber
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading admins: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAddAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                AddAdminForm form = new AddAdminForm();

                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadAdmins();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEditAdmin_Click(object sender, EventArgs e)
        {
            if (dgvAdmins.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an admin.");
                return;
            }

            try
            {
                int adminId =
                    Convert.ToInt32(
                        dgvAdmins.SelectedRows[0]
                        .Cells["AdminId"].Value);

                Admin admin =
                    _dataManager.GetAdminById(adminId);

                if (admin == null)
                {
                    MessageBox.Show("Admin not found.");
                    return;
                }

                EditAdminForm form =
                    new EditAdminForm(admin);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadAdmins();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteAdmin_Click(object sender, EventArgs e)
        {
            if (dgvAdmins.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an admin.");
                return;
            }

            try
            {
                int adminId =
                    Convert.ToInt32(
                        dgvAdmins.SelectedRows[0]
                        .Cells["AdminId"].Value);

                string adminName =
                    dgvAdmins.SelectedRows[0]
                    .Cells["FullName"].Value.ToString();

                DialogResult result =
                    MessageBox.Show(
                        $"Delete {adminName} ?",
                        "Confirm",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _dataManager.DeleteAdmin(adminId);
                    LoadAdmins();

                    MessageBox.Show(
                        "Admin deleted successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAdmins();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.dgvAdmins = new DataGridView();
            this.btnAddAdmin = new Button();
            this.btnEditAdmin = new Button();
            this.btnDeleteAdmin = new Button();
            this.btnRefresh = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvAdmins)).BeginInit();
            this.SuspendLayout();

            // Title
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Arial", 16F, FontStyle.Bold);
            this.lblTitle.Location = new Point(20, 20);
            this.lblTitle.Text = "Admin Management";

            // DataGridView
            this.dgvAdmins.Location = new Point(20, 80);
            this.dgvAdmins.Size = new Size(980, 450);
            this.dgvAdmins.ReadOnly = true;
            this.dgvAdmins.MultiSelect = false;
            this.dgvAdmins.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            this.dgvAdmins.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAdmins.AllowUserToAddRows = false;

            this.dgvAdmins.Columns.Add("AdminId", "ID");
            this.dgvAdmins.Columns.Add("FullName", "Full Name");
            this.dgvAdmins.Columns.Add("Email", "Email");
            this.dgvAdmins.Columns.Add("Username", "Username");
            this.dgvAdmins.Columns.Add("Role", "Role");
            this.dgvAdmins.Columns.Add("Status", "Status");
            this.dgvAdmins.Columns.Add("PhoneNumber", "Phone");

            // Add Button
            this.btnAddAdmin.BackColor =
                Color.FromArgb(40, 167, 69);
            this.btnAddAdmin.ForeColor = Color.White;
            this.btnAddAdmin.Location = new Point(20, 560);
            this.btnAddAdmin.Size = new Size(120, 40);
            this.btnAddAdmin.Text = "Add Admin";
            this.btnAddAdmin.Click +=
                new EventHandler(this.btnAddAdmin_Click);

            // Edit Button
            this.btnEditAdmin.BackColor =
                Color.FromArgb(0, 123, 255);
            this.btnEditAdmin.ForeColor = Color.White;
            this.btnEditAdmin.Location = new Point(160, 560);
            this.btnEditAdmin.Size = new Size(120, 40);
            this.btnEditAdmin.Text = "Edit Admin";
            this.btnEditAdmin.Click +=
                new EventHandler(this.btnEditAdmin_Click);

            // Delete Button
            this.btnDeleteAdmin.BackColor =
                Color.FromArgb(220, 53, 69);
            this.btnDeleteAdmin.ForeColor = Color.White;
            this.btnDeleteAdmin.Location = new Point(300, 560);
            this.btnDeleteAdmin.Size = new Size(120, 40);
            this.btnDeleteAdmin.Text = "Delete Admin";
            this.btnDeleteAdmin.Click +=
                new EventHandler(this.btnDeleteAdmin_Click);

            // Refresh Button
            this.btnRefresh.BackColor =
                Color.FromArgb(111, 66, 193);
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.Location = new Point(440, 560);
            this.btnRefresh.Size = new Size(120, 40);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click +=
                new EventHandler(this.btnRefresh_Click);

            // Form
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1030, 650);
            this.StartPosition =
                FormStartPosition.CenterScreen;
            this.Text = "Admin Management";

            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvAdmins);
            this.Controls.Add(this.btnAddAdmin);
            this.Controls.Add(this.btnEditAdmin);
            this.Controls.Add(this.btnDeleteAdmin);
            this.Controls.Add(this.btnRefresh);

            this.Load +=
                new EventHandler(this.AdminManagementForm_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvAdmins)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}