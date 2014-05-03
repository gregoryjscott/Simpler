require "albacore"
require "fileutils"
require "centroid"

Config = Centroid::Config.from_file("config.json")

ROOT = File.expand_path "."

desc "Run tests"
task :default => :test

namespace :build do
  desc "Build for debugging"
  build :debug do |b|
    b.sln = Config.build.solution
    b.prop "Configuration", "Debug"
    b.prop "OutputPath", File.join(ROOT, Config.build.output.debug)
    b.logging = "minimal"
  end

  desc "Build for release"
  build :release do |b|
    b.sln = Config.build.solution
    b.prop "Configuration", "Release"
    b.prop "OutputPath", File.join(ROOT, Config.build.output.release)
    b.target = ["Clean", "Rebuild"]
    b.logging = "minimal"
  end
end

desc "Run tests"
test_runner :test => ["build:debug"] do |tr|
  clean(Config.test.output)

  tr.exe = Config.tools.nunit
  tr.files = [Config.test.dll]
  tr.add_parameter "-xml=#{File.join(ROOT, Config.test.results)}"
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
  desc "Pack NuGet package"
  task :pack => ["build:release"] do
    clean(Config.release.output.prep)
    clean(Config.release.output.pack)

    lib = File.join(Config.release.output.prep, "lib");
    FileUtils.mkdir_p lib
    Config.release.files.each do |file|
      FileUtils.cp file, lib
    end

    nuspec = File.join(Config.release.output.prep, File.basename(Config.bump.nuspec))
    FileUtils.cp Config.bump.nuspec, nuspec

    command = "pack #{nuspec} -OutputDirectory #{Config.release.output.pack}"
    nuget(command)
  end

  desc "Push NuGet package"
  task :push do
    pattern = File.join(Config.release.output.pack, Config.release.nupkgPattern)
    package = Dir[pattern].first
    puts "Pushing #{package}"
    # nuget("push #{package}")
  end
end

desc "Pack and push NuGet package"
task :release => ["release:pack", "release:push"]

def nuget(command)
  system "#{Config.tools.nuget} #{command}"
end

def please(command)
  system "#{Config.tools.please} #{command}"
end

def clean(dir)
  FileUtils.rm_rf dir
  FileUtils.mkdir_p dir
end

def bump(type)
  Config.bump.files.each do |file|
    please("bump #{type} version in #{file}")
  end
end
