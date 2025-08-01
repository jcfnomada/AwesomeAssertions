﻿using System;
using Xunit;
using Xunit.Sdk;

namespace AwesomeAssertions.Equivalency.Specs;

public class MemberConversionSpecs
{
    [Fact]
    public void When_two_objects_have_the_same_properties_with_convertable_values_it_should_succeed()
    {
        // Arrange
        var subject = new { Age = "37", Birthdate = "1973-09-20" };

        var other = new { Age = 37, Birthdate = new DateTime(1973, 9, 20) };

        // Act / Assert
        subject.Should().BeEquivalentTo(other, o => o.WithAutoConversion());
    }

    [Fact]
    public void When_a_string_is_declared_equivalent_to_an_int_representing_the_numerals_it_should_pass()
    {
        // Arrange
        var actual = new { Property = "32" };

        var expectation = new { Property = 32 };

        // Act / Assert
        actual.Should().BeEquivalentTo(expectation,
            options => options.WithAutoConversion());
    }

    [Fact]
    public void When_an_int_is_compared_equivalent_to_a_string_representing_the_number_it_should_pass()
    {
        // Arrange
        var subject = new { Property = 32 };
        var expectation = new { Property = "32" };

        // Act / Assert
        subject.Should().BeEquivalentTo(expectation, options => options.WithAutoConversion());
    }

    [Fact]
    public void Numbers_can_be_converted_to_enums()
    {
        // Arrange
        var expectation = new { Property = EnumFour.Three };
        var subject = new { Property = 3UL };

        // Act / Assert
        subject.Should().BeEquivalentTo(expectation, options => options.WithAutoConversion());
    }

    [Fact]
    public void Enums_are_not_converted_to_enums_of_different_type()
    {
        // Arrange
        var expectation = new { Property = EnumTwo.Two };
        var subject = new { Property = EnumThree.Two };

        // Act / Assert
        subject.Should().BeEquivalentTo(expectation, options => options.WithAutoConversion());
    }

    [Fact]
    public void Strings_are_not_converted_to_enums()
    {
        // Arrange
        var expectation = new { Property = EnumTwo.Two };
        var subject = new { Property = "Two" };

        // Act / Assert
        var act = () => subject.Should().BeEquivalentTo(expectation, options => options.WithAutoConversion());

        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void Numbers_that_are_out_of_range_cannot_be_converted_to_enums()
    {
        // Arrange
        var expectation = new { Property = EnumFour.Three };
        var subject = new { Property = 4 };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expectation, options => options.WithAutoConversion());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*subject*Property*EnumFour*");
    }

    [Fact]
    public void When_injecting_a_null_predicate_into_WithAutoConversionFor_it_should_throw()
    {
        // Arrange
        var subject = new object();

        var expectation = new object();

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expectation,
            options => options.WithAutoConversionFor(predicate: null));

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("predicate");
    }

    [Fact]
    public void When_only_a_single_property_is_and_can_be_converted_but_the_other_one_doesnt_match_it_should_throw()
    {
        // Arrange
        var subject = new { Age = 32, Birthdate = "1973-09-20" };

        var expectation = new { Age = "32", Birthdate = new DateTime(1973, 9, 20) };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expectation,
            options => options.WithAutoConversionFor(x => x.Path.Contains("Birthdate")));

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*Age*String*int*");
    }

    [Fact]
    public void When_only_a_single_property_is_converted_and_the_other_matches_it_should_succeed()
    {
        // Arrange
        var subject = new { Age = 32, Birthdate = "1973-09-20" };

        var expectation = new { Age = 32, Birthdate = new DateTime(1973, 9, 20) };

        // Act / Assert
        subject.Should().BeEquivalentTo(expectation, options => options
            .WithAutoConversionFor(x => x.Path.Contains("Birthdate")));
    }

    [Fact]
    public void When_injecting_a_null_predicate_into_WithoutAutoConversionFor_it_should_throw()
    {
        // Arrange
        var subject = new object();

        var expectation = new object();

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expectation,
            options => options.WithoutAutoConversionFor(predicate: null));

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("predicate");
    }

    [Fact]
    public void When_a_specific_mismatching_property_is_excluded_from_conversion_it_should_throw()
    {
        // Arrange
        var subject = new { Age = 32, Birthdate = "1973-09-20" };

        var expectation = new { Age = 32, Birthdate = new DateTime(1973, 9, 20) };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expectation, options => options
            .WithAutoConversion()
            .WithoutAutoConversionFor(x => x.Path.Contains("Birthdate")));

        // Assert
        act.Should().Throw<XunitException>().Which.Message
            .Should().Match("Expected*<1973-09-20>*\"1973-09-20\"*", "{0} field is of mismatched type",
                nameof(expectation.Birthdate))
            .And.Subject.Should().Match("*Try conversion of all members*", "conversion description should be present")
            .And.Subject.Should().NotMatch("*Try conversion of all members*Try conversion of all members*",
                "conversion description should not be duplicated");
    }

    [Fact]
    public void When_declaring_equivalent_a_convertable_object_that_is_equivalent_once_converted_it_should_pass()
    {
        // Arrange
        string str = "This is a test";
        CustomConvertible obj = new(str);

        // Act / Assert
        obj.Should().BeEquivalentTo(str, options => options.WithAutoConversion());
    }
}
