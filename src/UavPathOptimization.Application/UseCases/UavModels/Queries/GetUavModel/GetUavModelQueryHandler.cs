using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities.UavEntities;
using UavPathOptimization.Domain.Repositories;

namespace UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModel;

internal sealed class GetUavModelQueryHandler : IRequestHandler<GetUavModelQuery, ErrorOr<UavModel>>
{
    private readonly IUavModelRepository _repository;

    public GetUavModelQueryHandler(IUavModelRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<UavModel>> Handle(GetUavModelQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (result == null)
        {
            return Errors.UavModelErrors.UavModelNotFound;
        }

        return result;
    }
}