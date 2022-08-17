#!/bin/bash

THIS_FILE=$(readlink --canonicalize-existing ${0})

PRJ_NAME="hello-world"

if [ ! -z ${1:+x} ]; then
    PRJ_NAME=${1}
fi

echo "creating new .NET project \"${PRJ_NAME}\" ..."


dotnet new console --language C# --name ${PRJ_NAME}

cd ${PRJ_NAME}

cp -vf ${THIS_FILE} .

echo "bin/" >> .gitignore
echo "obj/" >> .gitignore
echo "# ${PRJ_NAME} with C\#" >> README.md

PRJ_NAME_CLEAN=${PRJ_NAME}
# first, strip underscores
PRJ_NAME_CLEAN=${PRJ_NAME_CLEAN//_/}
# next, replace spaces with underscores
PRJ_NAME_CLEAN=${PRJ_NAME_CLEAN// /_}
# now, clean out anything that's not alphanumeric or an underscore
PRJ_NAME_CLEAN=${PRJ_NAME_CLEAN//[^a-zA-Z0-9_]/}
# finally, lowercase with TR
PRJ_NAME_CLEAN=`echo -n ${PRJ_NAME_CLEAN} | tr A-Z a-z`
CSHARP_SOURCE=${PRJ_NAME_CLEAN}.cs
echo "clean project name is \"${PRJ_NAME_CLEAN}\""
echo "C# source file is \"${CSHARP_SOURCE}\""

# delete the generated source Program.cs
rm -vf Program.cs

# create a new source file (.NET 5 style)
echo "// This is a .NET 5 (and earlier) console app template" >> ${CSHARP_SOURCE}
echo "// (See https://aka.ms/new-console-template for more information)" >> ${CSHARP_SOURCE}
echo "" >> ${CSHARP_SOURCE}
echo "using System;" >> ${CSHARP_SOURCE}
echo "" >> ${CSHARP_SOURCE}
echo "namespace MyApp" >> ${CSHARP_SOURCE}
echo "{" >> ${CSHARP_SOURCE}
echo "" >> ${CSHARP_SOURCE}
echo "    internal class ${PRJ_NAME_CLEAN}" >> ${CSHARP_SOURCE}
echo "    {" >> ${CSHARP_SOURCE}
echo "        static void Main(string[] args)" >> ${CSHARP_SOURCE}
echo "        {" >> ${CSHARP_SOURCE}
echo "            Console.WriteLine(\"Hello, World!\");" >> ${CSHARP_SOURCE}
echo "        }" >> ${CSHARP_SOURCE}
echo "" >> ${CSHARP_SOURCE}
echo "    } // class ${PRJ_NAME_CLEAN}" >> ${CSHARP_SOURCE}
echo "" >> ${CSHARP_SOURCE}
echo "} // namespace MyApp" >> ${CSHARP_SOURCE}

# generate an empty solution file
# dotnet new sln --name ${PRJ_NAME}
# TODO:
#   An empty solution file in the project directory
#   confuses VS Code. We must find a way to add at least
#   the created project file to the solution file.

git init
git add .
git commit -a -m"initial commit"
