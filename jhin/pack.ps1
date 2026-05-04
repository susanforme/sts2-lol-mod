param([switch]$Release, [switch]$Deploy)
$ErrorActionPreference = "Stop"

$ModId       = "JhinMod"
$ModName     = "Jhin"
$Author      = "YourName"
$Version     = "0.1.0"
$ProjectDir  = "E:\code\sts2-mod\jhin"
$GodotDir    = "E:\code\sts2-mod-demo\Spine" + [char]0x52A8 + [char]0x753B + [char]0x8BF7 + [char]0x4E0B + [char]0x8F7D
$GodotExe    = "$GodotDir\godot-4.2-4.5.1-stable-mono.exe"
$GameModsDir = "F:\steam\steamapps\common\Slay the Spire 2\Mods"
$Config      = if ($Release) { "Release" } else { "Debug" }
$OutDir      = "$ProjectDir\dist\$ModId"

# 1. Build C# DLL
Write-Host "[1/4] Building ($Config)..."
dotnet build "$ProjectDir\jhin.csproj" -c $Config
if ($LASTEXITCODE -ne 0) { Write-Host "Build failed"; exit 1 }

# 2. Prepare output dir (must exist before Godot runs)
Write-Host "[2/4] Preparing output dir..."
if (Test-Path $OutDir) { Remove-Item $OutDir -Recurse -Force }
New-Item -ItemType Directory -Force -Path $OutDir | Out-Null

# 3. Export PCK via Godot headless (Start-Process -Wait ensures we block until done)
Write-Host "[3/4] Exporting PCK..."
$PckPath = "$OutDir\$ModId.pck"
Start-Process -FilePath $GodotExe -ArgumentList "--headless", "--path", "$ProjectDir", "--export-pack", "WindowsDesktop", "$PckPath" -Wait -NoNewWindow
if (-not (Test-Path $PckPath)) { Write-Host "PCK export failed"; exit 1 }
Write-Host "PCK: $([math]::Round((Get-Item $PckPath).Length/1KB, 1)) KB"

# 4. Copy DLL + generate mod.json
Write-Host "[4/4] Copying DLL and generating mod.json..."
$BuildDir = "$ProjectDir\.godot\mono\temp\bin\$Config"
Copy-Item "$BuildDir\jhin.dll" "$OutDir\$ModId.dll"

$ModJson = "{`n  `"id`": `"$ModId`",`n  `"name`": `"$ModName`",`n  `"author`": `"$Author`",`n  `"description`": `"Jhin character mod for Slay the Spire 2.`",`n  `"version`": `"$Version`",`n  `"has_dll`": true,`n  `"has_pck`": true,`n  `"dependencies`": [`"BaseLib`"],`n  `"affects_gameplay`": true`n}"
[System.IO.File]::WriteAllText("$OutDir\$ModId.json", $ModJson, [System.Text.UTF8Encoding]::new($false))

# Result
Write-Host ""
Write-Host "Done: $OutDir"
Get-ChildItem $OutDir | ForEach-Object { Write-Host "  $($_.Name)  ($([math]::Round($_.Length/1KB, 1)) KB)" }

if ($Deploy) {
    Write-Host ""
    Write-Host "Deploying BaseLib..."
    $blSrc = "$ProjectDir\lib\BaseLib"
    $blDst = "$GameModsDir\BaseLib"
    if (Test-Path $blDst) { Remove-Item $blDst -Recurse -Force }
    New-Item -ItemType Directory -Force $blDst | Out-Null
    Copy-Item "$blSrc\BaseLib.dll" "$blDst\BaseLib.dll"
    Copy-Item "$blSrc\BaseLib.pck" "$blDst\BaseLib.pck"
    Copy-Item "$blSrc\BaseLib.json" "$blDst\BaseLib.json"
    Write-Host "Deployed: $blDst"

    Write-Host "Deploying $ModId..."
    $dst = "$GameModsDir\$ModId"
    if (Test-Path $dst) { Remove-Item $dst -Recurse -Force }
    Copy-Item $OutDir $dst -Recurse
    Write-Host "Deployed: $dst"
} else {
    Write-Host ""
    Write-Host "To deploy: .\pack.ps1 -Deploy"
}