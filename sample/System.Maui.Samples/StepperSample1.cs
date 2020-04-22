using System;
namespace System.Maui.Samples
{
    public class StepperSample1 : View
    {
        readonly State<double> min = 0;
        readonly State<double> max = 10;
        readonly State<double> increment = 1;
        readonly State<double> number1 = 0;
        private double currentValue;

        [Body]
        View body() => new VStack
        {
            new Label($"{number1.Value}"),
            new Stepper(number1,max,min, increment)
        };
    }
}
