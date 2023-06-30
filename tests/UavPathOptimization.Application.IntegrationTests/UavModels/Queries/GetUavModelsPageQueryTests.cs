using ErrorOr;
using FluentAssertions;
using NUnit.Framework;
using UavPathOptimization.Application.UseCases.UavModels.Commands.CreateUavModel;
using UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModelsPage;
using UavPathOptimization.Domain.Common.Enums;

namespace UavPathOptimization.Application.IntegrationTests.UavModels.Queries;

using static Testing;

public class GetUavModelsPageQueryTests : BaseTestFixture
{
    [Test]
    public async Task GetUavModelsPageQuery_Should_Return_Page()
    {
        // Arrange
        for (var i = 0; i < 10; i++)
        {
            await SendAsync(
                new CreateUavModelCommand($"UAV Model {i}", 10.0, TimeSpan.FromHours(1))
            );
        }

        var query = new GetUavModelsPageQuery(1, 10);

        // Act
        var result = await SendAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.PageNumber.Should().Be(1);
        result.Value.PageSize.Should().Be(10);
        result.Value.TotalCount.Should().Be(10);
        result.Value.TotalPages.Should().Be(1);
    }

    [Test]
    public async Task GetUavModelsPageQuery_Should_Return_Two_Pages()
    {
        // Arrange
        for (var i = 0; i < 20; i++)
        {
            await SendAsync(
                new CreateUavModelCommand($"UAV Model {i}", 10.0, TimeSpan.FromHours(1))
            );
        }

        var query = new GetUavModelsPageQuery(1, 10);

        // Act
        var result = await SendAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.PageNumber.Should().Be(1);
        result.Value.PageSize.Should().Be(10);
        result.Value.TotalCount.Should().Be(20);
        result.Value.TotalPages.Should().Be(2);
        result.Value.HasNextPage.Should().BeTrue();
        result.Value.HasPreviousPage.Should().BeFalse();
    }

    [Test]
    public async Task GetUavModelsPageQuery_Should_Return_Page_With_Filter()
    {
        // Arrange
        for (var i = 0; i < 10; i++)
        {
            await SendAsync(
                new CreateUavModelCommand($"UAV Model {i}", 10.0, TimeSpan.FromHours(1))
            );
        }

        var query = new GetUavModelsPageQuery(1, 10, "UAV Model 1");

        // Act
        var result = await SendAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.PageNumber.Should().Be(1);
        result.Value.PageSize.Should().Be(10);
        result.Value.TotalCount.Should().Be(1);
        result.Value.TotalPages.Should().Be(1);
    }

    [Test]
    public async Task GetUavModelsPageQuery_Should_Be_Sorted_By_MaxSpeed_Descending()
    {
        // Arrange
        for (var i = 0; i < 10; i++)
        {
            await SendAsync(
                new CreateUavModelCommand($"UAV Model {i}", 10.0 + i, TimeSpan.FromHours(1))
            );
        }

        var query = new GetUavModelsPageQuery(1, 10, SortField: "MaxSpeed", SortDirection: SortDirection.Descending);

        // Act
        var result = await SendAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.PageNumber.Should().Be(1);
        result.Value.PageSize.Should().Be(10);
        result.Value.TotalCount.Should().Be(10);
        result.Value.TotalPages.Should().Be(1);
        result.Value.Items.Should().BeInDescendingOrder(x => x.MaxSpeed);
    }

    [Test]
    public async Task GetUavModelPageQuery_Should_Return_Empty_Page_When_PageNumber_Is_Too_High()
    {
        // Arrange
        for (var i = 0; i < 10; i++)
        {
            await SendAsync(
                new CreateUavModelCommand($"UAV Model {i}", 10.0, TimeSpan.FromHours(1))
            );
        }

        var query = new GetUavModelsPageQuery(2, 10);

        // Act
        var result = await SendAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.PageNumber.Should().Be(2);
        result.Value.PageSize.Should().Be(10);
        result.Value.TotalCount.Should().Be(10);
        result.Value.TotalPages.Should().Be(1);
        result.Value.Items.Should().BeEmpty();
    }

    [Test]
    public async Task GetUavModelPageQuery_Should_Return_Error_When_PageNumber_Is_Negative()
    {
        // Arrange
        for (var i = 0; i < 10; i++)
        {
            await SendAsync(
                new CreateUavModelCommand($"UAV Model {i}", 10.0, TimeSpan.FromHours(1))
            );
        }

        var query = new GetUavModelsPageQuery(-1, 10);

        // Act
        var result = await SendAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.Errors.First().Type.Should().Be(ErrorType.Validation);
        result.Errors.First().Code.Should().Be("PageNumber");
    }
}