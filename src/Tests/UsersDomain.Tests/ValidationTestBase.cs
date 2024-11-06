using BuildingBlocks.CQRS;
using FluentAssertions;
using FluentValidation;

namespace UsersApplication.Tests;

public class ValidationTestBase<TCommand, TResponse, TValidation>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
    where TValidation : AbstractValidator<TCommand>
{
    private readonly TValidation _validator;

    protected ValidationTestBase(TValidation validator)
    {
        _validator = validator;
    }

    protected void ShouldBeValid(TCommand command)
    {
        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    protected void ShouldHaveSingleError(
        TCommand command,
        string expectedCode)
    {
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();

        result.Errors.Count.Should().Be(1);

        result.Errors.First().ErrorCode.Should().Be(expectedCode);
    }

    protected void ShouldHaveSingleError(
        TCommand command,
        string expectedCode,
        string expectedMessage)
    {
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();

        result.Errors.Count.Should().Be(1);

        result.Errors.First().ErrorCode.Should().Be(expectedCode);
        result.Errors.First().ErrorMessage.Should().Be(expectedMessage);
    }

    protected void ShouldHaveExpectedErrors(
        TCommand command,
        params KeyValuePair<string, string>[] expectedErrors)
    {
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(expectedErrors.Length);

        foreach (var error in expectedErrors)
        {
            result.Errors
                .Count(validation => validation.ErrorCode == error.Key && validation.ErrorMessage == error.Value)
                .Should()
                .Be(1);
        }
    }

    protected void ShouldHaveExpectedErrors(
        TCommand command,
        params string[] expectedErrors)
    {
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(expectedErrors.Length);

        foreach (var error in expectedErrors)
        {
            result.Errors
                .Count(validation => validation.ErrorCode == error)
                .Should()
                .Be(1);
        }
    }
}