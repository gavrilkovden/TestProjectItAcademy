using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Application.Abstractions.Transaction
{
    public interface IContextTransactionCreator
    {
        public Task<IContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    }
}
