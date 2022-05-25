using System;
using System.Windows;


namespace qinetiq {


    public partial class MainWindow : Window {


        private IPresenter iPresenter;

        private HandleDataSent hDataSent;

        private HandleSendClosed hSendClosed;

        private HandleSendError hSendError;


        public MainWindow(IPresenter iPresenter) {

            this.iPresenter = iPresenter;

            hDataSent = new HandleDataSent(onDataSent);

            this.iPresenter.OnDataSent += hDataSent;

            hSendClosed = new HandleSendClosed(onSendClosed);

            this.iPresenter.OnSendClosed += hSendClosed;

            hSendError = new HandleSendError(onSendError);

            this.iPresenter.OnSendError += hSendError;

            DataContext = iPresenter.model;

            InitializeComponent();

        }


        private void openConnWindow(object o, RoutedEventArgs r) {

            iPresenter.openConnWindow();

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

            iPresenter.OnSendClosed -= hSendClosed;

            iPresenter.OnSendError -= hSendError;

            iPresenter.OnDataSent -= hDataSent;

            iPresenter.closeApp();

            base.OnClosed(e);

            Application.Current.Shutdown();

        }


    }


}

