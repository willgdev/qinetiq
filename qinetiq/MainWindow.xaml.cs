using System;
using System.ComponentModel;
using System.Windows;


namespace qinetiq {


    public partial class MainWindow : Window {


        private IPresenter iPresenter;

        private HandleDisconnected hDisconnected;

        private bool shutdown;


        public MainWindow(IPresenter iPresenter) {

            this.iPresenter = iPresenter;

            DataContext = iPresenter.model;

            hDisconnected = new HandleDisconnected(onDisconnected);

            this.iPresenter.OnDisconnected += hDisconnected;

            InitializeComponent();

        }


        private void openConnWindow(object o, RoutedEventArgs r) {

            iPresenter.openConnWindow();

        }


        private void sendData(object o, RoutedEventArgs r) {

            iPresenter.sendData();

        }


        private void onDisconnected() {

            if (shutdown) Close();

        }


        protected override void OnClosing(CancelEventArgs c) {

            shutdown = true;

            if (!iPresenter.model.isNotConnected) {

                c.Cancel = true;

                iPresenter.disconnect();

            }

        }


        protected override void OnClosed(EventArgs e) {

            iPresenter.OnDisconnected -= hDisconnected;

            iPresenter.closeApp();

            base.OnClosed(e);

            Application.Current.Shutdown();

        }


    }


}

