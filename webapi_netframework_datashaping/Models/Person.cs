using System;
using System.Collections.Generic;

namespace webapiexample.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Birthdate { get; set; }

        public virtual ICollection<Movement> Movements { get; set; }
    }
}