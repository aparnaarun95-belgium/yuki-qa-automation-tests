# ?? Repository Cleanup - Without PowerShell Script Execution

Since you have execution policy restrictions preventing PowerShell scripts from running, here are **alternative methods** to clean your repository.

---

## **Option 1: Use Git GUI (Easiest - No Command Line)**

### Steps:

1. **Open Git GUI**
   - Right-click in your repository folder
   - Select: "Git GUI Here"
   - OR: Open Start Menu ? Search "Git GUI" ? Launch

2. **Create Backup Branch**
   - Menu: Repository ? Create Branch
   - Branch Name: `backup/cleanup-20240331`
   - Click: Create

3. **Delete Duplicate Folders**
   - Menu: Tools ? Delete Branch
   - Or use file explorer to delete:
     - `Configuration/`
     - `Core/`
     - `PageObjects/`
     - `Services/`
     - `Tests/`
     - `Utilities/`
     - `.vs/`

4. **Stage Changes**
   - In Git GUI, click "Rescan"
   - Select all deletions
   - Click "Stage Changed"

5. **Delete Files**
   - Right-click on files to delete:
     - `appsettings.json`
     - `appsettings.Development.json`
     - `InvoiceTestFramework.csproj`

6. **Commit**
   - Message: "Clean repository: Remove duplicate folders and build artifacts"
   - Click: Commit

7. **Push**
   - Menu: Remote ? Push
   - Click: Push All

---

## **Option 2: Use Visual Studio Code (Very Easy)**

### Steps:

1. **Open Repository in VS Code**
   - File ? Open Folder
   - Select repository root

2. **Open Git Panel**
   - Click Git icon in left sidebar
   - Or: Ctrl+Shift+G

3. **Create Backup Branch**
   - Click "Create Branch"
   - Name: `backup/cleanup-20240331`
   - Click: "Create Branch"

4. **Delete Folders**
   - Right-click each folder in Explorer:
     - Configuration/
     - Core/
     - PageObjects/
     - Services/
     - Tests/
     - Utilities/
     - .vs/
   - Delete

5. **Delete Files**
   - Right-click each file:
     - appsettings.json
     - appsettings.Development.json
     - InvoiceTestFramework.csproj
   - Delete

6. **Stage Changes**
   - In Git panel, click "+" on each change
   - Or click "+All Changes"

7. **Commit**
   - Type message: "Clean repository: Remove duplicate folders and build artifacts"
   - Press Ctrl+Enter or click commit button

8. **Push**
   - Click push button (up arrow with number)
   - Select "Push to origin/master"

---

## **Option 3: Use GitHub Desktop (Graphical)**

### Steps:

1. **Install GitHub Desktop** (if not already)
   - Download from: https://desktop.github.com/

2. **Open Repository**
   - File ? Clone Repository
   - Or: File ? Open Repository in GitHub Desktop

3. **Create Backup Branch**
   - Click "Current Branch"
   - Click "New Branch"
   - Name: `backup/cleanup-20240331`

4. **Delete Folders & Files**
   - Use Windows Explorer to delete:
     - Configuration/
     - Core/
     - PageObjects/
     - Services/
     - Tests/
     - Utilities/
     - .vs/
     - appsettings.json
     - appsettings.Development.json
     - InvoiceTestFramework.csproj

5. **Stage Changes in GitHub Desktop**
   - Changes should appear automatically
   - Check each one to stage

6. **Commit**
   - Message: "Clean repository: Remove duplicate folders and build artifacts"
   - Click: Commit

7. **Push**
   - Click: Push Origin

---

## **Option 4: Manual Git via Command Prompt (No PowerShell)**

If you can use Windows Command Prompt (`cmd.exe`), try:

```cmd
cd "C:\Users\arunk\Downloads\qa-automation-test\yuki-qa-automation-test\yuki-qa-automation-tests"

REM Create backup branch
git branch backup/cleanup-20240331

REM Remove folders
git rm -r Configuration
git rm -r Core
git rm -r PageObjects
git rm -r Services
git rm -r Tests
git rm -r Utilities
git rm -r .vs
git rm -r yuki-qa-automation-tests\bin
git rm -r yuki-qa-automation-tests\obj

REM Remove files
git rm appsettings.json
git rm appsettings.Development.json
git rm InvoiceTestFramework.csproj

REM Commit
git commit -m "Clean repository: Remove duplicate folders and build artifacts"

REM Push
git push origin master
```

**Note:** Command Prompt git commands might not support `-r` flag. If so, use:
```cmd
git rm -rf Configuration
```

---

## **Option 5: Delete Locally, Push to GitHub Later**

If you want maximum control:

1. **Manually delete folders** using Windows Explorer:
   - Delete: Configuration/, Core/, PageObjects/, Services/, Tests/, Utilities/, .vs/

2. **Manually delete files** using Windows Explorer:
   - Delete: appsettings.json, appsettings.Development.json, InvoiceTestFramework.csproj

3. **Open Git GUI**
   - Right-click repository ? Git GUI Here
   - Click "Rescan"
   - Select all deletions in "Unstaged Changes"
   - Click "Stage Changed"
   - Commit with message: "Clean repository"
   - Click "Push"

---

## **My Recommendation**

### **Use Visual Studio Code (Option 2)** because:
- ? Most intuitive
- ? Built-in Git support
- ? Visual file deletion
- ? No terminal needed
- ? Clear staging interface
- ? Already might be open

### Steps Summary for VS Code:
1. Open VS Code
2. Open Git panel (Ctrl+Shift+G)
3. Delete folders and files from Explorer
4. Stage all changes
5. Commit with message
6. Push

---

## **Visual Guide: VS Code Steps**

```
1. VS Code Main Window
   ??? Explorer (Left Sidebar)
   ?   ??? DELETE: Configuration/
   ?   ??? DELETE: Core/
   ?   ??? DELETE: PageObjects/
   ?   ??? DELETE: Services/
   ?   ??? DELETE: Tests/
   ?   ??? DELETE: Utilities/
   ?   ??? DELETE: .vs/
   ?   ??? DELETE: appsettings.json
   ?   ??? DELETE: appsettings.Development.json
   ?   ??? DELETE: InvoiceTestFramework.csproj
   ?
   ??? Git Panel (Ctrl+Shift+G)
       ??? Stage All Changes (+ icon)
       ??? Write Message: "Clean repository..."
       ??? Commit and Push

2. Done! Repository is cleaned
```

---

## ? What to Do Right Now

**Pick one method above:**

1. **Easiest:** GitHub Desktop (Option 3)
2. **Fastest:** VS Code (Option 2)
3. **Traditional:** Git GUI (Option 1)
4. **Command Line:** Command Prompt (Option 4)

---

## ?? After Cleanup

Regardless of method you choose:

1. Verify locally:
   ```
   dotnet build      (should succeed)
   dotnet test       (should pass)
   ```

2. Verify on GitHub:
   - Check repository structure
   - Confirm size reduced
   - Backup branch exists

---

**Which method would you prefer to use?**

I recommend **Option 2 (VS Code)** or **Option 3 (GitHub Desktop)** as they're most visual and beginner-friendly.
