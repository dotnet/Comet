using System;
using System.Collections.Generic;

namespace HotUI.Tests {
	public class GenericViewHandler: IViewHandler {
		public GenericViewHandler ()
		{
		}

		public View CurrentView { get; private set; }

        public object NativeView => throw new NotImplementedException();

        public bool HasContainer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public readonly Dictionary<string, object> ChangedProperties = new Dictionary<string, object> ();
		public void Remove (View view)
		{
			CurrentView = null;
		}

		public void SetView (View view)
		{
			ChangedProperties.Clear ();
			CurrentView = view;
		}

		public void UpdateValue (string property, object value)
		{
			ChangedProperties [property] = value;
		}

        public void Dispose()
        {
            ChangedProperties?.Clear();
        }
    }
}
