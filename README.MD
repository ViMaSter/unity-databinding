# Unity Databinding
This package allows databinding between components using a JSON document.

The document can be **updated** via **C#** or **automatically generated UI events** and **subscribed** to either via **C#** or **automatically update any Unity `MonoBehavior`/`Component`**.

## Examples
Clone this repository and open any of the scenes insides `Assets/Examples`:

- <details>
  <summary><b>Update any component automatically</b></summary>

  ## AutomatedSubscriber.scene

  This scene contains a `Document` and, using a `AutomatedSubscriber`, automatically updates the position of a Unity-internal UI Text component.
  
  #### `AutomatedSubscriber.cs`
  This script listens to the specified `KeyRoot` on the `Document` and **automatically** updates all properties and fields of the `TargetComponent`, that are visible in the Unity Inspector.

  #### `ArrowKeysHandler.cs`
  This script listens to arrow key presses. Based on those, a `x` and `y` coordinate inside the `Document` are set.
</details>

- <details>
  <summary><b>Subscribe and set values using C#</b></summary>
  
  ## ManualSubscriber.scene

  This scene contains a `Document` and manually **sets** and **subscribes to** changes inside this document using C#.
  
  #### `ManualSubscriber.cs`
  This script listens to the specified `KeyRoot` and manually forwards them to the UI element to update it's position.

  #### `ArrowKeysHandler.cs`
  This script listens to arrow key presses. Based on those, a `x` and `y` coordinate inside the document are set.
</details>

- <details>
  <summary><b>Automatically generate and update Prefabs using arrays</b></summary>
  
  ## Prefabs.scene

  This scene contains a `Document` and a `PrefabGenerator`. Based on the array at the specified `KeyRoot`, the `PrefabGenerator` automatically generates, updates or deletes instances of a `Prefab`.
  
  #### `PrefabGenerator.cs`
  This script subscribes to a path inside the `Document` containing an array.
  - If **array elements are added**, a **new instance** of the `Prefab` is generated
    - All `ReflectedSubscriber`s are updated to point to the array instance
  - If **array elements are removed**, the associated prefab **instance is deleted**
  - If an **array element's data changes**, the associated prefab **instance is updated** too

  #### `UpdateCount.cs`
  Using the up and down arrow keys, new elements can be added or removed.  
  Using the left and right arrow keys, the position of the last element can be changed.
</details>

- <details>
  <summary><b>Debugging</b></summary>

  ## Debugger.scene
  
  This scene contains a `Document` and a debugger which dumps the entire JSON document to an on-screen label.
  
  #### `DocumentDebugger.cs`
  This script fills a UI element with the current entire document and allows it to be copied to the clipboard, when Ctrl+Shift+D are pressed.

  #### `DebugDataGenerator.cs`
  This script sets example data on the `Document`.
</details>

## Usage
1. Add a `DataBinding.Document` to any GameObject to your scene
2. Subscribe to a path inside that `DataBinding.Document` using either use `DataBinding.Document.Subscribe()` or a `ReflectedSubscriber`
3. Set values on that path either using `DataBinding.Document.Set()` or using an `EventGenerator` on that document
4. Notice the subscriber being informed about changes
