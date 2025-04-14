// PersonEntry.cs
using System;

namespace PersonEntry
{
    public class PersonEntry
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public PersonEntry(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }

        public override string ToString()
        {
            return Name; 
        }
    }
}
