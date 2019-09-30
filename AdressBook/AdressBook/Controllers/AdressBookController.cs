using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdressBook.Controllers
{
    [ApiController]
    [Route("/contacts")]
    public class AdressBookController : ControllerBase
    {   //CTRL ++RR
        //CTRL + K+C/U
        // CTRL + K F/CTRL+ K Fs
        private static readonly List<Person> Contacts =
                new List<Person> { };

        [HttpGet]
        public IActionResult GetAllContacts() => Ok(Contacts);

        [HttpGet]
        [Route("{index}", Name = "GetSpecificContact")]
        public IActionResult GetContact(int index)
        {

            if (Contacts.Find(i => i.id == index) == null)
            {
                return NotFound("Person not found");
            }
            return Ok(Contacts.Where(i => i.id == index).FirstOrDefault());
        }
        //https://stackoverflow.com/questions/1485766/finding-an-item-in-a-list-using-c-sharp
        [HttpPost]
        public IActionResult AddContact([FromBody] Person newContact)
        {
            
            if (Contacts.Find(i => i.id == newContact.id) == null)
            {
                Contacts.Add(newContact);
                return CreatedAtRoute("GetSpecificContact", new { index = Contacts.IndexOf(newContact) }, newContact);

            }

            return BadRequest("Person already exists");
        }
        [HttpPut]
        [Route("{index}")]
        public IActionResult UpdateContact(int index, [FromBody] Person newContact)
        {
            if (index >= 0 && index < Contacts.Count)
            {
                Contacts[index] = newContact;
                return Ok();
            }

            return BadRequest("Invalid index");
        }
        [HttpDelete]
        [Route("{index}")]
        public IActionResult DeleteContact(int index)
        {
            try
            {
                if (Contacts.Single(x => x.id == index) != null)
                {
                    Contacts.Remove(Contacts.Single(x => x.id == index));
                    return Ok($"Deleted contact with id {index}");
                }
            }
            catch
            {
                return NotFound("Person not found");
            }
            return BadRequest("Invalid id");
        }
        [HttpGet]
        [Route("findByName", Name = "GetContactByName")]
        public IActionResult FindContactByName([FromQuery]string nameFilter)
        {
            if (nameFilter != null)
            {
                IEnumerable<Person> c = (Contacts.Where(i => i.firstName.Contains(nameFilter) || i.lastName.Contains(nameFilter)));
                if (c.Count() > 0)
                {
                    return Ok(c);
                }
            }
          
            return BadRequest("Invalid or missing name");
        }
    }
}

