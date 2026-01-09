# GitHubUpdater

> Zero‑config auto‑updates for Windows applications using GitHub Releases.

**GitHubUpdater** is a lightweight .NET library that lets your Windows application update itself automatically. No installers, no ClickOnce, no custom servers.

Add one NuGet package, publish a release on GitHub, and your app stays up to date.

---

## ✨ Why GitHubUpdater?

Updating Windows apps is still painful:

* ClickOnce is legacy and inflexible
* MSIX is heavy and complex
* Existing updaters are outdated or abandoned
* Rolling your own updater is risky (file locks, rollbacks, permissions)

**GitHubUpdater solves this by using GitHub Releases as your update server.**

---

## 🚀 Features

* 🔍 Automatic version checking
* 🐙 GitHub Releases as update source
* 📦 ZIP‑based updates
* 🔁 Safe update process (external updater agent)
* ♻️ Automatic rollback on failure
* 🤫 Silent updates (no UI required)
* 🧩 Simple .NET API (1–2 lines of code)

---

## 📦 Installation

```bash
dotnet add package GitHubUpdater
```

---

## 🧑‍💻 Quick Start

```csharp
using GitHubUpdater;

await Updater
    .Configure(options =>
    {
        options.Repository = "your-org/your-app";
        options.CurrentVersion = "1.2.3";
    })
    .CheckAndUpdateAsync();
```

That’s it.

On startup, your app will:

1. Check the latest GitHub Release
2. Compare versions
3. Download the update (if available)
4. Restart into the new version

---

## 🐙 How Updates Work

1. You publish a new GitHub Release
2. Attach a ZIP file with your app
3. GitHubUpdater downloads it
4. A small **Updater Agent** replaces the files
5. Your app restarts

Your application never updates itself directly — avoiding locked files and partial updates.

---

## 📁 Release Structure

```text
v1.3.0
├── app-win-x64.zip
├── manifest.json
```

Example `manifest.json`:

```json
{
  "version": "1.3.0",
  "entryPoint": "MyApp.exe",
  "sha256": "...",
  "minUpdater": "1.0.0"
}
```

---

## 🔄 Rollback Safety

If an update fails:

* original files are restored
* the previous version is restarted
* no broken installs

---

## ⚙️ CI/CD Integration (GitHub Actions)

```yaml
- name: Publish update
  uses: github-updater/publish@v1
  with:
    version: 1.3.0
    artifact: ./publish/app.zip
```

One pipeline step — updates are live.

---

## 🆓 Free vs Pro

| Feature             | Free | Pro |
| ------------------- | ---- | --- |
| Public GitHub repos | ✅    | ✅   |
| Private repos       | ❌    | ✅   |
| Encrypted updates   | ❌    | ✅   |
| Update channels     | ❌    | ✅   |
| Priority support    | ❌    | ✅   |

> Pricing will be announced after MVP.

---

## 🛣 Roadmap

* [ ] MVP release
* [ ] Rollback v1
* [ ] GitHub Actions publish step
* [ ] Update channels (beta / stable)
* [ ] Private repositories
* [ ] Telemetry (opt‑in)

---

## 🤝 Contributing

Feedback, issues, and PRs are welcome.

If you’ve ever struggled with updating a Windows app — this project is for you.

---

## 📄 License

MIT (Free for open‑source and public projects)
