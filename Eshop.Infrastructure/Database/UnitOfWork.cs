﻿using Eshop.Domain.Customers;
using Eshop.Domain.Orders;
using Eshop.Domain.SeedWork;
using MongoDB.Driver;

namespace Eshop.Infrastructure.Database
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly OrdersContext _ordersContext;

        private readonly CustomersContext _customersContext;
        private readonly IEntityTracker _entityTracker;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        public UnitOfWork(OrdersContext ordersContext, CustomersContext customersContext, IEntityTracker entityTracker, IDomainEventsDispatcher domainEventsDispatcher)
        {
            _ordersContext = ordersContext ?? throw new ArgumentNullException(nameof(ordersContext));
            _customersContext = customersContext ?? throw new ArgumentNullException(nameof(customersContext));
            _entityTracker = entityTracker ?? throw new ArgumentNullException(nameof(entityTracker));
            _domainEventsDispatcher = domainEventsDispatcher ?? throw new ArgumentNullException(nameof(domainEventsDispatcher));
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            var trackedEntities = _entityTracker.GetTrackedEntities().ToList();

            foreach (var entity in trackedEntities)
            {
                if (entity is Order order)
                {
                    var filter = Builders<Order>.Filter.Eq(c => c.Id, order.Id);
                    await _ordersContext.Orders.ReplaceOneAsync(filter, order, new ReplaceOptions { IsUpsert = true }, cancellationToken);
                }
                else if (entity is Customer customer)
                {
                    var filter = Builders<Customer>.Filter.Eq(c => c.Id, customer.Id);
                    await _customersContext.Customers.ReplaceOneAsync(filter, customer, new ReplaceOptions { IsUpsert = true }, cancellationToken);
                }
            }

            if (trackedEntities.Any())
            {
                await _domainEventsDispatcher.DispatchEventsAsync(trackedEntities);
                _entityTracker.ClearTrackedEntities();
            }

            return 0;
        }
    }
}