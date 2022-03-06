using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Queues
{
    public class UserDetailsCommand
    {
        public string UserName { get; set; }
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public UserDetailsCommand(string username, string firstname, string lastname, string email, DateTime dob)
        {
            UserName = username;
            FistName = firstname;
            LastName = lastname;
            Email = email;
            DOB = dob;
        }
    }
}
