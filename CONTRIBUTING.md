# Firefly Contributing Guidelines <!-- omit in toc -->

Thank you for your interest in contributing to Firefly!

This guide provides an overview of the contribution workflow, from starting a discussion or opening an issue to creating, reviewing, and merging a pull request.

For an overview of the project, see the [README](README.md).

## Start a discussion

If you have a question, idea, or problem, first [search the discussions](../../discussions) to see if it has already been discussed.

If you cannot find an existing discussion, you can [start a new discussion](../../discussions/new/choose).

Discussions are a good place for questions, ideas, design proposals, and topics that are not yet ready to become issues.

## Create an issue

If you find a bug or problem with Firefly, first [search the existing issues](../../issues) to make sure it has not already been reported.

> **NOTE:** If you believe you have discovered a potential security vulnerability, please do not report it publicly. See the [Security Policy](SECURITY.md) for information on reporting security issues.

If no related issue or discussion exists, you can [open a new issue](../../issues/new/choose).

When reporting a bug, please include enough information to reproduce the problem where possible.

## Build and run Firefly

Before contributing code, make sure you can successfully build and run Firefly on your system.

### 1. Fork the repository

Start by creating a fork of the Firefly repository using the **Fork** button on GitHub.

This creates your own copy of the repository where you can make changes.

### 2. Clone your fork

Clone your fork to your computer:

```bash
git clone https://github.com/YOUR-USERNAME/firefly.git
```

Then enter the repository:

```bash
cd firefly
```

### 3. Create a branch

Create a new branch for the change you want to make:

```bash
git checkout -b your-branch-name
```

Use a short, descriptive branch name related to the change you are making.

For example:

```bash
git checkout -b fix-directional-lighting
```

### 4. Restore dependencies

Restore the project's .NET dependencies:

```bash
dotnet restore
```

### 5. Build Firefly

Build the project:

```bash
dotnet build
```

The build should complete successfully before you begin making changes.

If the build fails, review the error messages and make sure the required development tools and dependencies are installed.

### 6. Run Firefly

Run Firefly using the appropriate project:

```bash
cd Firefly.Editor
dotnet run --project Firefly.Editor.csproj
```

You can also open the solution in Visual Studio and build and run Firefly from there.

### 7. Make your changes

Once Firefly builds and runs successfully, you can begin making your changes.

Build and test the project regularly while working:

```bash
dotnet build
```

Before submitting a pull request, make sure Firefly still builds and your changes are very specific to your feature or issue.
