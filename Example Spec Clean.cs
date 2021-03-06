Pickup(@"C:\Temp\*.txt")
	.As<Invoice>()
	.IgnoreBlankLines()
	.Quoting
		.With("\"")
		.EscapeWith("\"")
	.DefaultDelimiter(",")
	.Convention<DateTime>(s => DateTime.ParseExact(s, "yyyyMMdd", null))
	.Section(i => i.Messages)
		.EndsWhen(line => !line.StartsWith("#"))
		.Fields.ToEnd(s => s.Substring(1).Trim())
	.Section(i => i.Header)
		.Exactly(1).Lines()
		.Fields
			.Fixed(10, i => i.Id)
			.Delimited(new[] { " ", "," }, i => i.Date, i => i.CustomerId)
			.Delimited(",", i => i.Amount, s => decimal.Parse(s.Substring(1)))
			.Delimited(i => i.TransactionCount)
			.QuotedDelimited(",", "\"", i => i.Comments)
	.Section(i => i.Transactions)
		.Exactly(i => i.Header.TransactionCount).Lines()
		.EndsWhen(line => line.StartsWith("###")
		.Fields.Delimited(t => t.Type)
		.RecordType<Parts>(t => t.Type, ItemType.Parts)
			.Delimited(p => p.PartId, p => p.Quantity, p => p.Amount)
		.RecordType<Labour>(t => t.Type, ItemType.Labour)
			.QuotedDelimited(l => l.ServiceDescription)
			.Delimited(l => l.Hours, l => l.Amount)
		.UnknownRecordType.Error()
	.Section(i => i.Footer)
		.Fields.Delimited(f => f.TransactionCount)
	.ValidateWith<InvoiceAmountValidator>().And<InvoiceTransactionCountValidator>()
	.ParseErrors
		.HandleWith<StandardErrorLog>()
	.ValidationErrors
		.HandleWith<StandardErrorLog>().And<PersistentValidationErrorLog>()
	.WhenProcessed
		.TransformWith<CurrencyConverter>("CAD", "USD")
		.SendTo<InvoiceImporter>().InBatchesOf(10).Every(5).Seconds()
		.MoveIf(Status.OK).To(@"C:\Temp\Processed")
		.MoveIf(Status.Error).To(@"C:\Temp\Errors");