using Basket.Domain;

namespace Basket.Application.Interfaces;

public interface IBasketRepository
{
 // Basket? Get(Guid id);
    void Save(Basket basket);
}
