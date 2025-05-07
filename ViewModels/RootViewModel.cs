using Stylet;
using StyletIoC;

namespace BloodBagScanner.ViewModels
{
    public class RootViewModel : Conductor<Screen>.Collection.OneActive
    {
        private IContainer _container;
        private IWindowManager _windowManager;

        public RootViewModel(IContainer container, IWindowManager windowManager)
        {
            _container = container;
            _windowManager = windowManager;
        }

        protected override void OnViewLoaded()
        {
            InitViewModels();
        }

        private void InitViewModels()
        {
            var svm = _container.Get<ScanViewModel>();
            var hvm = _container.Get<HistoryViewModel>();

            Items.Add(svm);
            Items.Add(hvm);

            ActivateItem(svm);
        }
    }
}
