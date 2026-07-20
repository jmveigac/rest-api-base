using rest_api_base.Models;

namespace rest_api_base.Services;

public interface IPizzaService
{
    IReadOnlyList<Pizza> GetAll();

    Pizza? Get(int id);

    Pizza Add(PizzaCreateRequest request);

    bool Update(int id, PizzaUpdateRequest request);

    bool Delete(int id);
}
