using System.Windows;
using Microsoft.Extensions.DependencyInjection;


namespace qinetiq {


    public partial class App : Application {


        private ServiceProvider serviceProvider;


        public App () {

            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IPresenter, Presenter>();

            services.AddSingleton<MainWindow>();

            services.AddSingleton<ConnWindow>();

            services.AddSingleton<Model>();

            services.AddSingleton<Connection>();

            serviceProvider = services.BuildServiceProvider();

        }


        private void OnStartQinetiqApp(object o, StartupEventArgs s) {

            serviceProvider.GetService<MainWindow>()?.Show();

            serviceProvider.GetService<ConnWindow>();

            serviceProvider.GetService<Connection>();

        }


    }


}

