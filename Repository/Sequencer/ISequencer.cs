using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Repository.Sequencer
{
    public interface ISequencer<T>
    {
        void Initialize(T initId);
        T GenerateId();
    }
}
