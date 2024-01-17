# Architectures and Patterns for Unity Projects

## Dependency Injection

A light-weight example of dependency injection using `[Provide]` and `[Inject]` attributes.
The system relies on the[attributes and reflection](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection-and-attributes/) in .NET System.

### Attributes

- `ProvideAttribute` Providing dependencies. Any instances marked as `[Provide]` attribute, it is expected to be supplied with an instance of dependency.
- `InjectAttribute` Injecting dependencies. Any field or method marked as `[Inject]` attribute can expect to have the dependency satisfied by the system.

### Services

- `ServiceA` has `Initialze` method, consums and outputs a message on the console.
- `ServiceB` has `Initialze` method, consums and outputs a message on the console.
- `FactoryA` has a field to cache a `ServiceA` instance. `CreateServiceA()` only creats a new instance if the cache is null.

### Interfaces

- `IDependencyProvider` is a contract for identifying a type. It knows which classes in the system can provide dependencies.

### Components

- `ClassA` contains `ServiceA` and has an `Inject` method `Init` that accepts and assigns an instance of `ServiceA` into the field.

- `ClassB` contains both `Injuect` field attribute as for `ServiceA` and `ServiceB`, for variaty purposes. `FactoryA` follows `Inject` method attribute by using `Init` method.

- `Injector` finding and satisfying dependencies. All instances will be Monobehaviours.

  1. `Injector` utilizes reflection's `BindingFlags` to find the desired denpendency providers.
  2. To automatically find all dependency providers, take advantage of Unity's `FindMonoBehaviour` method to find all providers marked as `IDependencyProvider`
  3. `Injector` has a dictionary for registering the dependency providers and their return types. Once the providers are found, they can be invoked with optional parameters.
  4. A helper function `IsInjectable` returns any fields marked as `[Inject]`. Using `FindMonoBehaviour().Where()` to look for any injectable fields inside a MonoBehaviour object.
  5. `IsInjectable` uses `GetType().GetFeilds()` to find field `[Inject]` marks.
  6. `Any()` and `Where()` ([Know more about them](https://stackoverflow.com/questions/3703256/linq-extension-methods-any-vs-where-vs-exists)) are LINQ extention methods.`Any()` return a boolean (wheter or not any items match). `Where()` returns a new sequence of items matching the predicate.
  7. `Inject` method in `Injector` finds `[Inject]` marks in object fields by using `type.GetFields()`, resolve the provider type and assigns the instance to the `[Inject]` field instance.
  8. Similarly, `Inject` finds method `[Inject]` marks by using `type.GetMethods()`. These methods accept injectables as parameters. The query is using LINQ `Select()` to find matching parameter types and return them as an array.
  9. Addtionally, to find injectable properties using `type.GetProperties()'. Injectable properties are referred to as members have getters and setters.
  10. `Resovle` helps determine if the provider instance is registered.
  11. For injectable fields use `SetValue()` to assign instances. For injectable methods use `Invoke()` to call them.
  12. Set `Injector` class Attribute as `[DefaultExecutionOrder(<negative number>)]` will allow injector instantiated before any other objects.

- `Provider` supplies dependencies to the injection system. All instances will be Monobehaviours. Can supply itself as references.

- `Environment` provides itself as a dependency.
  
  1. This goes without letting `Factory` creating new instances. Simply use `Provide` Attribute on `ProvideEnvironmentSystem()` and let it returns itself as an instance.
  2. Best to combine with `Singleton`, that will allow autogenerated instance if one doesn't exist in the project.
  3. `Initialize()` outputs a message.

### Utilities

- `Singleton` inherits from `MonoBehaviour`. It attempts to find existing object that has the same Component. If not create one and attach a new Component of the same Type.

### Code Snippets

- Provide an instance through a provider.

```csharp
class Provider{
    [Provide]
    public IService ProvideService() {
        return new Service();
    }
}
```

- Self Provide

```csharp
class SelfProvider: IDependencyProvider, ISelfProvider{
    [Provide]
    public ISelfProvider ProvideSelf(){
        return this;
    }
}
```

`ISelfProvider` can be marked as `[Inject]` in other consumers as fields, method parameters or properties.

- Field Injection

```csharp
class Consumer{
    // A established provider can 
    [Inject] private IService _service;
}
```

- Method Injection

```csharp
class Consumer{
    private IService _service;
    [Inject]
    public void Init(IService service){
        _service = service;
    }
}
```

- Property Injection

```csharp
class Consumer{
    [Inject] public IService Service {get; private set; }
}
```

- Miltiple Injections

```csharp
class Consumer{
    private IService _service;
    private IFactory _factory;

    [Inject]
    public void Init(IService service, IFactory factory){
        _service = service;
        _factory = factory;
    }
}
```

### In-Editor Tools

- Components(MonoBehaviours) that has injectables will appear in `Component/Scripts/DependencyInjection`
- `Injector` component has two buttons `Validate Dependencies` and `Clear All Injectable Fields`.

## Service Locator

### Context

Inversion of Control is a way to decouple dependencies of services everytime a MonoBehaviour is trying to access them in the context of Unity. No need to manually link game objects together. Leave the automation process to Service Locator.

### Service Locator Core Modules

- `ServiceLocator` finds services for both the scene and global useage.
  1. It acts as a singleton for both global and scenes depends on configuration.
  2. The configurations check if other `ServiceLocator` is already registered in the scene and global, if so it does not create a new one.
  3. For scene level `ServiceLocator` it will go through all root game objects to find one if it's not registered in the container.
  4. Menu Items added under `GameObject/ServiceLocator/`. Both service locator variants can be added to the scene.
- `Bootstrapper` initializes services on `Awake()`.
  1. `GlobalBootstrapper` and `SceneBootstrapper` inherits from `Bootstrapper`. Each overrides `Bootstrap()` to call respective configurations and inistantiate instances on `Awake()`.
  2. `GlobalBootstrapper` has additional configuration for persistent singleton if `dontDestoryOnLoad` is set to `true`;
- `ServiceManager` Registers and gets services when needed.
  1. Both `Register` overloads go through the service type's registration and type check.
  2. Both `Get` and `TryGet` do registration and type check before return specified type of service.

### Mock Serviecs

| Interface | Class | Description |
|--|--|--|
| `ILocalization` | `MockLocalization` | Mocks a method `GetLocalizedWord()` that takes a word and get translation of another language. |
| `ISerializer` | `MockSerializer` | Mocks a method `Serialize()` to serialize objects. |
| `IAuthentication`| `MockAuthentication` | Mocks a method `Login()` to login user. Note: It's a `MonoBehaviour`. |
| `IGameService`| `MockGameService` | Mocks a method `StartGame()` to run the game. |

### Test Scenes

- Register Services
  
Inside `MainScene`, a `Player` object registers for global, scene and self scale services. Registration is done in `Awake()`
Note: MockAuthentication is a MonoBehaviour. In this case, GameObjectExtension `GetOrAdd` is used to instantiate it.

```csharp
ServiceLocator.Global.Register<IGameService>(_gameService = new MockGameService());
            ServiceLocator.ForSceneOf(this).Register<ILocalization>(_localization = new MockLocalization());
            ServiceLocator.For(this).Register<ISerializer>(_serializer = new MockSerializer());
            ServiceLocator.ForSceneOf(this)
                .Register<IAuthentication>(_authentication = gameObject.GetOrAdd<MockAuthentication>());
```

- Get Services

```csharp
ServiceLocator.For(this)
                .Get(out _serializer)
                .Get(out _localization)
                .Get(out _gameService)
                .Get(out _authentication);
```

After getting services, we can call any service methods however we want.

### Known Issues

- SubScenes do not recognize `ServiceLocator` Scene or global. Might worth looking at if subscene is used in the project.
- Loading multiple scenes at the same time `SceneServiceLocator` notice multiple instances, they should only be aware of the ones within the scene.

## Fluent Builder

When creating a new object with multiple attributes, overloading constructor can become cumbersome when more than for parameters need to be configured. Fluent Builder helps creating a new object with more readable code.

A built-in class can access owner class' properties. On top of passing parameters, the builder can attach the component to the game object. So no manual attachment required.

### Basic Field Type

```csharp
var enemy = new Enemy.Builder()
                .WithName("Kobolt")
                .WithDamange(10)
                .WithSpeed(3f)
                .WithHealth(20)
                .WithIsBoss(false)
                .Build();
```

No need to call `Instantiate()`, it's already instantiated.

### Complex Field Type

With data structures like maps and lists of objects, builder can shorten code lengths by using builder approach.

```csharp
    private Dictionary<string, Data> WeaponDataSet = new WeaponDataBuilder()
        .AddSword("sword", 11, 200.0f)
        .AddDagger("dagger", 13, 400.0f)
        .AddBow("bow", 10, 100.0f)
        .Build();
```

Initializing a dinctionary of weapon data is more compact comparing to adding entries one by one.

### Component Builder

Unity `MonoBehaviour` are attached to `GameObjects` as components. Component builder can streamline the process of adding components.

```csharp
var enemyWithComponents = new Enemy.ComponentBuilder()
                .AddArmorComponent()
                .AddWeaponComponent(weaponData)
                .AddHealthComponent()
                .Build();
```

Data-oriented objects can pass in as parameters for component configurations.

### Forced Sequence Builder

Interfaces enforce contracts, it's useful when component are constructed in a sequence. Each builder method returns the next return interface forces the builder call methods in a certain order.

## Observer Pattern

When having behaviour asssociated with a value, observer pattern can come in handy updating the value across the board. Such as losing health on the player, an Observer updates the health value to the UI when changed.

### Use Cases

An example: by hitting a button, player gain 10 points of energy.

- On `Player`

```csharp
public class Player : MonoBehaviour{
    [SerializeField] public Observer<int> Energy = new();
    private InputAction _clickAction;
    
    private void Start(){
        Energy.Invoke();
    }
    
    private void Enable(){
        var input = new PlayerController();
        _clickAction = input.Mouse.Click;
        _clickAction.Enable();
        _clickAction += _ => { Energy.Value += 10;}
    }

    private void Disable(){
        _clickAction.Disable();
    }
}
```

- On `Display`

```csharp
public class Display : MonoBehaviour{
    TMP_Text energyLabel;
    private void Awake() {
        energyLabel = GetComponent<TMP_Text>();
    }

    public void UpdateEnergyDisplay(int energy){
        energyLabel.text = $"Energy: {energy}";
    }
}
```

Afterward, go to the inspector and subscribe `UpdateEnergyDisplay` to `Player` component `Energy` section, open the drop down to add the function to the event.

Note: it can also be used for custom in-editor tools as well.

## Command Pattern

Reusable commands can be entities of their own. Commands can be abstracted and self-sufficient to provide generic instances.

### New Interface Features

C# has introduced interface new features:

- Default method implementations. It can reduce abstract class overhead. Not need to implement defaults in abstract classes with the interface.
- Inner class with the same interface can be used to represent default or null object instances.

## Singleton Pattern

Previously Singleton was used in Dependency Injection examples. Singleton pattern makes sure a single instance exists across the software life cycle. In Unity use cases, singleton can only exists as long as one scene exists, or persists as long as the game is running. More complicated than that singleton can self-updating by assign new updated instances to itself.

### Regular Singleton

It inherits from `MonoBehaviour` and takes generics that are `Component`. Check if the instance doesn't exist, create a new one. And initialize the instance upon `Awake()`.

### Persistent Singleton

It is the same logic as Regular Singleton. The only difference is that if a `GameObject` persists throughout multiple scenes, it should be located in the root level and no parent `GameObject`. `transform.SetParent(null)` unparent the `GameObject`. Upon `Awake()` after assigning the instance to self, add `DontDestroyOnLoad(gameObject)`.

### Regulator Singlton

Regulator Singleton records the time an instance is created and assign the newest one to itself, at the same time destroy the older instances. It manages itself so no need to expose time related properties.

### How To Use

Very simple. Replace `MonoBehaviour` with `Singleton<>` like:

```csharp
public class ForeverAlone : MonoBehaviour
{
    // Class implementations
}
```

```csharp
public class ForeverAlone : Singleton<ForeverAlone>{
    // Class implementations
}
```

## Class Extensions

Unity `Object` and its inheritance classes can have extension methods to expand the generalized functionality not implemented officially by Unity Engine developers.

### Code Snippet

Disable all children inside the hierarchy of a game object but keep the parrent active.

- Definition

```csharp
using UnityEngine;

public static class GameObjectExtensions{
    public static void DisableChildren(this GameObject gameObject) {
        for(var i=0; i<gameObject.transform.childCount; i++){
            gameObject.transform.GetChild(i).SetActive(false);
        }
    }
}
```

- Usage in the test scene

```csharp
public class DisableChildrenTest : MonoBehaviour
{
    void Start()
    {
        GameObject parent = new(name: "Parent");

        for (var i = 0; i < 5; i++)
        {
            GameObject child = new($"Child{i}");
            child.transform.parent = parent.transform;
        }
        
        parent.DisableChildren();
    }
}
```

While in play mode a GameObject named `Parent` is spawned and all it's children are disabled in the inspector.

## Credits

[Unity Dependency Injection Lite](https://github.com/adammyhre/Unity-Dependency-Injection-Lite/tree/master) from [adammyhre](https://github.com/adammyhre)

[Unity Service Locator](https://github.com/adammyhre/Unity-Service-Locator) from [adammyhre](https://github.com/adammyhre)
