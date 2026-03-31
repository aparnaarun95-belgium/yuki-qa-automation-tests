# Repository Structure Analysis - Unwanted Folders Issue

## ?? Problem Identified

Your GitHub repository has **duplicate/redundant folder structure** that should be cleaned up:

### Current Master Branch Structure (MESSY)
```
yuki-qa-automation-tests/  (root)
??? .github/
??? .vs/                          ? UNWANTED (IDE artifacts)
??? Configuration/                ? DUPLICATE
??? Core/                         ? DUPLICATE
??? PageObjects/                  ? DUPLICATE
??? Services/                     ? DUPLICATE
??? Tests/                        ? DUPLICATE
??? Utilities/                    ? DUPLICATE
??? yuki-qa-automation-tests/     (actual project folder)
?   ??? Base/                     ? CORRECT LOCATION
?   ??? bin/                      ? BUILD ARTIFACT
?   ??? obj/                      ? BUILD ARTIFACT
?   ??? Config/
?   ??? Configuration/
?   ??? Pages/
?   ??? Tests/
?   ??? Utilities/
?   ??? yuki-qa-automation-tests.csproj
??? appsettings.json              ? WRONG LOCATION (should be in project folder)
??? InvoiceTestFramework.csproj   ? WRONG PROJECT FILE
??? yuki-qa-automation-tests.sln  ? CORRECT
```

---

## ? Unwanted Folders/Files

### 1. **Top-Level Build Artifacts**
- `.vs/` - Visual Studio cache directory
- `bin/` - Build output (in nested folder)
- `obj/` - Build intermediates (in nested folder)

### 2. **Top-Level Duplicate Folders**
- `Configuration/` - Should only be in `yuki-qa-automation-tests/`
- `Core/` - Should only be in `yuki-qa-automation-tests/Base/`
- `PageObjects/` - Should only be in `yuki-qa-automation-tests/Pages/`
- `Services/` - Should only be in `yuki-qa-automation-tests/Services/`
- `Tests/` - Should only be in `yuki-qa-automation-tests/Tests/`
- `Utilities/` - Should only be in `yuki-qa-automation-tests/Utilities/`

### 3. **Misplaced Files**
- `appsettings.json` - Should be in `yuki-qa-automation-tests/Config/`
- `appsettings.Development.json` - Should be in `yuki-qa-automation-tests/Config/`
- `InvoiceTestFramework.csproj` - Old/wrong project file

### 4. **Build Output (should be in .gitignore)**
- `bin/` directories
- `obj/` directories

---

## ? Correct Structure Should Be

```
yuki-qa-automation-tests/  (root)
??? .github/
??? .gitattributes
??? .gitignore
??? README.md
??? CONTRIBUTING.md
??? azure-pipelines.yml
??? yuki-qa-automation-tests.sln
?
??? yuki-qa-automation-tests/     (project folder)
    ??? Base/
    ??? Config/
    ?   ??? appsettings.json
    ?   ??? appsettings.Development.json
    ??? Configuration/
    ??? Pages/
    ??? Tests/
    ??? Utilities/
    ??? yuki-qa-automation-tests.csproj
```

---

## ?? How to Fix

### Step 1: Update .gitignore
Add/ensure these entries exist:

```gitignore
# Build outputs
bin/
obj/

# IDE artifacts
.vs/
.vscode/
*.user
*.suo

# OS files
.DS_Store
Thumbs.db
```

### Step 2: Remove from Git (but keep locally)
```bash
# Remove unwanted folders from git tracking
git rm -r --cached .vs/
git rm -r --cached Configuration/
git rm -r --cached Core/
git rm -r --cached PageObjects/
git rm -r --cached Services/
git rm -r --cached Tests/
git rm -r --cached Utilities/
git rm --cached appsettings.json
git rm --cached appsettings.Development.json
git rm --cached InvoiceTestFramework.csproj
git rm -r --cached bin/
git rm -r --cached obj/

# For yuki-qa-automation-tests/bin and obj (if they exist)
git rm -r --cached yuki-qa-automation-tests/bin/
git rm -r --cached yuki-qa-automation-tests/obj/
```

### Step 3: Commit Changes
```bash
git commit -m "Remove unwanted build artifacts and duplicate folders from repository"
```

### Step 4: Push to Remote
```bash
git push origin master
```

---

## ?? Cleanup Checklist

- [ ] Verify `.gitignore` has `bin/`, `obj/`, `.vs/`
- [ ] Remove duplicate top-level `Configuration/` folder
- [ ] Remove duplicate top-level `Core/` folder
- [ ] Remove duplicate top-level `PageObjects/` folder
- [ ] Remove duplicate top-level `Services/` folder
- [ ] Remove duplicate top-level `Tests/` folder
- [ ] Remove duplicate top-level `Utilities/` folder
- [ ] Move `appsettings.json` from root to `yuki-qa-automation-tests/Config/`
- [ ] Move `appsettings.Development.json` from root to `yuki-qa-automation-tests/Config/`
- [ ] Remove `InvoiceTestFramework.csproj`
- [ ] Remove `.vs/` folder
- [ ] Remove `bin/` directories
- [ ] Remove `obj/` directories
- [ ] Commit cleanup changes
- [ ] Push to master branch

---

## Why This Happened

The repository likely has:
1. **Duplicate folder structure** at root and nested levels (migration artifact)
2. **Build artifacts** committed to git (`.vs/`, `bin/`, `obj/`)
3. **Misplaced configuration files** at the wrong level
4. **Old project files** that are no longer needed

---

## ?? Important Notes

- These are mostly **low-risk deletions** (can be recovered from git history)
- `.vs/`, `bin/`, `obj/` are automatically generated and should NEVER be in git
- Configuration files should be at the project level, not root
- The actual source code is likely safe in `yuki-qa-automation-tests/`

---

## ?? Prevention for Future

Add this to `.gitignore`:

```gitignore
# Visual Studio
.vs/
*.user
*.suo
*.sln.docstates

# Build artifacts
bin/
obj/
*.dll
*.exe

# IDE specific
.vscode/
.idea/

# OS specific
.DS_Store
Thumbs.db

# Temporary files
*.tmp
*.bak
```

Then run:
```bash
git add .gitignore
git commit -m "Update .gitignore with comprehensive exclusions"
git push origin master
```

---

## Summary

**Root Cause:** Mixed folder structure with duplicates and build artifacts committed to git

**Solution:** Remove unwanted folders and files, update `.gitignore`, and clean up master branch

**Impact:** Cleaner repository, easier to clone and work with, smaller repo size
