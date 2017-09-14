# RandomType
> RandomType is a C# class library to generate random models/types

Ideal to generate data for tests, or test application fragility, in relation to the entry of the most diverse values

## Installation

With package manager console, run:

```shell
Install-Package RandomType
```

The code above will install the latest release, and all dependencies needed

## Getting Started

Nothing complicated here, simply a line to generate a random model

```cs
var randomModel = RandomTypeGenerator.Generate<YourType>();
```

If you need a list
```cs
var randomModel = RandomTypeGenerator.GenerateList<YourType>();
```
The code above will create a list with a random number of elements, **within the configured limits**. To understand how to configure, just read the section below

### Range settings

If you need more specific random values, you can set the minimum and maximum values for some data types

```cs
var randomModel = RandomTypeGenerator.GenerateList<YourType>(settings =>
{
  settings.Min.Int32 = 1;
  settings.Max.Int32 = 50;
				
  //Number of chars
  settings.Min.String = 5;
  settings.Max.String = 30;

  settings.Min.ListSize = 0;
  settings.Max.ListSize = 50;
});
```

These are just a few examples of the settings that can be made

### Property Types

Not all types are available yet, are being implemented over time. Below are all types that are ready:
* String
* Int32
* Bool
* Double
* Date
* Decimal
* Int64
* TimeSpan
* Byte
* Char
* Float
* Enum
* Arrays & collections of all above except Enum

**If the model has other models inside it, or lists, they will also be generated randomly**

## Contributing

If you feel the need for new types, or find a bug
1. Pull requests are always welcome (you can fork the repository aswell)
2. [Issues][issues] created will help me to set some priority
