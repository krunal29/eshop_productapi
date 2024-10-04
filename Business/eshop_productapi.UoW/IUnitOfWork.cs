using eshop_productapi.Interfaces.Repositories;
using System;

namespace eshop_productapi.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }

IProductImageRepository ProductImageRepository { get; }


    }
}