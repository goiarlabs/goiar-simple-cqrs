using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

using Goiar.Simple.Cqrs.Queries;
using Goiar.Simple.Cqrs.Commands;
using goiar.simple.cqrs.sample.aspnetcore.eventhubs.Queries;
using goiar.simple.cqrs.sample.aspnetcore.eventhubs.Commands;
using goiar.simple.cqrs.sample.aspnetcore.eventhubs.Models;

namespace goiar.simple.cqrs.sample.aspnetcore.eventhubs.Controllers
{
    [ApiController]
    [Route("Product")]
    public class ProductController : ControllerBase
    {
        #region Fields

        private readonly IQueryRequester _queryRequester;
        private readonly ICommandSender _commandSender;

        #endregion

        #region Constructor

        public ProductController(IQueryRequester queryRequester, ICommandSender commandSender)
        {
            _queryRequester = queryRequester;
            _commandSender = commandSender;
        }

        #endregion

        #region Query Endpoints

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _queryRequester
                .Query<List<Product>, GetAllProductsQuery>(new GetAllProductsQuery());

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _queryRequester
                .Query<Product, GetProductByIdQuery>(new GetProductByIdQuery(id));

            return Ok(result);
        }

        #endregion

        #region Command Endpoints

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var result = await _commandSender.Send<Product, CreateProductCommand>(
                new CreateProductCommand(request.Price, request.Name));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _commandSender.Send(new DeleteProductCommand(id));

            return NoContent();
        }

        #endregion
    }
}
