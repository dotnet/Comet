<img src="https://repobeats.axiom.co/api/embed/f917a77cbbdeee19b87fa1f2f932895d1df18b56.svg" />

# Comet ☄️

[![dev-build](https://github.com/dotnet/Comet/actions/workflows/dev.yml/badge.svg)](https://github.com/dotnet/Comet/actions/workflows/dev.yml)  [![Clancey.Comet on fuget.org](https://www.fuget.org/packages/Clancey.Comet/badge.svg)](https://www.fuget.org/packages/Clancey.Comet)
[Chat on Discord](https://discord.gg/7Ms7ptM)


What is Comet? Comet is a modern way of writing cross-platform UIs. Based on [.Net MAUI](https://docs.microsoft.com/en-us/dotnet/maui/what-is-maui), it follows the Model View Update (MVU) pattern and magically databinds for you!

Watch this video to get a preview of the developer experience:

[![Video Demo](http://img.youtube.com/vi/-Ieg9UadN8s/0.jpg)](http://www.youtube.com/watch?v=-Ieg9UadN8s)

## Getting Started

When you're ready to take a ride on the comet, head over to the wiki and follow the [Getting Started](https://github.com/Clancey/Comet/wiki/Getting-Started) guide.

## Key Concepts

Comet is based on the MVU architecture:

![MVU pattern](art/mvu-pattern.png)

`View` is a screen. Views have a `Body` method that you can assign either by using an attribute `[Body]`:

``` cs
public class MyPage : View {
    [Body]
    View body () => new Text("Hello World");
}
```

Or manually from your constructor:

``` cs
public class MyPage : View {
    public MyPage() {
        Body = body;
    }
    View body () => new Text("Hello World");
}
```

## Hot Reload

Using Hot Reload is the fastest way to develop your user interface.

The setup is simple and only requires a few steps:
1. Install the Visual Studio extension `Comet.Reload` from [Releases](https://github.com/dotnet/Comet/releases) (or [Comet for .Net Mobile](https://marketplace.visualstudio.com/items?itemName=Clancey.comet-debug) if you use Visual Studio Code)
2. Install the [Comet project template](https://www.nuget.org/packages/Clancey.Comet.Templates.Multiplatform) available on Nuget.
3. Add this short snippet to your `AppDelegate.cs` and/or `MainActivity.cs`, or equivalent.

``` cs
#if DEBUG
Comet.Reload.Init();
#endif
```

 See the sample projects [here](https://github.com/dotnet/Comet/tree/dev/sample) for examples.

## State

As of right now there are two supported ways to add state.

### 1. Simple data types like int, bool?

Just add a `State<T>` field to your View

``` cs
class MyPage : View {
    readonly State<int> clickCount = 1;
}
```

`View` is state aware. When the state changes, databinding will automatically update, or rebuild the view as needed.

### 2. Do you want to use more complex data types?

You can either implement [INotifyPropertyRead](https://github.com/Clancey/Comet/blob/master/src/Comet/BindingObject.cs#L13) or you can use [BindingObject](https://github.com/Clancey/Comet/blob/master/src/Comet/BindingObject.cs) to make it even simpler.

Add it as a Field/Property, and add the `[State]` attribute!


``` cs
public class MainPage : View {
    class MyBindingObject : BindingObject {
        public bool CanEdit {
            get => GetProperty<bool> ();
            set => SetProperty (value);
        }
        public string Text {
            get => GetProperty<string> ();
            set => SetProperty (value);
        }
    }

    [State]
    readonly MyBindingObject state;
}

```

`INotifyPropertyRead` is just like `INotifyPropertyChanged`. Just call `PropertyRead` whenever a property Getter is called. And `PropertyChanged` whenever a property Value changes.

### How do I use the State?

Simply update the stateful value and the framework handles the rest.

``` cs
public class MyPage : View {

    readonly State<int> clickCount = 1;
    readonly State<string> text = "Hello World";

    public MyPage() {
        Body = () => new VStack {
            new Text (text),
            new Button("Update Text", () => state.Text = $"Click Count: {clickCount.Value++}")
        };

    }
}
```

That is all!, now when the Text Changes everything updates.

### What if I want to format my value without an extra state property?

While `new Button("Update Text", () => state.Text = $"Click Count: {clickCount.Value++}" )` works, it isn't efficient.

Instead, use `new Text(()=> $"Click Count: {clickCount}")`.

``` cs
public class MyPage : View {

    readonly State<int> clickCount = new State<int> (1);

    public MyPage() {
        Body = () => new VStack {
            new Text (() => $"Click Count: {clickCount}"),
            new Button("Update Text", () => {
                clickCount.Value++;
            }
        };
    }
}

```


## What platforms are supported?

Comet is developped on top of .Net MAUI handlers, providing its own implementation for interfaces such as `Microsoft.Maui.IButton` and other controls. Any platform supported by .Net MAUI can be targeted:

* Windows
* Android
* iOS
* macOS
* Blazor

Non-MAUI application models, such as UWP or WPF, aren't supported.

# Disclaimer

Comet is a **proof of concept**. There is **no** official support. Use at your own Risk.
