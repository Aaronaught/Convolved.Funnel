File Processing Rework


//Text(@"C:\Temp\*.txt")
Pickup(@"C:\Temp\*.txt")
	.As<Invoice>()
	.Text()
	.IgnoreBlankLines()
	.Quoting
		// .AutoDetect()
		.With("\"")
		.EscapeWith("\"")
	.DefaultDelimiter(",")
	.Convention<DateTime>(s => DateTime.ParseExact(s, "yyyyMMdd", null))
	.Section(i => i.Messages)
		.EndsWhen(line => !line.StartsWith("#"))
		// .EndsWithBlankLine()
		// .EndsWith("--- INVOICE ---")
		.Fields.ToEnd(s => s.Substring(1).Trim())
	.Section(i => i.Header)
		// Allow 1-line sections to write to single property rather than list
		// Throw an exception if the section ends up having more than 1 line
		.Exactly(1).Lines()
		.Fields
			.Fixed(10, i => i.Id)
			.Delimited(new[] { " ", "," }, i => i.Date, i => i.CustomerId)
			.Delimited(",", i => i.Amount, s => decimal.Parse(s.Substring(1)))
			.Delimited(i => i.TransactionCount)
			.QuotedDelimited(",", "\"", i => i.Comments)
	.Section(i => i.Transactions)
		// Property won't actually be set (see below), so this needs to be a pure
		// expression, which is resolved by mapping to its configuration...
		.Exactly(i => i.Header.TransactionCount).Lines()
		// .EndsWhen(line => line.StartsWith("###")
		// In order to implement this type of polymorphism, need to read all of
		// the field values as their correct types BEFORE attempting to create the
		// record or its subtype.
		.Fields.Delimited(t => t.Type)
		.RecordType<Parts>(t => t.Type, ItemType.Parts)
			.Delimited(p => p.PartId, p => p.Quantity, p => p.Amount)
		.RecordType<Labour>(t => t.Type, ItemType.Labour)
			.QuotedDelimited(l => l.ServiceDescription)
			.Delimited(l => l.Hours, l => l.Amount)
		// .RecordType<Foo>(line => line.StartsWith("#foo"))
		.UnknownRecordType.Error()	// or Ignore()
	.Section(i => i.Footer)
		.Fields.Delimited(f => f.TransactionCount)
	.ValidateWith<InvoiceAmountValidator>().And<InvoiceTransactionCountValidator>()
	.ParseErrors
		// .LeavePlaceholder()
		.HandleWith<StandardErrorLog>()
	.ValidationErrors
		// .Throw()
		.HandleWith<StandardErrorLog>().And<PersistentValidationErrorLog>()
	.WhenProcessed
		.TransformWith<CurrencyConverter>("CAD", "USD")
		// Sends when there are 10 in the buffer or when 5 seconds have elapsed,
		// whichever comes first. (May send less than 10, but never more).
		.SendTo<InvoiceImporter>().InBatchesOf(10).Every(5).Seconds()
		.MoveIf(Status.OK).To(@"C:\Temp\Processed")
		.MoveIf(Status.Error).To(@"C:\Temp\Errors");