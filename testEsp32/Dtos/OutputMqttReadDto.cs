using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testEsp32.Dtos
{
    public class OutputMqttReadDto
    {
        public int? Gpio { get; set; }
        public int? State { get; set; }
    }
}
