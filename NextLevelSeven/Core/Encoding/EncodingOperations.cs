using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NextLevelSeven.Core.Encoding
{
    /// <summary>Contains operations for escaping and unescaping HL7 strings.</summary>
    internal static class EncodingOperations
    {
        /// <summary>Add HL7 escape codes where necessary, according to this encoding configuration.</summary>
        /// <param name="config">Encoding configuration.</param>
        /// <param name="s">String to escape.</param>
        /// <returns>Escaped string.</returns>
        public static string Escape(IReadOnlyEncoding config, string s)
        {
            if (s == null)
            {
                return null;
            }

            var componentDelimiter = config.ComponentDelimiter;
            var escapeDelimiter = config.EscapeCharacter;
            var fieldDelimiter = config.FieldDelimiter;
            var repetitionDelimiter = config.RepetitionDelimiter;
            var subcomponentDelimiter = config.SubcomponentDelimiter;

            var data = s.ToCharArray();
            var length = data.Length;
            var output = new StringBuilder();

            for (var index = 0; index < length; index++)
            {
                var c = data[index];

                if (c == componentDelimiter)
                {
                    output.Append(new[]
                    {
                        escapeDelimiter, 'S', escapeDelimiter
                    });
                    continue;
                }

                if (c == escapeDelimiter)
                {
                    if (length - index >= 3)
                    {
                        if (data[index + 2] == escapeDelimiter)
                        {
                            switch (data[index + 1])
                            {
                                case 'N': // normal text
                                case 'H': // highlight text
                                    output.Append(new[]
                                    {
                                        escapeDelimiter, data[index + 1], escapeDelimiter
                                    });
                                    index += 2;
                                    continue;
                            }
                        }
                        else if (length - index >= 5)
                        {
                            switch (data[index + 1])
                            {
                                case 'C': // single byte character set escape
                                    if (length - index >= 7 && data[index + 6] == escapeDelimiter)
                                    {
                                        output.Append(new string(data, index, 7));
                                        index += 6;
                                        continue;
                                    }
                                    break;
                                case 'M': // multi-byte character set escape
                                    if (length - index >= 7 && data[index + 6] == escapeDelimiter)
                                    {
                                        // without optional third pair
                                        output.Append(new string(data, index, 7));
                                        index += 6;
                                        continue;
                                    }
                                    if (length - index >= 9 && data[index + 8] == escapeDelimiter)
                                    {
                                        // with optional third pair
                                        output.Append(new string(data, index, 9));
                                        index += 8;
                                        continue;
                                    }
                                    break;
                                case 'X': // locally defined hex codes
                                case 'Z': // locally defined escape
                                    var zEscapeIndex = index + 1;
                                    var zEscapeLength = 1;
                                    var zEscapeEndFound = false;

                                    while (zEscapeIndex < length)
                                    {
                                        zEscapeLength++;
                                        if (data[zEscapeIndex] == escapeDelimiter)
                                        {
                                            zEscapeEndFound = true;
                                            break;
                                        }
                                        zEscapeIndex++;
                                    }

                                    if (zEscapeEndFound)
                                    {
                                        output.Append(new string(data, index, zEscapeLength));
                                        index += zEscapeLength - 1;
                                        continue;
                                    }

                                    break;
                            }
                        }
                    }

                    output.Append(new[]
                    {
                        escapeDelimiter, 'E', escapeDelimiter
                    });
                    continue;
                }

                if (c == fieldDelimiter)
                {
                    output.Append(new[]
                    {
                        escapeDelimiter, 'F', escapeDelimiter
                    });
                    continue;
                }

                if (c == repetitionDelimiter)
                {
                    output.Append(new[]
                    {
                        escapeDelimiter, 'R', escapeDelimiter
                    });
                    continue;
                }

                if (c == subcomponentDelimiter)
                {
                    output.Append(new[]
                    {
                        escapeDelimiter, 'T', escapeDelimiter
                    });
                    continue;
                }

                output.Append(c);
            }

            return output.ToString();
        }

        /// <summary>Remove HL7 escape codes, using this encoding configuration for special characters.</summary>
        /// <param name="config">Encoding configuration.</param>
        /// <param name="s">String to unescape.</param>
        /// <returns>Unescaped string.</returns>
        public static string UnEscape(IReadOnlyEncoding config, string s)
        {
            var escapeRegex = new Regex($"\\{config.EscapeCharacter}(E|F|R|S|T)*.\\{config.EscapeCharacter}");
            
            if (s == null)
            {
                return null;
            }

            var componentDelimiter = new string(config.ComponentDelimiter, 1);
            var escapeDelimiter = new string(config.EscapeCharacter, 1);
            var fieldDelimiter = new string(config.FieldDelimiter, 1);
            var repetitionDelimiter = new string(config.RepetitionDelimiter, 1);
            var subcomponentDelimiter = new string(config.SubcomponentDelimiter, 1);

            var data = s.ToCharArray();
            var length = data.Length;
            var output = new StringBuilder(s);
            var matches = escapeRegex.Matches(s);

            foreach (var match in matches.Cast<Match>().OrderByDescending(m => m.Index))
            {
                var matchValue = match.Value;
                var value = match.Value;
                
                switch (matchValue[1])
                {
                    case 'E':
                        value = escapeDelimiter;
                        break;
                    case 'F':
                        value = fieldDelimiter;
                        break;
                    case 'R':
                        value = repetitionDelimiter;
                        break;
                    case 'S':
                        value = componentDelimiter;
                        break;
                    case 'T':
                        value = subcomponentDelimiter;
                        break;
                }

                if (value != matchValue)
                {
                    output.Remove(match.Index, matchValue.Length);
                    output.Insert(match.Index, value);
                }
            }
            
            return output.ToString();
        }
    }
}