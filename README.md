# BenjcoreUtil
[![NuGet](https://img.shields.io/nuget/vpre/BenjcoreUtil.svg)](https://www.nuget.org/packages/BenjcoreUtil)

### What is it?

A C# library designed to make some common programming tasks easier.

### Some Features Include :

- Versioning System
- A Simple yet Powerful Logger
- RSA Encryption
- Common Hashing Algorithms

### Logger Example

```csharp
using BenjcoreUtil.Logging;

var levels = new LogLevel[]
{
    new("FATAL", 0, LogSettings.PrintAndLog),
    new("ERROR", 1, LogSettings.PrintAndLog),
    new("WARN", 2, LogSettings.JustLog),
    new("INFO", 3, LogSettings.JustLog),
    new("DEBUG", 4, LogSettings.Nothing)
};

var logger = new Logger
(
    levels,
    "[%l] %t{yy/MM/dd HH:mm:ss} : ",
    "log.txt"
);

logger.Log("INFO", "Hello World!");
```
#### Output:
![Output Image](https://i.imgur.com/PlSuCQt.png)

### Versioning Example

```csharp
using BenjcoreUtil.Versioning;

var version1 = SimpleVersion.Parse("1.2.3");
var version2 = SimpleVersion.Parse("2.0.1");

// Compare with operators
bool result1 = version1 > version2; // false

// Compare with methods
bool result2 = version1.IsNewerThan(version2); // false
```

### Hashing Example

```csharp
using BenjcoreUtil.Security.Hashing;

// Output: 7F83B1657FF1FC53B92DC18148A1D65DFC2D4B1FA3D677284ADDD200126D9069
string hash = SHA256.GetSHA256("Hello World!");
Console.WriteLine(hash);
```

### License
Version 2.1.0 and newer are licensed under the [MIT License](LICENSE)<br>
Older versions are all rights reserved. (You wouldn't want to use them anyway)<br>
Copyright © Benj_2005 / Benjcore 2022