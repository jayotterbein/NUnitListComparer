$ErrorActionPreference = 'Stop'

$scriptPath = (Split-Path -Parent $MyInvocation.MyCommand.Definition)
Include (Join-Path $scriptPath "functions.ps1")

properties {
	$project_name = "NUnitListComparer"
	$build_config = "Release"

	$source_dir = Join-Path $scriptPath "source"
	$build_dir = Join-Path $scriptPath "build"
	$tools_dir = Join-Path $scriptPath "tools"

	$tools_nunit = Join-Path (Join-Path (Join-Path $tools_dir "NUnit.Runners.2.6.3") "tools") "nunit-console.exe"

	$solution_file = Join-Path $source_dir "$project_name.sln"
}
 
task deploy -depends test {
}

task clean {
    gci $source_dir -Include obj,debug,release -Recurse |? { Delete-Directory $_ }
	Delete-Directory $build_dir
	Create-Directory $build_dir
}
 
task compile -depends clean {
	exec { msbuild $solution_file /p:Configuration=$build_config /p:OutDir=""$build_dir\\"" /consoleloggerparameters:ErrorsOnly }
}

task test -depends compile {
    [string[]]$tests = @(gci $build_dir -Include *Tests.dll -Recurse)
    foreach ($testdll in $tests)
    {
        & $tools_nunit $testdll /config:$build_config /nodots /nologo

   		if ($lastexitcode -gt 0) {
			throw "{0} unit test{1} failed." -f $lastexitcode,(& { if ($lastexitcode -eq 1) { "" } else { "s" } })
		}

		if ($lastexitcode -lt 0) {
			throw "Unit test run was terminated by a fatal error."
		}
    }
}