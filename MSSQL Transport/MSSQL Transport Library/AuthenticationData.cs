using System;

namespace MSSQLTransportLibrary
{
    public class AuthenticationData
    {
        public string ServerAddress { get; set; }
        public bool UseWindowsAuthentication { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string ConnectionString => $"Data Source={this.ServerAddress};{(this.UseWindowsAuthentication ? "Trusted_Connection=True" : $"User ID={this.Username};Password={this.Password}")}";
    }
}
