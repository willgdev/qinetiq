using System;
using System.Windows;


namespace qinetiq {


    public partial class MainWindow : Window {


        private IPresenter iPresenter;

        private HandleDataReceived hDataReceived;

        private HandleDisconnected hDisconnected;

        private HandleReceiveError hReceiveError;

        private HandleDataSent hDataSent;

        private HandleSendError hSendError;


        public MainWindow(IPresenter iPresenter) {

            this.iPresenter = iPresenter;

            hDataSent = new HandleDataSent(onDataSent);

            this.iPresenter.OnDataSent += hDataSent;

            hSendError = new HandleSendError(onSendError);

            this.iPresenter.OnSendError += hSendError;

            hDataReceived = new HandleDataReceived(onDataReceived);

            this.iPresenter.OnDataReceived += hDataReceived;

            hDisconnected = new HandleDisconnected(onDisconnected);

            this.iPresenter.OnDisconnected += hDisconnected;

            hReceiveError = new HandleReceiveError(onReceiveError);

            this.iPresenter.OnReceiveError += hReceiveError;

            DataContext = iPresenter.model;

            InitializeComponent();

        }


        private void openConnWindow(object o, RoutedEventArgs r) {

            iPresenter.openConnWindow();

        }


        private void onDataReceived(string msg) {


        }


        private void onDisconnected() {


        }


        private void onReceiveError(string error) {


        }


        private void sendData(object o, RoutedEventArgs r) {

            iPresenter.sendData();

        }


        private void onDataSent() {



        }


        private void onSendError() {



        }


        private void onSendClosed() {



        }


        protected override void OnClosed(EventArgs e) {

            iPresenter.OnDataReceived -= hDataReceived;

            iPresenter.OnDisconnected -= hDisconnected;

            iPresenter.OnReceiveError -= hReceiveError;

            iPresenter.OnSendError -= hSendError;

            iPresenter.OnDataSent -= hDataSent;

            iPresenter.closeApp();

            base.OnClosed(e);

            Application.Current.Shutdown();

        }


    }


}

