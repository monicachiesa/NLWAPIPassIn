using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Events.GetById;
using PassIn.Application.UseCases.Events.Register;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;

namespace PassIn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterEventJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RequestEventJson request)
        {
            try
            {
                var useCase = new RegisterEventUseCase();

                var response = useCase.Execute(request);

                return Created(string.Empty, response);
            }
            catch (PassInException ex)
            {
                return BadRequest(new ResponseErrorJson(ex.Message)); //retorna a mensagem passada
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorJson("Erro desconhecido")); //retorna erro 500
            }

        }

        [HttpGet]
        [Route("{id}")] //tem que ter o mesmo nome que está abaixo 
        [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public IActionResult GetById([FromRoute] Guid id) //tem que ser o mesmo nome do Route
        {
            try
            {

                var useCase = new GetEventByIdUseCase();

                var response = useCase.Execute(id);

                return Ok(response);
            }
            catch (PassInException ex)
            {
                //Not Found retorna 404
                return NotFound(new ResponseErrorJson(ex.Message)); //retorna a mensagem passada
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorJson("Erro desconhecido")); //retorna erro 500
            }
        }
    }
}
