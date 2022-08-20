# Media-Compressor

Compress audio losslessly, and compress image lossy.

# Tech
- Linear Predictive Coding
- Rice Coding
- DCT transformation and quantization

# How to compile:
## Supported Platform:
    Windows 10 
## Prerequisite:
    Visual Studio 2019
    Microsoft .Net Framework 4.7.2
    MSBuild 16.0

## Compile with Visual Studio 2019: 
    Open Project2.sln with Visual Studio 2019
    Press F5 to compile and run

## Compile with MSBuild.exe
    {path-to-msbuild-16.0}\MSBuild.exe Project2.csproj /t:build /clp:ShowCommandLine /property:Configuration=Release
About MSBuild 16.0 path, see https://docs.microsoft.com/en-us/visualstudio/msbuild/whats-new-msbuild-16-0?view=vs-2019
### Example
    & "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" Project2.csproj /t:build /clp:ShowCommandLine /property:Configuration=Release

# How to run:
    Double click Project2.exe *See demo video*
