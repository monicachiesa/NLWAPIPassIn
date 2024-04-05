﻿using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Atteendees.GetAllByEventId;
using PassIn.Application.UseCases.Events.RegisterAttendee;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;

namespace PassIn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeesController : ControllerBase
    {
        //registro em um evento
        [HttpPost]
        [Route("{eventId}/register")]
        [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
        public IActionResult Register([FromBody] RequestRegisterEventJson request, [FromRoute] Guid eventId)
        {
            var useCase = new RegisterAteendeeEventUseCase();

            var response = useCase.Execute(eventId, request);

            return Created(string.Empty, response);

        }


        [HttpGet]
        [Route("eventId")]
        [ProducesResponseType(typeof(ResponseAllAttendeesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public IActionResult GetAll([FromRoute] Guid eventId)
        {
            var useCase = new GetAllAttendeesByEventIdUseCase();

            var response = useCase.Execute(eventId);

            return Ok(response);
        }
    }
}
