# hello-world with C#

This is a simple demo project demonstrating how to set up a larger C# project without 
Visual Studio. At the end, we'll have this project structure

      workspaceFolder
        ├── data
        │   ├── NLog.config
        │   └── package.json
        ├── hello-world.csproj
        ├── hello-world.sln
        ├── lib
        │   ├── Class1.cs
        │   ├── DataList.cs
        │   ├── library.csproj
        │   ├── MyMath.cs
        │   └── Pair.cs
        ├── README.md
        ├── scripts
        │   ├── create-dotnet-project.sh
        │   ├── make-clean.sh
        │   └── package.sh
        ├── src
        │   └── HelloWorld.cs
        └── test
            ├── UnitTest1.cs
            └── unit-tests.csproj


I did this on my Debian machine with bash and VS Code.

The entire information about how to use the .NET command line interface **dotnet** 
is available at [https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet).

## Creating A Solution File And Two Project Files

When we create a project with `dotnet new console --language C# --name hello-world` a project file
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


Executing `dotnet new sln` in our project folder creates a new, empty solution file 
*hello-world.sln*. The file looks like this:

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
```xml
    <ItemGroup>
        <Compile Include = "src/*.cs"/>
    </ItemGroup>
```
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

Of course we add the unit test project to our solution file *hello-world.sln* with `dotnet sln add ./test/unit-tests.csproj`:

```bash
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ dotnet sln add ./test/unit-tests.csproj 
Project `test/unit-tests.csproj` added to the solution.
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$
```

To run our test(s) we simply execute `dotnet test`:

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

## Adding Logging

First of all we add the required packages to out project files:

* `dotnet add package Microsoft.Extensions.Logging.Console`

* `dotnet add lib/library.csproj package Microsoft.Extensions.Logging.Console`

* `dotnet add test/unit-tests.csproj package Microsoft.Extensions.Logging.Console`

Then we add some logging code to our Main in file *src/HelloWorld.cs*:

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

What I really want is a logger writing to a file.

After browsing the MS docs for a while, I decided that this is too cumbersome. I found a lot of stuff about 
"Logger Provider" and "Extending Logger" but nothing logging to a file. I just want 
to write to a log file, and I don't want to implement my own Logger Provider. 

