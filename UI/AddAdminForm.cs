using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SmartMedPharmacy.DataAccess;
using SmartMedPharmacy.Models;

namespace SmartMedPharmacy.UI
{
    public partial class AddAdminForm : Form
    {
        private DataManager _dataManager;

        // Controls
        private Label lblTitle;

        private TextBox txtFullName;
        private TextBox txtEmail;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtPhone;
        private ComboBox cmbRole;
        private CheckBox chkActive;

        private Button btnSave;
        private Button btnCancel;

        public AddAdminForm()
        {
            InitializeComponent();
            _dataManager = new DataManager();
        }

        private void btnSave_Click(object sender, EventArgs e)
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

                Admin admin = new Admin
                {
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Text.Trim(),
                    PhoneNumber = txtPhone.Text.Trim(),
                    Role = cmbRole.SelectedItem.ToString(),
                    IsActive = chkActive.Checked
                };

                _dataManager.AddAdmin(admin);

                MessageBox.Show("Admin added successfully!",
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

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
                errors.Add("Password is required");

            if (txtPassword.Text.Length < 6)
                errors.Add("Password must be at least 6 characters");

            return errors;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();

            this.txtFullName = new TextBox();
            this.txtEmail = new TextBox();
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.txtPhone = new TextBox();

            this.cmbRole = new ComboBox();
            this.chkActive = new CheckBox();

            this.btnSave = new Button();
            this.btnCancel = new Button();

            this.SuspendLayout();

            // Title
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Arial", 16F, FontStyle.Bold);
            this.lblTitle.Location = new Point(20, 20);
            this.lblTitle.Text = "Add New Admin";

            int labelX = 20;
            int controlX = 160;
            int y = 80;
            int gap = 45;

            // Full Name
            AddLabel("Full Name", labelX, y);
            txtFullName.Location = new Point(controlX, y);
            txtFullName.Width = 250;
            y += gap;

            // Email
            AddLabel("Email", labelX, y);
            txtEmail.Location = new Point(controlX, y);
            txtEmail.Width = 250;
            y += gap;

            // Username
            AddLabel("Username", labelX, y);
            txtUsername.Location = new Point(controlX, y);
            txtUsername.Width = 250;
            y += gap;

            // Password
            AddLabel("Password", labelX, y);
            txtPassword.Location = new Point(controlX, y);
            txtPassword.Width = 250;
            txtPassword.PasswordChar = '*';
            y += gap;

            // Phone
            AddLabel("Phone", labelX, y);
            txtPhone.Location = new Point(controlX, y);
            txtPhone.Width = 250;
            y += gap;

            // Role
            AddLabel("Role", labelX, y);
            cmbRole.Location = new Point(controlX, y);
            cmbRole.Width = 250;
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.Items.AddRange(new object[] { "Admin", "Manager", "CEO" });
            cmbRole.SelectedIndex = 0;
            y += gap;

            // Active
            AddLabel("Active", labelX, y);
            chkActive.Location = new Point(controlX, y);
            chkActive.Text = "Is Active";
            chkActive.Checked = true;
            y += gap + 10;

            // Buttons
            btnSave.Text = "Save";
            btnSave.Location = new Point(controlX, y);
            btnSave.BackColor = Color.FromArgb(40, 167, 69);
            btnSave.ForeColor = Color.White;
            btnSave.Click += btnSave_Click;

            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(controlX + 120, y);
            btnCancel.BackColor = Color.FromArgb(220, 53, 69);
            btnCancel.ForeColor = Color.White;
            btnCancel.Click += btnCancel_Click;

            // Form
            this.ClientSize = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Add Admin";

            this.Controls.Add(lblTitle);
            this.Controls.Add(txtFullName);
            this.Controls.Add(txtEmail);
            this.Controls.Add(txtUsername);
            this.Controls.Add(txtPassword);
            this.Controls.Add(txtPhone);
            this.Controls.Add(cmbRole);
            this.Controls.Add(chkActive);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void AddLabel(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Location = new Point(x, y + 3);
            lbl.Width = 120;
            this.Controls.Add(lbl);
        }
    }
}