using ErrorOr;
using FluentAssertions;
using NUnit.Framework;
using UavPathOptimization.Application.UseCases.UavModels.Commands.CreateUavModel;
using UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModel;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.IntegrationTests.UavModels.Queries;

using static Testing;

public class GetUavModelQueryTests : BaseTestFixture
{
    [Test]
    public async Task GetUavModelQuery_Should_Return_Guid()
    {
        // Arrange
        var creationResult = await SendAsync(
            new CreateUavModelCommand("UAV Model", 10.0, TimeSpan.FromHours(1))
        );
        var uavId = creationResult.Value;

        var query = new GetUavModelQuery(uavId);

        // Act
        var result = await SendAsync(query);
        var confirmedResult = await FindAsync<UavModel>(uavId);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.Id.Should().Be(uavId);

        confirmedResult.Should().NotBeNull();
        confirmedResult!.Id.Should().Be(result.Value.Id);
        confirmedResult.Name.Should().Be(result.Value.Name);
        confirmedResult.MaxSpeed.Should().Be(result.Value.MaxSpeed);
        confirmedResult.MaxFlightTime.Should().Be(result.Value.MaxFlightTime);
    }

    [Test]
    public async Task GetUavModelQuery_Should_Return_Error_When_UavModel_Not_Found()
    {
        // Arrange
        var uavId = Guid.NewGuid();

        var query = new GetUavModelQuery(uavId);

        // Act
        var result = await SendAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }
}