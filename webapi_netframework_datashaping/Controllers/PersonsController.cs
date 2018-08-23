using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using webapiexample.Extensions;
using webapiexample.Models;
using webapiexample.Repository;

namespace webapiexample.Controllers
{
    public class PersonsController : ApiController
    {
        public IHttpActionResult Get(string fields = null)
        {
            //get data from repository
            var personRepository = new PersonRepository();
            var persons = personRepository.GetByFilter().ToList();

            //make de data shaping magic
            var fieldsList = new List<string>();
            if (fields != null)
            {
                fieldsList = fields.ToLower().Split(',').ToList();
            }

            //for each person record we create the shaped object
            var result = persons.Select(p => this.GetShapedObject(p, fieldsList));

            return this.Ok(result);
        }

        //create shaped object for person (and optionaly for movements)
        private object GetShapedObject(Person person, List<string> fieldsList)
        {
            if (fieldsList.Count == 0)
            {
                return person;
            }

            //campos de movimientos
            var lstOfMovementsFields = fieldsList.Where(p => p.Contains("movements")).ToList();
            bool returnPartialMovement = lstOfMovementsFields.Any() && !lstOfMovementsFields.Contains("movements");

            if (returnPartialMovement)
            {
                //a la lista de campos de personas, le elimino la lista de campos de movimientos
                fieldsList.RemoveRange(lstOfMovementsFields);

                //lista de campos para movements
                lstOfMovementsFields = lstOfMovementsFields.Select(f => f.Substring(f.IndexOf(".") + 1)).ToList();
            }            

            ExpandoObject result = new ExpandoObject();

            foreach (var field in fieldsList)
            {
                try
                {
                    var fieldValue = person.GetType()
                                            .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                                            .GetValue(person, null);

                    ((IDictionary<String, Object>)result).Add(field, fieldValue);
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Field: {0}", field);
                }                
            }

            if (returnPartialMovement)
            {
                List<object> movements = new List<object>();
                foreach (var movement in person.Movements)
                {
                    movements.Add(GetShapedObject(movement, lstOfMovementsFields));
                }

                ((IDictionary<String, Object>)result).Add("movements", movements);
            }

            return ((IDictionary<String, Object>)result).Count == 0 ? (object)person : result;
        }

        //create shaped object for movements
        private object GetShapedObject(Movement movement, List<string> fieldsList)
        {
            ExpandoObject result = new ExpandoObject();

            foreach (var field in fieldsList)
            {
                try
                {
                    var fieldValue = movement.GetType()
                                            .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                                            .GetValue(movement, null);

                    ((IDictionary<String, Object>)result).Add(field, fieldValue);
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Field: {0}", field);
                }
            }

            return ((IDictionary<String, Object>)result).Count == 0 ? (object)movement : result;
        }
    }
}
