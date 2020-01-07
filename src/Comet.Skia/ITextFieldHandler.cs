using System;
namespace Comet.Skia
{
	public interface ITextFieldHandler : ITextHandler
	{
		public void StartInput();
		public void EndInput();
		public void InsertText(string text);
		public void Backspace();
		public bool IsSecure { get; }
		
	}
}
