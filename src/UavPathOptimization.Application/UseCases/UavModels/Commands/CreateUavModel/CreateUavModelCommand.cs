﻿using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.CreateUavModel;

public record CreateUavModelCommand(
    string Name,
    double MaxSpeed,
    TimeSpan MaxFlightTime
) : IRequest<ErrorOr<UavModel>>;