using rest_api_base.Models;

namespace rest_api_base.Services;

public sealed class PizzaService : IPizzaService
{
    private readonly object sync = new();
    private readonly List<Pizza> pizzas =
    [
        new Pizza(1, "Classic Italian", false),
        new Pizza(2, "Veggie", true),
    ];

    private int nextId = 3;

    public IReadOnlyList<Pizza> GetAll()
    {
        lock (sync)
        {
            return pizzas.ToArray();
        }
    }

    public Pizza? Get(int id)
    {
        lock (sync)
        {
            return pizzas.FirstOrDefault(pizza => pizza.Id == id);
        }
    }

    public Pizza Add(PizzaCreateRequest request)
    {
        lock (sync)
        {
            var pizza = new Pizza(nextId++, request.Name, request.IsGlutenFree);
            pizzas.Add(pizza);
            return pizza;
        }
    }

    public bool Update(int id, PizzaUpdateRequest request)
    {
        lock (sync)
        {
            var index = pizzas.FindIndex(pizza => pizza.Id == id);
            if (index < 0)
            {
                return false;
            }

            pizzas[index] = new Pizza(id, request.Name, request.IsGlutenFree);
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (sync)
        {
            var index = pizzas.FindIndex(pizza => pizza.Id == id);
            if (index < 0)
            {
                return false;
            }

            pizzas.RemoveAt(index);
            return true;
        }
    }
}
