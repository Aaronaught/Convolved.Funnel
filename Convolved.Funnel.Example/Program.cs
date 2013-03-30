using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convolved.Funnel.Configuration;
using Convolved.Funnel.Tasks;

namespace Convolved.Funnel.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var configure = ((IConfigurationRoot)null);
            configure.Pickup(@"C:\Temp\*.txt")
                .As<Invoice>()
                .Text()
                .IgnoreBlankLines()
                .Quoting()
                    .AutoDetectQuotedFields()
                    .With("\"")
                    .EscapeWith("\"")
                .DefaultDelimiter(",")
                .Convention<DateTime>(s => DateTime.ParseExact(s, "yyyyMMdd", null))
                .Section(i => i.Messages)
                    .EndsWhen(line => !line.StartsWith("#"))
                    .Fields
                        .Custom(m => m, s => s.Substring(1).Trim())
                .Section(i => i.Header)
                    .Exactly(1).Lines()
                    .Fields
                        .Fixed(10, i => i.Id)
                        .Delimited(new[] { " ", "," }, i => i.Date, i => i.CustomerId)
                        .Delimited(",", i => i.Amount, s => decimal.Parse(s.Substring(1)))
                        .Delimited(i => i.TransactionCount)
                        .QuotedDelimited(",", "\"", "\\", i => i.Comments)
                .Section(i => i.Transactions)
                    .Exactly(i => i.Header.TransactionCount).Lines()
                    .Fields
                        .Delimited(t => t.Type)
                    .RecordType<PartTransaction>(t => t.Type, InvoiceTransactionType.Parts)
                        .Delimited(p => p.PartId, p => p.Quantity, p => p.UnitCost)
                    .RecordType<LaborTransaction>(t => t.Type, InvoiceTransactionType.Labor)
                        .QuotedDelimited(l => l.Description)
                        .Delimited(l => l.Hours, l => l.Rate)
                    .AnyRecordType
                        .Delimited(t => t.Amount)
                    .UnknownRecordType.Error()
                .Section(i => i.Footer)
                    .Fields.Delimited(f => f.TransactionCount)
                .ParseErrors
                    .HandleWith<StandardErrorLog>()
                .Validation
                    .ValidateWith<InvoiceValidator>()
                    .HandleWith<StandardErrorLog>()
                    .HandleWith<MyValidationErrorHandler>()
                .WhenProcessed
                    .TransformWith<CurrencyConverter>("CAD", "USD")
                    .SendTo<InvoiceImporter>()
                        .InBatchesOf(10)
                        .Every(5).Seconds()
                    .MoveIf(FileStatus.OK).To(@"C:\Temp\Processed")
                        .ConflictResolution.Replace()
                    .MoveIf(FileStatus.Error).To(@"C:\Temp\Errors")
                        .ConflictResolution.ReplaceIfNewer().DeleteIfOlder().Rename(20)
                .Reports()
                    .Summary().Every(1).Days()
                        .SendTo("manager@example.com", "it@example.com")
                        .PreferredTime(TimeSpan.Parse("08:00"))
                    .Errors().Every(3).Hours()
                        .SendTo("service@example.com")
                        .PreferredTime(TimeSpan.Parse("06:00"))
                    .NoFilesReceived().Every(1).Days()
                        .SendTo("it@example.com")
                        .PreferredTime(TimeSpan.Parse("09:00"))
                    .SendAllTo("alerts@example.com")
                    .SendWith<SmtpSender>();
        }
    }
}
