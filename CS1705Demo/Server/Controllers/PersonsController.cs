using Microsoft.AspNetCore.Mvc;
using ServerSide.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        Random random = new Random();

        [HttpGet()]
        public async Task<List<PersonDTO>> GetPeople() => await Task.Run(() => new List<PersonDTO>
        {
            new PersonDTO{ Id = 42, Age = 50.6, Name = "General May Hem", Balance = -600.23m },
            new PersonDTO{ Id = 43, Age = 100.0, Name = "Noah Way", Balance = 780200.14m },
            new PersonDTO{ Id = 44, Age = 50.6, Name = "Elijah Doitt R Knott", Balance = 0.79m },
            new PersonDTO{ Id = 45, Age = 50.6, Name = "Ava Nice-Day", Balance = 906.58m },
        });


    }
}