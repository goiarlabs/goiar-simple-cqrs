using goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Commands;
using goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Models;
using goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Queries;
using Goiar.Simple.Cqrs.Commands;
using Goiar.Simple.Cqrs.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Controllers
{
    [Route("human"), ApiController]
    public class HumanController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryRequester _queryRequester;

        public HumanController(ICommandSender commandSender, IQueryRequester queryRequester)
        {
            _commandSender = commandSender;
            _queryRequester = queryRequester;
        }

        #region Command Endpoints

        [HttpPost]
        public async Task<IActionResult> Create(CreateHumanCommand createHumanCommand)
        {
            var result = await _commandSender.Send<Human, CreateHumanCommand>(createHumanCommand);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateHumanCommand updateHumanCommand)
        {
            await _commandSender.Send(new UpdateHumanCommand(id, updateHumanCommand.Name));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _commandSender.Send(new DeleteHumanCommand(id));

            return NoContent();
        }

        #endregion

        #region Query endpoints

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _queryRequester.Query<List<Human>, GetAllHumansQuery>(new GetAllHumansQuery());

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByid(Guid id)
        {
            var result = await _queryRequester.Query<Human, GetHumanByIdQuery>(new GetHumanByIdQuery(id));

            return Ok(result);
        }

        #endregion
    }
}
