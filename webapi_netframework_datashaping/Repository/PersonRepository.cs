using System;
using System.Collections.Generic;
using webapiexample.Interfaces;
using webapiexample.Models;

namespace webapiexample.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private static readonly IList<Person> _lstPersons = new List<Person>();

        public PersonRepository()
        {
            if (_lstPersons.Count <= 0)
            {
                _lstPersons.Add(new Person()
                {
                    Birthdate = new DateTime(1980, 5, 13),
                    Id = 1,
                    Name = "Pedro",
                    Movements = new List<Movement>()
                    {
                        new Movement()
                        {
                            Id = 100,
                            Description = "Compra",
                            MoveDate = DateTime.Now,
                            PersonId = 1
                        },
                        new Movement()
                        {
                            Id = 101,
                            Description = "Venta",
                            MoveDate = DateTime.Now.AddDays(-10),
                            PersonId = 1
                        },

                    }
                });

                _lstPersons.Add(new Person()
                {
                    Birthdate = new DateTime(1984, 10, 2),
                    Id = 2,
                    Name = "Martin",
                    Movements = null
                });

                _lstPersons.Add(new Person()
                {
                    Birthdate = new DateTime(2001, 1, 30),
                    Id = 3,
                    Name = "Juan",
                    Movements = null
                });
            }
        }

        public IEnumerable<Person> GetByFilter(string name = null, DateTime? birthDate = null)
        {
            return _lstPersons;
        }
    }
}