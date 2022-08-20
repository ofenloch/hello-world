#!/bin/bash

DIST_DIR=~/tmp/HelloWorld

/bin/rm -rf ${DIST_DIR}
/usr/bin/mkdir -p ${DIST_DIR}/linux-x64
/usr/bin/mkdir -p ${DIST_DIR}/win10-x64

cat << _END_OF_README_MD_ > ${DIST_DIR}/README.md
# Hello World With .NET and C#

This is a simple demo project demonstrating how to set up a 
larger C# project with .NET CLI (without Visual Studio).

There is a [GitHub repository](https://github.com/ofenloch/hello-world.git) with all source files.

_END_OF_README_MD_

dotnet publish -r linux-x64 --self-contained true -c Release
/bin/cp -f bin/Release/net6.0/linux-x64/publish/hello-world ${DIST_DIR}/linux-x64
/bin/cp -f NLog.config ${DIST_DIR}/linux-x64

dotnet publish -r win10-x64 --self-contained true -c Release
/bin/cp -f bin/Release/net6.0/win10-x64/publish/hello-world.exe ${DIST_DIR}/win10-x64
/bin/cp -f NLog.config ${DIST_DIR}/win10-x64