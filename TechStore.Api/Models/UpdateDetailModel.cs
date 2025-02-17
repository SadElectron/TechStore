﻿namespace TechStore.Api.Models;

public class UpdateDetailModel
{
    public Guid Id { get; set; }
    public required string PropValue { get; set; }

    public Guid PropertyId { get; set; }
    public Guid ProductId { get; set; }
}
