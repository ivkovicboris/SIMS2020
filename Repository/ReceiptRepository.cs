using SIMS2020.Model;
using SIMS2020.Repository.Abstract;
using SIMS2020.Repository.CSV;
using SIMS2020.Repository.CSV.Stream;
using SIMS2020.Repository.Sequencer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Repository
{
    public class ReceiptRepository : CSVRepository<Receipt, long>, IReceiptRepository
    {
        private const string ENTITY_NAME = "Medicine";

        public ReceiptRepository(ICSVStream<Receipt> stream, ISequencer<long> sequencer)
                                : base(ENTITY_NAME, stream, sequencer)
        {
        }

    }
}
