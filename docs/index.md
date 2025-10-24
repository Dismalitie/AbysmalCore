---
_layout: landing
---

<div align="center">
  <img width="512" height="512" alt="AbysmalCore Icon" src="https://github.com/Dismalitie/AbysmalCore/blob/master/images/abysmalcore.png?raw=true" />
</div>

# AbysmalCore

AbysmalCore is the standard library with utilities, debugging suite and GUI system that all my future apps will use.

---

<div align="center">
  <img width="256" height="256" alt="AbysmalCore.Extensibility" src="https://github.com/Dismalitie/AbysmalCore/blob/master/images/extensibility.png?raw=true" />
</div>

# `AbysmalCore.Extensibility`

A clean and uniform framework for runtime compilation and reflection of C# source files.

## Example

```cs
// compile the assembly and get the test class
Assembly testAssembly = ExtensibilityHelper.CompileAssembly(File.ReadAllText(".\\ExtensibilityTest.cs"));
var asm = new UniformAssembly(testAssembly, true);

if (asm.HasClass("Tests.ExtensibilityTest"))
{
    var cls = asm.GetClass("Tests.ExtensibilityTest")!;
    string? output = null;

    if (cls.HasMethod("TestWith1Arg")) 
        output = cls.GetMethod("TestWith1Arg")!.Invoke<string>("Hello!");

    AbysmalDebug.Log(cls, output ?? "error!", true);
}
```

---

<div align="center">
  <img width="256" height="256" alt="AbysmalCore.Debugging" src="https://github.com/Dismalitie/AbysmalCore/blob/master/images/debugging.png?raw=true" />
</div>

# `AbysmalCore.Debugging`

A standardized, opinionated formatter for console logging and error throwing.

## Example

```cs
int layerDivisor = 3;
int coreDivisor = 5;

Color layer = new(c.R / layerDivisor, c.G / layerDivisor, c.B / layerDivisor);
AbysmalDebug.Log(this, $"Generated layer color {layer} from {c}");
Color core = new(c.R / coreDivisor, c.G / coreDivisor, c.B / coreDivisor);
AbysmalDebug.Log(this, $"Generated base color {core} from {c}");
```

---

<div align="center">
  <img width="256" height="256" alt="AbysmalCore.UI" src="https://github.com/Dismalitie/AbysmalCore/blob/master/images/ui.png?raw=true" />
</div>

# `AbysmalCore.UI`

A standardized, opinionated design for Graphical User Interfaces (GUIs) based off raylib.

## Example

```cs
Window w = new(new(500, 500), typeof(Window).FullName!);
UserInterface ui = new()
{
    Elements =
    {
        new Button("hello!", new(10,10))
        {
            Name = "btn",
            StyleMap = new(true)
        }
    }
};
ui.GetElement("btn")?.OnClicked += (UIElement sender, Vector2Int mouse, int frame) =>
{
    AbysmalDebug.Log(sender, $"Clicked on frame {frame}!");
};

w.Init(ui);
```
