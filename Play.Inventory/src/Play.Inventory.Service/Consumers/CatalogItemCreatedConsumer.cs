﻿using Common.Lib.Repositories;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public sealed class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {

        private readonly IRepository<CatalogItem> repository;

        public CatalogItemCreatedConsumer(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {

            var message = context.Message;

            var item = await repository.GetAsync(message.ItemId);

            if (item != null)
                return;

            item = new CatalogItem
            {
                Id = message.ItemId,
                Description = message.Description,
                Name = message.Name,
            };

            await repository.CreateAsync(item);
        }
    }
}




