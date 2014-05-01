require 'albacore'
require 'fileutils'

task :default => :test

desc "Build solution"
msbuild :msbuild do |msb|
 msb.properties :configuration => :Release
 msb.targets :Clean, :Build
 msb.solution = "Simpler.sln"
end

desc "Run unit tests"
nunit :test => :msbuild do |nunit|
 nunit.command = "packages/NUnit.2.5.9.10348/Tools/nunit-console.exe"
 nunit.assemblies "Simpler.Tests/Simpler.Tests.csproj"
end

desc "Prep the package folder"
task :prepPackage => :test do
	FileUtils.rm_rf "deploy" 
	FileUtils.mkdir_p "deploy/package/lib" 
	FileUtils.cp "Simpler/bin/Release/Simpler.dll", "deploy/package/lib"
end

desc "Create nuspec file"
nuspec :createSpec => :prepPackage do |nuspec|
   nuspec.id="simpler"
   nuspec.version = "1.0.0"
   nuspec.authors = "Greg Scott"
   nuspec.description = "YADAL"
   nuspec.language = "en-US"
   nuspec.licenseUrl = "https://github.com/gregoryjscott/Simpler/blob/master/LICENSE"
   nuspec.projectUrl = "https://github.com/gregoryjscott/Simpler"
   nuspec.dependency "Castle.Core", "2.5.2"
   nuspec.working_directory = "deploy/package"
   nuspec.output_file = "simpler.nuspec"
end

desc "Create the nuget package"
nugetpack :createPackage => :createSpec do |nugetpack|
   nugetpack.nuspec = "deploy/package/simpler.nuspec"
   nugetpack.base_folder = "deploy/package"
   nugetpack.output = "deploy"
   nugetpack.command = "Tools/nuget.exe"
end