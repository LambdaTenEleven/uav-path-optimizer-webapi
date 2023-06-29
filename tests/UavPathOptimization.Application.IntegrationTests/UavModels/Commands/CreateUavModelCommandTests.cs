using ErrorOr;
using FluentAssertions;
using NUnit.Framework;
using UavPathOptimization.Application.UseCases.UavModels.Commands.CreateUavModel;
using UavPathOptimization.Domain.Common.Errors;

namespace UavPathOptimization.Application.IntegrationTests.UavModels.Commands;

using static Testing;

public class UavModelCommandsTests : BaseTestFixture
{
    [Test]
    public async Task CreateUavModelCommand_Should_Return_UavModelId()
    {
        // Arrange
        var command = new CreateUavModelCommand("UAV Model", 10.0, TimeSpan.FromHours(1));

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeFalse();
        response.Value.Should().NotBe(Guid.Empty);
    }

    [Test]
    public async Task CreateUavModelCommand_Should_Return_Error_When_Name_Is_Empty()
    {
        // Arrange
        var command = new CreateUavModelCommand("", 10.0, TimeSpan.FromHours(1));

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.Validation);
        response.FirstError.Code.Should().Be("Name");
    }

    [Test]
    public async Task CreateUavModelCommand_Should_Return_Error_When_Name_Is_Null()
    {
        // Arrange
        var command = new CreateUavModelCommand(null!, 10.0, TimeSpan.FromHours(1));

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.Validation);
        response.FirstError.Code.Should().Be("Name");
    }

    [Test]
    public async Task CreateUavModelCommand_Should_Return_Error_When_Name_Is_WhiteSpace()
    {
        // Arrange
        var command = new CreateUavModelCommand(" ", 10.0, TimeSpan.FromHours(1));

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.Validation);
        response.FirstError.Code.Should().Be("Name");
    }

    [Test]
    public async Task CreateUavModelCommand_Should_Return_Error_When_Name_Already_Exists()
    {
        // Arrange
        var command = new CreateUavModelCommand("UAV Model", 10.0, TimeSpan.FromHours(1));
        await SendAsync(command);

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.Conflict);
    }

    [Test]
    public async Task CreateUavModelCommand_Should_Return_Error_When_MaxSpeed_Is_Less_Than_Zero()
    {
        // Arrange
        var command = new CreateUavModelCommand("UAV Model", -10.0, TimeSpan.FromHours(1));

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.Validation);
        response.FirstError.Code.Should().Be("MaxSpeed");
    }

    [Test]
    public async Task CreateUavModelCommand_Should_Return_Error_When_MaxSpeed_Is_Zero()
    {
        // Arrange
        var command = new CreateUavModelCommand("UAV Model", 0.0, TimeSpan.FromHours(1));

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.Validation);
        response.FirstError.Code.Should().Be("MaxSpeed");
    }

    [Test]
    public async Task CreateUavModelCommand_Should_Return_Error_When_MaxSpeed_Is_NaN()
    {
        // Arrange
        var command = new CreateUavModelCommand("UAV Model", double.NaN, TimeSpan.FromHours(1));

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.Validation);
        response.FirstError.Code.Should().Be("MaxSpeed");
    }

    [Test]
    public async Task CreateUavModelCommand_Should_Return_Error_When_MaxFlightTime_Is_Less_Than_Zero()
    {
        // Arrange
        var command = new CreateUavModelCommand("UAV Model", 10.0, TimeSpan.FromHours(-1));

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.Validation);
        response.FirstError.Code.Should().Be("MaxFlightTime");
    }

    [Test]
    public async Task CreateUavModelCommand_Should_Return_Error_When_MaxFlightTime_Is_Zero()
    {
        // Arrange
        var command = new CreateUavModelCommand("UAV Model", 10.0, TimeSpan.Zero);

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.Validation);
        response.FirstError.Code.Should().Be("MaxFlightTime");
    }
}