using System;
using System.Windows;
using System.Windows.Controls;


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


        private void sendData(object o, RoutedEventArgs r) {

            iPresenter.sendData();

        }


        protected override void OnClosed(EventArgs e) {

            iPresenter.closeApp();

            base.OnClosed(e);

            Application.Current.Shutdown();

        }


    }


}

