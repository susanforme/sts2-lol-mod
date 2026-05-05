param([switch]$Release, [switch]$Deploy)
$ErrorActionPreference = "Stop"

$ModId       = "JhinMod"
$ModName     = "Jhin"
$Author      = "peach"
$Version     = "0.1.0"
$BaseLibVersion = "3.1.0"
$ProjectDir  = "E:\code\sts2-lol-mod\jhin"
$GodotDir    = "E:\code\sts2-mod-demo\SPINE~1"
$GodotExe    = "$GodotDir\godot-4.2-4.5.1-stable-mono.exe"
$GameModsDir = "F:\steam\steamapps\common\Slay the Spire 2\Mods"
$Config      = if ($Release) { "Release" } else { "Debug" }
$OutDir      = "$ProjectDir\dist\$ModId"

# 1. Build C# DLL
Write-Host "[1/4] Building ($Config)..."
dotnet build "$ProjectDir\jhin.csproj" -c $Config
if ($LASTEXITCODE -ne 0) { Write-Host "Build failed"; exit 1 }

# 2. Prepare output dir
Write-Host "[2/4] Preparing output dir..."
if (Test-Path $OutDir) { Remove-Item $OutDir -Recurse -Force }
New-Item -ItemType Directory -Force -Path $OutDir | Out-Null

$BaseLibContentDir = "$env:USERPROFILE\.nuget\packages\alchyr.sts2.baselib\$BaseLibVersion\Content"
$BaseLibResourceDir = "$ProjectDir\lib\BaseLib"
if (-not (Test-Path "$BaseLibContentDir\BaseLib.json") -or -not (Test-Path "$BaseLibContentDir\BaseLib.pck")) {
    Write-Host "BaseLib package content not found: $BaseLibContentDir"
    exit 1
}
New-Item -ItemType Directory -Force -Path $BaseLibResourceDir | Out-Null
Copy-Item "$BaseLibContentDir\BaseLib.json" "$BaseLibResourceDir\BaseLib.json"
Copy-Item "$BaseLibContentDir\BaseLib.pck" "$BaseLibResourceDir\BaseLib.pck"

# 3. Export PCK
Write-Host "[3/4] Exporting PCK..."
$PckPath = "$OutDir\$ModId.pck"
Start-Process -FilePath $GodotExe -ArgumentList "--headless", "--path", "$ProjectDir", "--export-pack", "WindowsDesktop", "$PckPath" -Wait -NoNewWindow
if (-not (Test-Path $PckPath)) { Write-Host "PCK export failed"; exit 1 }
Write-Host "PCK: $([math]::Round((Get-Item $PckPath).Length/1KB, 1)) KB"

# 4. Copy DLL + mod.json
Write-Host "[4/4] Copying DLL and generating mod.json..."
$BuildDir = "$ProjectDir\.godot\mono\temp\bin\$Config"
Copy-Item "$BuildDir\jhin.dll" "$OutDir\$ModId.dll"

$ModJson = "{`n  `"id`": `"$ModId`",`n  `"name`": `"$ModName`",`n  `"author`": `"$Author`",`n  `"description`": `"Jhin character mod.`",`n  `"version`": `"$Version`",`n  `"has_dll`": true,`n  `"has_pck`": true,`n  `"dependencies`": [`"BaseLib`"],`n  `"affects_gameplay`": true`n}"
[System.IO.File]::WriteAllText("$OutDir\$ModId.json", $ModJson, [System.Text.UTF8Encoding]::new($false))

Write-Host ""
Write-Host "Done: $OutDir"
Get-ChildItem $OutDir | ForEach-Object { Write-Host "  $($_.Name)  ($([math]::Round($_.Length/1KB, 1)) KB)" }

if ($Deploy) {
    Write-Host ""
    Write-Host "Deploying BaseLib..."
    $blDst = "$GameModsDir\BaseLib"
    if (Test-Path $blDst) { Remove-Item $blDst -Recurse -Force }
    New-Item -ItemType Directory -Force -Path $blDst | Out-Null
    Copy-Item "$BuildDir\BaseLib.dll" "$blDst\BaseLib.dll"
    Copy-Item "$BaseLibContentDir\BaseLib.pck" "$blDst\BaseLib.pck"
    Copy-Item "$BaseLibContentDir\BaseLib.json" "$blDst\BaseLib.json"
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
