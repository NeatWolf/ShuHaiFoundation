# ShuHaiFoundation

Foundation libraries for ShuHai (Unity) projects.

## ShuHai.Core

Foundation library without any Unity assembly references that can be used without Unity.

The library is built for following pupose:

- Unity related types that doesn't require Unity assembly references, such as UnityVersion, HistoricalUnityVersions and
  PathUtil with Unity specialized separators, etc.

- Convenient functionalities and workarounds for common C# programming, most of which are just workarounds for .Net 3.5
  since Unity uses .Net 3.5 profile.

## ShuHai.Unity.Core

Foundation library that requries Unity assemblies for ShuHai Unity projects.
The library contains commonly used extensions and utilities for Unity projects.

## ShuHai.Unity.PackageSetup

Tools that automantically deploy unity plugins.
If any plugin projects are built for muliple Unity versions and contains multiple assemblies for each unity version, the
PackageSetup tool auomantically select the most appropriate version of the plugin and deploy it to unity project.