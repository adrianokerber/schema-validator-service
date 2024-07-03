﻿namespace SchemaValidator.HttpService.Shared;

/// <summary>
/// Interface for any service that requires DI.
/// </summary>
/// <typeparam name="T">The service class itself that is inheriting</typeparam>
public interface IService<T> { }