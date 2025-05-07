using Stylet;
using StyletIoC;
using BloodBagScanner.ViewModels;
using BloodBagScanner.Services;

namespace BloodBagScanner
{
    public class Bootstrapper : Bootstrapper<RootViewModel>
    {
        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // service
            builder.Bind<DatabaseService>().ToSelf().InSingletonScope();

            // vm
            builder.Bind<ScanViewModel>().ToSelf().InSingletonScope();
            builder.Bind<HistoryViewModel>().ToSelf().InSingletonScope();
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }
    }
}
