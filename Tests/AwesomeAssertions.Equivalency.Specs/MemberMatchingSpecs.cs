using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Xunit;
using Xunit.Sdk;

namespace AwesomeAssertions.Equivalency.Specs;

public class MemberMatchingSpecs
{
    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_excluding_missing_members_both_fields_and_properties_should_be_ignored()
    {
        // Arrange
        var class1 = new ClassWithSomeFieldsAndProperties
        {
            Field1 = "Lorem",
            Field2 = "ipsum",
            Field3 = "dolor",
            Property1 = "sit",
            Property2 = "amet",
            Property3 = "consectetur"
        };

        var class2 = new { Field1 = "Lorem" };

        // Act / Assert
        class1.Should().BeEquivalentTo(class2, opts => opts.ExcludingMissingMembers());
    }

    [Fact]
    public void When_a_property_shared_by_anonymous_types_doesnt_match_it_should_throw()
    {
        // Arrange
        var subject = new { Age = 36 };

        var other = new { Age = 37 };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(other, options => options.ExcludingMissingMembers());

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void Nested_properties_can_be_mapped_using_a_nested_expression()
    {
        // Arrange
        var subject = new ParentOfSubjectWithProperty1([new SubjectWithProperty1 { Property1 = "Hello" }]);

        var expectation = new ParentOfExpectationWithProperty2(
        [
            new ExpectationWithProperty2 { Property2 = "Hello" }
        ]);

        // Act / Assert
        subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping<ParentOfSubjectWithProperty1>(
                    e => e.Children[0].Property2,
                    s => s.Children[0].Property1));
    }

