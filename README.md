# NextLevelSeven

A class library for parsing and manipulating HL7 v2 messages, designed to be fast and easy to use. I
designed it as an nHapi replacement.

This library targets Microsoft .NET Framework version 4.0. This project is not affiliated with
Health Level Seven International, the developers of the HL7 v2 standard.

## Usage

This library was designed to have a small footprint and rely on the least amount of external resources
needed. Including it in your own project is easy.

### Reference

Include `NextLevelSeven` as a reference. Optionally, you may include any of the other modules such as
`NextLevelSeven.Web` or `NextLevelSeven.Specification`, which build on this functionality.

### Message Handling

There are two ways you can handle an HL7 message: **building** and **parsing**. Both methods provide the
same functionality from the top down, but are fundamentally different on the inside.

#### Message Builder

If you are creating a message from scratch, using a Message Builder is the way to go. Memory is allocated
as you populate segments and fields. When you're done building, the message can be exported to a string and
processed any other which way you'd like.

This starts you off with a new message that contains a default MSH segment:
```
var builder = Message.Build();
```

Or, you can initialize a builder with existing message content:
```
var builder = Message.Build(@"MSH|^~\&|ABCD|EFGH");
```

#### Message Parser

If you're working with a message that already exists, but you only need information from a few fields, using
the Message Parser is a better choice. Instead of allocating memory for each distinct piece of the message,
the parser uses cursors to extract the information you're looking for. The benefits of this method really start
to shine when your messages begin to get very large.

This starts you off with a new message that contains a default MSH segment:
```
var parser = Message.Parse();
```

Or, you can initialize a builder with existing message content:
```
var parser = Message.Parse(@"MSH|^~\&|ABCD|EFGH");
```

### Field Access

No matter if you're using a Message Parser or Message Builder, you have complete access to every bit of
data in a message. Accessing elements is a breeze. You can access them either as indexers or as an
IEnumerable for use with LINQ.

The heirarchy looks like this:
`Message -> Segment -> Field -> Field Repetition -> Component -> Subcompoment`

Here are a few examples:
```
// first segment in a message
var mshSegment = message[1];

// first segment, ninth field, first repetition, second component
var messageTriggerEvent = message[1][9][1][2];

// get the first PID segment
var pidSegment = message.Segments.OfType("PID").First();
```

## Specification Module

Simply accessing HL7 message data doesn't tell you very much about the message itself. Luckily,
the standard also gives you some known data types. The `NextLevelSeven.Specification` module
gives you the ability to wrap high level constructs around elements in the HL7 message easily.

## Development

Any help is greatly appreciated! Here's what you need to know...

### Testing

There are a few hundred tests written purely using Microsoft's testing framework. Check your work
against these tests and write new ones to support any additions or bug fixes. A plan to move to NUnit
is being considered.

### Pull Requests

When you've got something to contribute, and tests pass, put in a request and I (SaxxonPike) will
review it. After a quick review, if everything checks out, I'll bring it in. Thanks for your interest!