require "albacore"
require "fileutils"
require "centroid"

Config = Centroid::Config.from_file("config.json")

ROOT = File.expand_path "."

desc "Install NuGet dependencies"
task :install do
  Config.packages.configs.each do |file|
    nuget("install #{file}")
  end
end

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
test_runner :test => ["build:debug", "clean:test"] do |tr|
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

namespace :clean do
  desc "Clean all output"
  task :all => ["clean:build", "clean:test", "clean:release"]

  desc "Clean build output"
  task :build do
    clean(Config.build.output.debug)
    clean(Config.build.output.release)
  end

  desc "Clean test output"
  task :test do
    clean(Config.test.output)
  end

  desc "Clean release output"
  task :release do
    clean(Config.release.output.prep)
    clean(release_lib);
    clean(Config.release.output.pack)
  end
end

namespace :release do
  desc "Pack NuGet package"
  task :pack => ["build:release", "clean:release"] do
    FileUtils.cp Config.release.nuspec, release_nuspec
    Config.release.files.each do |file|
      FileUtils.cp file, release_lib
    end
    nuget("pack #{release_nuspec} -OutputDirectory #{Config.release.output.pack}")
  end

  desc "Push NuGet package"
  task :push do
    puts "Pushing #{release_nupkg}"
    # nuget("push #{release_nupkg}")
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

def release_lib
  File.join(Config.release.output.prep, "lib")
end

def release_nuspec
  File.join(Config.release.output.prep, File.basename(Config.release.nuspec))
end

def release_nupkg
  pattern = File.join(Config.release.output.pack, Config.release.nupkgPattern)
  Dir[pattern].first
end
