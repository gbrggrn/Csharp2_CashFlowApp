using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Csharp2_CashFlowApp.Model
{
    /// <summary>
    /// Defines a MonthReport instance.
    /// </summary>
    internal class MonthReport
    {
        public string Month { get; set; } = string.Empty;
        public List<string> Top3Revenues { get; set; } = [];
        public List<string> Top3Expenses { get; set; } = [];

        public double NetCashFlow { get; set; } = 0.0;
    }
}