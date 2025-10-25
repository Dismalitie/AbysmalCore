# Getting Started with AbysmalCore Debugging

`AbysmalDebug` provides a simply formatted interface to quickly log, warn, error and pause or stop. This guide will tell you how to use the `DebugInfoAttribute` and the `AbysmalDebug` methods.

## 1\. The `DebugInfoAttribute`

The `DebugInfoAttribute` is a custom C\# attribute you can apply to any class to provide extra debugging context, such as a description and an importance flag.

### Applying the Attribute

Apply the attribute to your class definition:

```cs
using AbysmalCore.Debugging;

[DebugInfo("standard debug lib", true)]
public class AbysmalDebug
{
    // ...
}
```

| Parameter | Type | Description |
| :--- | :--- | :--- |
| `desc` | `string` | A brief description of the class. This will appear in the log messages. |
| `important` | `bool` | (**Optional** - defaults to `false`) If set to `true`, all messages logged from an instance of this class will be treated as **important** logs. |

### Configuring what an important log does

You can globally control what happens when an **important** log message is generated.

The `DebugInfoAttribute.ImportanceActionType` enum has two values:

| Value | Description |
| :--- | :--- |
| `Highlight` | The important message is highlighted in **magenta** (default). |
| `Pause` | The application execution is paused until a key is pressed. |

You can set the action in your application's startup code:

```cs
using AbysmalCore.Debugging;

// set the action to pause execution on important logs
DebugInfoAttribute.ImportanceAction = DebugInfoAttribute.ImportanceActionType.Pause;

// or set it back to just highlighting (default)
// DebugInfoAttribute.ImportanceAction = DebugInfoAttribute.ImportanceActionType.Highlight;
```

> [!TIP]
> For manual and conditional pausing not invoked by a log, use `AbysmalDebug.Pause` instead

## 2\. The `AbysmalDebug` Class

The static `AbysmalDebug` class provides methods for logging messages, warnings, and errors to the console, as well as controlling application flow.

### Global Control

| Static Field | Type | Description |
| :--- | :--- | :--- |
| `Enabled` | `bool` | Controls whether any messages are written to the console. Set to `false` to disable all logging. |

> [!IMPORTANT]
> `Enabled` is set to `true` by default, meaning logs will be automatically generated. If making a console application, it's probably best to set it to `false` as some AbysmalCore functions use `AbysmalDebug`.

### Logging Methods

All logging methods require an instance of the calling class (`object @this`) to retrieve any associated `DebugInfoAttribute` via reflection. It's also generally good practice to keep track of where the message came from.

#### Standard Log

Logs a standard informational message.

```cs
// inside a class, 'this' refers to the current instance
AbysmalDebug.Log(this, "The component is starting up."); 
// messages are formatted like: [timestamp][ClassFullName:(description)] (i) message

// you can manually flag a specific log as important, even if the class isn't marked as important
AbysmalDebug.Log(this, "A critical step has been reached.", important: true); 
// this message will either be highlighted or pause execution, depending on ImportanceAction.
```

#### Warning Log

Logs a warning message, highlighted in **yellow**.

```cs
AbysmalDebug.Warn(this, "Cache file not found, creating a new one."); 
// messages are formatted like: [timestamp][ClassFullName:(description)] /!\ message
```

#### Error Log

Logs an error message, highlighted in **red**.

```cs
// logs the error
AbysmalDebug.Error(this, "Failed to load configuration file."); 

// we can also flag an error as fatal, which throws an exception to stop execution
// AbysmalDebug.Error(this, "An unrecoverable state has been reached.", fatal: true);
```

> [!TIP]
> In all logging methods, the placeholder **`{name}`** in the log message will be replaced with the `Description` from the applied `DebugInfoAttribute` (if present) or the class name itself.

### Flow Control Methods

#### Pause Execution

Pauses the application and waits for a key press.

```cs
// unconditionally pauses execution
AbysmalDebug.Pause(); 

// pauses only if 'isValid' is false (value != expected)
bool isValid = false;
AbysmalDebug.Pause(value: isValid, expected: true, reason: "Invalid state detected"); 
```

#### Stop Execution

Stops the application indefinitely using a `while(true) { }` loop.

```cs
// unconditionally stops execution
AbysmalDebug.Stop(); 

// stops only if 'shouldStop' is true (value != expected)
bool shouldStop = true;
AbysmalDebug.Stop(value: shouldStop, expected: false, reason: "Emergency stop signal");
```

### Writing Logs to a File

The logging system captures all console output produced by `AbysmalDebug`. You can write the entire log history to a file.

```cs
// write all logs to a text file
AbysmalDebug.WriteLogs("application.log");
```