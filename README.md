# hello-world with C#

This is a simple demo to show how to set up a larger C# project without Visual Studio. I did this on my Debian machine with bash and VS Code.

The entire information about how to use the .NET command line interface **dotnet** is available at [https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet).


## Create A Solution File And Two Project Files

When we creat a project with `dotnet new console --language C# --name hello-world` a project file
*hello-world.csproj* is generated. It looks like this:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <RootNamespace>hello_world</RootNamespace>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```


Executing `dotnet new sln`in our project folder creates a new, empty solution file *hello-world.sln*. The file looks like this:

```s
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 16
VisualStudioVersion = 16.0.30114.105
MinimumVisualStudioVersion = 10.0.40219.1
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal
```

If we execute `dotnet sln list` we get "No projects found in the solution.".

Execute `dotnet sln add ./` to add the existing project in file *hello-world.csproj* to the solution.

Now `dotnet sln list` shows our project in the solution:

```bash
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ dotnet sln list
Project(s)
----------
hello-world.csproj
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$
```

The commands `dotnet build` and `dotnet run` still work as before.

Let's create a new project for our Library:

Execute `dotnet new classlib --name library --output ./lib/` in the project's root directory. This
creates two files:

* a new project file *lib/library.csproj*
* a new, empty source file *lib/Class1.cs*

Add the new project with `dotnet sln add ./lib/`:

```bash
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ dotnet sln add ./lib/
Project `lib/library.csproj` added to the solution.
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ 
```

We check with `dotnet sln list`:

```bash
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ dotnet sln list
Project(s)
----------
hello-world.csproj
lib/library.csproj
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$
```

To avoid that the main project in file *hello-world.csproj* uses all sources in the project tree, we have to add

    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>

in the <PropertyGroup> and add a new entry

    <ItemGroup>
        <Compile Include = "src/*.cs"/>
    </ItemGroup>

If we don't do this, we get errors like 

    error CS0579: Duplicate 'System.Reflection.AssemblyCompanyAttribute' attribute  ...

Our project file *hello-world.csproj* now looks like this

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <RootNamespace>hello_world</RootNamespace>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <!-- set EnableDefaultCompileItems to false -->
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
  </PropertyGroup>

  <!-- specify the source files to be used for this project -->
  <ItemGroup>
    <Compile Include="src/*.cs" />
  </ItemGroup>

</Project>
```

Better safe than sorry: We do the same with the project file *lib/library.csproj*:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <!-- set EnableDefaultCompileItems to false -->
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
  </PropertyGroup>

  <!-- specify the source files to be used for this project -->
  <ItemGroup>
    <Compile Include="./*.cs" />
  </ItemGroup>

</Project>
```

The only not working is to tell out main project in file *hello-world.csproj* how to find the library in directory ./lib/.


The docs tell us to execute `dotnet add ./hello-world.csproj reference lib/library.csproj`

```bash
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ dotnet add ./hello-world.csproj reference lib/library.csproj
Reference `lib\library.csproj` added to the project.
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ 
```

Running `dotnet build`in the project's root looks promising. But we get some "privacy issues" like

    error CS0122: 'DataStore<TKey, TValue>' is inaccessible due to its protection level ...


To resolve them, we have to declare the classes in our library project as public. At least the ones we want to use outside their namespace.


## Adding Unit Tests


The docs at [https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test) say we should run `dotnet new xunit -n unit-tests -o ./test/` in the project's root. This creates the folder ./test/ with two new files:

* test/unit-tests.csproj
* test/UnitTest1.cs

```bash
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ dotnet new xunit -n unit-tests -o ./test/
The template "xUnit Test Project" was created successfully.

Processing post-creation actions...
Running 'dotnet restore' on /home/ofenloch/workspaces/dotnet/hello-world/test/unit-tests.csproj...
  Determining projects to restore...
  Restored /home/ofenloch/workspaces/dotnet/hello-world/test/unit-tests.csproj (in 7.42 sec).
Restore succeeded.

ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$
```
The other files - especially the solution file *create-dotnet-project.sh* - are not affected by this command.

With the things learnt above in mind, we modify the unit test project file *test/unit-tests.csproj*:

* we specify the source to be included
* we add a ProjectReference to our library (because that's what we want to test)

The new - modified - project file *test/unit-tests.csproj* looks like this

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <RootNamespace>unit_tests</RootNamespace>
    <Nullable>enable</Nullable>

    <IsPackable>false</IsPackable>
    <!-- set EnableDefaultCompileItems to false -->
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.11.0" />
    <PackageReference Include="xunit" Version="2.4.1" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.3">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="coverlet.collector" Version="3.1.0">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
  </ItemGroup>

  <!-- specify the source files to be used for this project -->
  <ItemGroup>
    <Compile Include="./*.cs" />
  </ItemGroup>

  <!-- add a reference to our library -->
  <ItemGroup>
    <ProjectReference Include="..\lib\library.csproj" />
  </ItemGroup>

</Project>
```

