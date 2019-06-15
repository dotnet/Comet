using System;
using System.Reflection;

namespace HotForms {
	

	class StateAdvice<T> : DispatchProxy {
		protected override object Invoke (MethodInfo targetMethod, object [] args)
		{
			throw new NotImplementedException ();
		}

		public static T Create(Action valueChanged)
		{
			throw new NotImplementedException ();
			//Mono doesnt implement this :(
			return DispatchProxy.Create<T, StateAdvice<T>> ();
		}
	}
}
