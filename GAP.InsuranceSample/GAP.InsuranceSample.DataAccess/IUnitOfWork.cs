using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAP.InsuranceSample.DataAccess
{
    public interface IUnitOfWork: IDisposable
    {
        IPolicyRepository Policies { get; }
        int Complete();
    }
}
