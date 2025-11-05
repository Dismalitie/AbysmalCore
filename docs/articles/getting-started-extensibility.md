<div align="center">
  <img width="256" height="256" alt="AbysmalCore.Extensibility" src="https://github.com/Dismalitie/AbysmalCore/blob/master/images/extensibility.png?raw=true" />
</div>

# Getting Started with AbysmalCore Extensibility

The AbysmalCore Extensibility Framework provides a simple, **uniform interface** for dynamically loading, compiling, and interacting with C\# code using reflection. This guide will walk you through the core concepts and steps for using the `ExtensibilityHelper`, `UniformAssembly`, `UniformClass`, `UniformMethod`, and `UniformProperty` classes.

## 1\. Core Concepts

The framework revolves around wrapping the standard .NET Reflection types (`Assembly`, `Type`, `MethodInfo`, `PropertyInfo`, `FieldInfo`) into **Uniform** classes. This abstraction is designed to make common reflection tasks easier and more consistent.

| Uniform Class | Wraps | Description |
| :--- | :--- | :--- |
| **`UniformAssembly`** | `Assembly` | Represents a compiled assembly, used to access its classes and invoke its entry point. |
| **`UniformClass`** | `Type` & `object` | Represents an instantiated class from an assembly, providing access to its methods and properties. |
| **`UniformMethod`** | `MethodInfo` | Represents a method, used for invoking it with arguments. |
| **`UniformProperty`** | `PropertyInfo` or `FieldInfo` | Represents a property or field, used for getting and setting its value. |

## 2\. Dynamic Compilation and Loading

The `ExtensibilityHelper` class is your starting point for turning raw C\# source code into an executable assembly.

### Step 1: Compiling Source Code

Use `ExtensibilityHelper.CompileAssemblyFromString` to compile a string of C\# code into a standard `System.Reflection.Assembly`.

```cs
using System.Reflection;
using AbysmalCore.Extensibility;

string sourceCode = """
namespace MyExtension
{
    public class MyClass
    {
        public string GetMessage() => "Hello from the dynamically loaded assembly!";
        public int Count { get; set; } = 0;
    }
}
""";

// internally we use CSharp.CodeAnalysis to compile
Assembly compiledAssembly = ExtensibilityHelper.CompileAssemblyFromString(sourceCode);
```

-----

### Step 2: Creating a Uniform Assembly

Once you have the `Assembly`, wrap it in a `UniformAssembly` to begin interacting with its contents.

```cs
// wrap the compiled assembly
// 'getPrivate = false' means only public members are exposed (default)
UniformAssembly uniformAssembly = ExtensibilityHelper.LoadAssembly(compiledAssembly);

bool hasClass = uniformAssembly.HasClass("MyExtension.MyClass"); // true
```

> [!IMPORTANT]
> Classes are found and retrieved using their full name. If your class is in a namespace, make sure to include its full path:
> 
> ❌ `MyClass`
> 
> ✅ `MyExtension.MyClass`

## 3\. Interacting with Classes and Members

With the `UniformAssembly`, you can now instantiate classes and manipulate their methods and properties using the rest of the Uniform classes.

### Step 3: Getting and Instantiating a Uniform Class

Retrieve the `UniformClass` by its name and get the instantiated object through the `Instance` property.

```cs
// get the wrapper for the class
UniformClass? myUniformClass = uniformAssembly.GetClass("MyExtension.MyClass");

if (myUniformClass != null)
{
    // the Instance property holds an object of the underlying class (MyExtension.MyClass)
    object myInstance = myUniformClass.Instance;
    
    // you can create a new instance as well
    object newInstance = myUniformClass.New(); 
}
```

> [!TIP]
> If your wrapped class implements a common interface or abstraction, use `UniformClass.DeriveFrom<T>` to create an instance of that interface or abstraction with the class's overrides and implementations

-----

### Step 4: Invoking Methods

Use `UniformClass.GetMethod` to retrieve a method wrapper, then use its `Invoke` methods.

```cs
if (myUniformClass != null)
{
    UniformMethod? getMessageMethod = myUniformClass.GetMethod("GetMessage");

    if (getMessageMethod != null)
    {
        // invoke the method with no arguments
        object? result = getMessageMethod.Invoke(); 

        // invoke and cast the result
        string message = getMessageMethod.Invoke<string>(); // "Hello from the dynamically loaded assembly!"
    }
}
```

-----

### Step 5: Accessing Properties and Fields

Use `UniformClass.GetProperty` to retrieve a property/field wrapper, then use its `Value` property to get or set.

