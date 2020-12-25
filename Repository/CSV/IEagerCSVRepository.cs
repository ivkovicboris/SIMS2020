using SIMS2020.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Repository.CSV
{
    public interface IEagerCSVRepository<E, ID>
        where E : IIdentifiable<ID>
        where ID : IComparable
    {
        E GetEager(ID id);
        IEnumerable<E> GetAllEager();
    }
}
