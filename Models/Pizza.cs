using System.ComponentModel.DataAnnotations;

namespace rest_api_base.Models;

public sealed record Pizza(int Id, string Name, bool IsGlutenFree);

public sealed record PizzaCreateRequest(
    [property: Required, MinLength(1)] string Name,
    bool IsGlutenFree
);

public sealed record PizzaUpdateRequest(
    [property: Required, MinLength(1)] string Name,
    bool IsGlutenFree
);
