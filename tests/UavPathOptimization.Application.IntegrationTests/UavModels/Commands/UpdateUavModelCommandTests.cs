using ErrorOr;
using FluentAssertions;
using NUnit.Framework;
using UavPathOptimization.Application.UseCases.UavModels.Commands.CreateUavModel;
using UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.IntegrationTests.UavModels.Commands;

using static Testing;

public class UpdateUavModelCommandTests : BaseTestFixture
{
    [Test]
    public async Task UpdateUavModelCommand_Should_Return_Unit()
    {
        // Arrange
        var creationResult = await SendAsync(
            new CreateUavModelCommand("UAV Model", 10.0, TimeSpan.FromHours(1))
        );
        var uavId = creationResult.Value;
        var newName = "New UAV Model Name";
        var newMaxSpeed = 20.0;
        var newMaxFlightTime = TimeSpan.FromHours(2);

        var command = new UpdateUavModelCommand(uavId, newName, newMaxSpeed, newMaxFlightTime);

        // Act
        var response = await SendAsync(command);
        var uavModel = await FindAsync<UavModel>(uavId);

        // Assert
        response.IsError.Should().BeFalse();
        uavModel.Should().NotBeNull();
        uavModel!.Name.Should().Be(newName);
        uavModel.MaxSpeed.Value.Should().Be(newMaxSpeed);
        uavModel.MaxFlightTime.Should().Be(newMaxFlightTime);
    }

    [Test]
    public async Task UpdateUavModelCommand_Should_Return_Error_When_UavModel_Not_Found()
    {
        // Arrange
        var uavId = Guid.NewGuid();
        var newName = "New UAV Model Name";
        var newMaxSpeed = 20.0;
        var newMaxFlightTime = TimeSpan.FromHours(2);

        var command = new UpdateUavModelCommand(uavId, newName, newMaxSpeed, newMaxFlightTime);

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Test]
    public async Task UpdateUavModelCommand_Should_Return_Error_When_UavModel_Name_Is_Empty()
    {
        // Arrange
        var creationResult = await SendAsync(
            new CreateUavModelCommand("UAV Model", 10.0, TimeSpan.FromHours(1))
        );
        var uavId = creationResult.Value;
        var newName = "";
        var newMaxSpeed = 20.0;
        var newMaxFlightTime = TimeSpan.FromHours(2);

        var command = new UpdateUavModelCommand(uavId, newName, newMaxSpeed, newMaxFlightTime);

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.Validation);
        response.FirstError.Code.Should().Be("Name");
    }

    [Test]
    public async Task UpdateUavModelCommand_Should_Return_Error_When_UavModel_Name_Already_Exists()
    {
        // Arrange
        var creationResult = await SendAsync(
            new CreateUavModelCommand("UAV Model", 10.0, TimeSpan.FromHours(1))
        );
        var uavId = creationResult.Value;
        var newName = "New UAV Model Name";
        var newMaxSpeed = 20.0;
        var newMaxFlightTime = TimeSpan.FromHours(2);
        await SendAsync(
            new CreateUavModelCommand(newName, 10.0, TimeSpan.FromHours(1))
        );

        var command = new UpdateUavModelCommand(uavId, newName, newMaxSpeed, newMaxFlightTime);

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.Conflict);
    }

    [Test]
    public async Task UpdateUavModelCommand_Should_Return_Error_When_UavModel_MaxSpeed_Is_Negative()
    {
        // Arrange
        var creationResult = await SendAsync(
            new CreateUavModelCommand("UAV Model", 10.0, TimeSpan.FromHours(1))
        );
        var uavId = creationResult.Value;
        var newName = "New UAV Model Name";
        var newMaxSpeed = -20.0;
        var newMaxFlightTime = TimeSpan.FromHours(2);

        var command = new UpdateUavModelCommand(uavId, newName, newMaxSpeed, newMaxFlightTime);

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.Validation);
        response.FirstError.Code.Should().Be("MaxSpeed");
    }

    [Test]
    public async Task UpdateUavModelCommand_Should_Return_Error_When_UavModel_MaxFlightTime_Is_Zero()
    {
        // Arrange
        var creationResult = await SendAsync(
            new CreateUavModelCommand("UAV Model", 10.0, TimeSpan.FromHours(1))
        );
        var uavId = creationResult.Value;
        var newName = "New UAV Model Name";
        var newMaxSpeed = 20.0;
        var newMaxFlightTime = TimeSpan.Zero;

        var command = new UpdateUavModelCommand(uavId, newName, newMaxSpeed, newMaxFlightTime);

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.Validation);
        response.FirstError.Code.Should().Be("MaxFlightTime");
    }
}