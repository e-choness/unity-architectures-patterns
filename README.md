# Dependency Injection for Unity Projects

## Start

A light-weight example of dependency injection using [Provide] and [Inject] attributes.

### Scripts Contents

Attributes

- `ProvideAttribute` Providing dependencies.
- `InjectAttribute` Injecting dependencies.

Services

- `Injector` finding and satisfying dependencies. All instances will be Monobehaviours.
- `Provider` supplies dependencies to the injection system. All instances will be Monobehaviours. Can supply itself as references.

Interfaces

- `IDependencyProvider` is a contract for identifying a type. It knows which classes in the system can provide dependencies.

Objects

- `ClassA` and `ClassB` are for testing purposes.