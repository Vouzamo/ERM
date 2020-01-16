using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;
using Vouzamo.ERM.DTOs;

namespace Vouzamo.ERM.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NodeTypeController : ControllerBase
    {
        private ILogger Logger { get; }
        private IMediator Mediator { get; }

        public NodeTypeController(ILogger<NodeTypeController> logger, IMediator mediator)
        {
            Logger = logger;
            Mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(new NodeTypeByIdQuery(id), cancellationToken);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string name, Guid id, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(new CreateNodeTypeCommand(name, id), cancellationToken);

            return Created(Url.RouteUrl(response.Id), response);
        }

        [HttpPost]
        public async Task<ActionResult> Post(string name, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(new CreateNodeTypeCommand(name), cancellationToken);

            return Created($"{Url.RouteUrl(response.Id)}/{response.Id}", response);
        }

        [HttpPost("{id}/fields")]
        public async Task<ActionResult> PostField(Guid id, [FromBody] FieldDTO field)
        {
            var response = await Mediator.Send(new NodeTypeAddFieldCommand(id, field));

            return Accepted(response);
        }

    }
}
