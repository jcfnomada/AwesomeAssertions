﻿using System;
using AwesomeAssertions.Common;
using Xunit;
using Xunit.Sdk;

namespace AwesomeAssertions.Specs.Types;

/// <content>
/// The [Not]HaveProperty specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class HaveProperty
    {
        [Fact]
        public void When_asserting_a_type_has_a_property_of_expected_type_and_name_which_it_does_then_it_succeeds()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act / Assert
            type.Should()
                .HaveProperty(typeof(string), "PrivateWriteProtectedReadProperty")
                .Which.Should()
                .BeWritable(CSharpAccessModifier.Private)
                .And.BeReadable(CSharpAccessModifier.Protected);
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_of_expected_name_which_it_does_then_it_succeeds()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act / Assert
            type.Should()
                .HaveProperty("PrivateWriteProtectedReadProperty")
                .Which.Should()
                .BeWritable(CSharpAccessModifier.Private)
                .And.BeReadable(CSharpAccessModifier.Protected);
        }

        [Fact]
        public void The_name_of_the_property_is_passed_to_the_chained_assertion()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () => type
                .Should().HaveProperty(typeof(string), "PrivateWriteProtectedReadProperty")
                .Which.Should().NotBeWritable();

            // Assert
            act.Should().Throw<XunitException>("Expected property PrivateWriteProtectedReadProperty not to have a setter.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_which_it_does_not_it_fails()
        {
            // Arrange
            Type type = typeof(ClassWithNoMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty(typeof(string), "PublicProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected AwesomeAssertions.*ClassWithNoMembers to have a property PublicProperty of type String because we want to test the failure message, but it does not.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_with_expected_name_which_it_does_not_it_fails()
        {
            // Arrange
            Type type = typeof(ClassWithNoMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty("PublicProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected AwesomeAssertions.*ClassWithNoMembers to have a property PublicProperty because we want to test the failure message, but it does not.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_which_it_has_with_a_different_type_it_fails()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveProperty(typeof(int), "PrivateWriteProtectedReadProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected property PrivateWriteProtectedReadProperty " +
                    "to be of type int because we want to test the failure message, but it is not.");
        }

        [Fact]
        public void When_subject_is_null_have_property_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveProperty(typeof(string), "PublicProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Cannot determine if a type has a property named PublicProperty if the type is <null>.");
        }

        [Fact]
        public void When_subject_is_null_have_property_by_name_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveProperty("PublicProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Cannot determine if a type has a property named PublicProperty if the type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_of_null_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty((Type)null, "PublicProperty");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("propertyType");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_with_valid_type_and_a_null_name_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty(typeof(string), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_with_valid_type_and_an_empty_name_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty(typeof(string), string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_with_a_null_name_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_with_an_empty_name_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }
    }

    public class HavePropertyOfT
    {
        [Fact]
        public void When_asserting_a_type_has_a_propertyOfT_which_it_does_then_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act / Assert
            type.Should()
                .HaveProperty<string>("PrivateWriteProtectedReadProperty")
                .Which.Should()
                .BeWritable(CSharpAccessModifier.Private)
                .And.BeReadable(CSharpAccessModifier.Protected);
        }

        [Fact]
        public void When_asserting_a_type_has_a_propertyOfT_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty<string>(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_a_propertyOfT_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty<string>(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }
    }

    public class NotHaveProperty
    {
        [Fact]
        public void When_asserting_a_type_does_not_have_a_property_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithoutMembers);

            // Act / Assert
            type.Should().NotHaveProperty("Property");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_property_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveProperty("PrivateWriteProtectedReadProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect AwesomeAssertions.*.ClassWithMembers to have a property PrivateWriteProtectedReadProperty because we want to test the failure message, but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_property_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveProperty("PublicProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Cannot determine if a type has an unexpected property named PublicProperty if the type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_property_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveProperty(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_property_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveProperty(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }
    }
}
