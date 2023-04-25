using System.Reflection;
using CometApp1;
using Comet.Tests;
using Comet;
using Button = Comet.Button;
namespace CometApp1Test;

public class MainPageUnitTest : TestBase
{
    [Fact]
    public void ButtonClickTest()
    {
        var page = new MainPage();
        page.SetViewHandlerToGeneric();
        var button = page.GetViewWithTag<Button>("button");
        ((Action)button.Clicked.Value).Invoke();
        Assert.Equal(page.comet.Rides, 1);
    }
}