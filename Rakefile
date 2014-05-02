require "albacore"
require "fileutils"
require "centroid"

Config = Centroid::Config.from_file("config.json")

desc "Run tests"
task :default => :test

namespace :build do
  desc "Build for debugging"
  build :debug do |b|
    b.sln = Config.solution
    b.prop "Configuration", "Debug"
    b.logging = "minimal"
  end

  desc "Build for release"
  build :release do |b|
    b.sln = Config.solution
    b.prop "Configuration", "Release"
    b.target = ["Clean", "Rebuild"]
    b.logging = "minimal"
  end
end

desc "Run tests"
test_runner :test => ["build:debug"] do |cmd|
  cmd.exe = Config.test_runner
  cmd.files = [Config.tests_dll]
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
    clean(Config.package_lib)
    FileUtils.cp Config.simpler_dll, Config.package_lib
    FileUtils.cp Config.simpler_xml, Config.package_lib
  end

  desc "Pack NuGet package"
  task :pack do
    clean(Config.package_output)
    pack_command = "pack #{Config.nuspec} -OutputDirectory #{Config.package_output} -NoPackageAnalysis"
    nuget(pack_command)
  end

  desc "Push NuGet package"
  task :push do
    pattern = File.join(Config.package_output, "Simpler.?.?.?.nupkg")
    package = Dir[pattern].first
    puts "Pushing #{package}"
    # nuget("push #{package}")
  end
end

desc "Prepare, pack, and push NuGet package"
task :release => ["release:prep", "release:pack", "release:push"]

def nuget(command)
  system "#{Config.nuget} #{command}"
end

def please(command)
  system "#{Config.please} #{command}"
end

def clean(dir)
  FileUtils.rm_rf dir
  FileUtils.mkdir_p dir
end

def bump(type)
  please("bump #{type} version in #{Config.assembly_info}")
  please("bump #{type} version in #{Config.nuspec}")
end
