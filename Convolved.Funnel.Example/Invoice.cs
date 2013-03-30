using System;
using System.Collections.Generic;
using Convolved.Funnel.Tasks;
using Convolved.Funnel.Validation;

namespace Convolved.Funnel.Example
{
    public class Invoice
    {
        public InvoiceHeader Header { get; set; }
        public IList<InvoiceTransaction> Transactions { get; set; }
        public IList<string> Messages { get; set; }
        public InvoiceFooter Footer { get; set; }
    }

    public class InvoiceHeader
    {
        public decimal Id { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public uint TransactionCount { get; set; }
        public string Comments { get; set; }
    }

    public class InvoiceFooter
    {
        public uint TransactionCount { get; set; }
    }

    public enum InvoiceTransactionType
    {
        Parts,
        Labor
    }

    public class InvoiceTransaction
    {
        public InvoiceTransactionType Type { get; set; }
        public decimal Amount { get; set; }
    }

    public class PartTransaction : InvoiceTransaction
    {
        public int PartId { get; set; }
        public decimal UnitCost { get; set; }
        public int Quantity { get; set; }
    }

    public class LaborTransaction : InvoiceTransaction
    {
        public string Description { get; set; }
        public int Hours { get; set; }
        public decimal Rate { get; set; }
    }

    public class InvoiceValidator : IValidate<Invoice>
    {
    }

    public class CurrencyConverter : ITransform<Invoice>, IAcceptParameters
    {
        public void Transform(Invoice model)
        {
            throw new NotImplementedException();
        }

        public void SetParameters(object[] parameters)
        {
            throw new NotImplementedException();
        }
    }

    public class InvoiceImporter : IReceive<Invoice>
    {
        public void Receive(IEnumerable<Invoice> items)
        {
            throw new NotImplementedException();
        }
    }
}