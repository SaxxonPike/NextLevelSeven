# NextLevelSeven

[![Build status](https://ci.appveyor.com/api/projects/status/rsckwoeg84r9wmph?svg=true)](https://ci.appveyor.com/project/SaxxonPike/nextlevelseven)

A class library for parsing and manipulating HL7 v2 messages, designed to be
fast and easy to use. There's a few things you should know:

1. This library targets Microsoft .NET Framework version 4.6. This means you'll
need at least Windows Vista with Service Pack 2.
	- It can theoretically be reworked to use version 4, which is XP compatible,
	if you remove references to `IReadOnlyList`, but this has not and will not be
	tested)
1. This project is not affiliated with Health Level Seven International, the
developers and maintainers of the HL7 v2 standard
	- Their website is located at
	[https://hl7.org/](https://hl7.org/ "Health Level Seven International website")

### License

**NextLevelSeven** is available under the [ISC license](LICENSE). It would be
greatly appreciated (but certainly not required) to link back to this repository
if you use this in a project.

## Usage

This library was designed to have a small footprint and rely on the smallest
number of external resources.

### Element Heirarchy

Before working with messages, you'll need to know what's inside what. The
heirarchy looks like this:

* Message
	* Segment
		* Field
			* Repetition
				* Component
					* Subcomponent

A Message is an atomic unit which contains the others. It is the root element
and does not have ancestors.

The first Segment of a Message must be an MSH Segment. The MSH Segment must
contain Fields 1 and 2, which are the Field delimiter and encoding characters
respectively. Each Segment must contain at least one Field, the first of which
is the Segment type. Besides these required components, everything is optional.

All of these elements have indices beginning with 1, which matches the HL7
standard. However, the Segment type can be accessed by a Field index of 0, and
is the only time an index of 0 is valid.

### Messages

There are two ways you can handle an HL7 message: **building** and **parsing**.
Both methods provide the same functionality from the top down, but are
fundamentally different on the inside.

#### Message Builder

If you are creating a message from scratch, using a Message Builder is the way
to go. Memory is allocated as you populate segments and fields. When you're done
building, the message can be exported to a string.

```csharp
// create a message builder with the default MSH segment
var builder = Message.Build();

// create a message builder with existing content
var builder = Message.Build(@"MSH|^~\&|ABCD|EFGH");
```

#### Message Parser

If you're working with a message that already exists, but you only need
information from a few fields, using the Message Parser is a better choice.
Instead of allocating memory for each distinct piece of the message, the parser
uses cursors to extract the information you're looking for. The benefits of this
method really start to add up when your messages are very large.

```csharp
// create a message parser with the default MSH segment
var parser = Message.Parse();

// create a message parser with existing content
var parser = Message.Parse(@"MSH|^~\&|ABCD|EFGH");
```

### Field Access

No matter if you're using a message *parser* or message *builder*, you have
complete access to every bit of data in a message. Accessing elements is a
breeze. You can access them either as indexers or as an `IEnumerable` for use
with LINQ.

Here are a few examples:
```csharp
// first segment in a message (returns IElement)
var mshSegment = message[1];

// alternate way to get first segment (returns ISegment)
var mshSegment = message.Segment(1);

// LINQ works on just about everything
var mshSegment = message.Segments.First();

// 1st segment, 9th field, 1st repetition, 2nd component (returns IElement)
var messageTriggerEvent = message[1][9][1][2];

// 1st segment, 9th field, 2nd component (returns IComponent)
// note: the 1st repetition is implied in this format unless specified
var messageTriggerEvent = message.Segment(1).Field(9).Component(2);

// get the first PID segment
var pidSegment = message.Segments.OfType("PID").First();
```

#### Write Limitations

Both the parser and builder have the ability to read from any field and will
give you identical results given identical input. However, a builder has the
ability to change encoding characters while a parser does not. If you need the
ability to change MSH-1 or MSH-2 for some reason, you must use a builder.

### Type Conversion

If you need to access an element in a message of a type other than a string,
built-in conversion methods are very easy to access.

```csharp
// get the message timestamp (returns DateTimeOffset? type)
var dateTime = message[1][7].Converter.AsDateTime;

// get the message sequence number (returns decimal)
var sequenceNumber = message.Segment(1).Field(13).Converter.AsDecimal;
```

## Manipulation

Sometimes, you might want to modify an existing message. Using either a Builder
or Parser, you can do just that. This functionality works on any `IElement`!

```csharp
// make a field null
message.Segment(1).Field(3).Nullify();

// delete the second segment of a message
message.Segment(2).Delete();

// insert a component value at the beginning of MSH.18
message.Segment(1).Field(18).Component(1).Insert("ASCII");

// move the third segment before the second segment
message.Segment(3).Move(2);

// an alternative way to move a segment within a message
message.Move(3, 2);
```

## Development

Any help is greatly appreciated! Here's what you need to know...

### Testing

The project has successfully been moved to [NUnit](http://nunit.org). Currently,
version 2 is being used. The tests do use `ExpectedExceptionAttribute` which is
not supported in version 3. [FluentAssertions](http://www.fluentassertions.com/)
is also used for testing. [Moq](https://github.com/Moq/moq4) is included as a
reference, but is not used for anything just yet.

There are over 600 tests for this project and over 90% code coverage for the
most used functionality. Hopefully, the project will reach 100% coverage soon.

### Pull Requests

When you've got something to contribute, and tests pass, put in a request and I
(SaxxonPike) will review it. After a quick review, if everything checks out,
I'll bring it in. Thanks for your interest!
