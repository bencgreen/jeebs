---
layout: default
title: Option<T> Type
nav_order: 1
has_children: true
---

# The `Option<T>` Type
{: .no_toc }

## Contents
{: .no_toc }

- TOC
{:toc}

## Why?

F# contains a built-in `Option` type, and I wanted to be able to code in that style but using C#.  Over a year or so I have designed `Option<T>` to mimic some of F#'s behaviour using C# way.  It works best when you chain pure (and small) functions together - and if you use it well your exception handler will have very little to do!

The other inspiration behind the type is Scott Wlaschin's [Railway Oriented Programming](https://fsharpforfunandprofit.com/posts/against-railway-oriented-programming/).  I have limited experience of functional programming, so my `Option<T>` is not an implementation of his work, but it contains some similar ideas: in particular the 'failure' or 'sad' path, whereby if one function fails, the rest are skipped over.

## Concepts

The following serves as an introduction to the concepts behind `Option<T>` which are likely to be a little alien to most C# developers.  However, if you want to skip ahead straight to the code, feel free!

### Function Signatures

As `Option<T>` is a mix of C# and F# I will be using the following notation across the documentation: `function signature -> return type`, for example `AddTwoNumbers(int, int) -> int`.  This is partly for brevity, and partly because it is similar to how function signatures are written in F#, which will become useful as the functions get more complex.

### Pure Functions

'Pure' functions have no effect outside themselves.  In other words, they receive an input, act on it, and return an output.  They don't affect state, and they don't affect the input object.

In the OO world of C#, I'll admit, this is odd.  There's not really any such thing as a 'function' - at least not one that exists outside a class definition.  However as far as I can, `Option<T>` is written as a series of pure functions, so even the methods in the `Option<T>` class and the extension methods are simply wrappers for the functions, which all live in the `F.OptionF` namespace.

### `Some` and `None`

`Option<T>` is an abstract class with two implementations.  The return type for a function is *always* `Option<T>`, but the actual object will be one of these:

- `Some<T>` which contains a `Value` property of type `T`
- `None<T>` which contains a `Reason` property of type `IMsg?`

Think of `None<T>` as a more useful nullable, because it comes with a reason *why* it has no value.

### No More Exceptions

Within the Jeebs library - and I encourage you to follow the same discipline if you decide to you `Option<T>`, the contract is that **if a function returns `Option<T>` it has handled all its exceptions so you don't have to**.

This is a critical part of the usefulness of `Option<T>`, and to be honest if you prefer having and handling exceptions I suggest you stop reading!  However, I do believe there is a better way...

### The World of Options

F# developers like to talk about *monads*, which we have in C# too: `IEnumerable<T>` is a monad, for example.  Effectively, monads are a wrapper type with certain properties - if you want to know more, there are better teachers than me!

**You cannot create an `Option<T>` - whether `Some<T>` or `None<T>` directly.**  Instead there are wrapper functions that do the creating for you, and assist with exception handling.

So, let's begin with them!