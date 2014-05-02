require "albacore"
require "fileutils"

desc "Run tests"
task :default => :test

namespace :build do
  desc "Build for debugging"
  build :debug do |b|
    b.sln = "app/Simpler.sln"
    b.prop "Configuration", "Debug"
    b.logging = "minimal"
  end

  desc "Build for release"
  build :release do |b|
    b.sln = "app/Simpler.sln"
    b.prop "Configuration", "Release"
    b.target = ["Clean", "Rebuild"]
    b.logging = "minimal"
  end
end

desc "Run tests"
test_runner :test => ["build:debug"] do |cmd|
  cmd.exe = "test/tools/nunit-console.exe"
  cmd.files = ["app/Simpler.Tests/bin/Debug/Simpler.Tests.dll"]
end

def bump(type)
  system "release/tools/please.exe bump #{type} version in app/Simpler/Properties/AssemblyInfo.cs"
  system "release/tools/please.exe bump #{type} version in release/template/Simpler.nuspec"
end

namespace :bump do
  desc "Bump major version"
  task :major do
    bump "major"
  end

  desc "Bump minor version"
  task :minor do
    bump "minor"
  end

  desc "Bump patch version"
  task :patch do
    bump "patch"
  end
end

namespace :release do
  desc "Prepare package contents"
  task :prep => ["build:release"] do
    FileUtils.rm_rf "release/template/lib"
    FileUtils.mkdir_p "release/template/lib"
    FileUtils.cp "app/Simpler/bin/Release/Simpler.dll", "release/template/lib"
    FileUtils.cp "app/Simpler/bin/Release/Simpler.xml", "release/template/lib"
  end

  desc "Pack NuGet package"
  task :pack do
    FileUtils.rm_rf "release/output"
    FileUtils.mkdir_p "release/output"
    system "release/tools/NuGet.exe pack release/template/Simpler.nuspec -OutputDirectory release/output -NoPackageAnalysis"
  end

  desc "Push Nuget package"
  task :push do
    package = Dir["release/output/Simpler.?.?.?.nupkg"].first
    puts "Pushing #{package}"
    # system "release/tools/NuGet.exe push #{package}"
  end
end
