using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Common.Application.Abstractions.Transaction;

namespace Common.Application
{
    public class ContextTransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IContextTransactionCreator _contextTransactionCreator;

        public ContextTransactionBehavior(IContextTransactionCreator contextTransactionCreator)
        {
            _contextTransactionCreator = contextTransactionCreator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            await using var transaction = await _contextTransactionCreator.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await next();
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(CancellationToken.None);
                throw;
            }
        }
    }
  
}
