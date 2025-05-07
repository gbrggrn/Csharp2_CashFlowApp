using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp2_CashFlowApp.Model
{
    public record Category
    {
        public string Name { get; set; } = string.Empty;
        public Enums.CategoryType CategoryType { get; set; }
    }
}
