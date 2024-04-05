using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.CheckIns.DoCheckIn
{
    public class DoAttendeeCheckInUseCase
    {
        private readonly PassInDBContext _dbContext;
        public DoAttendeeCheckInUseCase()
        {
            _dbContext = new PassInDBContext();
        }

        public ResponseRegisterJson Execute(Guid attendeeId)
        {
            Validate(attendeeId);

            var entity = new CheckIn
            {
                Attendee_Id = attendeeId,
                Created_At = DateTime.UtcNow,
            };

            _dbContext.CheckIns.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseRegisterJson
            {
                Id = entity.Id
            };
        }

        private void Validate(Guid attendeeId)
        {
            var exists = _dbContext.Attendees.Any(at => attendeeId == at.Id);

            if (!exists)
            {
                throw new NotFoundException("O Participante com este id não foi encontrado");
            }

            var existCheckIn = _dbContext.CheckIns.Any(ch => ch.Attendee_Id == attendeeId);

            if (existCheckIn)
            {
                throw new ConflictException("O participante não pode fazer checkin duas vezes no mesmo evento");
            }
        }
    }
}
