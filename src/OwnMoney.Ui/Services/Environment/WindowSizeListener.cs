using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnMoney.Ui.Services.Environment
{
    ///<summary> Responsible for listening for window size changes </summary>
    public interface IWindowSizeListener
    {
        ///<summary> To be called when the window size changes </summary>
        void OnWindowSizeChange(double width, double height, string title);

        ///<summary> The event to be hooked into for listening to window size changes </summary>
        EventHandler<WindowSizeUpdate> WindowSizeHasChanged { get; set; }
    }

    public class WindowSizeListener : IWindowSizeListener
    {
        public EventHandler<WindowSizeUpdate> WindowSizeHasChanged { get; set; }

        public void OnWindowSizeChange(double width, double height, string title)
        {
            WindowSizeHasChanged?.Invoke(this, new WindowSizeUpdate()
            {
                Width = width,
                Height = height,
                Title = title
            });
        }
    }

    public class WindowSizeUpdate
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public string Title { get; set; }
    }
}
