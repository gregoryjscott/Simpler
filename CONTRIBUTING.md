# Contributing

Welcome! This document is intended to turn you into a Simpler contributor. The
goal is to make that process as smooth and painless as possible. Please
follow the instructions below to get setup.

If you experience problems or have feedback on this document then please
read the section on [Issues](#issues) and let us know about it.

## Git and GitHub

We use git and GitHub to store and manage our content. Content is not just
source code - it's code, documentation, scripts, notes, etc.

### Steps

1. Setup a [GitHub](https://github.com) account.
1. [Install git](https://help.github.com/articles/set-up-git).

>Technically, simple contributions (e.g. fixing a typo in a document) can
 be made entirely through GitHub without needing to install git locally.
However, this document assumes the user is using git and the user's local
drive to push contributions to GitHub.

## Issues

We use GitHub [Issues](https://guides.github.com/features/issues/) to track the
backlog of work including bugs. Before submitting an issue, please browse the
[current list](https://github.com/gregoryjscott/Simpler/issues) to see if your
issue has already been identified. If it isn't clear if your issue is
unique then go ahead and enter a new issue. Thanks in advance for your
feedback.

## Pull Requests

All content contributions are made using GitHub
[Pull Requests](https://help.github.com/articles/using-pull-requests).
Pull Requests allow team members to review incoming contributions before they are merged.

We prefer contributors to Pull Request as soon as they have something that can be shared with the
team. It isn't expected to be perfect. The idea is to get feedback early from
team members so that we can turn good ideas into great ones, and so we don't waste effort
going down a wrong path. By convention we list the remaining tasks as a list of checkboxes
by making use of a
[GitHub Flavored Markdown trick](https://github.com/blog/1375%0A-task-lists-in-gfm-issues-pulls-comments).
Here's an example.

Remaining Tasks
- [ ] Refactor hacks
- [ ] Squash commits

You can find examples in the list of
[closed Pull Requests](https://github.com/gregoryjscott/Simpler/pulls?direction=desc&page=1&sort=created&state=closed).

### Steps

1. [Fork](https://github.com/gregoryjscott/Simpler/fork) the Simpler repository.
2. Create a feature branch.
3. Push your feature branch to your fork repository.
4. Create a pull request from your feature branch to gregoryjscott:master.

## Branches and Tags

The `master` branch should be considered the "edge" branch and should be the basis for all Pull
Requests. Other important branches should be associated to an open Pull Request.

There are [tags](https://github.com/gregoryjscott/Simpler/tags) for each Simpler release. If you
are building Simpler for production use then you'd be wise to use the latest tagged version of the source.

## Continuous Integration

We believe strongly in the benefits or [Continuous Integration]
(http://en.wikipedia.org/wiki/Continuous_integration) (CI). All the steps to
achieve CI must be automated in order for CI to be successful. We use
[rake](http://rake.rubyforge.org/) tasks to drive the automation. You'll need
ruby and bundler installed to run rake tasks.

>If you don't plan to ever run or build the programs locally then you can skip the CI steps.

### Steps

1. Install Ruby.
1. Run `gem install bundler`.
1. Run `bundle install`.
1. Run `rake install`.

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
project and runs the tests. You should inspect the output and
verify that no errors were found.

```
....................................................
Tests run: 52, Errors: 0, Failures: 0, Inconclusive: 0, Time: 5.237 seconds
  Not run: 0, Invalid: 0, Ignored: 0, Skipped: 0

```
##IDEs

If you're contributing source code then you'll probably want an IDE. Take your pick
based on your preferred operating system.

### Windows

[Microsoft Visual Studio](http://www.microsoft.com/en-us/download/details.aspx?id=34673) is the tool. Currently the solution is VS 2010.

### OSX

[Xamarin Studio](http://xamarin.com/download) works great. The install will also install Mono.
