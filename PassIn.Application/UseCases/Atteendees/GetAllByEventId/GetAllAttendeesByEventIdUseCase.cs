using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassIn.Application.UseCases.Atteendees.GetAllByEventId
{
    public class GetAllAttendeesByEventIdUseCase
    {
        private readonly PassInDBContext _dbContext;
        public GetAllAttendeesByEventIdUseCase()
        {
            _dbContext = new PassInDBContext();
        }
        public ResponseAllAttendeesJson Execute(Guid eventId)
        {
            var entity = _dbContext.Events
                .Include(p => p.Attendees)
                .ThenInclude(ch => ch.CheckIn)
                .FirstOrDefault(ev => ev.Id == eventId);

            if (entity is null)
            {
                throw new NotFoundException("O evento não existe");
            }

            return new ResponseAllAttendeesJson
            {
                Attendees = entity.Attendees
                .Select(at => new ResponseAttendeeJson
                {
                    Id = at.Id,
                    Name = at.Name,
                    Email = at.Email,
                    CreatedAt = at.Created_At,
                    CheckedInAt = at.CheckIn?.Created_At
                })
                .ToList()
            };
        }
    }
}
