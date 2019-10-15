# Mcms.Api.Data.Core
Definition library for the data layer of mcms.
## Contents
This library contains the definitions of the core interface and classes that describe the mechanics inside the data layer of the mcms api.
Of particular interest are its definition of:
 - Stores, the `IDataStore<TEntity>` interface.
 - QueryFilters, the `IDataQueryFilter<TEntity>` interface.
 - Managers, the `IXXXXXXXXDataManager` interfaces.
### Stores
An `IDataStore<TEntity>` for a given type handles the interaction between mcms and the backing implementation that stores mcms data.
Additionally its job is to isolate the individual interactions with the backing implementation from each other.

Addition, deletion and updating of entities that are stored within a single instance of an `IDataStore<TEntity>` as such should not be able to influence a different instance of an `IDataStore<TEntity>`.
### DataQueryFilters
Since `IDataStore<TEntity>` are defined in a generic CRUD fashion, their knowledge of the type stored within them is limited.
To ensure that, while reading data from the `IDataStore<TEntity>`, no performance is waisted, an `IDataQueryFilter<TEntity>` takes in the raw
query and then transforms it so that it can be executed as much as possible in the backing implementation of the `IStore<TEntity>`.
### Managers
Managers are classes which are defined for each entity that mcms stores in its database.
Managers combine 'IDataStore<TEntity>' and the relevant 'IDataQueryFilter<TEntity>' with knowledge from the given entity to allow easy and reusable lookup and logic for handling the stores.
Examples of duties that managers fulfill are for example: 'Looking up en entity by Id' or 'Looking up an entity based on an entity property and a given regex.'
To fulfill these duties the manager builds a relevant query filter and passes it along to the store for execution.
Additionally managers provider wrapper and passthrough-methods for creating, deleting and updating of the entity type that it manages.
