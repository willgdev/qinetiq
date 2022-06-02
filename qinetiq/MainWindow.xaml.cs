using System;
using System.ComponentModel;
using System.Windows;


namespace qinetiq {


    public partial class MainWindow : Window {


        private IPresenter iPresenter;

        private HandleDisconnected hDisconnected;

        private HandleReceiveError hReceiveError;

        private enum Shutdown { NONE, CALLED, CONFIRMED };

        private Shutdown shutdown = Shutdown.NONE;


        public MainWindow(IPresenter iPresenter) {

            this.iPresenter = iPresenter;

            DataContext = iPresenter.model;

            hDisconnected = new HandleDisconnected(onDisconnected);

            hReceiveError = new HandleReceiveError(onDisconnected);

            this.iPresenter.OnDisconnected += hDisconnected;

            this.iPresenter.OnReceiveError += hReceiveError;

            InitializeComponent();

        }


        private void openConnWindow(object o, RoutedEventArgs r) {

            iPresenter.openConnWindow();

        }


        private void sendData(object o, RoutedEventArgs r) {

            iPresenter.sendData();

        }


        private void onDisconnected() {

            if (shutdown == Shutdown.CALLED) {

                shutdown = Shutdown.CONFIRMED;

                Close();

            }

        }


        protected override void OnClosing(CancelEventArgs c) {

            if (shutdown == Shutdown.CONFIRMED) return;

            if (shutdown == Shutdown.CALLED) {

                c.Cancel = true;

                return;

            }

            shutdown = Shutdown.CALLED;

            if (iPresenter.model.allowDisconnect) {

                c.Cancel = true;

                iPresenter.disconnect();

            }

        }


        protected override void OnClosed(EventArgs e) {

            iPresenter.OnDisconnected -= hDisconnected;

            iPresenter.OnReceiveError -= hReceiveError;

            iPresenter.closeApp();

            base.OnClosed(e);

            Application.Current.Shutdown();

        }


    }


}