```csharp
if (myUniformClass != null)
{
    UniformProperty? countProperty = myUniformClass.GetProperty("Count");

    if (countProperty != null)
    {
        // get the value (returns an object, so casting is often necessary)
        int currentValue = (int)countProperty.Value!; // 0

        // set the value
        countProperty.Value = 42;

        // verify new value
        int newValue = (int)countProperty.Value!; // 42
    }
}
```

## 4\. Special Scenarios

### Invoking an Assembly Entry Point

If your dynamically compiled assembly has a `Main` method (an entry point), you can invoke it directly.

```cs
// lets assume your source code has a public static void Main(string[] args)
object? output;
string[] cliArgs = { "arg1", "arg2" };

bool invoked = uniformAssembly.InvokeEntrypoint(cliArgs, out output);

if (invoked)
{
    // 'output' will contain the result of the entry point method (e.g., an integer if it returns int)
}
```

### Exposing Private Members

When creating a `UniformAssembly` or `UniformClass`, passing `true` for `getPrivate` will include non-public instance properties and fields in the resulting collections.

```cs
// this UniformClass instance will include private members
UniformClass privateClass = new UniformClass(myInstance, getPrivate: true); 

// note: you can check if a member is private via the IsPrivate property
```

<img height=30 alt="author" src="data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjUuODUwMDEzNzMyOTEwMTYiIGhlaWdodD0iMzUiIHZpZXdCb3g9IjAgMCAxNjUuODUwMDEzNzMyOTEwMTYgMzUiPjxyZWN0IHdpZHRoPSI4Mi42MTY2NzI1MTU4NjkxNCIgaGVpZ2h0PSIzNSIgZmlsbD0iIzI4MTAxMCIvPjxyZWN0IHg9IjgyLjYxNjY3MjUxNTg2OTE0IiB3aWR0aD0iODMuMjMzMzQxMjE3MDQxMDIiIGhlaWdodD0iMzUiIGZpbGw9IiM4MTM0MzQiLz48dGV4dCB4PSI0MS4zMDgzMzYyNTc5MzQ1NyIgeT0iMjEuNSIgZm9udC1zaXplPSIxMiIgZm9udC1mYW1pbHk9IidSb2JvdG8nLCBzYW5zLXNlcmlmIiBmaWxsPSIjRkZGRkZGIiB0ZXh0LWFuY2hvcj0ibWlkZGxlIiBsZXR0ZXItc3BhY2luZz0iMiI+QVVUSE9SPC90ZXh0Pjx0ZXh0IHg9IjEyNC4yMzMzNDMxMjQzODk2NSIgeT0iMjEuNSIgZm9udC1zaXplPSIxMiIgZm9udC1mYW1pbHk9IidNb250c2VycmF0Jywgc2Fucy1zZXJpZiIgZmlsbD0iI0ZGRkZGRiIgdGV4dC1hbmNob3I9Im1pZGRsZSIgZm9udC13ZWlnaHQ9IjkwMCIgbGV0dGVyLXNwYWNpbmc9IjIiPkRJU01BTDwvdGV4dD48L3N2Zz4="/>
<img height=30 alt="date" src="data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNDkuNzgzMzI5MDEwMDA5NzciIGhlaWdodD0iMzUiIHZpZXdCb3g9IjAgMCAxNDkuNzgzMzI5MDEwMDA5NzcgMzUiPjxyZWN0IHdpZHRoPSI1OS4xMTY2NjQ4ODY0NzQ2MSIgaGVpZ2h0PSIzNSIgZmlsbD0iIzI4MTAxMCIvPjxyZWN0IHg9IjU5LjExNjY2NDg4NjQ3NDYxIiB3aWR0aD0iOTAuNjY2NjY0MTIzNTM1MTYiIGhlaWdodD0iMzUiIGZpbGw9IiM4MTM0MzQiLz48dGV4dCB4PSIyOS41NTgzMzI0NDMyMzczMDUiIHk9IjIxLjUiIGZvbnQtc2l6ZT0iMTIiIGZvbnQtZmFtaWx5PSInUm9ib3RvJywgc2Fucy1zZXJpZiIgZmlsbD0iI0ZGRkZGRiIgdGV4dC1hbmNob3I9Im1pZGRsZSIgbGV0dGVyLXNwYWNpbmc9IjIiPkRBVEU8L3RleHQ+PHRleHQgeD0iMTA0LjQ0OTk5Njk0ODI0MjE5IiB5PSIyMS41IiBmb250LXNpemU9IjEyIiBmb250LWZhbWlseT0iJ01vbnRzZXJyYXQnLCBzYW5zLXNlcmlmIiBmaWxsPSIjRkZGRkZGIiB0ZXh0LWFuY2hvcj0ibWlkZGxlIiBmb250LXdlaWdodD0iOTAwIiBsZXR0ZXItc3BhY2luZz0iMiI+MjQvMTAvMjU8L3RleHQ+PC9zdmc+"/>