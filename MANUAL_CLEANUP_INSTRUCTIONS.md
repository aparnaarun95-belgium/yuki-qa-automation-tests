# ?? Manual Repository Cleanup - Step by Step Instructions

## Prerequisites

1. ? Git is installed on your system
2. ? You have your GitHub credentials configured
3. ? You're in the repository directory

---

## Option 1: Run PowerShell Script (Easiest)

### Steps:
1. Open PowerShell as Administrator
2. Navigate to the repository root:
   ```powershell
   cd "C:\Users\arunk\Downloads\qa-automation-test\yuki-qa-automation-test\yuki-qa-automation-tests"
   ```
3. Run the cleanup script:
   ```powershell
   .\cleanup-repository.ps1
   ```
4. Wait for completion - it will handle everything!

**That's it! The script does all the work automatically.**

---

## Option 2: Manual Commands (More Control)

If you prefer to run commands manually, follow these steps:

### Step 1: Create Backup Branch
```bash
git branch backup/cleanup-20240331
git push origin backup/cleanup-20240331
```

**Why:** Safety net if anything goes wrong. You can revert by checking out this branch.

### Step 2: Remove Duplicate Folders
```bash
git rm -r Configuration/
git rm -r Core/
git rm -r PageObjects/
git rm -r Services/
git rm -r Tests/
git rm -r Utilities/
```

**What it does:** Removes all 6 duplicate folders from git tracking.

### Step 3: Remove Build Artifacts
```bash
git rm -r .vs/
git rm -r yuki-qa-automation-tests/bin/
git rm -r yuki-qa-automation-tests/obj/
```

**What it does:** Removes IDE cache and build outputs.

### Step 4: Remove Misplaced Files
```bash
git rm appsettings.json
git rm appsettings.Development.json
```

**What it does:** Removes config files from root (they're already in yuki-qa-automation-tests/Config/).

### Step 5: Remove Obsolete Project File
```bash
git rm InvoiceTestFramework.csproj
```

**What it does:** Removes the old, unused project file.

### Step 6: Verify What's Being Removed
```bash
git status
```

**What it shows:** All files/folders marked for deletion. Review to make sure nothing important is there.

### Step 7: Commit Changes
```bash
git commit -m "Clean repository: Remove duplicate outer folders and build artifacts

- Removed duplicate folders at root level (Configuration, Core, PageObjects, Services, Tests, Utilities)
- Removed IDE cache (.vs/)
- Removed build outputs (bin/, obj/)
- Removed misplaced config files from root
- Removed obsolete InvoiceTestFramework.csproj

All code remains in yuki-qa-automation-tests/ folder."
```

**What it does:** Records all changes with a clear message.

### Step 8: Push to Master
```bash
git push origin master
```

**What it does:** Uploads changes to GitHub.

---

## Verification

After cleanup, verify everything worked:

### Check Repository Size
```bash
git rev-parse --git-dir
```

### Clone in a Test Directory (Optional)
```bash
cd C:\temp
git clone https://github.com/aparnaarun95-belgium/yuki-qa-automation-tests test-clone
cd test-clone
dotnet build
```

### Verify Structure
The repository should now have:
```
? yuki-qa-automation-tests/              (Project folder)
  ??? Base/
  ??? Config/
  ?   ??? appsettings.json
  ?   ??? appsettings.Development.json
  ??? Configuration/
  ??? Pages/
  ??? Tests/
  ??? Utilities/
  ??? yuki-qa-automation-tests.csproj

? NO duplicate folders at root level
? NO build artifacts
? NO misplaced config files
```

---

## If Something Goes Wrong

### Option 1: Revert the Entire Cleanup
```bash
git reset --hard backup/cleanup-20240331
git push origin master --force
```

### Option 2: Check Git Log
```bash
git log --oneline -5
```

### Option 3: Undo Last Commit (Before Push)
```bash
git reset --soft HEAD~1
```

---

## Timeline

| Step | Time | Action |
|------|------|--------|
| 1 | 30 sec | Create backup branch |
| 2-5 | 2 min | Remove folders/artifacts |
| 6 | 1 min | Verify changes |
| 7 | 1 min | Commit |
| 8 | 1 min | Push to master |
| **Total** | **~5 min** | **Complete cleanup** |

---

## Expected Results

### Before Cleanup
```
Repository Size:   200-300 MB
Duplicate Folders: 6
Structure:         Messy ?
```

### After Cleanup
```
Repository Size:   ~1-2 MB (100x smaller!)
Duplicate Folders: 0
Structure:         Clean ?
```

---

## FAQ

**Q: Will I lose any code?**
A: No! All code is in yuki-qa-automation-tests/ folder which stays intact.

**Q: Can I undo this?**
A: Yes! Use the backup branch or git history to revert.

**Q: Why is .vs/ so big?**
A: IDE cache can be 50-200 MB. It's regenerated when needed.

**Q: Should I delete bin/ and obj/?**
A: Yes! They're build outputs. .gitignore prevents future ones from being committed.

**Q: What if something breaks?**
A: Revert using: `git reset --hard backup/cleanup-20240331`

---

## Next Steps After Cleanup

1. ? Run tests to verify everything works
2. ? Build the project locally
3. ? Clone fresh copy to verify
4. ? Delete backup branch (optional, after confirmation)

---

## Recommended Sequence

**Choose one option:**

### Option A: Automated (Recommended)
1. Open PowerShell in repo directory
2. Run: `.\cleanup-repository.ps1`
3. Done! ?

### Option B: Step-by-Step Manual
1. Copy commands from "Option 2" above
2. Run each command
3. Verify at each step
4. Done! ?

### Option C: GitHub Web Interface
1. Log into GitHub
2. Delete folders manually one by one
3. Done! ? (slower, but works)

---

## Support

If you have issues:
1. Check git status: `git status`
2. Review what's staged: `git diff --cached`
3. Check backup branch exists: `git branch -a`
4. Revert if needed: `git reset --hard backup/cleanup-20240331`

---

**Ready to clean? Pick an option above and execute!** ??
