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

1. `Injector` utilizes reflection's `BindingFlags` to find the desired denpendency providers.
2. To automatically find all dependency providers, take advantage of Unity's `FindMonoBehaviour` method to find all providers marked as `IDependencyProvider`
3. `Injector` has a dictionary for registering the dependency providers and their return types. Once the providers are found, they can be invoked with optional parameters.
4. A helper function `IsInjectable` returns any fields marked as `[Inject]`. Using `FindMonoBehaviour().Where()` to look for any injectable fields inside a MonoBehaviour object.
5. `IsInjectable` uses `GetType().GetFeilds()` to find field `[Inject]` marks.
6. `Any()` and `Where()` ([Know more about them](https://stackoverflow.com/questions/3703256/linq-extension-methods-any-vs-where-vs-exists)) are LINQ extention methods.`Any()` return a boolean (wheter or not any items match). `Where()` returns a new sequence of items matching the predicate.
7. `Inject` method in `Injector` finds `[Inject]` marks in object fields by using `type.GetFields()`, resolve the provider type and assigns the instance to the `[Inject]` field instance.
8. Similarly `Inject` finds method `[Inject]` marks by using `type.GetMethods()`. These methods accept injectables as parameters. The query is using LINQ `Select()` to find matching parameter types and return them as an array.
9. `Resovle` helps determine if the provider instance is registered.
10. For injectable fields use `SetValue()` to assign instances. For injectable methods use `Invoke()` to call them.

- `Provider` supplies dependencies to the injection system. All instances will be Monobehaviours. Can supply itself as references.

Interfaces

- `IDependencyProvider` is a contract for identifying a type. It knows which classes in the system can provide dependencies.

Objects

- `ClassA` contains `ServiceA` and has an `Inject` method `Init` that accepts and assigns an instance of `ServiceA` into the field.
- `ClassB` contains both `Injuect` field attribute as for `ServiceA` and `ServiceB`, for variaty purposes. `FactoryA` follows `Inject` method attribute by using `Init` method.
  