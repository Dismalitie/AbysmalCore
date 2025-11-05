<div align="center">
  <img width="256" height="256" alt="AbysmalCore.Debugging" src="https://github.com/Dismalitie/AbysmalCore/blob/master/images/debugging.png?raw=true" />
</div>

# Getting Started with AbysmalCore `Debugging.Analyzers`

This guide explains how to use the `AbysmalCore.Debugging.Analyzer<TClass, TAnalyzer>` system to perform static analysis on your C\# classes at runtime.

## 1\. Quick Start

The core usage pattern is to specify the **class you want to analyze** (`TClass`) and the **analyzer you want to use** (`TAnalyzer`), run the analysis, and then emit the results. 

### Example Usage

```cs
using AbysmalCore.Debugging;
using AbysmalCore.Debugging.Analyzers;

public class MyTestClass 
{
    public string Name { get; set; } = null; // should be flagged
    public int? OptionalCount { get; set; } // should be flagged as nullable
    public string InitializedName = "Default";
    public string? NullableField; // should be flagged as nullable

    public string GetName() => Name; // should be flagged as having a nullable return type
    public void DoSomething() { }
}

// now we need to actually analyze the class and emit the logs
public class AnalysisRunner 
{
    public static void RunAnalysis()
    {
        // you can selectively choose to analyze methods, properties or fields too
        Analyzer<MyTestClass, NullValueAnalyzer>.AnalyzeMembers();

        // this will print the logs
        Analyzer<MyTestClass, NullValueAnalyzer>.Emit();
    }
}
```

