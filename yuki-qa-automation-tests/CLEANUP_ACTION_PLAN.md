# ?? Repository Cleanup Action Plan

## ? Good News
Your `.gitignore` file is **already well-configured** with:
- ? `bin/`, `obj/` exclusions
- ? `.vs/` exclusion
- ? Test results exclusions
- ? Playwright artifacts exclusions

## ? The Problem
Despite good `.gitignore`, these folders are **already committed to the repository**:
- `Configuration/` (top-level)
- `Core/` (top-level)
- `PageObjects/` (top-level)
- `Services/` (top-level)
- `Tests/` (top-level)
- `Utilities/` (top-level)
- `.vs/` directory
- `appsettings.json` (in root instead of Config/)
- `appsettings.Development.json` (in root instead of Config/)
- `InvoiceTestFramework.csproj` (old project file)

These are **duplicates** of what's already in `yuki-qa-automation-tests/` folder.

---

## ?? Steps to Clean Master Branch

### Phase 1: Backup (Local Only - Safe)
```bash
# Create backup branch
git branch backup/cleanup-before
git push origin backup/cleanup-before
```

### Phase 2: Remove from Git History (Recommended)

#### Option A: Using BFG Repo Cleaner (Recommended)
```bash
# Install BFG (faster, simpler)
# https://rtyley.github.io/bfg-repo-cleaner/

# Download and run
bfg --delete-folders Configuration,Core,PageObjects,Services,Tests,Utilities .

# Clean up git
git reflog expire --expire=now --all
git gc --prune=now --aggressive

git push origin master --force
```

#### Option B: Using git filter-branch (Manual)
```bash
# Remove specific folders
git filter-branch --tree-filter 'rm -rf Configuration Core PageObjects Services Tests Utilities' HEAD

# Remove specific files
git filter-branch --tree-filter 'rm -f appsettings.json appsettings.Development.json InvoiceTestFramework.csproj' HEAD

# Clean git database
git reflog expire --expire=now --all
git gc --prune=now --aggressive

git push origin master --force
```

#### Option C: Simpler Approach (If Not Critical History)
```bash
# 1. Create clean branch
git checkout --orphan clean-master

# 2. Add only wanted files
git add -A

# 3. Commit
git commit -m "Clean repository structure - remove duplicates and build artifacts"

# 4. Delete old master
git branch -D master

# 5. Rename
git branch -m clean-master master

# 6. Force push
git push origin master --force
```

### Phase 3: Verify Cleanup
```bash
# Verify what will be pushed
git log --oneline | head -5

# Verify directory structure
git ls-tree -r HEAD | grep "^[^/]*$" | head -20
```

---

## ?? Current vs Desired Structure

### CURRENT (Messy)
```
Root Level:
??? Configuration/     ? Remove
??? Core/             ? Remove
??? PageObjects/      ? Remove
??? Services/         ? Remove
??? Tests/            ? Remove
??? Utilities/        ? Remove
??? yuki-qa-automation-tests/
?   ??? Configuration/ ? Keep
?   ??? Base/          ? Keep
?   ??? Pages/         ? Keep
?   ??? Tests/         ? Keep
?   ??? Utilities/     ? Keep
?   ??? yuki-qa-automation-tests.csproj
??? appsettings.json           ? Remove (wrong location)
??? appsettings.Development.json ? Remove (wrong location)
??? InvoiceTestFramework.csproj ? Remove (old file)
```

### DESIRED (Clean)
```
Root Level:
??? .github/
??? .gitignore
??? .gitattributes
??? README.md
??? CONTRIBUTING.md
??? azure-pipelines.yml
??? yuki-qa-automation-tests.sln
?
??? yuki-qa-automation-tests/
    ??? Base/
    ??? Config/
    ?   ??? appsettings.json  ? Moved here
    ?   ??? appsettings.Development.json ? Moved here
    ??? Configuration/
    ??? Pages/
    ??? Tests/
    ??? Utilities/
    ??? yuki-qa-automation-tests.csproj
```

---

## ?? Things to Consider

### Risks (LOW)
- ? Only removing duplicates and build artifacts
- ? Actual source code is safe
- ? Can recover from git history if needed
- ? Force push will update all clones

### Benefits
- ?? Smaller repo size (less storage)
- ?? Cleaner project structure
- ?? Faster clones and pulls
- ?? Professional appearance
- ?? Easier to navigate

### Before Force Push
- [ ] Verify backup branch created
- [ ] Confirm local copy has all needed files
- [ ] Test that project still builds
- [ ] Verify `.sln` file points to correct project
- [ ] Check if any CI/CD pipelines reference old structure

---

## ??? Recommended Order

### Option 1: Simple Manual Cleanup (Safest)
1. ? Keep local repository as-is
2. ? These files don't harm development
3. ? Document the structure
4. ? Cleanup when time permits

### Option 2: Immediate Cleanup (Recommended)
1. Create backup branch
2. Move files to correct locations locally
3. Commit changes
4. Push to master (doesn't require force push)

```bash
# Files to move/delete:
# Move: appsettings.json ? yuki-qa-automation-tests/Config/
# Move: appsettings.Development.json ? yuki-qa-automation-tests/Config/
# Delete: Configuration/, Core/, PageObjects/, Services/, Tests/, Utilities/
# Delete: InvoiceTestFramework.csproj

git add -A
git commit -m "Clean repository structure - remove duplicates and reorganize files"
git push origin master
```

### Option 3: Clean History (Advanced)
Use BFG repo cleaner to remove from git history (requires force push).

---

## ?? Recommended Action

**For your current situation, I recommend:**

1. **Keep as-is for now** - not harmful for local development
2. **Document the structure** - you now know what's redundant
3. **Plan cleanup** - do it when appropriate

OR

**Do manual cleanup:**
```bash
# 1. Move config files to correct location
cp appsettings.json yuki-qa-automation-tests/Config/
cp appsettings.Development.json yuki-qa-automation-tests/Config/

# 2. Remove duplicates from root
rm -r Configuration Core PageObjects Services Tests Utilities
rm appsettings.json appsettings.Development.json
rm InvoiceTestFramework.csproj

# 3. Commit
git add -A
git commit -m "Cleanup: move config files and remove duplicate folders"
git push origin master
```

---

## ?? Files That Need Action

### DELETE (Top-level duplicates)
```
? Configuration/
? Core/
? PageObjects/
? Services/
? Tests/
? Utilities/
? appsettings.json (move to Config/ first)
? appsettings.Development.json (move to Config/ first)
? InvoiceTestFramework.csproj
```

### DELETE (Build artifacts - if present)
```
? .vs/ (IDE cache)
? yuki-qa-automation-tests/bin/
? yuki-qa-automation-tests/obj/
```

### KEEP (Correct locations)
```
? yuki-qa-automation-tests/ (main project)
? .github/
? yuki-qa-automation-tests.sln
? .gitignore
? README.md
```

---

## ?? Summary

**Problem:** Duplicate folders and misplaced files at root level  
**Root Cause:** Repository initialization included both root and nested structures  
**Solution:** Remove duplicates and reorganize (2-3 commits)  
**Risk Level:** LOW (only removing duplicates)  
**Timeline:** 15 minutes to implement  
**Impact:** Cleaner repo, professional structure

---

## ? Next Steps

Choose one:

1. **Do Nothing** - Works fine, document structure
2. **Manual Cleanup** - Clean and commit normally
3. **History Cleanup** - Use BFG to rewrite history

**Recommendation:** Go with **Option 2 (Manual Cleanup)** - simple, safe, and improves repo structure.

Would you like me to provide the exact git commands to execute this cleanup?
