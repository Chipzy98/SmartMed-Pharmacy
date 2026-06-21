using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SmartMedPharmacy.DataAccess;
using SmartMedPharmacy.Models;

namespace SmartMedPharmacy.UI
{
    public partial class EditAdminForm : Form
    {
        private Admin _admin;
        private DataManager _dataManager;

        private TextBox txtAdminId;
        private TextBox txtFullName;
        private TextBox txtEmail;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtPhone;
        private ComboBox cmbRole;
        private CheckBox chkActive;

        private Button btnUpdate;
        private Button btnCancel;

        public EditAdminForm(Admin admin)
        {
            _admin = admin;
            _dataManager = new DataManager();
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            txtAdminId.Text = _admin.AdminId.ToString();
            txtFullName.Text = _admin.FullName;
            txtEmail.Text = _admin.Email;
            txtUsername.Text = _admin.Username;
            txtPhone.Text = _admin.PhoneNumber;

            cmbRole.SelectedItem = _admin.Role;
            chkActive.Checked = _admin.IsActive;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> errors = ValidateInputs();

                if (errors.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errors),
                        "Validation Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                _admin.FullName = txtFullName.Text.Trim();
                _admin.Email = txtEmail.Text.Trim();
                _admin.Username = txtUsername.Text.Trim();
                _admin.PhoneNumber = txtPhone.Text.Trim();
                _admin.Role = cmbRole.SelectedItem.ToString();
                _admin.IsActive = chkActive.Checked;

                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    _admin.Password = txtPassword.Text;
                }

                _dataManager.UpdateAdmin(_admin);

                MessageBox.Show("Admin updated successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<string> ValidateInputs()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
                errors.Add("Full Name is required");

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
                errors.Add("Email is required");

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
                errors.Add("Username is required");

            if (!string.IsNullOrWhiteSpace(txtPassword.Text) && txtPassword.Text.Length < 6)
                errors.Add("Password must be at least 6 characters");

            return errors;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.Text = "Edit Admin";
            this.Size = new Size(520, 520);
            this.StartPosition = FormStartPosition.CenterScreen;

            int x1 = 20;
            int x2 = 160;
            int y = 30;
            int gap = 40;

            // ID
            AddLabel("Admin ID", x1, y);
            txtAdminId = new TextBox { Location = new Point(x2, y), Width = 250, ReadOnly = true };
            y += gap;

            // Full Name
            AddLabel("Full Name", x1, y);
            txtFullName = new TextBox { Location = new Point(x2, y), Width = 250 };
            y += gap;

            // Email
            AddLabel("Email", x1, y);
            txtEmail = new TextBox { Location = new Point(x2, y), Width = 250 };
            y += gap;

            // Username
            AddLabel("Username", x1, y);
            txtUsername = new TextBox { Location = new Point(x2, y), Width = 250 };
            y += gap;

            // Password
            AddLabel("New Password", x1, y);
            txtPassword = new TextBox { Location = new Point(x2, y), Width = 250, PasswordChar = '*' };
            y += gap;

            // Phone
            AddLabel("Phone", x1, y);
            txtPhone = new TextBox { Location = new Point(x2, y), Width = 250 };
            y += gap;

            // Role
            AddLabel("Role", x1, y);
            cmbRole = new ComboBox
            {
                Location = new Point(x2, y),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.AddRange(new object[] { "Admin", "Manager", "CEO" });
            y += gap;

            // Active
            chkActive = new CheckBox
            {
                Text = "Active",
                Location = new Point(x2, y),
                Checked = true
            };
            y += gap + 10;

            // Buttons
            btnUpdate = new Button
            {
                Text = "Update",
                Location = new Point(x2, y),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White
            };
            btnUpdate.Click += btnUpdate_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(x2 + 120, y),
                BackColor = Color.Gray,
                ForeColor = Color.White
            };
            btnCancel.Click += btnCancel_Click;

            // Add controls
            this.Controls.AddRange(new Control[]
            {
                txtAdminId, txtFullName, txtEmail, txtUsername,
                txtPassword, txtPhone, cmbRole, chkActive,
                btnUpdate, btnCancel
            });
        }

        private void AddLabel(string text, int x, int y)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = new Point(x, y + 5),
                Width = 120
            };
            this.Controls.Add(lbl);
        }
    }
}