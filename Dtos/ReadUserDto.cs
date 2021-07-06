using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    public class ReadUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<int> Following { get; set; }
        public List<int> Followers { get; set; }
    }
}
