# HotUI

What is Hot UI? It is a new UI Framework/Patern to write app UI.  It follows the Model View Update patern. You will notice there is no Databinding defined. It magically databinds for you!  

# Key Concepts
HotUI is an MVU style patern.

HotPage is a screen
It has a build method, this is where you define your UI.

```
public class MyPage : HotPage{
	protected override View Build () => new Label{Text="Hello World};
}
```

HotPage is state aware. When the state changes, databinding will automatically update, or rebuild the view if needed.

# State
As of right now there are two supported ways to add state.
Simmple data types like int, bool?
Just add a State<T> field to your HotPage
```
class MyPage : HotPage{
	State<int> clickCount = new State<int>(1);
}
```

## Do you want to use more complex data types?

For now you need to subclass [BindingObject](https://github.com/Clancey/HotUI/blob/master/src/HotUI/BindingObject.cs).

```
public class MainPage : HotPage {
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

Soon you can implement an interface.

## How do I use the sate?

```	public class MyPage : HotPage {

		readonly State<int> clickCount = new State<int> (1);
		readonly State<string> text = new State<string> ("Hello World");


		protected override View Build () => new Stack {
			new Label {Text = text.Value},
			new Button{Text = "Update Text", OnClick = ()=>{
					text.Value = $"Click Count: {clickCount.Value++}";
				}
			}
		};
	}
```

That is all!, now when the Text Changes everything updates. 

#What if I want to format my value without an extra state property?

While `new Label{Text = $"Click Count: {clickCount}"` works, it isnt efficient.

You should use `Label.TextBinding`

```
public class MyPage : HotPage {

		readonly State<int> clickCount = new State<int> (1);

		protected override View Build () => new Stack {
			new Label {TextBinding = ()=> $"Click Count: {clickCount}" },
			new Button{Text = "Update Text", OnClick = ()=>{
				clickCount.Value++;
				}
			}
		};
	}

```


## What platforms will be supported

* Xamarin.Forms
* iOS
* Android
* UWP
* Mac OS


# Does this require Xamarin.Forms?
No!  You can use the navitve versions, and get lots of native awesomesause!