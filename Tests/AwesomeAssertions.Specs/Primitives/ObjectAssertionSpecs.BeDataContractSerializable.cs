﻿using System;
using System.Runtime.Serialization;
using AwesomeAssertions.Extensions;
using JetBrains.Annotations;
using Xunit;
using Xunit.Sdk;

namespace AwesomeAssertions.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class BeDataContractSerializable
    {
        [Fact]
        public void When_an_object_is_data_contract_serializable_it_should_succeed()
        {
            // Arrange
            var subject = new DataContractSerializableClass
            {
                Name = "John",
                Id = 1
            };

            // Act / Assert
            subject.Should().BeDataContractSerializable();
        }

        [Fact]
        public void When_an_object_is_not_data_contract_serializable_it_should_fail()
        {
            // Arrange
            var subject = new NonDataContractSerializableClass();

            // Act
            Action act = () => subject.Should().BeDataContractSerializable("we need to store it on {0}", "disk");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*we need to store it on disk*EnumMemberAttribute*");
        }

        [Fact]
        public void When_an_object_is_data_contract_serializable_but_doesnt_restore_all_properties_it_should_fail()
        {
            // Arrange
            var subject = new DataContractSerializableClassNotRestoringAllProperties
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            // Act
            Action act = () => subject.Should().BeDataContractSerializable();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to be serializable, but serialization failed with:*property subject.Name*to be*");
        }

        [Fact]
        public void When_a_data_contract_serializable_object_doesnt_restore_an_ignored_property_it_should_succeed()
        {
            // Arrange
            var subject = new DataContractSerializableClassNotRestoringAllProperties
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            // Act / Assert
            subject.Should()
                .BeDataContractSerializable<DataContractSerializableClassNotRestoringAllProperties>(
                    options => options.Excluding(x => x.Name));
        }

        [Fact]
        public void When_injecting_null_options_to_BeDataContractSerializable_it_should_throw()
        {
            // Arrange
            var subject = new DataContractSerializableClassNotRestoringAllProperties();

            // Act
            Action act = () => subject.Should()
                .BeDataContractSerializable<DataContractSerializableClassNotRestoringAllProperties>(
                    options: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("options");
        }
    }

    public class NonDataContractSerializableClass
    {
        public Color Color { get; set; }
    }

    public class DataContractSerializableClass
    {
        [UsedImplicitly]
        public string Name { get; set; }

        public int Id;
    }

    [DataContract]
    public class DataContractSerializableClassNotRestoringAllProperties
    {
        public string Name { get; set; }

        [DataMember]
        public DateTime BirthDay { get; set; }
    }

    public enum Color
    {
        Red = 1,
        Yellow = 2
    }
}
