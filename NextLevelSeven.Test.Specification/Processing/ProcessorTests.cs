using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.MessageGeneration;
using NextLevelSeven.Specification.Processing;

namespace NextLevelSeven.Test.Specification.Processing
{
    [TestClass]
    public class ProcessorTests
    {
        /// <summary>
        ///     Create a message, create a processor, attach test handlers, and run a test to see which ones respond.
        ///     Assertions are made that only (and all) handlers that should respond do. If no handlers are expected to
        ///     respond, then an assertion is made that the "unhandled message" event is raised.
        /// </summary>
        static void ProcessHandlers(string messageType, string messageTrigger, IEnumerable<ProcessorTestHandlerOptions> options)
        {
            var message = MessageGenerator.Generate(messageType, messageTrigger, Randomized.String());
            var processor = new Processor();
            var optionsList = new List<ProcessorTestHandlerOptions>(options);
            var result = new bool[optionsList.Count];
            var index = 0;
            var unhandled = false;
            var expectHandled = optionsList.Any(e => e.Expected);

            foreach (var option in optionsList)
            {
                processor.Register(option.Type, option.Trigger, CreateProcessHandler(ref result, index++));
            }
            processor.OnMessageUnhandled += (sender, e) => { unhandled = true; };
            processor.Process(message);

            index = 0;
            foreach (var option in optionsList)
            {
                Assert.AreEqual(result[index], option.Expected, string.Format(
                    "{0} Message: {1}^{2}, Handler: {3}^{4}",
                    option.Message ?? "Incorrectly handled message.",
                    messageType, messageTrigger, option.Type, option.Trigger));
                index++;
            }

            Assert.AreEqual(expectHandled, !unhandled, "Unhandled message event differs from expected occurence.");
        }

        static ProcessorEventHandler CreateProcessHandler(ref bool[] results, int index)
        {
            var localResults = results;
            return (sender, e) => { localResults[index] = true; };
        }

        [TestMethod]
        public void Processor_CallsUnhandledIfNoMatches_AndSingleHandler()
        {
            var type = Randomized.StringLetters(3);
            var trigger = Randomized.StringLetters(3);
            ProcessHandlers(type, trigger, new[]
            {
                new ProcessorTestHandlerOptions { Type = Randomized.StringNumbers(3), Trigger = string.Empty, Expected = false },
            });
        }

        [TestMethod]
        public void Processor_CallsUnhandledIfNoMatches_AndMultipleHandlers()
        {
            var type = Randomized.StringLetters(3);
            var trigger = Randomized.StringLetters(3);
            ProcessHandlers(type, trigger, new[]
            {
                // no trigger and no match
                new ProcessorTestHandlerOptions { Type = Randomized.StringNumbers(3), Trigger = string.Empty, Expected = false },

                // no match with both type and trigger
                new ProcessorTestHandlerOptions { Type = Randomized.StringNumbers(3), Trigger = Randomized.StringNumbers(3), Expected = false },
            });
        }

        [TestMethod]
        public void Processor_CallsMatchingTypeAndTriggerHandler()
        {
            var type = Randomized.StringLetters(3);
            var trigger = Randomized.StringLetters(3);
            ProcessHandlers(type, trigger, new[]
            {
                new ProcessorTestHandlerOptions { Type = type, Trigger = trigger, Expected = true },
            });
        }

        [TestMethod]
        public void Processor_CallsMatchingTypeHandler()
        {
            var type = Randomized.StringLetters(3);
            var trigger = Randomized.StringLetters(3);
            ProcessHandlers(type, trigger, new[]
            {
                new ProcessorTestHandlerOptions { Type = type, Trigger = string.Empty, Expected = true },
            });
        }

        [TestMethod]
        public void Processor_DoesNotCallNonMatchingGenericHandler()
        {
            var type = Randomized.StringLetters(3);
            var trigger = Randomized.StringLetters(3);
            ProcessHandlers(type, trigger, new[]
            {
                new ProcessorTestHandlerOptions { Type = Randomized.StringNumbers(3), Trigger = string.Empty, Expected = false },
            });
        }

        [TestMethod]
        public void Processor_DoesNotCallNonMatchingTriggerHandler()
        {
            var type = Randomized.StringLetters(3);
            var trigger = Randomized.StringLetters(3);
            ProcessHandlers(type, trigger, new[]
            {
                new ProcessorTestHandlerOptions { Type = type, Trigger = Randomized.StringNumbers(3), Expected = false },
            });
        }

        [TestMethod]
        public void Processor_DoesNotCallDifferentTypeAndTriggerHandler()
        {
            var type = Randomized.StringLetters(3);
            var trigger = Randomized.StringLetters(3);
            ProcessHandlers(type, trigger, new[]
            {
                new ProcessorTestHandlerOptions { Type = Randomized.StringNumbers(3), Trigger = Randomized.StringNumbers(3), Expected = false },
            });
        }

        [TestMethod]
        public void Processor_CallsMultipleHandlersThatMatch()
        {
            var type = Randomized.StringLetters(3);
            var trigger = Randomized.StringLetters(3);
            ProcessHandlers(type, trigger, new[]
            {
                // type and trigger are specified and matching
                new ProcessorTestHandlerOptions { Type = type, Trigger = trigger, Expected = true },

                // only type is specified (generic) and matches
                new ProcessorTestHandlerOptions { Type = type, Trigger = string.Empty, Expected = true },

                // only type is specified (generic) and does not match
                new ProcessorTestHandlerOptions { Type = Randomized.StringNumbers(3), Trigger = string.Empty, Expected = false },

                // type matches but trigger does not
                new ProcessorTestHandlerOptions { Type = type, Trigger = Randomized.StringNumbers(3), Expected = false },

                // neither type nor trigger match
                new ProcessorTestHandlerOptions { Type = Randomized.StringNumbers(3), Trigger = Randomized.StringNumbers(3), Expected = false },
            });
        }
    }
}
