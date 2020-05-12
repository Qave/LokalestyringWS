using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    public class User
    {
        public int User_Id { get; set; }

        public string User_Name { get; set; }

        public string User_Email { get; set; }

        public string Password { get; set; }

        public bool Teacher { get; set; }

        public override string ToString()
        {
            return $"{User_Id}, {User_Name}, {User_Email}, {Teacher}";
        }
    }
}
