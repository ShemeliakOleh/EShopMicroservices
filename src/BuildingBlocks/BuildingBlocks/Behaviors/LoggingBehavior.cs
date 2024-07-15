using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;
public class LoggingBehavior<TRequest, TResponse>
    (ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[Start] Handle reuqest = {Request} - Response = {Response}, request body = {Body}",
            typeof(TRequest).Name, typeof(TResponse).Name, request
        );

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();

        var timeTaken = timer.Elapsed;
        if(timeTaken.Seconds > 3)
        {
            logger.LogWarning(
                "[PERFORMANCE] Handle request = {Request} - Response = {Response}, time taken = {TimeTaken}",
                typeof(TRequest).Name, typeof(TResponse).Name, timeTaken
            );
        }

        logger.LogInformation(
            "[End] Handle request = {Request} - Response = {Response}",
            typeof(TRequest).Name, typeof(TResponse).Name
          );

        return response;
    }
}
