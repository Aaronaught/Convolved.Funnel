# Funnel

### Put Files Back in their Place

Have you ever been merrily breezing along a Pretty Cool Project when you realized you had to start importing files from users, business partners, or other applications that you don't control?

If so, then you probably also remember it being a boring, repetitive, tedious, frustrating, error-prone, and generally miserable process, sucking away time from more useful features and inevitably producing bloated code full of bug fixes and try-catch blocks. And that's *with* the libraries like [FileHelpers](http://www.filehelpers.com/) or [CsvReader](http://www.codeproject.com/Articles/9258/A-Fast-CSV-Reader).

# This is a work in progress!

I'm currently pushing this out to GitHub for stability and the occasional code review. It is **not in a working state**. Please do not download this expecting to be able to use it right away. If you're interested in what you see, stay tuned and maybe bookmark/star it to motivate me. :)

## Background

File processing always *sounds* like an easy task, right up until around the first production release when you start working on real data instead of reference data. That's when you find out:

  * The data never follows the spec properly.
  * If it does follow the spec, it's buggy and files often come in malformed or corrupt.
  * The files never arrive on the schedule they are supposed to. They come late or not at all, and you eventually end up with a huge backlog.
  * Files come in duplicate or triplicate and mess up a bunch of things that assume uniqueness.
  * Subtle bugs in your own implementation and/or changes/clarifications to the business requirements creep up and force you to re-process a bunch of files. Hopefully you kept them around.
  * Files get locked due to manual workarounds or slow transfers.

And so on and so forth. So you end up needing to add all kinds of functionality:

  * Extensive error handling at *every* stage of the process (open, read, map, validate, store, etc.)
  * Retry logic to handle transient failures;
  * Notifications and alerts to developers and/or operators;
  * An archiving system that can handle duplicates;
  * A batching/throttling system to handle heavy loads;
  * Reinventing the wheel and writing your own parser when it turns out you can't fix the malformed CSV or EDI.

It's depressing how we always seem to end up solving the *same* problems over and over again, and are never fully able to reuse old code or lean on proven tools because very little ground has been covered between the hard-coded hacks and the hyper-strict ETL tools.

## Features

Funnel is aimed at creating a **fluent, flexible, declarative** syntax for describing files and the process of reading and importing them, as well as providing all of the "plumbing" features out of the box.

You decide what your files look like and what to do with the data and/or errors in them. Funnel automates the workflow so that you only have to worry about the parts of the process that are actually relevant to your problem, and not the tedious mechanics of file I/O and string twiddling.

Here's how simple it *could* look, in an ideal world:

```c#
Configure.Pickup(@"C:\FTP\Joe\*.txt")
	.Every(10).Seconds()
	.As<Person>()
	.Text()
	.DefaultDelimiter(",")
	.Body().Fields.Delimited(p => p.FirstName, p => p.LastName, p => p.BirthDate, p => p.IsVip)
	.WhenProcessed.SendTo<PersonImporter>().Delete();
```

This would be all it would take to start importing a simple CSV file with no special error handling, suitable for a reliable source and small number of files. The example shows how the framework is useful even for basic scenarios; all you would have to do here is create the `Person` class and implement the `PersonImporter` which (for example) inserts the information in a database.

## A Real-World Example

Now here's what it looks like for a worst-case scenario when the rules start to get really ugly and we need to use all of the features:

