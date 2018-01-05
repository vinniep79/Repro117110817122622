This solution is a basic repro of Microsoft support issue REG:117110817122622

An Azure Worker Role (WorkerRole1) references a class library (FeedAccess) that 
in turn references a handful of NuGet packages.

When the worker role is run, it'll make a single HTTP request to demonstrate 
that everything is working.

The worker role can be run in 2 ways:
1.  Running ConsoleApp1 will invoke it and trace to the console UI.  This works as expected.
2.  Running the Repro117110817122622 cloud project in the Azure Cloud Compule Emulator.  This fails.

Note that the failure message is slightly different than we were seeing in the actual support ticket.
This solution fails in the IDE debugger with:

System.IO.FileLoadException: 'Could not load file or assembly 'System.Net.Http, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' or one of its dependencies. The located assembly's manifest definition does not match the assembly reference. (Exception from HRESULT: 0x80131040)'

Whereas the main ticket is failing with the following error in the debug output with:

System.TypeLoadException: Unable to load the role entry point due to the following exceptions:
-- System.BadImageFormatException: Could not load file or assembly 'System.Net.Http, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' or one of its dependencies. Reference assemblies should not be loaded for execution.  They can only be loaded in the Reflection-only loader context. (Exception from HRESULT: 0x80131058)
File name: 'System.Net.Http, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' ---> System.BadImageFormatException: Cannot load a reference assembly for execution.

The errors look similar enough that I believe the difference is just due to the added logic in our main solution.