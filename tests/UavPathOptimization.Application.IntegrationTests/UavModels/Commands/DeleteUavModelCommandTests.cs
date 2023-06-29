using ErrorOr;
using FluentAssertions;
using NUnit.Framework;
using UavPathOptimization.Application.UseCases.UavModels.Commands.CreateUavModel;
using UavPathOptimization.Application.UseCases.UavModels.Commands.DeleteUavModel;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.IntegrationTests.UavModels.Commands;

using static Testing;

public class DeleteUavModelCommandTests : BaseTestFixture
{
    [Test]
    public async Task DeleteUavModelCommand_Should_Delete_Uav_Model()
    {
        // Arrange
        var creationResult = await SendAsync(
            new CreateUavModelCommand("UAV Model", 10.0, TimeSpan.FromHours(1))
        );
        var uavId = creationResult.Value;

        var command = new DeleteUavModelCommand(uavId);

        // Act
        var response = await SendAsync(command);
        var uavModel = await FindAsync<UavModel>(uavId);

        // Assert
        response.IsError.Should().BeFalse();
        uavModel.Should().BeNull();
    }

    [Test]
    public async Task DeleteUavModelCommand_Should_Return_Error_When_UavModel_Not_Found()
    {
        // Arrange
        var uavId = Guid.NewGuid();

        var command = new DeleteUavModelCommand(uavId);

        // Act
        var response = await SendAsync(command);

        // Assert
        response.IsError.Should().BeTrue();
        response.FirstError.Type.Should().Be(ErrorType.NotFound);
    }
}