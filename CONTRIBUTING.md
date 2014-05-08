# Contributing

Welcome! This document is intended to turn you into a contributor. The
goal is to make that process as smooth and painless as possible. Please
follow the instructions below to get setup.

If you experience problems or have feedback on this document then please
read the section on Issues and let us know about it.

## Git and GitHub

We used git and GitHub to store and manage our content. Content is not just
source code - it can be documentation, scripts, etc.

Steps
1. **Install git.** GitHub has good instructions in their Help section.
2. **Setup a GitHub account.**

Technically, simple contributions (e.g. fixing a typo in a document) can
 be made entirely through GitHub without needing to install git locally.
However, this document assumes the user is using git and the user's local
drive to push contributions to GitHub.

## Issues

We use GitHub issues to track our backlog of work items including bugs.
Before submitting an issue, please browse the
[current list](https://github.com/gregoryjscott/Simpler/issues) to see if your
issue has already been identified. If it isn't clear if your issue is
unique then go ahead and enter a new issue. Thanks in advance for your
feedback.

## Pull Requests

All content contributions are made using GitHub Pull Requests. Pull Requests 
allow team members to review incoming contributions before they are merged.

We prefer contributors to Pull Request early. We like to specify what else we
have planned as a list of checkboxes.

E.g.
- [ ] Squash my WIP commits

You'll find examples in the list of [closed Pull Requests](https://github.com/gregoryjscott/Simpler/pulls?direction=desc&page=1&sort=created&state=closed).

Steps
1. **Fork the repository**.
2. **Create a feature branch.**
3. **Push your feature branch to your fork repository.**
4. **Create a pull request from your feature branch to gregoryjscott:master.**

## Branches and Tags

`master` should be considered the edge branch and should be the basis for all Pull
Requests. Other important branches should be associated to an open Pull Request.

There are [tags](https://github.com/gregoryjscott/Simpler/tags) for stable versions.

## Continuous Integration

We believe strongly in the benefits or [Continuous Integration]
(http://en.wikipedia.org/wiki/Continuous_integration) (CI). All the steps to
achieve CI must be scripted in order to do CI well. We use
[rake](http://rake.rubyforge.org/) tasks to drive everything. You'll need
ruby and bundler installed to run rake tasks.

Steps
1. Install Ruby.
1. Run `gem install bundler`.
1. Run `bundle install`.
2. Run `rake install`.

`rake` commands are issued from the root of the repo. Run `rake -T` to see the
list of tasks. The current output is shown below.

```
$ rake -T
rake build:debug    # Build for debugging
rake build:release  # Build for release
rake bump:major     # Bump major version
rake bump:minor     # Bump minor version
rake bump:patch     # Bump patch version
rake clean:all      # Clean all output
rake clean:release  # Clean release output
rake clean:test     # Clean test output
rake default        # Run tests
rake install        # Install NuGet dependencies
rake release        # Pack and push NuGet package
rake release:pack   # Pack NuGet package
rake release:push   # Push NuGet package
rake test           # Run tests
```

99% of the time you'll use `rake` (shortcut to `rake default`) which builds the
project and tests to verify the project is OK. You should inspect the output and
verify that no errors were found.

```
....................................................
Tests run: 52, Errors: 0, Failures: 0, Inconclusive: 0, Time: 5.237 seconds
  Not run: 0, Invalid: 0, Ignored: 0, Skipped: 0

```
##IDEs

If you're contributing source code then you'll probably want an IDE for debugging
and/or editing. Take your pick based on your preferred operating system.

* Windows - Microsoft Visual Studio
* OSX - Xamarin Studio
