using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp2_CashFlowApp.Model;

namespace Csharp2_CashFlowApp.Control
{
    internal class TransactionManager
    {
        public List<TransactionEntry> TransactionLedger;
        public Dictionary<DateTime, HashSet<string>> MonthlyGrouping;
    }
}
