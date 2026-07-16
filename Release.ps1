<#
Automatiza o processo de release: atualiza a versão em Version.json e nos dois csproj,
publica a aplicação e gera o TestCoverageUI.zip pronto para subir como asset da release no GitHub.

Uso:
  .\Release.ps1 -Version 1.0.3
#>
param(
  [Parameter(Mandatory = $true)]
  [string]$Version
)

$ErrorActionPreference = "Stop"

$solutionDir = $PSScriptRoot
$uiCsproj = Join-Path $solutionDir "TestCoverageUI.UI\TestCoverageUI.UI.csproj"
$servicesCsproj = Join-Path $solutionDir "TestCoverageUI.Services\TestCoverageUI.Services.csproj"
$versionJson = Join-Path $solutionDir "version.json"
$publishDir = Join-Path $solutionDir "publish"
$zipPath = Join-Path $solutionDir "TestCoverageUI.zip"

function Set-CsprojVersion($path, $version) {
  $content = Get-Content $path -Raw
  $content = $content -replace "<Version>[^<]*</Version>", "<Version>$version</Version>"
  $content = $content -replace "<AssemblyVersion>[^<]*</AssemblyVersion>", "<AssemblyVersion>$version</AssemblyVersion>"
  $content = $content -replace "<FileVersion>[^<]*</FileVersion>", "<FileVersion>$version</FileVersion>"
  Set-Content -Path $path -Value $content -NoNewline
}

Write-Host "1) Atualizando versão para $Version..."
Set-CsprojVersion $uiCsproj $Version
Set-CsprojVersion $servicesCsproj $Version
Set-Content -Path $versionJson -Value @"
{
  "version": "$Version",
  "url": "https://github.com/christiandmelo/TestCoverageUI/releases/download/$Version/TestCoverageUI.zip"
}
"@ -NoNewline

Write-Host "2) Publicando (dotnet publish)..."
if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }
dotnet publish $uiCsproj -c Release -r win-x64 --self-contained false -o $publishDir -p:SolutionDir="$solutionDir\"
if ($LASTEXITCODE -ne 0) { throw "dotnet publish falhou" }

Write-Host "3) Gerando $zipPath..."
if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
Compress-Archive -Path (Join-Path $publishDir "*") -DestinationPath $zipPath

Write-Host ""
Write-Host "Pronto. Version.json, csproj's e $zipPath atualizados para $Version."
Write-Host "Faltam apenas: revisar/commitar as mudanças, dar push e criar a release '$Version' no GitHub anexando o zip."
