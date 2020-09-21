using System.Data;
using System.Threading.Tasks;
using Core.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Common.ActionFilters {
    public class UowActionFilter : ActionFilterAttribute {
        private readonly IsolationLevel transactionIsolationLevel;
        private readonly IUnitOfWork<IDbConnection> unitOfWork;

        public UowActionFilter(
            IUnitOfWork<IDbConnection> unitOfWork,
            IsolationLevel transactionIsolationLevel =
                IsolationLevel.ReadCommitted
        ) {
            this.unitOfWork = unitOfWork;
            this.transactionIsolationLevel = transactionIsolationLevel;
        }

        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
        ) {
            try {
                unitOfWork.Begin(transactionIsolationLevel);
            }
            catch {
                context.Result = new StatusCodeResult(
                    statusCode: StatusCodes.Status503ServiceUnavailable
                );
                return;
            }

            var executedContext = await next();

            try {
                if (executedContext.Exception == null) {
                    unitOfWork.Commit();
                }
                else {
                    unitOfWork.Rollback();
                }
            }
            catch {
                executedContext.Result = new StatusCodeResult(
                    statusCode: StatusCodes.Status503ServiceUnavailable
                );
                executedContext.ExceptionHandled = true;
            }
        }
    }
}