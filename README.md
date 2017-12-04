# Constraint Capers Workbench
A tool to explore how to model a constraint satisfaction problem with an engine for solving it and presenting a solution. Uses the [Google or-tools](https://developers.google.com/optimization/) for solving the problem.

The program idea is outlined in [The strange case of the missing application](http://techteapot.com/strange-case-of-the-missing-application/). The Workbench is an attempt to build the missing application, to see if such a thing exists, or has *any* utility. In the past whenever I've tried to come up with a suitable interface I've always eventually come up with something a lot like a spreadsheet. Just goes to show just how good a paradigm the spreadsheet is.

Plainly a big barrier to any kind of constraint satisfaction project is going to be the fact that they are [NP-hard](http://en.wikipedia.org/wiki/NP-hard) problems. In other words, they tend to be *very* hard to solve quickly. I'm kind of working on the assumption that Moore's Law is going to rescue me. All of those hundreds or thousands of CPU cores we are supposed to be getting over the next decade or so should help. Quantum computers should help a lot too. So, I am just going to assume that, at some point in the future, the hardware will be there to solve even very complex constraint satisfaction problems in a reasonable amount of time.

## Project manifesto
* **Technical level** - the user of the software should require a technical level at or below that of an average spreadsheet user;
* **Build a playground** - the user should have a constraint workbench in which they are able to model various types of constraint problems and then able to solve them in an interactive manner and present the results;
* **Solution display** - should be configurable, taking a technical model and displaying the resulting solution in a manner that makes sense to the user;
* **No programming** - absolutely no programming. There are plenty of development environments available, there is no need to build yet another one here;
* **Optimisation** - The user should be able to optimise the solution interactively.

## Current State

The image below is a picture of the model used to solve the n-queens problem produced by version 0.4. As you can see the application supports aggregate variables, expression and all different constraints and domains.

![Workbench Displaying a Model](https://techteapot.com/wp-content/uploads/2016/10/workbench-model-nqueens-1024x611.png)

You can also create a solution. When I say a solution I don't mean just a set of values that represent one state that satisfies all of the constraints. I mean you can design how your solution is displayed in very rudimentary ways. Currently there are two ways of displaying a solution, a chess board and a table.

![n-queens solution](https://techteapot.com/wp-content/uploads/2016/10/workbench-solution-nqueens-768x761.png)

The most egregious failing of the project manifesto to date is the breach of the *no programming* rule. There plainly is quite a lot of what looks suspiciously like programming in the various languages included in the project. The most obvious being the visualizer binding language. I have a few ideas how I can reduce the level of programming in the visualizer bindings but I doubt I can remove it altogether.

Please do not use this project for anything other than experimentation. I make no guarantees about backward compatibility or indeed anything else. The project is currently just a prototype. It may well never be anything beyond that.

## Built With

+ C#
+ Google or-tools - CSP library amongst many other things
+ WPF
+ Caliburn Micro
+ Irony - used to build the parsers
+ NUnit - unit test framework

## Authors

+ Jack Hughes - Initial work - (digitalbricklayer)[https://github.com/digitalbricklayer]

## License
Constraint Capers Workbench is [licensed under a BSD license](LICENSE.md).

## Acknowledgements

Many thanks to [Ashley Davies](http://www.codecapers.com.au) for writing [NetworkView: A WPF custom control for visualizing and editing networks, graphs and flow-charts](http://www.codeproject.com/Articles/182683/NetworkView-A-WPF-custom-control-for-visualizing-a) upon which the prototype of the model display is based.

Thanks also to [David Hopkins](http://semlabs.co.uk/) for the edit icon used in the program. The original can be found [here](http://findicons.com/icon/180721/pencil_small?id=378530).
