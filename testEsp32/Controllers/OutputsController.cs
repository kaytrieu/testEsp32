using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class OutputsController : ControllerBase
    {
        private readonly EspconnecttestContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public OutputsController(EspconnecttestContext context, IMapper mapper,  IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        // GET: api/Outputs
        [HttpGet]
        public ActionResult<IEnumerable<OutputReadDto>> GetOutputsInBoard([FromQuery] int boardId)
        {
            return Ok(_mapper.Map<IEnumerable<OutputReadDto>>(_context.Outputs.Where(x => boardId == 0 || x.Board == boardId)));
        }

        [HttpGet("esp")]
        public ActionResult<IEnumerable<OutputMqttReadDto>> GetOutputsInBoardForEsp([FromQuery] int boardId)
        {
            return Ok(_mapper.Map<IEnumerable<OutputMqttReadDto>>(_context.Outputs.Where(x => boardId == 0 || x.Board == boardId)));
        }

        // GET: api/Outputs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Outputs>> GetOutputs(int id)
        {
            var outputs = await _context.Outputs.FindAsync(id);

            if (outputs == null)
            {
                return NotFound();
            }

            return outputs;
        }

        // PUT: api/Outputs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOutputs(int id, OutputPutDto outputs)
        {
            Outputs outputFromRepo = _context.Outputs.Where(x => x.Id == id).FirstOrDefault();

            if (outputFromRepo == null)
            {
                return BadRequest();
            }

            _mapper.Map(outputs, outputFromRepo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OutputsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            OutputMqttReadDto outputMqtt = _mapper.Map<OutputMqttReadDto>(outputFromRepo);

            string outputMqttJson = JsonConvert.SerializeObject(outputMqtt).ToLower();

            sendMqqt(outputMqttJson);

            return NoContent();
        }

        // POST: api/Outputs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<OutputReadDto>> PostOutputs(OutputPostDto outputs)
        {
            Outputs outputModel = _mapper.Map<Outputs>(outputs);

            try
            {
                _context.Outputs.Add(outputModel);
            }
            catch
            {
                return BadRequest("Error");
            }
            await _context.SaveChangesAsync();

            OutputReadDto outputRead = _mapper.Map<OutputReadDto>(outputModel);

            OutputMqttReadDto outputMqtt = _mapper.Map<OutputMqttReadDto>(outputModel);

            string outputMqttJson = JsonConvert.SerializeObject(outputMqtt).ToLower();

            sendMqqt(outputMqttJson);

            return CreatedAtAction("GetOutputs", new { id = outputRead.Id }, outputRead);
        }

        // DELETE: api/Outputs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Outputs>> DeleteOutputs(int id)
        {
            var outputs = await _context.Outputs.FindAsync(id);
            if (outputs == null)
            {
                return NotFound();
            }
            outputs.State = 0;

            OutputMqttReadDto outputMqtt = _mapper.Map<OutputMqttReadDto>(outputs);

            string outputMqttJson = JsonConvert.SerializeObject(outputMqtt).ToLower();

            sendMqqt(outputMqttJson);


            _context.Outputs.Remove(outputs);
            await _context.SaveChangesAsync();

            return outputs;
        }

        private bool OutputsExists(int id)
        {
            return _context.Outputs.Any(e => e.Id == id);
        }


        private void sendMqqt(string msg)
        {
            try { 
            MqttClient client = new MqttClient("mqtt.dioty.co");

            string clientId = "dotnetClientHuyTM";
            client.Connect(clientId, "huytmse130336@fpt.edu.vn", "da9413f8");
            client.Publish("/huytmse130336@fpt.edu.vn/in", Encoding.UTF8.GetBytes(msg));
            }
            catch (Exception ex)
            {

            }
        }

        
    }
}
