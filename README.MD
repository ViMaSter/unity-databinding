# Unity Data-binding
This package allows data-binding between components using a JSON blob called a `Document`.

![alt text](https://github.com/ViMaSter/unity-data-binding/raw/main/example.gif "Example of a UI label being moved using data-binding")

The `Document` can be **updated** via **C#** or **automatically generated UI events** and **subscribed to** to either via **C#** or **automatically update any Unity `MonoBehavior`/`Component`**.

## Examples
Clone this repository and open any of the scenes inside `Assets/Examples`.  
(Use `Ctrl+Shift+D` to see a debug view of the `Document`!)

- <details>
    <summary><b>Update any component automatically</b></summary>

    ## AutomatedSubscriber.scene

    This scene contains a `Document` and, using an `AutomatedSubscriber`, automatically updates the position of a Unity-internal UI Text component.
    
    #### `AutomatedSubscriber.cs`
    This script listens to the specified `Path` on the `Document` and **automatically** updates all properties and fields of the `TargetComponent`, that are visible in the Unity Inspector. (In this case a [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html).)

    #### `ArrowKeysHandler.cs`
    This script listens to arrow key presses. Based on those, `x` and `y` coordinates inside the `Document` are set.
  </details>

- <details>
    <summary><b>Handle Unity UI events</b></summary>
    
    ## EventGenerator.scene

    This scene uses an `EventGenerator` which makes interactive UI elements (like [`UnityEngine.UI.Button`](https://docs.unity3d.com/2017.3/Documentation/ScriptReference/UI.Button.html)s) automatically send their events to the `Document` for others to subscribe to.
    
    #### `EventGenerator.cs`
    This component scans its `GameObject` for any `Component` that can generate events.
    In this scene, each GameObject with a `Button` also has an `EventGenerator`, so that there's `Left`, `Right`, `Up`, `Down` objects with `down` and `clicked` events

    #### `UIArrowEventSubscriber.cs`
    This script subscribes to the buttons' events inside the `Document` (`Right.events.down`, `Left.events.down`, etc.) and - when pressed - changes the text's position via data-binding like in `AutomatedSubscriber.scene`
  </details>

- <details>
    <summary><b>Subscribe and set values using C#</b></summary>
    
    ## ManualSubscriber.scene

    This scene contains a `Document` and manually **sets** and **subscribes to** changes inside a `Document` using C#.
    
    #### `ManualSubscriber.cs`
    This script listens to the specified `Path` and manually forwards them to the UI element to update it's position.

    #### `ArrowKeysHandler.cs`
    This script listens to arrow key presses. Based on those, `x` and `y` coordinates inside the `Document` are set.
  </details>

- <details>
    <summary><b>Automatically generate and update Prefab instances using arrays</b></summary>
    
    ## Prefabs.scene

    This scene contains a `Document` and a `PrefabGenerator`.  
    Based on the array at the specified `Path`, the `PrefabGenerator` automatically generates, updates or deletes instances of a `Prefab`.
    
    #### `PrefabGenerator.cs`
    This script subscribes to a path inside the `Document` containing an array.
    - If **array elements are added**, a **new instance** of the `Prefab` is generated
      - All `ReflectedSubscriber`s are updated to point to their relative `Path` (i.e. `UIElements[0]`, `UIElements[1]`, `UIElements[2]`, etc.)
    - If **array elements are removed**, the associated prefab **instance is deleted**
    - If an **array element's data changes**, the associated prefab **instance is updated** too

    #### `UpdateCount.cs`
    Using the up and down arrow keys, new elements can be added or removed.  
    Using the left and right arrow keys, the position of the last element can be changed.
  </details>

- <details>
    <summary><b>Debugging</b></summary>

    ## Debugger.scene
    
    This scene contains a `Document` and a debugger which dumps the entire JSON `Document` to an on-screen label.
    
    #### `DocumentDebugger.cs`
    This script fills a UI element with the current entire `Document` and allows it to be copied to the clipboard.  
    Toggle it's visibility with `Ctrl`+`Shift`+`D`.

    #### `DebugDataGenerator.cs`
    This script sets example data on the `Document`.
  </details>

## Vocabulary
- A `Document` is a Unity component containing a `JSON object`:
  ```json
  {
    "key1": "value1",
    "key2": [
              "value2",
              3
            ]
  }
  ```
- A `path` or "document `path`" is a route to take inside the `Document` to find a value (i.e. `key2[1]` would be `3` in the example above)

## Usage
1. Add a `DataBinding.Document` to any GameObject to your scene
2. Subscribe to a path inside that `DataBinding.Document` using either use `DataBinding.Document.Subscribe()` or a `AutomatedSubscriber`
3. Set values on that path either using `DataBinding.Document.Set()` or using an `EventGenerator` pointed to that `Document`
4. Notice the subscriber being informed about changes
