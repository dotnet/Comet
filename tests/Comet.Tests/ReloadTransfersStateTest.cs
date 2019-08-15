using System;
using Comet.Internal;
using Xunit;
namespace Comet.Tests
{
    public class ReloadTransfersStateTest : TestBase
    {
        [Fact]
        public void StateIsTransferedToReloadedView()
        {
            ResetComet();
            const string textValue = "Hello";
            var orgView = new MyOrgView();
            
            var orgText = orgView.GetView() as Text;
            Assert.NotEqual(orgView.bindingObject.Title, textValue);
            orgView.bindingObject.Title = textValue;
            Assert.Equal(textValue, orgText.Value);



            HotReloadHelper.RegisterReplacedView(typeof(MyOrgView).FullName, typeof(MyNewView));
            var newText = orgView.GetView() as Text;

            Assert.Equal(textValue, newText.Value);
        }


        [Fact]
        public void StateTransfersOnlyChangedValues()
        {
            ResetComet();
            const string textValue = "Hello";
            var orgView = new MyOrgView();

            var orgText = orgView.GetView() as Text;
            Assert.NotEqual(orgView.bindingObject.Title, textValue);
            orgView.bindingObject.Title = textValue;
            Assert.Equal(textValue, orgText.Value);
            //IsEnabled is defaulted to true.
            Assert.True(orgView.bindingObject.IsEnabled);

            HotReloadHelper.RegisterReplacedView(typeof(MyOrgView).FullName, typeof(MyNewView));
            HotReloadHelper.TriggerReload();

            var newText = orgView.GetView() as Text;
            Assert.NotNull(newText);
            var v = orgView.GetReplacedView();
            var newView = v as MyNewView;
            Assert.IsType<MyNewView>(v);
            Assert.NotNull(newView);
            Assert.False(newView.bindingObject.IsEnabled);
            Assert.Equal(textValue, newView.bindingObject.Title);
        }



        public class MyBindingObject : BindingObject
        {
            public string Title
            {
                get => GetProperty<string>();
                set => SetProperty(value);
            }

            public bool IsEnabled
            {
                get => GetProperty<bool>();
                set => SetProperty(value);
            }
        }

        public class MyOrgView : View
        {
            [State]
            public readonly MyBindingObject bindingObject = new MyBindingObject { IsEnabled = true};

            readonly State<bool> MyBoolean = false;

            [Body]
            View body() => new Text(bindingObject.Title);

        }
        public class MyNewView : View
        {
            [State]
            public readonly MyBindingObject bindingObject = new MyBindingObject { IsEnabled = false };

            readonly State<bool> MyBoolean = false;

            [Body]
            View body() => new Text(bindingObject.Title);
        }
    }
}
