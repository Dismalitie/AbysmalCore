<div align="center">
  <img width="512" height="512" alt="AbysmalCore Icon" src="https://github.com/user-attachments/assets/c357199a-6b0e-47b7-8f1a-0747b0d9e89f" />
  
  # AbysmalCore
  AbysmalCore is the standard library with utilities, debugging suite and GUI system that all my future apps will use.
</div>
<br/>

> [!WARNING]
> This readme is under construction. Expect additions and changes

---

<div align="center">
  <img width="256" height="256" alt="AbysmalCore.Extensibility" src="https://github.com/user-attachments/assets/c44aba67-47bc-4dea-8d0f-5102d3096308" />
</div>

# AbysmalCore.Extensibility

A clean and uniform framework for runtime compilation and reflection of C# source files.

## Example

```cs
/// compile the assembly and get the test class
Assembly testAssembly = AbysmalExtensibility.CompileAssembly(File.ReadAllText(".\\ExtensibilityTest.cs"));
AbysmalExtensibilityClass testClass = AbysmalExtensibility.GetClass(testAssembly, "Tests.ExtensibilityTest", true);

string output = "";
/// check if the method exists, then invoke it
if (testClass.HasMethod("TestWith1Arg"))
    output = testClass.Methods["TestWith1Arg"].Invoke<string>("Hello!");

AbysmalDebug.Log(testClass.New(), output, true);
```

---

## 1. `AbysmalExtensibility` (Static Class)

This class provides **static methods** for compiling C\# source code into an assembly and instantiating classes from it.

### Methods

| Name | Signature | Description |
| :--- | :--- | :--- |
| **`CompileAssembly`** | `public static Assembly CompileAssembly(string src)` | Compiles the provided C\# source code (`src`) into an in-memory `Assembly` object, resolving references from the host application domain. |
| **`GetUniformClass<T>`** | `public static T GetUniformClass<T>(Assembly asm, string cls)` | Returns an instance of a class named `cls` from the compiled assembly (`asm`), casting it directly to the specified interface or abstract class type `<T>`. |
| **`GetClass`** | `public static AbysmalExtensibilityClass GetClass(Assembly asm, string cls, bool getPrivate = false)` | Returns an `AbysmalExtensibilityClass` wrapper instance for a class named `cls` from the assembly (`asm`), allowing uniform access to its members. Optionally includes private members. |

### Properties

None.

---

## 2. `AbysmalExtensibilityClass`

This class is a uniform wrapper around an instantiated object from a compiled assembly, providing access to its methods and properties via reflection.

### Methods

| Name | Signature | Description |
| :--- | :--- | :--- |
| **`HasMethod`** | `public bool HasMethod(string name)` | Returns `true` if the wrapped class contains a method with the specified `name`. |
| **`HasProperty`** | `public bool HasProperty(string name)` | Returns `true` if the wrapped class contains a property or field with the specified `name`. |
| **`New`** | `public object New()` | Instantiates and returns a new instance of the underlying class using its default constructor. |

### Properties

| Name | Type | Description |
| :--- | :--- | :--- |
| **`Properties`** | `Dictionary<string, AbysmalExtensibilityProperty>` | A dictionary mapping property/field names to their `AbysmalExtensibilityProperty` wrappers. |
| **`Methods`** | `Dictionary<string, AbysmalExtensibilityMethod>` | A dictionary mapping method names to their `AbysmalExtensibilityMethod` wrappers. |
| **`Instance`** | `object` | The actual instantiated object of the compiled class. |

---

## 3. `AbysmalExtensibilityMethod`

This class wraps a reflected method (`MethodInfo`), allowing simplified invocation on an object instance.

### Methods

| Name | Signature | Description |
| :--- | :--- | :--- |
| **`GetParameterType`** | `public Type GetParameterType(int index)` | Gets the `Type` of the parameter at the specified zero-based `index`. |
| **`Invoke`** | `public object? Invoke(params object[] args)` | Invokes the wrapped method with the provided arguments and returns the result as an `object?`. |
| **`Invoke<T>` (1)** | `public T Invoke<T>(params object[] args)` | Invokes the wrapped method and attempts to cast the result to the generic type `<T>`. |
| **`Invoke<T>` (2)** | `public T Invoke<T>(Func<object?, T> converter, params object[] args)` | Invokes the wrapped method and uses a custom `converter` function to convert the result to the generic type `<T>`. |

### Properties

| Name | Type | Description |
| :--- | :--- | :--- |
| **`Name`** | `string` | The name of the wrapped method. |
| **`ReturnType`** | `Type` | The return type of the wrapped method. |
| **`ParameterCount`** | `int` | The number of parameters the wrapped method accepts. |

---

## 4. `AbysmalExtensibilityProperty`

This class wraps a reflected property or field (`PropertyInfo` or `FieldInfo`), allowing simplified read and write access to its value on an object instance.

### Methods

None.

### Properties

| Name | Type | Description |
| :--- | :--- | :--- |
| **`Name`** | `string` | The name of the wrapped property or field. |
| **`Type`** | `Type` | The data type of the wrapped property or field. |
| **`IsPrivate`** | `bool` | Returns `true` if the wrapped member is a private field. |
| **`Value`** | `object?` | The value of the wrapped property or field (read/write access). Logs if read-only or write-only checks fail. |
