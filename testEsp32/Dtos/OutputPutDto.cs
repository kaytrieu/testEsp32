using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testEsp32.Dtos
{
    public class OutputPutDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Board { get; set; }
        public int? Gpio { get; set; }
        public int? State { get; set; }
    }
}
