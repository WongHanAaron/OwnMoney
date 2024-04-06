using OwnMoney.Ui.Services.Environment;

namespace OwnMoney.Ui
{
    public partial class App : Application
    {
        protected IWindowSizeListener _sizeListener;

        public App(IWindowSizeListener sizeListener)
        {
            InitializeComponent();
            _sizeListener = sizeListener;

            RegisterForSizeChanges();
        }

        public void RegisterForSizeChanges()
        {
            foreach (var window in Windows)
            {
                window.SizeChanged += (s, o) =>  Window_SizeChanged(s, o, window);
            }
        }

        private void Window_SizeChanged(object? sender, EventArgs e, Window window)
        {
            _sizeListener.OnWindowSizeChange(window.Width, window.Height, window.Title);
        }
    }
}
