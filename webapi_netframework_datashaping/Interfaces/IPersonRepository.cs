using System;
using System.Collections.Generic;
using webapiexample.Models;

namespace webapiexample.Interfaces
{
    interface IPersonRepository
    {
        IEnumerable<Person> GetByFilter(string name = null, DateTime? birthDate = null);
    }
}
