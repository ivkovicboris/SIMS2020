using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Repository.Sequencer
{
    public class LongSequencer : ISequencer<long>
    {
        //private long _newId;
        //public long GenerateId() => _newId = new Random().Next(101);

        private long _newId;
        public long GenerateId() => _newId++;
        public void Initialize(long initId) => _newId = initId;
    }
}
