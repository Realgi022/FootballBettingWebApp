using System;

namespace BLL.Models
{
    public class User
    {
        private int _userId;
        private string _username = string.Empty;
        private string _passwordHash = string.Empty;
        private string _email = string.Empty;
        private int _age;
        private UserRole _role = UserRole.User;

        public int UserId
        {
            get => _userId;
            set
            {
                if (value < 0)
                    throw new ArgumentException("UserId cannot be negative.");
                _userId = value;
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username cannot be empty.");
                _username = value.Trim();
            }
        }

        public string PasswordHash
        {
            get => _passwordHash;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Password cannot be empty.");
                _passwordHash = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (!value.Contains("@"))
                    throw new ArgumentException("Invalid email format.");
                _email = value.Trim();
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                if (value < 18)
                    throw new ArgumentException("User must be at least 18 years old.");
                _age = value;
            }
        }

        public UserRole Role
        {
            get => _role;
            set => _role = value;
        }
    }
}