**So, I decided to use [NLog](https://nlog-project.org/) instead.**

Within minutes I was able to write to a log file.

The config file *NLog.config* has to be copied to the binary directory (bin/Debug/net6.0/NLog.config in our case). 
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

This seems to be what I was looking for...

After some research, I found this solution for the logger's proper configuration:

* set up the project to copy the *NLog.config* to the output directory by adding
  ```xml
  <!-- copy file NLog.config to target directory -->
  <ItemGroup >
    <None Update="NLog.config" CopyToOutputDirectory="PreserveNewest" />
  </ItemGroup>
  ```
* set environment variable LOGDIR to a proper value, e.g. in *.vscode/launch.json*:
  ```json
  "environment": [
      { "name": "LOGDIR", "value" : "${workspaceFolder}" }
  ],
   ```
* use environment variable LOGDIR in *NLog.config*, e.g.
  ```xml
  <target name="logfile" xsi:type="File" fileName="${environment:LOGDIR}/hello-world.log" />
  ```

So, building the application copies the logger configuration file *NLog.config* to the target 
directory. The launch configuration sets the environment variable LOGDIR to our project's root 
directory. And the program itself logs to file *${environment:LOGDIR}/hello-world.log*.

Without this setup (or an equivalent one) you have to do this "manually":

`dotnet build`

`/bin/cp -f NLog.config ./bin/Debug/net6.0/`

`LOGDIR=$(pwd) dotnet ./bin/Debug/net6.0/hello-world.dll`

## Publishing The App (Deployment)

Running the command 

`dotnet publish -c Release` 

builds the solution and creates a the binary
*bin/Release/net6.0/publish/hello-world*. To execute this binary we simply run 

`./bin/Release/net6.0/publish/hello-world`  

or `LOGDIR=$(pwd) bin/Release/net6.0/publish/hello-world` 
if we want to set the log directory.

The [docs](https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli) say there's 
three ways of publishing an app:

* **Framework-dependent deployment** produces a cross-platform .dll file that uses the locally installed .NET runtime. 
* **Framework-dependent executable** produces a platform-specific executable that uses the locally installed .NET runtime. 
* **Self-contained executable** produces a platform-specific executable and includes a local copy of the .NET runtime.

(see [.NET application publishing overview](https://docs.microsoft.com/en-us/dotnet/core/deploying/) for more details)

So, running `dotnet publish -r linux-x64 --self-contained -c Release` produces a "self-contained" app for a 64Bit Linux 
system in folder *./bin/Release/net6.0/linux-x64/publish/*. I put the "self-contained" in quotes because the app seems 
to need all files in this folder.

Running `dotnet publish -r win10-x64 --self-contained -c Release` does the same for a Windows 10 (or 11) system. The 
executable is *./bin/Release/net6.0/win10-x64/publish/hello-world.exe*. I assume the entire folder is needed, too, 
but I'd have to verify this on a Windows machine.

The various Runtime IDs (RIDs) are listed in the [.NET RID Catalog](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog).

Adding

```xml
  <!-- create a single binar file that contains all we need to run the app -->
  <PropertyGroup>
    <PublishSingleFile>true</PublishSingleFile>
  </PropertyGroup>
```

to the project file *hello-world.csproj* enables us to create a single file that can be run without an other dependencies.

The command `dotnet publish -r linux-x64 --self-contained true -c Release` creates the file 
*./bin/Release/net6.0/linux-x64/publish/hello-world* which can be run as a stand-alone binary.

We copy the stand-alone binary to an empty directory and try to run it. Of course we have to copy the logger 
config *NLog.config*, too, if we want logging to work properly.

```bash
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ /bin/cp bin/Release/net6.0/linux-x64/publish/hello-world ~/tmp/
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ /bin/cp NLog.config ~/tmp/
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ LOGDIR=~/tmp/ ~/tmp/hello-world 
Hello, World!
1.234 plus 4.321 makes 5.555
1.234 times 4.321 makes 5.332114
idx 42: key 42, value This is element 42.
2022-08-20 08:42:47.5491|WARN|MyApp.HelloWorld|idx 100: no such element in DataStore
idx 100: no such element in DataStore
2022-08-20 08:42:47.5491|WARN|MyApp.HelloWorld|idx 101: no such element in DataStore
idx 101: no such element in DataStore
2022-08-20 08:42:47.5491|WARN|MyApp.HelloWorld|idx 102: no such element in DataStore
idx 102: no such element in DataStore
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$ ll ~/tmp/
total 63M
drwxr-xr-x  2 ofenloch teben 4.0K 2022-08-20--08-42-47 ./
drwxr-xr-x 17 ofenloch teben 4.0K 2022-08-19--03-53-44 ../
-rwxr-xr-x  1 ofenloch teben  63M 2022-08-20--08-37-56 hello-world*
-rw-r--r--  1 ofenloch teben  704 2022-08-20--08-42-47 hello-world.log
-rw-r--r--  1 ofenloch teben  912 2022-08-20--08-42-42 NLog.config
ofenloch@3fb1caa5b6d0:~/workspaces/dotnet/hello-world$
```

To produce a binary for Windows 10 (and 11), we execute `dotnet publish -r win10-x64 --self-contained true -c Release`. Again: I still have to test this binary on my Windows machine.

So, a simple deployment / packaging script could look like this

```bash
#!/bin/bash

DIST_DIR=~/tmp/HelloWorld

/bin/rm -rf ${DIST_DIR}
/usr/bin/mkdir -p ${DIST_DIR}/linux-x64
/usr/bin/mkdir -p ${DIST_DIR}/win10-x64

dotnet publish -r linux-x64 --self-contained true -c Release
/bin/cp -f bin/Release/net6.0/linux-x64/publish/hello-world ${DIST_DIR}/linux-x64
/bin/cp -f NLog.config ${DIST_DIR}/linux-x64

dotnet publish -r win10-x64 --self-contained true -c Release
/bin/cp -f bin/Release/net6.0/win10-x64/publish/hello-world.exe ${DIST_DIR}/win10-x64
/bin/cp -f NLog.config ${DIST_DIR}/win10-x64

cat << _END_OF_README_MD_ > ${DIST_DIR}/README.md
# Hello World With .NET and C#

This is a simple demo project demonstrating how to set up a 
larger C# project with .NET CLI (without Visual Studio).

There is a [GitHub repository](https://github.com/ofenloch/hello-world.git) with all source files.

_END_OF_README_MD_
```

This would produce the distribution directory *~/tmp/HelloWorld* with this contents:

      HelloWorld
          ├── linux-x64
          │   ├── hello-world
          │   └── NLog.config
          ├── README.md
          └── win10-x64
              ├── hello-world.exe
              └── NLog.config

This directory can be used for the package mechanism (e.g. tar -czf ..., or npm ... ).

Now we move the logger configuration to directory ./data/ in our project's root folder. Adjusting the 
ItemGroup with the Update command to
```xml
    <ItemGroup>
      <None Update="./data/NLog.config" CopyToOutputDirectory="PreserveNewest" />
    </ItemGroup>
```

copies the config file to *\$(OutDir)/data/NLog.config* instead of *\$(OutDir)/NLog.config*.

To get the logger's config file to the correct location, we define a new Target
```xml
  <!-- copy file ./data/NLog.config to $(OutDir)/NLog.config -->
  <Target Name="CopyFiles">
    <Copy SourceFiles="./data/NLog.config" DestinationFolder="$(OutDir)" />
  </Target>
```

For `dotnet build` "building" this target, it must be listet as Default target in the Project:
```xml
<Project DefaultTargets="Build;CopyFiles" Sdk="Microsoft.NET.Sdk">
```


## NuGet Errors "error NU1100: "..." kann für "net6.0" nicht aufgelöst werden."

Sometimes there are errors about dotnet not being able to resolve packages. 
[Stackoverflow suggests](https://stackoverflow.com/questions/68283730/error-nu1100-unable-to-resolve-microsoftofficecore-15-0-0-for-net5-0) 
executing `dotnet nuget locals all --clear` and/or deleting NuGet's configuration file *C:\Users\<username>\AppData\Roaming\NuGet* and then re-running `dotnet restore`.

## Dockerizing the App

We start with the Dockerfile from the MS [Tutorial: Containerize a .NET app](https://docs.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=linux):

```Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]
```

For our project we have to change the line with the 'publish' command. It must be

    RUN dotnet publish -r linux-x64 --self-contained true -c Release -o out

instead of 

    RUN dotnet publish -c Release -o out

because we build a single, self-contained binary. Further, we have to change the ENTRYPOINT to

    ENTRYPOINT ["./hello-world"]

so our binary *hello-world* is executed.

To run our unit tests, we add the line

    RUN dotnet test
  
in front of the publish command.

The complete Dockerfile now looks like this

```Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Run unit tests (if the tests fail the build process is stopped)
RUN dotnet test
# Build and publish a release
RUN dotnet publish -r linux-x64 --self-contained true -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["./hello-world"]
```

If the tests fail the command `RUN dotnet test` returns a non-zero value and the build process is stopped. This way we make sure the app is only published if the unit tests succeed. (That's why we fix the test in file *test/UnitTest1.cs* now.)

With the file *.dockerignore* we exclude files from being sent to the Docker daemon for the build process. In this case it's only a couple of things that are excluded. But there may be cases where you want to keep your image as small as possible.


### Cleaning Up Docker

Once we've been developing images and containers for a while, there will be leftovers we don't need any more. To clean them we use the Docker CLI.

* **Dangling Images** are images that are not tagged and not referenced by any container. To remove dangling images we execute `docker image prune`

* When you stop a container, it is not automatically removed unless you started it with the `--rm` flag. To see all containers on the Docker host, including stopped containers, use `docker ps -a`. You may be surprised how many **unused containers** exist, especially on a development system! A stopped container’s writable layers still take up disk space. To clean this up, you can use the docker container prune command: `docker container prune`. Be carful: This will remove all stopped containers.