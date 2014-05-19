## CsvGenerator 

### Introduction

The CsvGenerator is simple and easy to use tool for generation CSV data 
using a fluent interface and lambda expressions. 
CsvGenerator can be accessed by calling IEnumerable<T>.Csv extension method.

### Usage

Assume we have class Person defined: 
```csharp
public class Person
{
	public string FirstName { get; set; }
	public string LastName { get; set; }   
	public string Email;                
	public DateTime BirthDate { get; set; }
	public string Note { get; set; }   
}
```

Definition of csv content would look like this: 
```csharp
string csvContent = listOfPerson.Csv().Columns(column =>
	{
		column.For(person => person.FirstName + " " + person.LastName).Named("Name");
		column.For(person => person.Email); 
		column.For(person => person.BirthDate).Format("{0:dd.MM.yyyy}");
	}).Generate();
```
Output will be 
```
Name,Email,BirthDate
Chuck Testa,chuck@testa.com,22.11.1954
```

### CSV options

|Option                     | Description                                                                                        | Example															|
|---------------------------|----------------------------------------------------------------------------------------------------|------------------------------------------------------------------|
|Columns                    | Defines columns of a CSV                                                                           | .Csv().Columns(column => { column.For(p => p.Name); })			|
|UseSurroundingQuotesAlways | Defines if quotes have to be used around every field. Default is false.                            | .Csv().Columns( /* Columns */ ).UseSurroundingQuotesAlways(true) |
|IncludeHeader              | Defines if output should contain header - names of columns. Default is True.                       | .Csv().Columns( /* Columns */ ).IncludeHeader(false)             |
|IncludeEndingLineBreak     | Defines if Line Break is written after last field of last record. Default is True                  | .Csv().Columns( /* Columns */ ).IncludeEndingLineBreak(false)    |
|SetLineBreak               | Allows change line break chars between CRLF, CR and LF. Default is CRLF.                           | .Csv().Columns( /* Columns */ ).SetLineBreak(LineBreak.Lf)       |
|SetFieldQuote              | Specifies char (string) that is used as a "quote character". Default is double quote (").          | .Csv().Columns( /* Columns */ ).SetFieldQuote("'")               |
|SetFieldDelimiter          | Specifies char (string) that is used as delimiter (separator) between fields. Default is comma (,) | .Csv().Columns( /* Columns */ ).SetFieldDelimiter(";")           |

### Column options                                                                                                                                                                                  

Additional options for every column can be configured by chaining 
additional methods to the end of column.For(...).

Option        | Description                                                                                   | Example															|
--------------|-----------------------------------------------------------------------------------------------|-----------------------------------------------------------------|
Named         | Specifies column's name                                                                       | column.For(x => x.Name).Named("Customer Name")   				|
Format        | A custom format to use when building the cell's value                                         | column.For(p => p.BirthDate).Format("{0:dd.MM.yyyy}");		|
CellCondition | Delegate used to hide the contents of the cells in a column which does not fulfill condition. | column.For(p => p.Income).CellCondition(p=> p.Income < 100000); |

## Details
###What is CSV?

The comma-separated values (CSV) format is a widely used text file 
format often used to exchange data between applications. It contains 
multiple records (one per line), and each field is delimited by a comma. 
Wikipedia (http://en.wikipedia.org/wiki/Comma-separated_values) has a 
good explanation of the CSV format and its history.

There is no definitive standard for CSV, however the most commonly accepted 
definition is RFC 4180 (http://tools.ietf.org/html/rfc4180) - the MIME type 
definition for CSV. CsvGenerator is 100% compliant with RFC 4180, while still 
allowing some flexibility where CSV files deviate from the definition.

The following shows each rule defined in RFC 4180, and how it is treated by CsvGenerator.
### Rule 1

```
1. Each record is located on a separate line, delimited by a line
   break (CRLF).  For example:

   aaa,bbb,ccc CRLF
   zzz,yyy,xxx CRLF
```
CsvGenerator uses CrLf by default, but the end of line symbols can 
be specified by the user using SetLineBreak() method.

### Rule 2
```
2. The last record in the file may or may not have an ending line
   break.  For example:

   aaa,bbb,ccc CRLF
   zzz,yyy,xxx
```
CsvGenerator will add a line break by default, but this behavior can 
be disabled by calling method IncludeEndingLineBreak(false).

### Rule 3
```
3. There maybe an optional header line appearing as the first line
   of the file with the same format as normal record lines.  This
   header will contain names corresponding to the fields in the file
   and should contain the same number of fields as the records in
   the rest of the file (the presence or absence of the header line
   should be indicated via the optional "header" parameter of this
   MIME type).  For example:

   field_name,field_name,field_name CRLF
   aaa,bbb,ccc CRLF
   zzz,yyy,xxx CRLF
```
CsvGenerator writes header line by default. This can be disabled by 
calling IncludeHeader(false) method. 

### Rule 4
```
4. Within the header and each record, there may be one or more
   fields, separated by commas.  Each line should contain the same
   number of fields throughout the file.  Spaces are considered part
   of a field and should not be ignored.  The last field in the
   record must not be followed by a comma.  For example:

   aaa,bbb,ccc
```
The delimiter in CsvGenerator is configurable by calling SetFieldDelimiter() method.
Default is comma.

### Rule 5
```
5. Each field may or may not be enclosed in double quotes (however
   some programs, such as Microsoft Excel, do not use double quotes
   at all).  If fields are not enclosed with double quotes, then
   double quotes may not appear inside the fields.  For example:

   "aaa","bbb","ccc" CRLF
   zzz,yyy,xxx
```
By default CsvGenerator only encloses fields in double quotes when 
they require escaping (see Rule 6), but it is possible to enable 
quotes always by calling UseSurroundingQuotesAlways(true) method.

The quote character is configurable by calling SetFieldQuote() method, default is double quote (").

### Rule 6
```
6. Fields containing line breaks (CRLF), double quotes, and commas
   should be enclosed in double-quotes.  For example:

   "aaa","b CRLF
   bb","ccc" CRLF
   zzz,yyy,xxx
```
CsvGenerator encloses a field in quotes if it contains a newline, quote character or delimiter character.

### Rule 7
```
7. If double-quotes are used to enclose fields, then a double-quote
   appearing inside a field must be escaped by preceding it with
   another double quote.  For example:

   "aaa","b""bb","ccc"
```
CsvGenerator escapes double-quotes with a preceding double-quote. 
Please note that the sometimes-used convention of 
escaping double-quotes as \" (instead of "") is **not supported**.

### Credits
Creation of CsvGenerator was inspired by:  
Super CSV (http://supercsv.sourceforge.net/)  
MvcContrib.Grid (http://mvccontrib.codeplex.com/wikipage?title=Grid)

### Support or Contact
CsvGenerator is not working correctly? Contact me at pavol@koman.sk .
