using System;

namespace webapiexample.Models
{
    public class Movement
    {
        public int Id { get; set; }

        public DateTime MoveDate { get; set; }

        public string Description { get; set; }

        public virtual int PersonId { get; set; }
    }
}