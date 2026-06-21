using System;

namespace SmartMedPharmacy.Models
{
    /// <summary>
    /// Admin user model with role management
    /// </summary>
    public class Admin
    {
        public int AdminId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }  // "Admin", "Manager", "CEO"
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Admin()
        {
            IsActive = true;
            CreatedDate = DateTime.Now;
            Role = "Admin";  // Default role
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        public Admin(int adminId, string fullName, string email, string username, string password)
        {
            AdminId = adminId;
            FullName = fullName;
            Email = email;
            Username = username;
            Password = password;
            IsActive = true;
            CreatedDate = DateTime.Now;
            Role = "Admin";  // Default role
            PhoneNumber = "";
        }

        /// <summary>
        /// Constructor with all parameters
        /// </summary>
        public Admin(int adminId, string fullName, string email, string username, string password, string role, string phoneNumber)
        {
            AdminId = adminId;
            FullName = fullName;
            Email = email;
            Username = username;
            Password = password;
            Role = role;
            PhoneNumber = phoneNumber;
            IsActive = true;
            CreatedDate = DateTime.Now;
        }

        /// <summary>
        /// Gets the full name of the admin
        /// </summary>
        public string GetFullName()
        {
            return FullName ?? "Unknown";
        }
    }
}