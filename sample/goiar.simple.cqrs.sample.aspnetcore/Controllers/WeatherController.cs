using Goiar.Simple.Cqrs.sample.aspnetcore.Commands;
using Goiar.Simple.Cqrs.sample.aspnetcore.Model;
using Goiar.Simple.Cqrs.sample.aspnetcore.Queries;
using Goiar.Simple.Cqrs.Commands;
using Goiar.Simple.Cqrs.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.sample.aspnetcore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryRequester _queryRequester;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(ILogger<WeatherController> logger, ICommandSender commandSender, IQueryRequester queryRequester)
        {
            _logger = logger;
            _commandSender = commandSender;
            _queryRequester = queryRequester;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _queryRequester.Query<IList<Weather>, GetAllWeatherQuery>(new GetAllWeatherQuery());

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _queryRequester.Query<Weather, GetWeatherByIdQuery>(new GetWeatherByIdQuery(id));

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create(WheatherRequest request)
        {
            var result = await _commandSender.Send<Weather, CreateWeatherCommand>(
                new CreateWeatherCommand(
                    request.Date,
                    request.Summary,
                    request.TemperatureC));

            return CreatedAtAction("GetById", new { id = result.Id }, result);
        }
    }
}
