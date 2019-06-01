using MvvmCross.Platform.IoC;

namespace Countr.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .EndingWith(("Repository"))
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterNavigationServiceAppStart<ViewModels.CounterViewModel>();
        }
    }
}
