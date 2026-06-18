$source = Get-Location
$temp = Join-Path $env:TEMP "SourceZip"

Remove-Item $temp -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path $temp | Out-Null

Get-ChildItem $source -Recurse -File |
    Where-Object {
        $_.Extension -in '.cs', '.csproj', '.sln' -and
        $_.FullName -notmatch '\\bin\\|\\obj\\|\\.vs\\'
    } |
    ForEach-Object {
        $relative = Resolve-Path -Relative $_.FullName
        $destination = Join-Path $temp $relative

        New-Item -ItemType Directory `
            -Path (Split-Path $destination) `
            -Force | Out-Null

        Copy-Item $_.FullName $destination
    }

Compress-Archive -Path "$temp\*" -DestinationPath "SourceCode.zip"