---
_layout: landing
---

<div align="center">
  <img width="512" height="512" alt="AbysmalCore Icon" src="https://github.com/user-attachments/assets/c357199a-6b0e-47b7-8f1a-0747b0d9e89f" />
</div>

# AbysmalCore
  
AbysmalCore is the standard library with utilities, debugging suite and GUI system that all my future apps will use.

---

<div align="center">
  <img width="256" height="256" alt="AbysmalCore.Extensibility" src="https://github.com/user-attachments/assets/c44aba67-47bc-4dea-8d0f-5102d3096308" />
</div>

# AbysmalCore.Extensibility

A clean and uniform framework for runtime compilation and reflection of C# source files.

## Example

```cs
// compile the assembly and get the test class
Assembly testAssembly = AbysmalExtensibility.CompileAssembly(File.ReadAllText(".\\ExtensibilityTest.cs"));
AbysmalExtensibilityClass testClass = AbysmalExtensibility.GetClass(testAssembly, "Tests.ExtensibilityTest", true);

string output = "";
// check if the method exists, then invoke it
if (testClass.HasMethod("TestWith1Arg"))
    output = testClass.Methods["TestWith1Arg"].Invoke<string>("Hello!");

AbysmalDebug.Log(testClass.Instance, output, true);
```

---

<div align="center">
  <img width="256" height="256" alt="AbysmalCore.Debugging" src="https://github.com/user-attachments/assets/c5cf3611-4706-4c91-a724-4118096512d1" />
</div>

# AbysmalCore.Debugging

This map outlines the public **static** methods and properties for the primary debugging class, `AbysmalDebug`.

---

```cs
AbysmalDebug.Warn(this, "Warning!");
```