using eshop_productapi.Domain;
using eshop_productapi.Interfaces.Repositories;
using eshop_productapi.Repositories;
using System;

namespace eshop_productapi.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly eshop_productapiContext Context;

        public UnitOfWork(eshop_productapiContext context)
        {
            this.Context = context;
            ProductRepository = new ProductRepository(Context); 

ProductImageRepository = new ProductImageRepository(Context); 


        }


        public IProductRepository ProductRepository { get; }

public IProductImageRepository ProductImageRepository { get; }



        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                Context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}