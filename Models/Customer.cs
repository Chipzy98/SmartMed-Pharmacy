using System;

namespace SmartMedPharmacy.Models
{
    /// <summary>
    /// Represents a customer in the pharmacy system
    /// </summary>
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }

        public Customer()
        {
            RegistrationDate = DateTime.Now;
            IsActive = true;
        }

        public Customer(int id, string firstName, string lastName, string email, 
                        string phone, string username, string password)
        {
            CustomerId = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phone;
            Username = username;
            Password = password;
            RegistrationDate = DateTime.Now;
            IsActive = true;
        }

        /// <summary>
        /// Gets the full name of the customer
        /// </summary>
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        /// <summary>
        /// Validates customer details
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
                return false;
            if (string.IsNullOrWhiteSpace(Email))
                return false;
            if (string.IsNullOrWhiteSpace(PhoneNumber))
                return false;
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                return false;
            if (Password.Length < 6)
                return false;

            return true;
        }

        /// <summary>
        /// Validates email format
        /// </summary>
        public bool ValidateEmail()
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(Email);
                return addr.Address == Email;
            }
            catch
            {
                return false;
            }
        }
    }
}
