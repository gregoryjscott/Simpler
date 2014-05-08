# Contributing

Welcome! This document is intended to turn you into a contributor. The
goal is to make that process as smooth and painless as possible. Please
follow the instructions below to get setup.

If you experience problems or have feedback on this document then please
read the section on Issues and let us know about it.

# Git and GitHub

We used git and GitHub to store and manage our content. Content is not just
source code - it can be documentation, scripts, etc.

Steps
1. **Install git.** GitHub has good instructions in their Help section.
2. **Setup a GitHub account.**

Technically, simple contributions (e.g. fixing a typo in a document) can
 be made entirely through GitHub without needing to install git locally.
However, this document assumes the user is using git and the user's local
drive to push contributions to GitHub.

# Issues

We use GitHub issues to track our backlog of work items including bugs.
Before submitting an issue, please browse the
[current list](https://github.com/gregoryjscott/Simpler/issues) to see if your
issue has already been identified. If it isn't clear if your issue is
unique then go ahead and enter a new issue. Thanks in advance for your
feedback.

# Pull Requests

All content contributions are made using GitHub Pull Requests. Pull Requests 
allow team members to review incoming contributions before they are merged.

Steps
1. **Fork the repository**.
2. **Create a feature branch.**
3. **Push your feature branch to your fork repository.**
4. **Create a pull request from your feature branch to gregoryjscott:master.**

# Continuous Integration

We believe strongly in the benefits or [Continuous Integration]
(http://en.wikipedia.org/wiki/Continuous_integration) (CI). All the steps to
achieve CI must be scripted in order to do CI well. We use
[rake](http://rake.rubyforge.org/) tasks to drive everything. You'll need
ruby and bundler installed to run rake tasks.

Steps
1. Install Ruby.
1. `gem install bundler`
1. `bundle install`

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

