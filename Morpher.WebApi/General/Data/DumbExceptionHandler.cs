using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Morpher.WebService.V3.General.Data
{
    public class DumbExceptionHandler : IExceptionHandler
    {
        public async Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            var exception = ExceptionDispatchInfo.Capture(context.Exception);
            exception.Throw();
        }
    }
}