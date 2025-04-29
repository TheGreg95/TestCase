using Basket.Application.Interfaces;
using Basket.Domain;

namespace Basket.Application;

public class BasketService
{
    private readonly IBasketRepository _repo;
    private readonly IProductClient _products;

    public BasketService(IBasketRepository repo, IProductClient products)
    {
        _repo = repo;
        _products = products;
    }

    public Guid CreateBasket()
    {
        var b = new Basket();
        _repo.Save(b);
        return b.Id;
    }

    public Basket? GetBasket(Guid id) => _repo.Get(id);

    public async Task<bool> AddItemAsync(Guid basketId, int productId, int qty, CancellationToken ct = default)
    {
        var product = await _products.GetByIdAsync(productId, ct);
        if (product is null || product.Rank > 100) return false;

        var basket = _repo.Get(basketId);
        if (basket is null) return false;

        basket.Add(productId, qty, product.Price);
        _repo.Save(basket);
        return true;
    }

}
