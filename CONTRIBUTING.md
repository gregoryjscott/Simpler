# Contributing

Welcome! This document gives you the information needed to become a Simpler contributor. Our goal is to make contributing as smooth and as painless as possible. 

If you experience problems or have feedback on this document, please [submit an issue] (#issues).

## <a name="preparing"></a>Preparing to Contribute

We use git and GitHub to store and manage our content. Content is not just source code—it's code, documentation, scripts, notes, etc.

To contribute to Simpler or submit an issue, you should complete the following steps:

1. Set up a [GitHub](https://github.com) account.
1. [Install git](https://help.github.com/articles/set-up-git).
1. If you will be running or building the programs locally, install [Ruby and bundler] (#ci).

>Technically, simple contributions (e.g., fixing a typo in a document) can be made entirely through GitHub without needing to install git locally. However, this document assumes the user is using git and the user's local drive to push contributions to GitHub.

###IDEs

If you're contributing source code, you'll probably want an IDE. Take your pick based on your preferred operating system.

- Windows: [Microsoft Visual Studio](http://www.microsoft.com/en-us/download/details.aspx?id=34673). Current solution is VS 2010.
- OSX: [Xamarin Studio](http://xamarin.com/download) works great. The install will also install Mono.

## <a name="issues"></a>Submitting Issues

We use GitHub [Issues](https://guides.github.com/features/issues/) to track the backlog of work, including bugs. 

>You must have a [GitHub account] (#preparing) to submit an issue. 

1. Before submitting an issue, please browse the [current list](https://github.com/gregoryjscott/Simpler/issues) to see if your issue has already been identified. 
1. If your issue is unique, or if it isn't clear whether it is unique, enter a new issue. 

Thanks in advance for your feedback.

## Contributing Content 

You can contribute content using using GitHub [Pull Requests](https://help.github.com/articles/using-pull-requests), which allow team members to review contributions before they are merged.

To contribute content, follow these steps:

1. [Fork](https://github.com/gregoryjscott/Simpler/fork) the Simpler repository.
2. Create a feature branch.
3. Push your feature branch to your fork repository.
4. Create a Pull Request from your feature branch to gregoryjscott:master.

### Branches

The `master` branch is the "edge" branch and should be the basis for all Pull Requests. Other important branches should be associated to an open Pull Request.

If you are building Simpler for production use, you should use the latest release version of the source. Each Simpler release is [tagged](https://github.com/gregoryjscott/Simpler/tags). 

### When to Create a Pull Request
We prefer that you do a Pull Request as soon as you have something that can be shared with the team. It isn't expected to be perfect. The idea is to get feedback from team members early so that we can turn good ideas into great ones to avoid wasting effort going down a wrong path. 

### Identifying Open Tasks in the Pull Request
In the Pull Request, list the remaining open tasks as a list of checkboxes by using a [GitHub Flavored Markdown trick](https://github.com/blog/1375%0A-task-lists-in-gfm-issues-pulls-comments). The list will be rendered like the following.

Remaining Tasks
- [ ] Refactor hacks
- [ ] Squash commits

You can find examples in the list of [closed Pull Requests](https://github.com/gregoryjscott/Simpler/pulls?direction=desc&page=1&sort=created&state=closed).

## <a name="ci"></a>Using Continuous Integration

We strongly believe in the benefits of [Continuous Integration] (http://en.wikipedia.org/wiki/Continuous_integration) (CI). 

For CI to be successful, all of the steps to achieve CI must be automated. We use [Rake](http://rake.rubyforge.org/) tasks to drive this automation. To run Rake tasks, you'll need to have Ruby and bundler installed .

>If you don't plan to ever run or build the programs locally, you can skip the CI steps.

### Installation Steps

1. Install Ruby.
1. Run `gem install bundler`.
1. Run `bundle install`.
1. Run `rake install`.

###About Rake Commands

`rake` commands are issued from the root of the repo. Run `rake -T` to see the list of tasks. The current output is shown below.

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

You'll use `rake` (shortcut to `rake default`), which builds the project and runs the tests, 99% of the time. After running `rake`, you should inspect the output and verify that no errors were found.

```
....................................................
Tests run: 52, Errors: 0, Failures: 0, Inconclusive: 0, Time: 5.237 seconds
  Not run: 0, Invalid: 0, Ignored: 0, Skipped: 0

```

