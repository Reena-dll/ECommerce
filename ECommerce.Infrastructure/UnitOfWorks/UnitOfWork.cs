using ECommerce.Infrastructure.Context;
using ECommerce.Infrastructure.Repositories;

namespace ECommerce.Infrastructure.UnitOfWorks
{
    public class UnitOfWork(ECommerceDBContext dbContext) : IUnitOfWork
    {
        public async Task<int> SaveAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        IRepository<T> IUnitOfWork.GetRepository<T>()
        {
            return new Repository<T>(dbContext);
        }
    }
}
