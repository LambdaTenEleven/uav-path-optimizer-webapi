﻿using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.Common.Persistence.Uav;

public sealed record UpdateUavModelFromDbCommand(UavModel UavModel) : IRequest<ErrorOr<Unit>>;