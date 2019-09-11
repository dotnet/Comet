using System;
using System.Collections.ObjectModel;

namespace Comet.Samples
{
    public class Issue125b : View
    {
        private class TodoItem
        {
            public string Name { get; set; }
            public bool Done { get; set; }
        }

        readonly ObservableCollection<TodoItem> items = new ObservableCollection<TodoItem>{
            new TodoItem{
                Name = "Hi",
                Done = true,
            },
            new TodoItem
            {
                Name ="Finish Tasky",
            }
        };


        [Body]
        View body() => new NavigationView{
            new ListView<TodoItem>(items){
                ViewFor = (item)=>new HStack
                    {
                        new Text(item.Name).Frame(alignment: Alignment.Leading),
                        new Spacer(),
                        new Toggle(item.Done).Frame(alignment:Alignment.Center)
                    }.Padding(6).FillHorizontal()
            }.Title("Tasky"),
        };
    }
}
