# MiniTest — Lightweight Unit Test Framework

A custom unit testing framework built from scratch in C# (.NET 8) as part of the "Programming 3 — Advanced" course at Warsaw University of Technology.

The project implements two main components: a test library with custom attributes and assertions, and a test runner that dynamically loads and executes tests from compiled assemblies using reflection.

## How it works

**MiniTest Library** provides attributes to mark test classes and methods (`[TestClass]`, `[TestMethod]`, `[BeforeEach]`, `[AfterEach]`, `[Priority]`, `[DataRow]`, `[Description]`) and an `Assert` class with standard assertions (`AreEqual`, `IsTrue`, `ThrowsException`, etc.).

**MiniTestRunner** is a console app that takes `.dll` paths as arguments, loads them into isolated `AssemblyLoadContext` instances, discovers test classes via reflection, and runs tests in priority order with colored output.

### Features

- Dynamic assembly loading/unloading with `AssemblyLoadContext`
- Parameterized tests via `[DataRow]` attribute
- Test prioritization and alphabetical ordering
- BeforeEach/AfterEach lifecycle hooks bound via delegates
- Colored console output (green/red/yellow) with per-class and total summaries
- Graceful handling of missing constructors and parameter mismatches

## Usage

```bash
dotnet build
MiniTestRunner path/to/test-assembly.dll
```

## Project structure

```
MiniTest/
├── AuthenticationService/         # Library with test attributes, assertions, and sample service
├── AuthenticationService.Tests/   # Sample tests using the framework
└── MiniTestRunner/                # Console app that discovers and runs tests
```

## Tech

C#, .NET 8, System.Reflection, AssemblyLoadContext
