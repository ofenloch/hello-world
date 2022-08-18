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