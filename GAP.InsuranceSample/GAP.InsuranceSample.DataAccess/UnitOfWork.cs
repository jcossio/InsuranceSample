using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAP.InsuranceSample.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InsuranceEntities _context;

        public UnitOfWork(InsuranceEntities context)
        {
            _context = context;
            Policies = new PolicyRepository(_context);
        }

        public IPolicyRepository Policies { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
