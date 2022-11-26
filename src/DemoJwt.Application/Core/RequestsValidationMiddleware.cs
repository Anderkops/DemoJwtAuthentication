using Flunt.Notifications;
using MediatR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DemoJwt.Application.Core
{
    public sealed class RequestsValidationMiddleware<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
        //where TRequest : Request<Response>
        //where TResponse : Response
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // NOTE QUE A EXECUÇÃO DO HANDLER ACONTECE AQUI NO MEIO
            var response = next();

            stopwatch.Stop();            

            return response = next();
        }

        //private static Task<TResponse> Errors(IEnumerable<Notification> notifications)
        //{
        //    var response = new Response();            
        //    response.AddNotifications(notifications);

        //    return Task.FromResult(response as TResponse);
        //}
    }
}
