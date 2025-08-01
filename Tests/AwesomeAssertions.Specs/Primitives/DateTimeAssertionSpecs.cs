﻿using System;
using AwesomeAssertions.Extensions;
using Xunit;

namespace AwesomeAssertions.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class ChainingConstraint
    {
        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            DateTime earlierDateTime = new(2016, 06, 03);
            DateTime? nullableDateTime = new DateTime(2016, 06, 04);

            // Act / Assert
            nullableDateTime.Should()
                .HaveValue()
                .And
                .BeAfter(earlierDateTime);
        }
    }

    public class Miscellaneous
    {
        [Fact]
        public void Should_throw_a_helpful_error_when_accidentally_using_equals()
        {
            // Arrange
            DateTime someDateTime = new(2022, 9, 25, 13, 38, 42, DateTimeKind.Utc);

            // Act
            Action action = () => someDateTime.Should().Equals(someDateTime);

            // Assert
            action.Should().Throw<NotSupportedException>()
                .WithMessage("Equals is not part of Awesome Assertions. Did you mean Be() instead?");
        }

        [Fact]
        public void Should_throw_a_helpful_error_when_accidentally_using_equals_with_a_range()
        {
            // Arrange
            DateTime someDateTime = new(2022, 9, 25, 13, 38, 42, DateTimeKind.Utc);

            // Act
            Action action = () => someDateTime.Should().BeLessThan(0.Seconds()).Equals(someDateTime);

            // Assert
            action.Should().Throw<NotSupportedException>()
                .WithMessage("Equals is not part of Awesome Assertions. Did you mean Before() or After() instead?");
        }
    }
}
