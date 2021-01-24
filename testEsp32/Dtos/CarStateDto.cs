using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testEsp32.Dtos
{
    public class CarStateDto
    {
        public int Car { get; set; }
        public int State { get; set; }

        public CarStateDto(int id, int state)
        {
            Car = id;
            State = state;
        }
    }
}
