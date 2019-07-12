using System;
using System.Collections.Generic;

namespace HotUI.Samples
{
    public class BasicTestView : View
    {
        class MyBindingObject : BindingObject
        {
            public bool CanEdit
            {
                get => GetProperty<bool>();
                set => SetProperty(value);
            }

            public string Text
            {
                get => GetProperty<string>();
                set => SetProperty(value);
            }
        }

        [State]
        readonly MyBindingObject state;

        readonly State<int> clickCount = new State<int>(1);

        readonly State<bool> bar = new State<bool>();

        public BasicTestView()
        {
            state = new MyBindingObject
            {
                Text = "Bar",
                CanEdit = true,
            };
            Body = Build;
        }

        View Build() =>
            new VStack
            {
                (state.CanEdit
                    ? (View) new TextField(() => state.Text, onCommit: value => state.Text = value)
                    : new Text(() => $"{state.Text}: multiText")), // Text will warn you. This should be done by TextBinding
                new Text(state.Text),
                new HStack
                {
                    new Button("Toggle Entry/Label",
                        () => state.CanEdit = !state.CanEdit)
                        .Background(Color.Salmon),
                    new Button("Update Text",
                        () => state.Text = $"Click Count: {clickCount.Value++}" )
                }
            };
    }
}