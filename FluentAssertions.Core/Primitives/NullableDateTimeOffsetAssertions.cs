﻿using System;
using System.Diagnostics;

using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is in the expected state.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="FluentDateTimeExtensions"/> for a more fluent way of specifying a <see cref="DateTime"/>.
    /// </remarks>
    [DebuggerNonUserCode]
    public class NullableDateTimeOffsetAssertions : DateTimeOffsetAssertions
    {
        public NullableDateTimeOffsetAssertions(DateTimeOffset? expected)
            : base(expected)
        {
        }

        /// <summary>
        /// Asserts that a nullable <see cref="DateTime"/> or <see cref="DateTimeOffset"/> value is not <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>      
        public AndConstraint<NullableDateTimeOffsetAssertions> HaveValue(string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected variable to have a value{reason}, but found {0}", Subject);

            return new AndConstraint<NullableDateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that a nullable <see cref="DateTime"/> or <see cref="DateTimeOffset"/> value is <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>      
        public AndConstraint<NullableDateTimeOffsetAssertions> NotHaveValue(string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue)
                .BecauseOf(because, reasonArgs)
                .FailWith("Did not expect variable to have a value{reason}, but found {0}", Subject);
            
            return new AndConstraint<NullableDateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the value is equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> Be(DateTimeOffset? expected, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject == expected)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }
        
        /// <summary>
        /// Asserts that the value is equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> Be(DateTime? expected, string because = "", params object[] reasonArgs)
        {
            return Be(expected.HasValue ? expected.Value.ToDateTimeOffset() : (DateTimeOffset?)null, because, reasonArgs);
        }
    }
}