# Dependency Injection for Unity Projects

## Start

A light-weight example of dependency injection using `[Provide]` and `[Inject]` attributes.
The system relies on the[ attributes and reflection](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection-and-attributes/) in .NET System.

### Scripts Contents

Attributes

- `ProvideAttribute` Providing dependencies. Any instances marked as `[Provide]` attribute, it is expected to be supplied with an instance of dependency.
- `InjectAttribute` Injecting dependencies. Any field or method marked as `[Inject]` attribute can expect to have the dependency satisfied by the system.

Services

- `Injector` finding and satisfying dependencies. All instances will be Monobehaviours.
- `Provider` supplies dependencies to the injection system. All instances will be Monobehaviours. Can supply itself as references.

Interfaces

- `IDependencyProvider` is a contract for identifying a type. It knows which classes in the system can provide dependencies.

Objects

- `ClassA` contains `ServiceA` and has an `Inject` method `Init` that accepts and assigns an instance of `ServiceA` into the field.
- `ClassB` contains both `Injuect` field attribute as for `ServiceA` and `ServiceB`, for variaty purposes. `FactoryA` follows `Inject` method attribute by using `Init` method.
  