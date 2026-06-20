using System;
using System.Drawing;
using System.Windows.Forms;
using SmartMedPharmacy.DataAccess;
using SmartMedPharmacy.Models;

namespace SmartMedPharmacy.UI
{
    public partial class LoginForm : Form
    {
        private DataManager _dataManager;

        public LoginForm()
        {
            InitializeComponent();
            _dataManager = new DataManager();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Initialize sample data if needed
            InitializeSampleData();
            LoadPharmacyImage();
        }

        /// <summary>
        /// Loads pharmacy image from resources or file
        /// </summary>
        private void LoadPharmacyImage()
        {
            try
            {
                // Try to load from application directory
                string imagePath = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Assets", "pharmacy.jpg");

                if (System.IO.File.Exists(imagePath))
                {
                    pictureBoxPharmacy.Image = Image.FromFile(imagePath);
                    pictureBoxPharmacy.BackColor = Color.White;
                }
                else
                {
                    // Set a default background color if image not found
                    pictureBoxPharmacy.BackColor = Color.FromArgb(200, 220, 230);
                }
            }
            catch
            {
                // Silently fail - just use background color
                pictureBoxPharmacy.BackColor = Color.FromArgb(200, 220, 230);
            }
        }

        /// <summary>
        /// Initializes sample data for testing
        /// </summary>
        private void InitializeSampleData()
        {
            try
            {
                var admins = _dataManager.GetAllAdmins();
                if (admins.Count == 0)
                {
                    Admin admin = new Admin(1, "Tharaka", "Bandara", "admin", "admin123");
                    admin.Email = "admin@smartmed.com";
                    admin.PhoneNumber = "0771234567";
                    _dataManager.AddAdmin(admin);
                }
            }
            catch { }
        }

        private void btnAdminLogin_Click(object sender, EventArgs e)
        {
            if (ValidateLoginInputs())
            {
                try
                {
                    Admin admin = _dataManager.AuthenticateAdmin(txtUsername.Text, txtPassword.Text);
                    if (admin != null)
                    {
                        AdminDashboard adminDashboard = new AdminDashboard(admin, _dataManager);
                        adminDashboard.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid admin credentials", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPassword.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCustomerLogin_Click(object sender, EventArgs e)
        {
            if (ValidateLoginInputs())
            {
                try
                {
                    Customer customer = _dataManager.AuthenticateCustomer(txtUsername.Text, txtPassword.Text);
                    if (customer != null)
                    {
                        CustomerDashboard customerDashboard = new CustomerDashboard(customer, _dataManager);
                        customerDashboard.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid customer credentials", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPassword.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm = new RegistrationForm(_dataManager);
            registrationForm.ShowDialog();
        }

        private bool ValidateLoginInputs()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter username", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter password", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            return true;
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Confirm Exit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pictureBoxPharmacy = new System.Windows.Forms.PictureBox();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnCustomerLogin = new System.Windows.Forms.Button();
            this.btnAdminLogin = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPharmacy)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.pnlLeft.Controls.Add(this.pictureBoxPharmacy);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(350, 500);
            this.pnlLeft.TabIndex = 0;
            // 
            // pictureBoxPharmacy
            // 
            this.pictureBoxPharmacy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.pictureBoxPharmacy.BackgroundImage = global::SmartMed_Pharmacy.Properties.Resources.A4___1;
            this.pictureBoxPharmacy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPharmacy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxPharmacy.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxPharmacy.Name = "pictureBoxPharmacy";
            this.pictureBoxPharmacy.Size = new System.Drawing.Size(350, 500);
            this.pictureBoxPharmacy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxPharmacy.TabIndex = 0;
            this.pictureBoxPharmacy.TabStop = false;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.White;
            this.pnlRight.Controls.Add(this.btnRegister);
            this.pnlRight.Controls.Add(this.btnCustomerLogin);
            this.pnlRight.Controls.Add(this.btnAdminLogin);
            this.pnlRight.Controls.Add(this.txtPassword);
            this.pnlRight.Controls.Add(this.lblPassword);
            this.pnlRight.Controls.Add(this.txtUsername);
            this.pnlRight.Controls.Add(this.lblUsername);
            this.pnlRight.Controls.Add(this.lblTitle);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(350, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(400, 500);
            this.pnlRight.TabIndex = 1;
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnRegister.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegister.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(40, 340);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(320, 40);
            this.btnRegister.TabIndex = 7;
            this.btnRegister.Text = "📝 New Customer? Register";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnCustomerLogin
            // 
            this.btnCustomerLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnCustomerLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCustomerLogin.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.btnCustomerLogin.ForeColor = System.Drawing.Color.White;
            this.btnCustomerLogin.Location = new System.Drawing.Point(220, 280);
            this.btnCustomerLogin.Name = "btnCustomerLogin";
            this.btnCustomerLogin.Size = new System.Drawing.Size(140, 40);
            this.btnCustomerLogin.TabIndex = 6;
            this.btnCustomerLogin.Text = "👤 Customer Login";
            this.btnCustomerLogin.UseVisualStyleBackColor = false;
            this.btnCustomerLogin.Click += new System.EventHandler(this.btnCustomerLogin_Click);
            // 
            // btnAdminLogin
            // 
            this.btnAdminLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnAdminLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdminLogin.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.btnAdminLogin.ForeColor = System.Drawing.Color.White;
            this.btnAdminLogin.Location = new System.Drawing.Point(40, 280);
            this.btnAdminLogin.Name = "btnAdminLogin";
            this.btnAdminLogin.Size = new System.Drawing.Size(140, 40);
            this.btnAdminLogin.TabIndex = 5;
            this.btnAdminLogin.Text = "🔐 Admin Login";
            this.btnAdminLogin.UseVisualStyleBackColor = false;
            this.btnAdminLogin.Click += new System.EventHandler(this.btnAdminLogin_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Arial", 11F);
            this.txtPassword.Location = new System.Drawing.Point(40, 225);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(320, 29);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Arial", 11F);
            this.lblPassword.Location = new System.Drawing.Point(40, 200);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(99, 22);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";
            // 
            // txtUsername
            // 
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsername.Font = new System.Drawing.Font("Arial", 11F);
            this.txtUsername.Location = new System.Drawing.Point(40, 155);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(320, 29);
            this.txtUsername.TabIndex = 2;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Arial", 11F);
            this.lblUsername.Location = new System.Drawing.Point(40, 130);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(102, 22);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username:";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblTitle.Location = new System.Drawing.Point(40, 40);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(311, 35);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "SmartMed Pharmacy";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(750, 500);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SmartMed Pharmacy - Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPharmacy)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.PictureBox pictureBoxPharmacy;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnAdminLogin;
        private System.Windows.Forms.Button btnCustomerLogin;
        private System.Windows.Forms.Button btnRegister;

        #endregion
    }
}