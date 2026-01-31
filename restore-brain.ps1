# Restore Antigravity Brain Context
# This script restores the brain artifacts to the local app data directory for this conversation

$ConversationId = "48cd631f-585f-4914-a8fc-77ccfc5217d6"
$AppDataBrainPath = "$env:USERPROFILE\.gemini\antigravity\brain\$ConversationId"
$LocalBrainPath = "./.antigravity/brain"

if (Test-Path $LocalBrainPath) {
    if (-not (Test-Path $AppDataBrainPath)) {
        New-Item -ItemType Directory -Force -Path $AppDataBrainPath
    }
    Copy-Item -Path "$LocalBrainPath\*" -Destination $AppDataBrainPath -Recurse -Force
    Write-Host "Brain context restored to $AppDataBrainPath"
} else {
    Write-Error "Local brain backup not found at $LocalBrainPath"
}
