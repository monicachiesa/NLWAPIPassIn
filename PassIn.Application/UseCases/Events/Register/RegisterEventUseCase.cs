using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassIn.Application.UseCases.Events.Register
{
    public class RegisterEventUseCase
    {
        public ResponseRegisterEventJson Execute(RequestEventJson request)
        {
            Validate(request);

            var dbContext = new PassInDBContext();

            var entity = new Infrastructure.Entities.Event
            {
                Title = request.Title,
                Details = request.Details,
                Slug = request.Title.ToLower().Replace(" ", "-")
            };

            dbContext.Events.Add(entity);
            dbContext.SaveChanges();

            return new ResponseRegisterEventJson
            {
                Id = entity.Id
            };
        }

        private void Validate(RequestEventJson request)
        {
            if (request.MaximumAttendees <= 0)
            {
                throw new ArgumentException("O máximo de participantes é inválido");
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ArgumentException("Título não pode ser vazio");
            }

            if (string.IsNullOrWhiteSpace(request.Details))
            {
                throw new ArgumentException("Detalhes não pode ser vazio");
            }
        }
    }
}