    [Fact]
    public void Nested_properties_can_be_mapped_using_a_nested_type_and_property_names()
    {
        // Arrange
        var subject = new ParentOfSubjectWithProperty1([new SubjectWithProperty1 { Property1 = "Hello" }]);

        var expectation = new ParentOfExpectationWithProperty2(
        [
            new ExpectationWithProperty2 { Property2 = "Hello" }
        ]);

        // Act / Assert
        subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping<ExpectationWithProperty2, SubjectWithProperty1>("Property2", "Property1"));
    }

    [Fact]
    public void Nested_explicitly_implemented_properties_can_be_mapped_using_a_nested_type_and_property_names()
    {
        // Arrange
        var subject =
            new ParentOfSubjectWithExplicitlyImplementedProperty(new[] { new SubjectWithExplicitImplementedProperty() });

        ((IProperty)subject.Children[0]).Property = "Hello";

        var expectation = new ParentOfExpectationWithProperty2(
        [
            new ExpectationWithProperty2 { Property2 = "Hello" }
        ]);

        // Act / Assert
        subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping<ExpectationWithProperty2, SubjectWithExplicitImplementedProperty>("Property2", "Property"));
    }

    [Fact]
    public void Nested_fields_can_be_mapped_using_a_nested_type_and_field_names()
    {
        // Arrange
        var subject = new ClassWithSomeFieldsAndProperties { Field1 = "John", Field2 = "Mary" };

        var expectation = new ClassWithSomeFieldsAndProperties { Field1 = "Mary", Field2 = "John" };

        // Act / Assert
        subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping<ClassWithSomeFieldsAndProperties, ClassWithSomeFieldsAndProperties>("Field1", "Field2")
                .WithMapping<ClassWithSomeFieldsAndProperties, ClassWithSomeFieldsAndProperties>("Field2", "Field1"));
    }

    [Fact]
    public void Nested_properties_can_be_mapped_using_a_nested_type_and_a_property_expression()
    {
        // Arrange
        var subject = new ParentOfSubjectWithProperty1([new SubjectWithProperty1 { Property1 = "Hello" }]);

        var expectation = new ParentOfExpectationWithProperty2(
        [
            new ExpectationWithProperty2 { Property2 = "Hello" }
        ]);

        // Act / Assert
        subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping<ExpectationWithProperty2, SubjectWithProperty1>(
                    e => e.Property2, s => s.Property1));
    }

    [Fact]
    public void Nested_properties_on_a_collection_can_be_mapped_using_a_dotted_path()
    {
        // Arrange
        var subject = new { Parent = new[] { new SubjectWithProperty1 { Property1 = "Hello" } } };

        var expectation = new { Parent = new[] { new ExpectationWithProperty2 { Property2 = "Hello" } } };

        // Act / Assert
        subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping("Parent[].Property2", "Parent[].Property1"));
    }

    [Fact]
    public void Properties_can_be_mapped_by_name()
    {
        // Arrange
        var subject = new SubjectWithProperty1 { Property1 = "Hello" };

        var expectation = new ExpectationWithProperty2 { Property2 = "Hello" };

        // Act / Assert
        subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping("Property2", "Property1"));
    }

    [Fact]
    public void Properties_can_be_mapped_by_name_to_an_explicitly_implemented_property()
    {
        // Arrange
        var subject = new SubjectWithExplicitImplementedProperty();

        ((IProperty)subject).Property = "Hello";

        var expectation = new ExpectationWithProperty2
        {
            Property2 = "Hello"
        };

        // Act / Assert
        subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping("Property2", "Property"));
    }

    [Fact]
    public void Fields_can_be_mapped_by_name()
    {
        // Arrange
        var subject = new ClassWithSomeFieldsAndProperties { Field1 = "Hello", Field2 = "John" };

        var expectation = new ClassWithSomeFieldsAndProperties { Field1 = "John", Field2 = "Hello" };

        // Act / Assert
        subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping("Field2", "Field1")
                .WithMapping("Field1", "Field2"));
    }

    [Fact]
    public void Fields_can_be_mapped_to_a_property_by_name()
    {
        // Arrange
        var subject = new ClassWithSomeFieldsAndProperties { Property1 = "John" };

        var expectation = new ClassWithSomeFieldsAndProperties { Field1 = "John", };

        // Act / Assert
        subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping("Field1", "Property1")
                .Including(e => e.Field1));
    }

    [Fact]
    public void Properties_can_be_mapped_to_a_field_by_expression()
    {
        // Arrange
        var subject = new ClassWithSomeFieldsAndProperties { Field1 = "John", };

        var expectation = new ClassWithSomeFieldsAndProperties { Property1 = "John" };

        // Act / Assert
        subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping<ClassWithSomeFieldsAndProperties>(e => e.Property1, s => s.Field1)
                .Including(e => e.Property1));
    }

    [Fact]
    public void Properties_can_be_mapped_to_inherited_properties()
    {
        // Arrange
        var subject = new Derived { BaseProperty = "Hello World" };

        var expectation = new { AnotherProperty = "Hello World" };

        // Act / Assert
        subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping<Derived>(e => e.AnotherProperty, s => s.BaseProperty));
    }

    [Fact]
    public void A_failed_assertion_reports_the_subjects_mapped_property()
    {
        // Arrange
        var subject = new SubjectWithProperty1 { Property1 = "Hello" };

        var expectation = new ExpectationWithProperty2 { Property2 = "Hello2" };

        // Act
        Action act = () => subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping<SubjectWithProperty1>(e => e.Property2, e => e.Property1));

        // Assert
        act.Should()
            .Throw<XunitException>()
            .WithMessage("Expected property subject.Property1 to be*Hello*");
    }

    [Fact]
    public void An_empty_expectation_member_path_is_not_allowed()
    {
        var subject = new SubjectWithProperty1();
        var expectation = new ExpectationWithProperty2();

        // Act
        Action act = () => subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping("", "Parent[0].Property1"));

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*member path*");
    }

    [Fact]
    public void An_empty_subject_member_path_is_not_allowed()
    {
        var subject = new SubjectWithProperty1();
        var expectation = new ExpectationWithProperty2();

        // Act
        Action act = () => subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping("Parent[0].Property1", ""));

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*member path*");
    }

    [Fact]
    public void Null_as_the_expectation_member_path_is_not_allowed()
    {
        var subject = new SubjectWithProperty1();
        var expectation = new ExpectationWithProperty2();

        // Act
        Action act = () => subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping(null, "Parent[0].Property1"));

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*member path*");
    }

    [Fact]
    public void Null_as_the_subject_member_path_is_not_allowed()
    {
        var subject = new SubjectWithProperty1();
        var expectation = new ExpectationWithProperty2();

        // Act
        Action act = () => subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping("Parent[0].Property1", null));

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*member path*");
    }

    [Fact]
    public void Subject_and_expectation_member_paths_must_have_the_same_parent()
    {
        var subject = new SubjectWithProperty1();
        var expectation = new ExpectationWithProperty2();

        // Act
        Action act = () => subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping("Parent[].Property1", "OtherParent[].Property2"));

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*parent*");
    }

    [Fact]
    public void Numeric_indexes_in_the_path_are_not_allowed()
    {
        var subject = new { Parent = new[] { new SubjectWithProperty1 { Property1 = "Hello" } } };

        var expectation = new { Parent = new[] { new ExpectationWithProperty2 { Property2 = "Hello" } } };

        // Act
        Action act = () => subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping("Parent[0].Property2", "Parent[0].Property1"));

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*without specific index*");
    }

    [Fact]
    public void Mapping_to_a_non_existing_subject_member_is_not_allowed()
    {
        // Arrange
        var subject = new SubjectWithProperty1 { Property1 = "Hello" };

        var expectation = new ExpectationWithProperty2 { Property2 = "Hello" };

        // Act
        Action act = () => subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping("Property2", "NonExistingProperty"));

        // Assert
        act.Should()
            .Throw<MissingMemberException>()
            .WithMessage("*not have member NonExistingProperty*");
    }

    [Fact]
    public void A_null_subject_should_result_in_a_normal_assertion_failure()
    {
        // Arrange
        SubjectWithProperty1 subject = null;

        ExpectationWithProperty2 expectation = new() { Property2 = "Hello" };

        // Act
        Action act = () => subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping("Property2", "Property1"));

        // Assert
        act.Should()
            .Throw<XunitException>()
            .WithMessage("*Expected*ExpectationWithProperty2*found <null>*");
    }

    [Fact]
    public void Nested_types_and_dotted_expectation_member_paths_cannot_be_combined()
    {
        // Arrange
        var subject = new ParentOfSubjectWithProperty1([new SubjectWithProperty1 { Property1 = "Hello" }]);

        var expectation = new ParentOfExpectationWithProperty2(
        [
            new ExpectationWithProperty2 { Property2 = "Hello" }
        ]);

        // Act
        Action act = () => subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping<ExpectationWithProperty2, SubjectWithProperty1>("Parent.Property2", "Property1"));

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*cannot be a nested path*");
    }

    [Fact]
    public void Nested_types_and_dotted_subject_member_paths_cannot_be_combined()
    {
        // Arrange
        var subject = new ParentOfSubjectWithProperty1([new SubjectWithProperty1 { Property1 = "Hello" }]);

        var expectation = new ParentOfExpectationWithProperty2(
        [
            new ExpectationWithProperty2 { Property2 = "Hello" }
        ]);

        // Act
        Action act = () => subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping<ExpectationWithProperty2, SubjectWithProperty1>("Property2", "Parent.Property1"));

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*cannot be a nested path*");
    }

    [Fact]
    public void The_member_name_on_a_nested_type_mapping_must_be_a_valid_member()
    {
        // Arrange
        var subject = new ParentOfSubjectWithProperty1([new SubjectWithProperty1 { Property1 = "Hello" }]);

        var expectation = new ParentOfExpectationWithProperty2(
        [
            new ExpectationWithProperty2 { Property2 = "Hello" }
        ]);

        // Act
        Action act = () => subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .WithMapping<ExpectationWithProperty2, SubjectWithProperty1>("Property2", "NonExistingProperty"));

        // Assert
        act.Should()
            .Throw<MissingMemberException>()
            .WithMessage("*does not have member NonExistingProperty*");
    }

    [Fact]
    public void Exclusion_of_missing_members_works_with_mapping()
    {
        // Arrange
        var subject = new
        {
            Property1 = 1
        };

        var expectation = new
        {
            Property2 = 2,
            Ignore = 3
        };

        // Act / Assert
        subject.Should()
            .NotBeEquivalentTo(expectation, opt => opt
                .WithMapping("Property2", "Property1")
                .ExcludingMissingMembers()
            );
    }

    [Fact]
    public void Mapping_works_with_exclusion_of_missing_members()
    {
        // Arrange
        var subject = new
        {
            Property1 = 1
        };

        var expectation = new
        {
            Property2 = 2,
            Ignore = 3
        };

        // Act / Assert
        subject.Should()
            .NotBeEquivalentTo(expectation, opt => opt
                .ExcludingMissingMembers()
                .WithMapping("Property2", "Property1")
            );
    }

    [Fact]
    public void Can_map_members_of_a_root_collection()
    {
        // Arrange
        var entity = new Entity
        {
            EntityId = 1,
            Name = "Test"
        };

        var dto = new EntityDto
        {
            Id = 1,
            Name = "Test"
        };

        Entity[] entityCol = [entity];
        EntityDto[] dtoCol = [dto];

        // Act / Assert
        dtoCol.Should().BeEquivalentTo(entityCol, c =>
            c.WithMapping<EntityDto>(s => s.EntityId, d => d.Id));
    }

    [Fact]
    public void Can_explicitly_include_a_property_on_a_mapped_type()
    {
        // Arrange
        var expectation = new CustomerWithPropertiesDto
        {
            AddressInformation = new CustomerWithPropertiesDto.ResidenceDto
            {
                Address = "123 Main St",
                IsValidated = true,
            },
        };

        var subject = new CustomerWithProperty
        {
            Address = new CustomerWithProperty.Residence
            {
                Address = "123 Main St",
            },
        };

        // Act / Assert
        subject.Should().BeEquivalentTo(expectation, o => o
            .Including(r => r.AddressInformation.Address)
            .WithMapping<CustomerWithPropertiesDto, CustomerWithProperty>(s => s.AddressInformation, d => d.Address));
    }

    [Fact]
    public void Can_exclude_a_property_on_a_mapped_type()
    {
        // Arrange
        var expectation = new CustomerWithPropertiesDto
        {
            AddressInformation = new CustomerWithPropertiesDto.ResidenceDto
            {
                Address = "123 Main St",
                IsValidated = true,
            },
        };

        var subject = new CustomerWithProperty
        {
            Address = new CustomerWithProperty.Residence
            {
                Address = "123 Main St",
            },
        };

        // Act / Assert
        subject.Should().BeEquivalentTo(expectation, o => o
            .Excluding(r => r.AddressInformation.IsValidated)
            .WithMapping<CustomerWithPropertiesDto, CustomerWithProperty>(s => s.AddressInformation, d => d.Address));
    }

    [Fact]
    public void Can_explicitly_include_a_field_on_a_mapped_type()
    {
        // Arrange
        var expectation = new CustomerWithFieldDto
        {
            AddressInformation = new CustomerWithFieldDto.ResidenceDto
            {
                Address = "123 Main St",
                IsValidated = true,
            },
        };

        var subject = new CustomerWithField
        {
            Address = new CustomerWithField.Residence
            {
                Address = "123 Main St",
            },
        };

        // Act / Assert
        subject.Should().BeEquivalentTo(expectation, o => o
            .Including(r => r.AddressInformation.Address)
            .WithMapping<CustomerWithFieldDto, CustomerWithField>(s => s.AddressInformation, d => d.Address));
    }

    [Fact]
    public void Can_exclude_a_field_on_a_mapped_type()
    {
        // Arrange
        var expectation = new CustomerWithFieldDto
        {
            AddressInformation = new CustomerWithFieldDto.ResidenceDto
            {
                Address = "123 Main St",
                IsValidated = true,
            },
        };

        var subject = new CustomerWithField
        {
            Address = new CustomerWithField.Residence
            {
                Address = "123 Main St",
            },
        };

        // Act / Assert
        subject.Should().BeEquivalentTo(expectation, o => o
            .Excluding(r => r.AddressInformation.IsValidated)
            .WithMapping<CustomerWithFieldDto, CustomerWithField>(s => s.AddressInformation, d => d.Address));
    }

    private class CustomerWithProperty
    {
        public Residence Address { get; set; }

        public class Residence
        {
            [UsedImplicitly]
            public string Address { get; set; }
        }
    }

    private class CustomerWithPropertiesDto
    {
        public ResidenceDto AddressInformation { get; set; }

        public class ResidenceDto
        {
            public string Address { get; set; }

            public bool IsValidated { get; set; }
        }
    }

    private class CustomerWithField
    {
        public Residence Address;

        public class Residence
        {
            [UsedImplicitly]
            public string Address;
        }
    }

    private class CustomerWithFieldDto
    {
        public ResidenceDto AddressInformation;

        public class ResidenceDto
        {
            public string Address;

            [UsedImplicitly]
            public bool IsValidated;
        }
    }

    private class Entity
    {
        public int EntityId { get; init; }

        [UsedImplicitly]
        public string Name { get; init; }
    }

    private class EntityDto
    {
        public int Id { get; init; }

        [UsedImplicitly]
        public string Name { get; init; }
    }

    internal class ParentOfExpectationWithProperty2
    {
        public ExpectationWithProperty2[] Children { get; }

        public ParentOfExpectationWithProperty2(ExpectationWithProperty2[] children)
        {
            Children = children;
        }
    }

    internal class ParentOfSubjectWithProperty1
    {
        public SubjectWithProperty1[] Children { get; }

        public ParentOfSubjectWithProperty1(SubjectWithProperty1[] children)
        {
            Children = children;
        }
    }

    internal class ParentOfSubjectWithExplicitlyImplementedProperty
    {
        public SubjectWithExplicitImplementedProperty[] Children { get; }

        public ParentOfSubjectWithExplicitlyImplementedProperty(SubjectWithExplicitImplementedProperty[] children)
        {
            Children = children;
        }
    }

    internal class SubjectWithProperty1
    {
        public string Property1 { get; set; }
    }

    internal class SubjectWithExplicitImplementedProperty : IProperty
    {
        string IProperty.Property { get; set; }
    }

    internal interface IProperty
    {
        string Property { get; set; }
    }

    internal class ExpectationWithProperty2
    {
        public string Property2 { get; set; }
    }

    internal class Base
    {
        public string BaseProperty { get; set; }
    }

    internal class Derived : Base
    {
        public string DerivedProperty { get; set; }
    }
}
