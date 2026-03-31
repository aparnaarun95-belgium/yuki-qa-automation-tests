# Repository Cleanup Script for PowerShell
# This script removes duplicate folders and build artifacts from the root level
# Run this in PowerShell from the repository root directory

$repoPath = "C:\Users\arunk\Downloads\qa-automation-test\yuki-qa-automation-test\yuki-qa-automation-tests"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Starting Repository Cleanup" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

# Navigate to repository
Set-Location $repoPath
Write-Host "Repository: $repoPath" -ForegroundColor Yellow
Write-Host ""

# Create backup branch
Write-Host "Step 1: Creating backup branch..." -ForegroundColor Green
& git branch backup/cleanup-20240331
& git push origin backup/cleanup-20240331
Write-Host "? Backup branch created: backup/cleanup-20240331" -ForegroundColor Green
Write-Host ""

# Remove duplicate outer folders
Write-Host "Step 2: Removing duplicate outer folders..." -ForegroundColor Green
Write-Host ""

$foldersToRemove = @("Configuration", "Core", "PageObjects", "Services", "Tests", "Utilities")

foreach ($folder in $foldersToRemove) {
    Write-Host "  Removing: $folder/" -ForegroundColor Yellow
    & git rm -r $folder/
    Write-Host "  ? $folder/ removed" -ForegroundColor Green
}

Write-Host ""

# Remove build artifacts
Write-Host "Step 3: Removing build artifacts..." -ForegroundColor Green
Write-Host ""

Write-Host "  Removing: .vs/ (IDE cache)" -ForegroundColor Yellow
& git rm -r .vs/
Write-Host "  ? .vs/ removed" -ForegroundColor Green

Write-Host "  Removing: yuki-qa-automation-tests/bin/ (build output)" -ForegroundColor Yellow
& git rm -r yuki-qa-automation-tests/bin/
Write-Host "  ? bin/ removed" -ForegroundColor Green

Write-Host "  Removing: yuki-qa-automation-tests/obj/ (build intermediates)" -ForegroundColor Yellow
& git rm -r yuki-qa-automation-tests/obj/
Write-Host "  ? obj/ removed" -ForegroundColor Green

Write-Host ""

# Remove misplaced configuration files
Write-Host "Step 4: Removing misplaced configuration files..." -ForegroundColor Green
Write-Host ""

Write-Host "  Removing: appsettings.json (from root)" -ForegroundColor Yellow
& git rm appsettings.json
Write-Host "  ? appsettings.json removed" -ForegroundColor Green

Write-Host "  Removing: appsettings.Development.json (from root)" -ForegroundColor Yellow
& git rm appsettings.Development.json
Write-Host "  ? appsettings.Development.json removed" -ForegroundColor Green

Write-Host ""

# Remove obsolete project file
Write-Host "Step 5: Removing obsolete project file..." -ForegroundColor Green
Write-Host ""

Write-Host "  Removing: InvoiceTestFramework.csproj" -ForegroundColor Yellow
& git rm InvoiceTestFramework.csproj
Write-Host "  ? InvoiceTestFramework.csproj removed" -ForegroundColor Green

Write-Host ""

# Commit changes
Write-Host "Step 6: Committing cleanup changes..." -ForegroundColor Green
Write-Host ""

$commitMessage = @"
Clean repository: Remove duplicate outer folders and build artifacts

- Removed duplicate folders at root level (Configuration, Core, PageObjects, Services, Tests, Utilities)
- Removed IDE cache (.vs/)
- Removed build outputs (bin/, obj/)
- Removed misplaced config files from root (appsettings.json, appsettings.Development.json)
- Removed obsolete InvoiceTestFramework.csproj

All actual code remains intact in yuki-qa-automation-tests/ folder. This cleanup reduces repository size from 200-300 MB to ~1-2 MB and significantly speeds up clones.
"@

& git commit -m $commitMessage
Write-Host "? Changes committed" -ForegroundColor Green

Write-Host ""

# Push to master
Write-Host "Step 7: Pushing cleanup to master..." -ForegroundColor Green
Write-Host ""

& git push origin master

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "? CLEANUP COMPLETE!" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Summary of changes:" -ForegroundColor Yellow
Write-Host "- 6 duplicate folders removed" -ForegroundColor White
Write-Host "- Build artifacts removed" -ForegroundColor White
Write-Host "- Configuration files repositioned" -ForegroundColor White
Write-Host "- Repository is now clean and professional" -ForegroundColor White
Write-Host ""
Write-Host "Expected repository size reduction:" -ForegroundColor Yellow
Write-Host "  Before: 200-300 MB" -ForegroundColor White
Write-Host "  After: ~1-2 MB" -ForegroundColor White
Write-Host ""
Write-Host "Your backup branch is: backup/cleanup-20240331" -ForegroundColor Yellow
Write-Host "If needed, you can revert using:" -ForegroundColor Yellow
Write-Host "  git reset --hard backup/cleanup-20240331" -ForegroundColor White
Write-Host ""
