require "albacore"
require "fileutils"
require "centroid"

Config = Centroid::Config.from_file("config.json")

OUTPUT_ROOT = File.expand_path "output"

desc "Run tests"
task :default => :test

namespace :build do
  desc "Build for debugging"
  build :debug do |b|
    b.sln = Config.solution
    b.prop "Configuration", "Debug"
    b.prop "OutputPath", File.join(OUTPUT_ROOT, "build", "debug")
    b.logging = "minimal"
  end

  desc "Build for release"
  build :release do |b|
    b.sln = Config.solution
    b.prop "Configuration", "Release"
    b.prop "OutputPath", File.join(OUTPUT_ROOT, "build", "release")
    b.target = ["Clean", "Rebuild"]
    b.logging = "minimal"
  end
end

desc "Run tests"
test_runner :test => ["build:debug"] do |tr|
  output_path = File.join(OUTPUT_ROOT, "test")
  clean(output_path)
  tr.exe = Config.test_runner
  tr.files = [Config.tests_dll]
  tr.add_parameter "-xml=#{File.join(output_path, "results.xml")}"
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
    clean(Config.release.prep.root)
    FileUtils.cp Config.simpler_dll, Config.release.prep.lib
    FileUtils.cp Config.simpler_xml, Config.release.prep.lib
    FileUtils.cp Config.nuspec, Config.release.prep.nuspec
  end

  desc "Pack NuGet package"
  task :pack do
    clean(Config.package_output)
    pack_command = "pack #{Config.release.prep.nuspec} -OutputDirectory #{Config.package_output}"
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
