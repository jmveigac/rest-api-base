using Microsoft.AspNetCore.Mvc;
using rest_api_base.Models;
using rest_api_base.Services;

namespace rest_api_base.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class PizzaController(IPizzaService pizzaService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<Pizza>> GetAll() => Ok(pizzaService.GetAll());

    [HttpGet("{id:int}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = pizzaService.Get(id);
        return pizza is null ? NotFound() : Ok(pizza);
    }

    [HttpPost]
    public ActionResult<Pizza> Create(PizzaCreateRequest request)
    {
        var pizza = pizzaService.Add(request);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, PizzaUpdateRequest request) =>
        pizzaService.Update(id, request) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) => pizzaService.Delete(id) ? NoContent() : NotFound();
}
