using System;

namespace SmartMedPharmacy.Models
{
    /// <summary>
    /// Represents an administrator user in the system
    /// </summary>
    public class Admin
    {
        public int AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        public Admin()
        {
            CreatedDate = DateTime.Now;
            IsActive = true;
            Role = "Manager";
        }

        public Admin(int id, string firstName, string lastName, string username, string password)
        {
            AdminId = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            CreatedDate = DateTime.Now;
            IsActive = true;
            Role = "Manager";
        }

        /// <summary>
        /// Gets the full name of the admin
        /// </summary>
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        /// <summary>
        /// Validates admin details
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
                return false;
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                return false;
            if (Password.Length < 6)
                return false;

            return true;
        }
    }
}
