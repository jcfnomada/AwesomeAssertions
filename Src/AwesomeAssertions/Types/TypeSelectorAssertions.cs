using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using AwesomeAssertions.Common;
using AwesomeAssertions.Execution;

namespace AwesomeAssertions.Types;

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
/// <summary>
/// Contains a number of methods to assert that all <see cref="Type"/>s in a <see cref="TypeSelector"/>
/// meet certain expectations.
/// </summary>
[DebuggerNonUserCode]
public class TypeSelectorAssertions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeSelectorAssertions"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="types"/> is or contains <see langword="null"/>.</exception>
    public TypeSelectorAssertions(AssertionChain assertionChain, params Type[] types)
    {
        CurrentAssertionChain = assertionChain;
        Guard.ThrowIfArgumentIsNull(types);
        Guard.ThrowIfArgumentContainsNull(types);

        Subject = types;
    }

    /// <summary>
    /// Gets the object whose value is being asserted.
    /// </summary>
    public IEnumerable<Type> Subject { get; }

    /// <summary>
    /// Provides access to the <see cref="AssertionChain"/> that this assertion class was initialized with.
    /// </summary>
    public AssertionChain CurrentAssertionChain { get; }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is decorated with the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeSelectorAssertions> BeDecoratedWith<TAttribute>([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Type[] typesWithoutAttribute = Subject
            .Where(type => !type.IsDecoratedWith<TAttribute>())
            .ToArray();

        CurrentAssertionChain
            .ForCondition(typesWithoutAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected all types to be decorated with {0}{reason}, but the attribute was not found on the following types:
                {{GetDescriptionsFor(typesWithoutAttribute)}}.
                """,
                typeof(TAttribute));

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is decorated with an attribute of type <typeparamref name="TAttribute"/>
    /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
    /// </summary>
    /// <param name="isMatchingAttributePredicate">
    /// The predicate that the attribute must match.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="isMatchingAttributePredicate"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeSelectorAssertions> BeDecoratedWith<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        Type[] typesWithoutMatchingAttribute = Subject
            .Where(type => !type.IsDecoratedWith(isMatchingAttributePredicate))
            .ToArray();

        CurrentAssertionChain
            .ForCondition(typesWithoutMatchingAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected all types to be decorated with {0} that matches {1}{reason}, but no matching attribute was found on the following types:
                {{GetDescriptionsFor(typesWithoutMatchingAttribute)}}.
                """,
                typeof(TAttribute), isMatchingAttributePredicate);

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is decorated with, or inherits from a parent class, the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeSelectorAssertions> BeDecoratedWithOrInherit<TAttribute>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Type[] typesWithoutAttribute = Subject
            .Where(type => !type.IsDecoratedWithOrInherit<TAttribute>())
            .ToArray();

        CurrentAssertionChain
            .ForCondition(typesWithoutAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected all types to be decorated with or inherit {0}{reason}, but the attribute was not found on the following types:
                {{GetDescriptionsFor(typesWithoutAttribute)}}.
                """,
                typeof(TAttribute));

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is decorated with, or inherits from a parent class, an attribute of type <typeparamref name="TAttribute"/>
    /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
    /// </summary>
    /// <param name="isMatchingAttributePredicate">
    /// The predicate that the attribute must match.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="isMatchingAttributePredicate"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeSelectorAssertions> BeDecoratedWithOrInherit<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        Type[] typesWithoutMatchingAttribute = Subject
            .Where(type => !type.IsDecoratedWithOrInherit(isMatchingAttributePredicate))
            .ToArray();

        CurrentAssertionChain
            .ForCondition(typesWithoutMatchingAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected all types to be decorated with or inherit {0} that matches {1}{reason}, but no matching attribute was found on the following types:
                {{GetDescriptionsFor(typesWithoutMatchingAttribute)}}.
                """,
                typeof(TAttribute), isMatchingAttributePredicate);

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not decorated with the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeSelectorAssertions> NotBeDecoratedWith<TAttribute>([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Type[] typesWithAttribute = Subject
            .Where(type => type.IsDecoratedWith<TAttribute>())
            .ToArray();

        CurrentAssertionChain
            .ForCondition(typesWithAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected all types to not be decorated with {0}{reason}, but the attribute was found on the following types:
                {{GetDescriptionsFor(typesWithAttribute)}}.
                """,
                typeof(TAttribute));

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not decorated with an attribute of type <typeparamref name="TAttribute"/>
    /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
    /// </summary>
    /// <param name="isMatchingAttributePredicate">
    /// The predicate that the attribute must match.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="isMatchingAttributePredicate"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeSelectorAssertions> NotBeDecoratedWith<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        Type[] typesWithMatchingAttribute = Subject
            .Where(type => type.IsDecoratedWith(isMatchingAttributePredicate))
            .ToArray();

        CurrentAssertionChain
            .ForCondition(typesWithMatchingAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected all types to not be decorated with {0} that matches {1}{reason}, but a matching attribute was found on the following types:
                {{GetDescriptionsFor(typesWithMatchingAttribute)}}.
                """,
                typeof(TAttribute), isMatchingAttributePredicate);

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not decorated with and does not inherit from a parent class, the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeSelectorAssertions> NotBeDecoratedWithOrInherit<TAttribute>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Type[] typesWithAttribute = Subject
            .Where(type => type.IsDecoratedWithOrInherit<TAttribute>())
            .ToArray();

        CurrentAssertionChain
            .ForCondition(typesWithAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected all types to not be decorated with or inherit {0}{reason}, but the attribute was found on the following types:
                {{GetDescriptionsFor(typesWithAttribute)}}.
                """,
                typeof(TAttribute));

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not decorated with and does not inherit from a parent class,  an attribute of type <typeparamref name="TAttribute"/>
    /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
    /// </summary>
    /// <param name="isMatchingAttributePredicate">
    /// The predicate that the attribute must match.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="isMatchingAttributePredicate"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeSelectorAssertions> NotBeDecoratedWithOrInherit<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        Type[] typesWithMatchingAttribute = Subject
            .Where(type => type.IsDecoratedWithOrInherit(isMatchingAttributePredicate))
            .ToArray();

        CurrentAssertionChain
            .ForCondition(typesWithMatchingAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected all types to not be decorated with or inherit {0} that matches {1}{reason}, but a matching attribute was found on the following types:
                {{GetDescriptionsFor(typesWithMatchingAttribute)}}.
                """,
                typeof(TAttribute), isMatchingAttributePredicate);

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected types are sealed
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeSelectorAssertions> BeSealed([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        var notSealedTypes = Subject.Where(type => !type.IsCSharpSealed()).ToArray();

        CurrentAssertionChain.ForCondition(notSealedTypes.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected all types to be sealed{reason}, but the following types are not:
                {{GetDescriptionsFor(notSealedTypes)}}.
                """);

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the all <see cref="Type"/> are not sealed classes
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeSelectorAssertions> NotBeSealed([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        var sealedTypes = Subject.Where(type => type.IsCSharpSealed()).ToArray();

        CurrentAssertionChain.ForCondition(sealedTypes.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected all types not to be sealed{reason}, but the following types are:
                {{GetDescriptionsFor(sealedTypes)}}.
                """);

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is in the specified <paramref name="namespace"/>.
    /// </summary>
    /// <param name="namespace">
    /// The namespace that the type must be in.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeSelectorAssertions> BeInNamespace(string @namespace,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Type[] typesNotInNamespace = Subject
            .Where(t => t.Namespace != @namespace)
            .ToArray();

        CurrentAssertionChain
            .ForCondition(typesNotInNamespace.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected all types to be in namespace {0}{reason}, but the following types are in a different namespace:
                {{GetDescriptionsFor(typesNotInNamespace)}}.
                """,
                @namespace);

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not in the specified <paramref name="namespace"/>.
    /// </summary>
    /// <param name="namespace">
    /// The namespace that the type must not be in.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeSelectorAssertions> NotBeInNamespace(string @namespace,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Type[] typesInNamespace = Subject
            .Where(t => t.Namespace == @namespace)
            .ToArray();

        CurrentAssertionChain
            .ForCondition(typesInNamespace.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected no types to be in namespace {0}{reason}, but the following types are in the namespace:
                {{GetDescriptionsFor(typesInNamespace)}}.
                """,
                @namespace);

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the namespace of the current <see cref="Type"/> starts with the specified <paramref name="namespace"/>.
    /// </summary>
    /// <param name="namespace">
    /// The namespace that the namespace of the type must start with.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeSelectorAssertions> BeUnderNamespace(string @namespace,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Type[] typesNotUnderNamespace = Subject
            .Where(t => !t.IsUnderNamespace(@namespace))
            .ToArray();

        CurrentAssertionChain
            .ForCondition(typesNotUnderNamespace.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected the namespaces of all types to start with {0}{reason}, but the namespaces of the following types do not start with it:
                {{GetDescriptionsFor(typesNotUnderNamespace)}}.
                """,
                @namespace);

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the namespace of the current <see cref="Type"/>
    /// does not starts with the specified <paramref name="namespace"/>.
    /// </summary>
    /// <param name="namespace">
    /// The namespace that the namespace of the type must not start with.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeSelectorAssertions> NotBeUnderNamespace(string @namespace,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Type[] typesUnderNamespace = Subject
            .Where(t => t.IsUnderNamespace(@namespace))
            .ToArray();

        CurrentAssertionChain
            .ForCondition(typesUnderNamespace.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith($$"""
                Expected the namespaces of all types to not start with {0}{reason}, but the namespaces of the following types start with it:
                {{GetDescriptionsFor(typesUnderNamespace)}}.
                """,
                @namespace);

        return new AndConstraint<TypeSelectorAssertions>(this);
    }

    private static string GetDescriptionsFor(IEnumerable<Type> types)
    {
        IEnumerable<string> descriptions = types.Select(type => GetDescriptionFor(type));
        return string.Join(Environment.NewLine, descriptions);
    }

    private static string GetDescriptionFor(Type type)
    {
        return type.ToString();
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException(
            "Equals is not part of Awesome Assertions. Did you mean BeInNamespace() or BeDecoratedWith() instead?");
}
