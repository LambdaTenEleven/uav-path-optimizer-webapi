﻿namespace UavPathOptimization.Domain.Entities.Base;

public class BaseEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
}