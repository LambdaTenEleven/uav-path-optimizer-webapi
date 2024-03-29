﻿namespace UavPathOptimization.Domain.Repositories;

public interface IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}