using System;
using System.Windows;


namespace qinetiq {


    public partial class MainWindow : Window {


        private IPresenter iPresenter;


        public MainWindow(IPresenter iPresenter) {

            this.iPresenter = iPresenter;

            DataContext = iPresenter.model;

            InitializeComponent();

        }


        private void openConnWindow(object o, RoutedEventArgs r) {

            iPresenter.openConnWindow();

        }


        protected override void OnClosed(EventArgs e) {

            base.OnClosed(e);

            Application.Current.Shutdown();

        }


    }


}

