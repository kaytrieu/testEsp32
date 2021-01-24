using AutoMapper;
using testEsp32.Dtos;
using testEsp32.Models;

namespace testEsp32.Profiles
{
    public class OutputProfile : Profile
    {
        public OutputProfile()
        {
            CreateMap<Outputs, OutputReadDto>();
            CreateMap<Outputs, OutputMqttReadDto>();
            CreateMap<OutputPostDto, Outputs>();
            CreateMap<OutputPutDto, Outputs>();
        }
    }
}