```c#
Configure.Pickup(@"C:\FTP\Ouch\*.txt")
	.Every(10).Seconds()
	.As<Invoice>()
	.Text()
	.IgnoreBlankLines()
	.Quoting()
		.AutoDetectQuotedFields()
		.With("'")
		.EscapeWith("\\")
	.DefaultDelimiter(",")
	.Convention<DateTime>(s => DateTime.ParseExact(s, "yyyyMMdd", null))
	.Section(i => i.Messages)
		.EndsWhen(line => !line.StartsWith("#"))
		.Fields.Custom(m => m, s => s.Substring(1).Trim())
	.Section(i => i.Header)
		.Exactly(1).Lines()
		.Fields
			.Fixed(10, i => i.Id)
			.Delimited(new[] { " ", "," }, i => i.Date, i => i.CustomerId)
			.Delimited("|", i => i.Amount, s => decimal.Parse(s.Substring(1)))
			.Delimited(i => i.TransactionCount)
			.QuotedDelimited("\"", "\"", i => i.Comments)
	.Section(i => i.Transactions)
		.Exactly(i => i.Header.TransactionCount).Lines()
		.Fields
			.Delimited(t => t.Type)
		.RecordType<Parts>(t => t.Type, TransactionType.Parts)
			.Delimited(p => p.PartId, p => p.Quantity, p => p.UnitCost)
		.RecordType<Labor>(t => t.Type, TransactionType.Labor)
			.Delimited(l => l.Hours, l => l.Rate)
		.AnyRecordType
			.Delimited(t => t.Amount)
		.UnknownRecordType.Ignore()
	.Section(i => i.Footer)
		.Fields.Delimited(f => f.TransactionCount)
	.ParseErrors
		.HandleWith<StandardErrorLog>()
		.LeavePlaceholder()
	.Validation
		.ValidateWith<TransactionCountValidator>()
		.HandleWith<StandardErrorLog, InvoiceValidationErrorLog>()
	.WhenProcessed
		.TransformWith<CurrencyConverter>("CAD", "USD")
		.SendTo<InvoiceImporter>()
			.InBatchesOf(10)
			.Every(5).Seconds()
	    .MoveIf(FileStatus.OK).To(@"C:\FTP\Ouch\Processed")
		    .ConflictResolution.Replace()
	    .MoveIf(FileStatus.Error).To(@"C:\FTP\Ouch\Errors")
		    .ConflictResolution.DeleteIfOlder().Rename(20);
```

If this looks unrealistic, just imagine a scenario in which your original spec was translated into a foreign language and implemented by a team of $5/hr sweatshop developers, then the files get manually edited in Excel and sent over a long-distance dial-up connection. *(P.S. Yes, this really happens.)*

So what's going on here?

  * `IgnoreBlankLines` tells Funnel to skip lines that are just whitespace instead of trying to process them as records.
  
  * `Quoting` sets up the *default* settings for detecting quoted values. Along with `AutoDetectQuotedFields`, Funnel will automatically un-quote any quoted fields, even if the column is inconsistent with some quoted values and some not.

  * The `Convention<DateTime>` tells Funnel that whenever it needs to convert a text value to a `DateTime`, it should use that custom conversion function (i.e. instead of `DateTime.Parse`).

  * Each `Section` indicates that the file format isn't consistent; instead, it's split into different sections including a comment block at the top, a header and footer for validation and common data, and the main transactions in the middle.

  * The `EndsWhen` and `Exactly` lines tell Funnel how many lines are in each section. Note the second `Exactly` which doesn't specify an exact number of lines, but instead refers to some info from the header.

  * `Fields` define what's actually in any given line, and can support just about any hack you can think of while still quietly following the default conventions unless overridden. In the second section, we deal with a space that's been substituted for a comma, and override the default parsing for `decimal` to handle a currency symbol, and then a comment field which doesn't follow the normal quoting rules - perhaps due to that field being edited in Excel.

  * `RecordType` and `AnyRecordType` support polymorphism in record types. Many existing file formats start with a "code" which tells you what kind of information comes after. Funnel will support this natively, without any need for hacks.

  * `UnknownRecordType` determines what to do if none of the `RecordType` entries match. The choices are `Ignore` and `Error`.

  * `ParseErrors` tells Funnel what to do if it can't read a particular line. In this case we log them and also leave a placeholder record, e.g. a null value to indicate that there *was* a line there but no data for it. This might be done to satisfy validation, or to enable a subsequent data recovery process to try to fill it in.

  * `Validation` adds some custom validators to the mix and says what to do if any validation errors are produced. In this case we have a validator that checks for consistency of transaction counts, and any errors are added to a special log.

  * `WhenProcessed` says what to do after the file is (successfully) processed. `TransformWith` mutates the fully-parsed data before the `InvoiceImporter` is allowed to take care of it. In case of heavy load, Funnel will send a maximum of 10 at a time or however many came through in the last 5 seconds - whichever is smaller.

  * Finally, the two `MoveIf` lines route successful and failed files to different journal directories and indicate how to resolve file name conflicts. For successful files we always replace any existing file, but for failed files we want to keep the records around until they can be dealt with, so as long as they're new, we rename them and keep up to 20 rotating backup versions in the archive.

All told, this code snippet of less than 50 lines describes and fully automates a file-loading process that could otherwise have been hundreds or thousands of lines of tedious and barely-maintainable boilerplate code.

## Copyright and License

Copyright (C) 2012 Convolved Software

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.