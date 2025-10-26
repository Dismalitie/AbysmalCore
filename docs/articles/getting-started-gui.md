<div align="center">
  <img width="256" height="256" alt="AbysmalCore.UI" src="https://github.com/Dismalitie/AbysmalCore/blob/master/images/ui.png?raw=true" />
</div>


# Getting Started with AbysmalCore UI

This guide provides an overview and instructions for setting up and using the AbysmalCore Graphical User Interface (GUI) system, covering core structures, controls, styling, and event handling.

## 1\. Getting Started: The Core Setup

To run a GUI, you need two core components: a **`Window`** to serve as the application container and a **`UserInterface`** to manage the elements displayed within that window.

### `Window`

The `Window` class manages the application viewport. It is where your GUI will be drawn.

| Property | Description |
| :--- | :--- |
| **`Size`** | The size of the window in pixels. |
| **`Title`** | The text displayed in the window title bar. |
| **`Resizable`** | Determines if the window can be resized by the user. |

**Key Method:**

The `Init(UserInterface ui)` method starts the window's main loop and begins drawing the provided `UserInterface`.

### `UserInterface`

The `UserInterface` acts as the root container and manager for all top-level GUI elements.

| Property | Description |
| :--- | :--- |
| **`Elements`** | A list of all top-level `UIElement`s in the GUI. |
| **`Frame`** | The current frame count since the UI started drawing. |
| **`Mouse`** | The current absolute mouse position. |

**Key Methods:**

| Method | Description |
| :--- | :--- |
| **`AddElement(UIElement)`** | Adds a `UIElement` to the top level. |
| **`RemoveElement(...)`** | Removes an element by instance or `Name`. |
| **`GetElement(string name)`** | Retrieves the first element matching the specified `Name`. |

### Example Setup

```cs
// create a new Window with a size and title
Window w = new(new(500, 500), typeof(Window).FullName!);

// create a UserInterface instance
UserInterface ui = new()
{
    // add root elements to the Elements list
    Elements =
    {
        // e.g: add a Button control
        new Button("hello!", new(10,10)) 
        {
            Name = "btn",
            StyleMap = new(true)
        }
    }
};

// initialize the window loop with the UserInterface
w.Init(ui); 
```

> [!IMPORTANT]
> `Window.Init` should always be called after defining your UI and setting off events. It will take control of the current thread, so any code after it will be unreachable until the window closes.

## 2\. Building Blocks: UI Elements

All interactive and visual components derive from the abstract class **`UIElement`**. Elements can be organized hierarchically, allowing you to nest elements within containers like a `Panel`.

### Core `UIElement` Properties

| Property/Field | Description |
| :--- | :--- |
| **`Name`** | Unique identifier for retrieval (e.g., via `GetElement("name")`). |
| **`Position`** | The element's position *relative* to its parent (or the window for root elements). |
| **`Size`** | The size of the element in pixels. |
| **`Visible`** | If `true`, the element is drawn. |
| **`Enabled`** | If `true`, the element can be interacted with (e.g., clicked). |
| **`CurrentStyle`** | The calculated `Style` based on the element's current state (`Hovered`, `Clicked`, etc.). |
| **`Children`** | A list of other `UIElement`s nested inside this element. |

### Available Controls

| Control | Description |
| :--- | :--- |
| **`Button`** | A standard clickable control for initiating actions. |
| **`Label`** | Displays a string of text. |
| **`Panel`** | A generic container used for grouping and organizing child elements. |
| **`Picture`** | Displays an image. |
| **`Toggle`** | A control for managing a boolean state (e.g., on/off, checked/unchecked). |

> [!TIP]
> You can make your own controls too by deriving them from `UIElement`. Just implement the `_draw` method and use the style accessors to draw the control to the window.

## 3\. Handling Interaction (Events)

`UIElement` provides powerful events to respond to user input. Events are added using the `+=` operator.

### Key Events

| Event | Description |
| :--- | :--- |
| **`OnClicked`** | Fired when the mouse button is pressed and released over the element. |
| **`OnHovered`** | Fired once when the mouse moves over the element's bounds. |
| **`OnMouseEnter`** | Fired once when the mouse enters the element's bounds. |
| **`OnMouseExit`** | Fired once when the mouse leaves the element's bounds. |

### Example Event Handling (from provided code)

To handle a button click, you retrieve the element by its `Name` and subscribe to the `OnClicked` event.

```cs
// retrieve the element by its Name
ui.GetElement("btn")?.OnClicked += (UIElement sender, Vector2Int mouse, int frame) =>
{
    // the code block to execute when the button is clicked
    AbysmalDebug.Log(sender, $"Clicked on frame {frame}!");
};
```

The event delegate provides:

  * `sender`: The `UIElement` that fired the event.
  * `mouse`: The mouse position at the time of the click.
  * `frame`: The current application frame count.

-----

## 4\. Styling Your GUI

Visual appearance is managed using **`StyleMap`**, **`Style`**, and various **`Brushes`**.

### `StyleMap` and `Style`

Every `UIElement` has a **`StyleMap`** that defines how it looks in different states (e.g., `Normal`, `Hovered`, `Clicked`, `Disabled`).

  * **`StyleMap`**: Maps a control state to a specific `Style`. The argument `new(true)` in the example code indicates that a default `StyleMap` should be created, derived from the global theme.
  * **`Style`**: Contains the visual properties for a given state, such as colors, margins, text font, and the **`Brushes`** used for drawing the background and foreground.

### Brushes

Brushes define the pattern and color used to draw a visual area.

| Brush Class | Description |
| :--- | :--- |
| **`SolidBrush`** | Fills an area with a single, solid color. |
| **`LinearGradientBrush`** | Fills an area with a color gradient along a straight line. |
| **`RadialGradientBrush`** | Fills an area with a color gradient radiating from a center point. |
| **`ImageBrush`** | Fills an area using a texture or image. |
| **`NineSliceBrush`** | Useful for resizable images (e.g., buttons, panels) that maintain corner integrity. |
| **`ShaderBrush`** | Allows for advanced, custom rendering effects using a shader. |

### Theming

The static **`Window.GlobalTheme`** property can be set to a **`Theme`** instance to apply a default set of styles across all controls in your application.

-----

## 5\. Summary of Example Code

The provided code creates a basic, functional, clickable button:

```cs
// setup 500x500 window
Window w = new(new(500, 500), typeof(Window).FullName!);
UserInterface ui = new()
{
    Elements =
    {
        new Button("hello!", new(10,10)) // create a Button at x:10, y:10
        {
            Name = "btn", // assign a name for lookup
            StyleMap = new(true) // assign a default StyleMap
        }
    }
};
ui.GetElement("btn")?.OnClicked += (UIElement sender, Vector2Int mouse, int frame) =>
{
    AbysmalDebug.Log(sender, $"Clicked on frame {frame}!"); // subscribe to the click event
};
w.Init(ui); // start the application loop
```

> [!IMPORTANT]
> `UIElement.Name`'s default value is `*`, so when using `UserInterface.GetElement("*")` it will return the first element with an unset name. Element names should be unique; if not it will return the first instance with that name or `null` if no instance qualifies.