> [!TIP]
> The source code for the `NullValueAnalyzer` used in this example can be found [here](https://github.com/Dismalitie/AbysmalCore/blob/master/AbysmalCore/Debugging/Analyzers/NullValueAnalyzer.cs)

## 2\. Creating a Custom Analyzer

To create your own custom analysis, you must define a class that inherits from `IAnalyzer`.

### `IAnalyzer` Definition

The `IAnalyzer` class provides virtual methods you can override to perform analysis on specific member types:

```cs
namespace AbysmalCore.Debugging
{
    public class IAnalyzer
    {
        public Type ParentClass { get; set; }

        // override these methods to do the actual analyzing
        public virtual void AnalyzeMethods(MethodInfo[] methods) { /* ... */ }
        public virtual void AnalyzeProperties(PropertyInfo[] properties) { /* ... */ }
        public virtual void AnalyzeFields(FieldInfo[] fields) { /* ... */ }
        public virtual void AnalyzeParentClass(Type parent) { /* ... */ }
        
        // use this method to log analytics for emission
        public void Log(string analytic);
    }
}
```

### Writing a Custom Analyzer

Here is a template for a custom analyzer:

```cs
using System.Reflection;
using AbysmalCore.Debugging;

namespace MyCustomAnalyzers
{
    public class MyDocumentationAnalyzer : IAnalyzer
    {
        /// <inheritdoc/>
        public override void AnalyzeProperties(PropertyInfo[] properties)
        {
            foreach (PropertyInfo prop in properties)
            {
                // example: check if a property is obsolete
                if (prop.GetCustomAttribute<ObsoleteAttribute>() != null)
                {
                    Log($"/!\\ Property '{prop.Name}' is marked as Obsolete."); // /!\ is a warning
                }
            }
        }
        
        /// <inheritdoc/>
        public override void AnalyzeParentClass(Type parent)
        {
            if (parent.IsSealed)
            {
                Log("(i) This class is sealed and cannot be inherited."); // (i) is an informational log
            }
        }
    }
}
```

The system includes special log markers that determine the console color when the logs are emitted via `Emit()`:

| Marker | Log Type | Color (Default) |
| :---: | :---: | :---: |
| `/!\` | **Warning** (Potential Issue) | Yellow |
| `(!)` | **Error** (High Severity Issue) | Red |
| `[!]` | **Critical Error** (Highest Severity) | Dark Red |
| `(i)` | **Informational** (No issue) | Dark Gray |
| *(None)* | **General Info** | Magenta |

-----

## 3\. Analysis Methods

The `Analyzer<TClass, TAnalyzer>` class provides several static methods to run specific parts of the analysis:

| Method | Description |
| :--- | :--- |
| `AnalyzeMembers()` | **Runs all analysis methods:** `AnalyzeParentClass()`, `AnalyzeMethods()`, `AnalyzeProperties()`, and `AnalyzeFields()`. |
| `AnalyzeParentClass()` | Runs the `AnalyzeParentClass` override in your analyzer. |
| `AnalyzeMethods()` | Runs the `AnalyzeMethods` override, analyzing all public instance methods. |
| `AnalyzeProperties()` | Runs the `AnalyzeProperties` override, analyzing all public instance properties. |
| `AnalyzeFields()` | Runs the `AnalyzeFields` override, analyzing all public instance fields. |

### Emitting Results

You can retrieve the analysis logs in two ways:

1.  **Emit to Console:**
    ```cs
    Analyzer<MyClass, MyAnalyzer>.Emit(); 
    // outputs logs to the console
    ```
2.  **Get Logs as String Array:**
    ```cs
    string[] logs = Analyzer<MyClass, MyAnalyzer>.Emit(returnLogs: true); 
    // returns the raw log strings
    ```

<img height=30 alt="author" src="data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjUuODUwMDEzNzMyOTEwMTYiIGhlaWdodD0iMzUiIHZpZXdCb3g9IjAgMCAxNjUuODUwMDEzNzMyOTEwMTYgMzUiPjxyZWN0IHdpZHRoPSI4Mi42MTY2NzI1MTU4NjkxNCIgaGVpZ2h0PSIzNSIgZmlsbD0iIzI4MTAxMCIvPjxyZWN0IHg9IjgyLjYxNjY3MjUxNTg2OTE0IiB3aWR0aD0iODMuMjMzMzQxMjE3MDQxMDIiIGhlaWdodD0iMzUiIGZpbGw9IiM4MTM0MzQiLz48dGV4dCB4PSI0MS4zMDgzMzYyNTc5MzQ1NyIgeT0iMjEuNSIgZm9udC1zaXplPSIxMiIgZm9udC1mYW1pbHk9IidSb2JvdG8nLCBzYW5zLXNlcmlmIiBmaWxsPSIjRkZGRkZGIiB0ZXh0LWFuY2hvcj0ibWlkZGxlIiBsZXR0ZXItc3BhY2luZz0iMiI+QVVUSE9SPC90ZXh0Pjx0ZXh0IHg9IjEyNC4yMzMzNDMxMjQzODk2NSIgeT0iMjEuNSIgZm9udC1zaXplPSIxMiIgZm9udC1mYW1pbHk9IidNb250c2VycmF0Jywgc2Fucy1zZXJpZiIgZmlsbD0iI0ZGRkZGRiIgdGV4dC1hbmNob3I9Im1pZGRsZSIgZm9udC13ZWlnaHQ9IjkwMCIgbGV0dGVyLXNwYWNpbmc9IjIiPkRJU01BTDwvdGV4dD48L3N2Zz4="/>
<img height=30 alt="date" src="data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNDkuNzgzMzI5MDEwMDA5NzciIGhlaWdodD0iMzUiIHZpZXdCb3g9IjAgMCAxNDkuNzgzMzI5MDEwMDA5NzcgMzUiPjxyZWN0IHdpZHRoPSI1OS4xMTY2NjQ4ODY0NzQ2MSIgaGVpZ2h0PSIzNSIgZmlsbD0iIzI4MTAxMCIvPjxyZWN0IHg9IjU5LjExNjY2NDg4NjQ3NDYxIiB3aWR0aD0iOTAuNjY2NjY0MTIzNTM1MTYiIGhlaWdodD0iMzUiIGZpbGw9IiM4MTM0MzQiLz48dGV4dCB4PSIyOS41NTgzMzI0NDMyMzczMDUiIHk9IjIxLjUiIGZvbnQtc2l6ZT0iMTIiIGZvbnQtZmFtaWx5PSInUm9ib3RvJywgc2Fucy1zZXJpZiIgZmlsbD0iI0ZGRkZGRiIgdGV4dC1hbmNob3I9Im1pZGRsZSIgbGV0dGVyLXNwYWNpbmc9IjIiPkRBVEU8L3RleHQ+PHRleHQgeD0iMTA0LjQ0OTk5Njk0ODI0MjE5IiB5PSIyMS41IiBmb250LXNpemU9IjEyIiBmb250LWZhbWlseT0iJ01vbnRzZXJyYXQnLCBzYW5zLXNlcmlmIiBmaWxsPSIjRkZGRkZGIiB0ZXh0LWFuY2hvcj0ibWlkZGxlIiBmb250LXdlaWdodD0iOTAwIiBsZXR0ZXItc3BhY2luZz0iMiI+MDUvMTEvMjU8L3RleHQ+PC9zdmc+"/>