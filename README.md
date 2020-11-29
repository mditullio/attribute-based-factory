# attribute-based-factory

This sample shows how to create an Object Factory based on Attributes in .NET.

The factory will provide **IPrinter** objects, detecting the correct subclass to instantiate by PrinterId using Reflection.

This pattern allows the factory to create a new IPrinter object without having to know the needed subclass, which can be in another assembly.
