using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp2_CashFlowApp.Model
{
    /// <summary>
    /// Defines a Category.
    /// </summary>
    public record Category
    {
        public Enums.CategoryName CategoryName {  get; set; }
        public Enums.CategoryType CategoryType { get; set; }
    }
}
