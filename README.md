# Husky.Net Demo — ASP.NET Core Web API

A demo project showcasing **Husky.Net** git hooks integration with an ASP.NET Core Web API. The hooks enforce code quality, build integrity, and a consistent commit message convention automatically on every commit.

---

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or later
- [Husky.Net](https://alirezanet.github.io/Husky.Net/) (`dotnet tool install husky -g`)
- Git

---

## Getting Started

```bash
# Clone the repo
git clone <repo-url>
cd husky

# Restore dependencies and install Husky hooks
dotnet restore
husky install

# Run the API
dotnet run --project webapi
```

---

## Branch Naming Convention

Branches must follow the pattern `ticket/<TICKET-ID>` so the hooks can extract the ticket number automatically.

```
ticket/KBOLAPP2-20
ticket/PROJ-42
```

---

## Commit Message Format

Every commit message must follow this pattern:

```
[<TICKET-ID>] <type>(<scope>): <description>
```

| Part | Description | Example |
|---|---|---|
| `TICKET-ID` | Jira/issue ticket (uppercase) | `KBOLAPP2-20` |
| `type` | Change category | `feat`, `fix`, `docs`, … |
| `scope` | Affected module | `auth`, `order`, `login` |
| `description` | Short summary | `add JWT refresh token` |

**Allowed types:** `feat` · `fix` · `docs` · `style` · `refactor` · `test` · `chore` · `perf` · `ci` · `build` · `revert`

### Examples

```bash
# Good commits
git commit -m "feat(auth): add JWT refresh token"
git commit -m "fix(order): handle null customer id"
git commit -m "refactor(login): extract token validation logic"
git commit -m "test(health): add endpoint integration tests"
git commit -m "chore(ci): update build pipeline"

# Bad commits — will be rejected by commit-msg hook
git commit -m "fix stuff"
git commit -m "WIP"
git commit -m "feat: missing scope"
```

> **Tip:** When your branch is named `ticket/KBOLAPP2-20`, the `prepare-commit-msg` hook automatically prepends `[KBOLAPP2-20]` to your message — so you only need to type the part after the ticket prefix:
>
> ```bash
> git commit -m "feat(auth): add JWT refresh token"
> # → commit message becomes: [KBOLAPP2-20] feat(auth): add JWT refresh token
> ```

---

## Git Hooks Overview

### `prepare-commit-msg`
Automatically injects the ticket ID from the branch name into the commit message.

```
Branch: ticket/KBOLAPP2-20
You type: feat(login): add OTP screen
Result:  [KBOLAPP2-20] feat(login): add OTP screen
```

### `commit-msg`
Validates the final commit message against the required pattern. Rejects commits that don't conform.

```
❌ Invalid commit message format!

   Expected:[<ticket>] <type>(<scope>): <description>
   Example:[KBOLAPP2-1] feat(auth): add JWT refresh token
   Example:[KBOLAPP2-1] fix(order): handle null customer id

   Types: feat | fix | docs | style | refactor | test | chore | perf | ci | build | revert
```

### `pre-commit`
Runs three quality gates on every commit (only against staged `.cs` files when possible):

1. **Format check** — `dotnet format --verify-no-changes`
2. **Build** — fails on compiler errors or warnings in staged files
3. **Unit tests** — runs the full test suite via `husky run --name test`

```
🔍 Running pre-commit checks...
📐 Checking code format... ✅ Format OK
🔨 Building solution...    ✅ Build OK
🧪 Running unit tests...   ✅ Tests OK
✅ All checks passed!
```

### `post-commit`
Prints a summary banner after a successful commit.

```
╔════════════════════════════════════════╗
║         ✅  Commit Successful!          ║
╚════════════════════════════════════════╝

  🌿 Branch  : ticket/KBOLAPP2-20
  🔖 Hash    : f81bfd4
  💬 Message : [KBOLAPP2-20] feat(login): add OTP screen

  🚀 Keep up the good work!
```

---

## Running Checks Manually

```bash
# Format check
dotnet format webapi.sln --verify-no-changes --severity warn

# Auto-fix formatting
dotnet format webapi.sln

# Build
dotnet build webapi.sln

# Tests
dotnet test webapi.sln --verbosity normal

# Run a specific Husky task
dotnet husky run --name test
```

---

## Project Structure

```
husky/
├── .husky/
│   ├── commit-msg          # Validates commit message format
│   ├── prepare-commit-msg  # Auto-injects ticket ID from branch name
│   ├── pre-commit          # Format + build + test gate
│   ├── post-commit         # Success banner
│   └── task-runner.json    # Husky task definitions
├── webapi/                 # ASP.NET Core Web API
│   ├── Endpoints/
│   │   └── HealthEndpoints.cs
│   └── Program.cs
├── webapi.Tests/           # xUnit integration tests
└── webapi.sln
```

---

## API Endpoints

| Method | Path | Description |
|---|---|---|
| GET | `/` | Hello World |
| GET | `/user` | User endpoint |
| GET | `/{id}/user` | User by ID |
| GET | `/health` | Health check |
