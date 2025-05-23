﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Csharp2_CashFlowApp.Control.TransactionManager;

namespace Csharp2_CashFlowApp.Model
{
    /// <summary>
    /// Defines a transaction.
    /// </summary>
    public record Transaction
    {
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public Category? Category { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
