# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build

Build the solution (x86, Release):
```
msbuild "Constraint Workbench.sln" /p:Configuration=Release /p:Platform=x86
```

Build Debug (AnyCPU for development):
```
msbuild "Constraint Workbench.sln" /p:Configuration=Debug /p:Platform=AnyCPU
```

## Tests

Tests use NUnit 3. Build first, then run with the NUnit console runner:
```
nunit3-console tests\Workbench.Core.Tests.Unit\bin\Release\Workbench.Core.Tests.Unit.dll
nunit3-console tests\Workbench.UI.Tests.Unit\bin\Release\Workbench.UI.Tests.Unit.dll
nunit3-console tests\Workbench.Core.Tests.Integration\bin\Release\Workbench.UI.Tests.Integration.dll
```

Run a single test fixture:
```
nunit3-console <dll> --where "class == MyTestFixture"
```

## Architecture

Two-project core structure:

- **`src/Workbench.Core`** — domain model and solver. No UI dependencies.
- **`src/Workbench.UI`** — WPF desktop app using Caliburn.Micro (MVVM) and Castle.Windsor (DI).

### Domain model hierarchy (`src/Workbench.Core/Models/`)

`WorkspaceModel` is the composition root. It contains:
- **`ModelModel`** (extends `BundleModel`) — the constraint satisfaction problem definition
- **`SolutionModel`** — snapshot of the last solve result
- **`DisplayModel`** — visualizer configuration

**`BundleModel`** is the key recursive container: it holds singleton variables, aggregate variables, bucket variables, constraints, shared domains, and nested bundles. `ModelModel` extends it.

Variable types:
- `SingletonVariableModel` — single value with a domain
- `AggregateVariableModel` — fixed-size array of variables
- `BucketVariableModel` — container for bundle instances (cardinality)

Constraint types:
- `ExpressionConstraintModel` — user-entered expression (parsed by Irony)
- `AllDifferentConstraintModel` — all values differ

### Solving (`src/Workbench.Core/Solvers/`)

`OrToolsSolver` implements `ISolvable`. `OrModelConverter` maps the domain model to the Google OR-Tools CP-SAT API before solving. Domains and expressions are evaluated via Irony-based grammars in `src/Workbench.Core/Grammars/`.

### UI patterns

- **MVVM** via Caliburn.Micro — ViewModels in `src/Workbench.UI/ViewModels/`
- **DI** via Castle.Windsor — configured in `src/Workbench.UI/Bootstrapper/`
- **Event aggregation** (Caliburn) for loosely-coupled component communication (e.g., `ModelSolvedMessage`, `VariableRenamedMessage`)
- **XML serialization** for `.dpf` workspace files — readers/writers in `src/Workbench.UI/Services/`

### Expression parsing

User-facing expressions (constraint expressions, domain ranges, visualizer bindings) are parsed via Irony grammars in `src/Workbench.Core/Grammars/`. They produce ASTs that builders (`AllDifferentConstraintBuilder`, `ExpressionConstraintBuilder`, etc.) convert to domain model objects.

## Key files

| Purpose | Path |
|---|---|
| Workspace root model | `src/Workbench.Core/Models/WorkspaceModel.cs` |
| Bundle/model container | `src/Workbench.Core/Models/BundleModel.cs` |
| OR-Tools solver | `src/Workbench.Core/Solvers/OrToolsSolver.cs` |
| Model→OR-Tools mapping | `src/Workbench.Core/Solvers/OrModelConverter.cs` |
| DI bootstrapper | `src/Workbench.UI/Bootstrapper/` |
| Sample problem (N-queens) | `Samples/nqueens.dpf` |

## Platform notes

- Target: .NET Framework 4.8 (not .NET Core/5+)
- Distribution build: x86 (32-bit, required by Google.OrTools.x86 NuGet package)
- Installer: WiX v3 project in `installer/Workbench.UI.Wix/`
