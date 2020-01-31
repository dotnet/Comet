using System.ComponentModel;

namespace Comet
{
	public interface INotifyPropertyRead : INotifyPropertyChanged
	{
		event PropertyChangedEventHandler PropertyRead;
	}
}
