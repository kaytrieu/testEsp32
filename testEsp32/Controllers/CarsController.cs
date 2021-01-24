using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testEsp32.Data;
using testEsp32.Dtos;
using testEsp32.Models;
using uPLibrary.Networking.M2Mqtt;

namespace testEsp32
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly EspconnecttestContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public CarsController(EspconnecttestContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }


        // POST: api/Cars
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult PostCars([Required] int id, [Required] int carState)
        {
            CarStateDto carDto = new CarStateDto(id, carState);

            string mqttJson = JsonConvert.SerializeObject(carDto).ToLower();
            try
            {
                sendMqqt(mqttJson);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Changing State");
        }

        private void sendMqqt(string msg)
        {

            MqttClient client = new MqttClient("mqtt.dioty.co");

            string clientId = "dotnetClientHuyTM";
            client.Connect(clientId, "huytmse130336@fpt.edu.vn", "da9413f8");
            client.Publish("/huytmse130336@fpt.edu.vn/in", Encoding.UTF8.GetBytes(msg));

        }



    }
}
