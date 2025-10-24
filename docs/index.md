---
_layout: landing
---

<div align="center">
  <img width="512" height="512" alt="AbysmalCore Icon" src="https://github.com/user-attachments/assets/c357199a-6b0e-47b7-8f1a-0747b0d9e89f" />

  # AbysmalCore
  AbysmalCore is the standard library with utilities, debugging suite and GUI system that all my future apps will use.
</div>
<br/>

> [!WARNING]
> This readme is under construction. Expect additions and changes

More thorough documentation available at https://dismalitie.github.io/AbysmalCore

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

<div align="center">
  <img width="256" height="256" alt="AbysmalCore.Debugging" src="https://github.com/user-attachments/assets/c5cf3611-4706-4c91-a724-4118096512d1" />
</div>

# AbysmalCore.Debugging

This map outlines the public **static** methods and properties for the primary debugging class, `AbysmalDebug`.

## Example

```cs
int layerDivisor = 3;
int coreDivisor = 5;

Color layer = new(c.R / layerDivisor, c.G / layerDivisor, c.B / layerDivisor);
AbysmalDebug.Log(this, $"Generated layer color {layer} from {c}");
Color core = new(c.R / coreDivisor, c.G / coreDivisor, c.B / coreDivisor);
AbysmalDebug.Log(this, $"Generated base color {core} from {c}");
```