# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

```bash
# Build entire solution
dotnet build GitHubUpdater.sln

# Build specific project
dotnet build src/GitHubUpdater/GitHubUpdater.csproj
dotnet build src/UpdaterAgent/UpdaterAgent.csproj

# Build release
dotnet build -c Release
```

## Architecture

GitHubUpdater is a .NET 8 auto-update library for Windows applications using GitHub Releases as the update source.

### Two-Component Design

1. **GitHubUpdater** (`src/GitHubUpdater/`) - Class library consumed by host applications
   - `Updater.cs` - Main entry point with fluent configuration API (`Updater.Configure().CheckAndUpdateAsync()`)
   - `Providers/GitHubReleaseProvider.cs` - Fetches release info and asset URLs from GitHub API

2. **UpdaterAgent** (`src/UpdaterAgent/`) - Standalone executable that performs the actual file replacement
   - Runs as a separate process to avoid file locking issues
   - Receives ZIP path and target directory as command-line arguments
   - Extracts and overwrites application files

### Update Flow

1. Host app calls `CheckAndUpdateAsync()`
2. `GitHubReleaseProvider` checks `api.github.com/repos/{repo}/releases/latest`
3. If newer version found, downloads `app-win-x64.zip` to temp folder
4. Launches `UpdaterAgent.exe` with ZIP path and app directory
5. Host app exits; UpdaterAgent extracts files and overwrites

### Key Classes

- `UpdaterOptions` - Configuration: `Repository` (e.g., "owner/repo") and `CurrentVersion`
- `GitHubReleaseProvider` - GitHub API client for releases (expects `tag_name` for version, `assets` array for downloads)