Of cource we add the unit test project to our solution file *hello-world.sln* with `dotnet sln add ./test/unit-tests.csproj`:

```bash
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ dotnet sln add ./test/unit-tests.csproj 
Project `test/unit-tests.csproj` added to the solution.
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$
```

To run our test(s) we simple execute `dotnet test`:

```bash
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ dotnet test
  Determining projects to restore...
  All projects are up-to-date for restore.
  library -> /home/ofenloch/workspaces/dotnet/hello-world/lib/bin/Debug/net6.0/library.dll
  unit-tests -> /home/ofenloch/workspaces/dotnet/hello-world/test/bin/Debug/net6.0/unit-tests.dll
Test run for /home/ofenloch/workspaces/dotnet/hello-world/test/bin/Debug/net6.0/unit-tests.dll (.NETCoreApp,Version=v6.0)
Microsoft (R) Test Execution Command Line Tool Version 17.1.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: < 1 ms - /home/ofenloch/workspaces/dotnet/hello-world/test/bin/Debug/net6.0/unit-tests.dll (net6.0)
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$
```

## Add Logging

First of all we add the required packages to out project files:

* `dotnet add package Microsoft.Extensions.Logging.Console`

* `dotnet add lib/library.csproj package Microsoft.Extensions.Logging.Console`

* `dotnet add test/unit-tests.csproj package Microsoft.Extensions.Logging.Console`

The we add some logging code to out Main in file *src/HelloWorld.cs*:

```C#
            using ILoggerFactory loggerFactory =
                LoggerFactory.Create(builder =>
                    builder.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = true;
                        options.SingleLine = true;
                        options.TimestampFormat = "hh:mm:ss ";
                    }));

            ILogger<HelloWorld> logger = loggerFactory.CreateLogger<HelloWorld>();
            using (logger.BeginScope("[scope is enabled]"))
            {
                logger.LogInformation("Logs contain timestamp and log level.");
                logger.LogInformation("Each log message is fit in a single line.");
                logger.LogTrace("Trace");
                logger.LogDebug("Debug");
                logger.LogInformation("Info");
                logger.LogWarning("Warning");
                logger.LogError("Error");
                logger.LogCritical("Critical");

            }
```

This sample was stolen from [https://github.com/dotnet/docs/blob/main/docs/core/extensions/snippets/logging/console-formatter-simple/Program.cs](https://github.com/dotnet/docs/blob/main/docs/core/extensions/snippets/logging/console-formatter-simple/Program.cs).


Now, we have a logger logging to the console at log level INFO:

```bash
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ dotnet run
Hello, World!
06:49:10 info: MyApp.HelloWorld[0] => [scope is enabled] Logs contain timestamp and log level.
06:49:10 info: MyApp.HelloWorld[0] => [scope is enabled] Each log message is fit in a single line.
06:49:10 info: MyApp.HelloWorld[0] => [scope is enabled] Info
06:49:10 warn: MyApp.HelloWorld[0] => [scope is enabled] Warning
06:49:10 fail: MyApp.HelloWorld[0] => [scope is enabled] Error
06:49:10 crit: MyApp.HelloWorld[0] => [scope is enabled] Critical
1.234 plus 4.321 makes 5.555
1.234 times 4.321 makes 5.332114
idx 42: key 42, value This is element 42.
idx 100: no such element in DataStore
idx 101: no such element in DataStore
idx 102: no such element in DataStore
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$
```

After browsin the MS docs for a while, I decided that this is too cumbersome. I just want 
to write to a log file, and I don't want to implement my own Logger Provider. So I switched to 
[NLog](https://github.com/NLog/).

Within minutes I was able to write to a log file.

The config file *NLog.config* must be copied to the binary directory (bin/Debug/net6.0/NLog.config in our case). 
The log file is created in the same directory.

The first draft of file *NLog.config* is

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

    <targets>
        <target name="logfile" xsi:type="File" fileName="./hello-world.log" />
        <target name="logconsole" xsi:type="Console" />
    </targets>

    <rules>
        <logger name="*" minlevel="Warn" writeTo="logconsole" />
        <logger name="*" minlevel="Debug" writeTo="logfile" />
    </rules>
</nlog>
```

The C# code is simple, too:

```C#
using NLog;
using NLog.Targets;
using System.Text;

namespace MyApp
{

    internal class HelloWorld
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {

            Logger.Trace("Trace");
            Logger.Debug("Debug");
            Logger.Info("Info");
            Logger.Warn("Warn");
            Logger.Error("Error");
            Logger.Fatal("Fatal");

        }

    } // class HelloWorld

} // namespace MyApp
```

This seems to be what I was looking for.