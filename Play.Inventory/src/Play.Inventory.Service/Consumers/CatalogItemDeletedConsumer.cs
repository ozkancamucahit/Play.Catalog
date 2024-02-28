using Common.Lib.Repositories;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public sealed class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {

        private readonly IRepository<CatalogItem> repository;

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {

            var message = context.Message;

            var item = await repository.GetAsync(message.ItemId);

            if (item == null)
                return;

            else
            {
                await repository.RemoveAsync(message.ItemId);

            }

        }
    }
}




