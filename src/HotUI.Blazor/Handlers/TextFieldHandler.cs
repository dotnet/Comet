using HotUI.Blazor.Components;

namespace HotUI.Blazor.Handlers
{
    internal class TextFieldHandler : BlazorHandler<TextField, BTextField>
    {
        public static PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>
        {
            { nameof(TextField.Text), (view, field) => ((BTextField)view.NativeView).Text = field.Text },
            { nameof(TextField.OnEditingChanged), (view, field) => ((BTextField)view.NativeView).OnChange = field.OnEditingChanged },
        };

        public TextFieldHandler()
            : base(Mapper)
        {
        }
    }
}
