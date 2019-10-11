# Mcms.Api.Business.Core
Definition library for the business layer of mcms.
## Contents
This library contains the definitions of the core interface and classes that describe the mechanics inside the business layer of the mcms api.
Of particular interest are its definition of:
 - Stores, the `IStore<TEntity>` interface.
 - QueryFilters, the `IQueryFilter<TEntity>` interface.
 - Managers, the `IXXXXXXXXManager` interfaces.
### Stores
An `IStore<TEntity>` for a given type handles the interaction between mcms and the backing implementation that stores mcms data.
Additionally its job is to isolate the individual interactions with the backing implementation from each other.

Addition, deletion and updating of entities that are stored within a single instance of an `IStore<TEntity>` as such should not be able to influence a different instance of an `IStore<TEntity>`.
### QueryFilters
Since `IStore<TEntity>` are defined in a generic CRUD fashion, their knowledge of the type stored within them is limited.
To ensure that, while reading data from the `IStore<TEntity>`, no performance is waisted, an `IQueryFilter<TEntity>` takes in the raw
query and then transforms it so that it can be executed as much as possible in the backing implementation of the `IStore<TEntity>`.
### Managers
