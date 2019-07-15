# HotUI

What is Hot UI? It is a new UI Framework/Patern to write app UI.  It follows the Model View Update patern. It magically databinds for you!  

# Key Concepts
HotUI is an MVU style patern.

View is a screen
Views have a Body method that you can assign either by an Attribute `[Body]` or by specifying

```
public class MyPage : View{
	[Body]
	View body () => new Text("Hello World);
}
```

or

```
public class MyPage : View{
	public MyPage(){
		Body = body;
	}
	View body () => new Text("Hello World);
}
```


# Hot Reload
HotReload is included by default!
Download and install the VS extension from the [Releases](https://github.com/Clancey/HotUI/releases/)
Then add to your apps code.

``` 
 #if DEBUG
            HotUI.Reload.Init();
 #endif
```


# State
As of right now there are two supported ways to add state.
Simmple data types like int, bool?
Just add a `State<T>` field to your View

```
class MyPage : View{
	readonly State<int> clickCount = 1;
}
```


View is state aware. When the state changes, databinding will automatically update, or rebuild the view if needed.

## Do you want to use more complex data types?

You can either implement [INotifyPropertyRead](https://github.com/Clancey/HotUI/blob/master/src/HotUI/BindingObject.cs#L13) or you can use [BindingObject](https://github.com/Clancey/HotUI/blob/master/src/HotUI/BindingObject.cs) to make it simpler.

Add it as a Field/Property, and add the [State] attribute!


```
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

`INotifyPropertyRead` is just like INotifyPropertyChanged. Just call `PropertyRead` whenever a property Getter is called. And `PropertyChanged` whenever a property Value changes.

## How do I use the State?

```	public class MyPage : View {

		readonly State<int> clickCount = 1;
		readonly State<string> text = "Hello World";

		public MyPage() {
			Body = () => new Stack {
				new Text (text),			
				new Button("Update Text",
	                        () => state.Text = $"Click Count: {clickCount.Value++}" )
				}
			};

		}
	}
```

That is all!, now when the Text Changes everything updates. 

#What if I want to format my value without an extra state property?

While `new Text($"Click Count: {clickCount})"` works, it isnt efficient.

You should use `new Text(()=> $"Click Count: {clickCount}")`

```
public class MyPage : View {

		readonly State<int> clickCount = new State<int> (1);

		public MyPage() {
			Body = () => new Stack {
				new Text (()=> $"Click Count: {clickCount}"),
				new Button("Update Text", ()=>{
					clickCount.Value++;
				}
			};
		}
	}

```


## What platforms will be supported

* Xamarin.Forms
* iOS
* Android
* UWP
* Mac OS


# Does this require Xamarin.Forms?
No!  You can use the native versions, and get lots of native awesomesause!