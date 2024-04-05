using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System.Data;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendee
{
    public class RegisterAteendeeEventUseCase
    {
        private readonly PassInDBContext _dbContext;
        public RegisterAteendeeEventUseCase()
        {
            _dbContext = new PassInDBContext();
        }
        public ResponseRegisterJson Execute(Guid eventId, RequestRegisterEventJson request)
        {
            Validate(eventId, request);

            var entity = new Infrastructure.Entities.Attendee
            {
                Name = request.Name,
                Email = request.Email,
                Event_Id = eventId,
                Created_At = DateTime.UtcNow
            };

            _dbContext.Attendees.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseRegisterJson
            {
                Id = entity.Id
            };
        }


        private void Validate(Guid eventId, RequestRegisterEventJson request)
        {
            var eventEntity = _dbContext.Events.Find(eventId);

            if (eventEntity is null)
            {
                throw new NotFoundException("Não existe um evento com este id");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ErrorOnValidationException("Nome não pode ser vazio");
            }

            if (EmailIsValid(request.Email) == false)
            {
                throw new ErrorOnValidationException("O e-mail é inválido");
            }

            var attendeeAlreadyRegistered = _dbContext.Attendees
                .Any(at => at.Email.Equals(request.Email) && at.Event_Id == eventId);

            if (attendeeAlreadyRegistered)
            {
                throw new ConflictException("Você ja está inscrito neste evento");
            }

            var attendeesForEvent = _dbContext.Attendees.Count(a => a.Event_Id != eventId);

            if(attendeesForEvent > eventEntity.Maximum_Attendees)
            {
                throw new ErrorOnValidationException("Não há mais vagas para este evento");
            }
        }

        private bool EmailIsValid(string email)
        {
            try
            {
                //se o e-mail for válido, continua a execução do código, 
                new MailAddress(email);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
