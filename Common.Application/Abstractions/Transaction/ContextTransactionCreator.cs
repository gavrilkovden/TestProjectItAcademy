using Common.Application.Abstractions.Transaction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Application.Abstractions.Transaction
{
    public class ContextTransactionCreator: IContextTransactionCreator
    {
        private readonly ApplicationDbContext _dbContext;
        public ContextTransactionCreator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            return new ContextTransaction(await _dbContext.Database.BeginTransactionAsync(cancellationToken));
        }

    }
}
