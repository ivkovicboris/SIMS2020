using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SIMS2020.Repository.Abstract
{
    public interface IIdentifiable<T>
    {
        T GetId();
        void SetId(T id);
    }
}
