<#
.SYNOPSIS
	Run postman tests collections using Newman.
#>
[cmdletbinding()]
param (
    # Path to search for test collections (*.collection.json)
    [ValidateNotNullOrEmpty()]
    [string]$Path = "$PWD",
    # Global variable for {{hosturl}}
    [ValidateNotNullOrEmpty()]
    $HostUrl = "https://abc.com/",
    # Global variable for {{api-key}}
    [ValidateNotNullOrEmpty()]
    $ApiKey = $env:ITSP_APIKEY
)

$LastErrorActionPreference = $ErrorActionPreference
$ErrorActionPreference = "Stop"

try {
    if ([string]::IsNullOrEmpty($apiKey)) {
        throw "Missing api key, you can set $env:APIKEY or provide via -ApiKey"
		# .\sign.ps1; .\Run_PostmanTests.ps1 -ApiKey key123abc
    }

    # Collection files are named <colleciton>.collection.json and may have an associated data file <colleciton>.data.json
    $collectionsFiles = @(Get-ChildItem -Path $Path -Filter *.collection.json -Recurse);
    write-host "Found $($collectionsFiles.Count) test collections."

    foreach ($thisFile in $collectionsFiles) {
        $collectionName = $thisFile.Name.Split(".")[0]
        write-host "Running collection: $($thisFile.FullName)";

        # build the options for newman run
        $ops = [System.Collections.ArrayList]::new()
        $ops.Add("--global-var api-key=$ApiKey") | out-null
        $ops.Add("--global-var hostUrl=$hostUrl")| out-null
        $ops.Add("-r junit,cli")| out-null
        # TODO: allow to be set as param()
        # set the report folder for junit
        $junitOutFolder = "$PSScriptRoot/TestResults/$collectionName.junit.xml"
        $ops.Add("--reporter-junit-export `"$junitOutFolder`"")| out-null
        $ops.add("--disable-unicode") | out-null # so it renders better in powershell console
        $ops.add("--color on")| out-null

        # Get environment file from the collection directory or parent folder.
        # File has to be named environment.json
        [System.IO.DirectoryInfo]$testEnvDir = $thisFile.Directory
        do {
            $testEnvPath = Join-Path $testEnvDir.Fullname "environment.json"

            if (Test-path $testEnvPath) {
                $Ops.add("--environment `"$testEnvPath`"") | out-null
                break;
            }
            else {
                $testEnvDir = $testEnvDir.Parent
            }
        }
        while ($testEnvDir)

        # data file for collection
        $dataFile = Join-Path $thisFile.Directory.FullName "$collectionName.data.json"
        if (Test-Path $dataFile) {
            $Ops.add("--iteration-data `"$dataFile`"") | out-null
            # $Ops.add("--iteration-count 1") | out-null
        }
        else {
            Write-Warning "$collectionName does not have a data file.";
        }

        $newmanRunOptions = $Ops -join " "
        Write-host "Running: newman run "$($thisFile.FullName)" $newmanRunOptions"
        Invoke-Expression "& '$PSScriptRoot\node_modules\.bin\newman.cmd' --% run $($thisFile.FullName) $newmanRunOptions" 2>&1 -ErrorVariable errors | ForEach-Object {
            if ($_ -is [System.Management.Automation.ErrorRecord]) {
                Write-Host "##vso[task.logissue;type=error]$($_.ToString())"
            }
            else {
                Write-Host $_
            }
        }
    }
}
finally {
    $ErrorActionPreference = $LastErrorActionPreference
}


