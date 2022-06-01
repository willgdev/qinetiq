using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;


namespace qinetiq {


    public partial class ConnWindow : Window {


        private IPresenter iPresenter;

        private HandleOpenConnWindow hOpenConnWindow;

        private HandleConnected hConnected;

        private HandleDisconnected hDisconnected;


        public ConnWindow(IPresenter iPresenter) {

            InitializeComponent();

            this.iPresenter = iPresenter;

            DataContext = iPresenter.model;

            hOpenConnWindow = new HandleOpenConnWindow(show);

            hConnected = new HandleConnected(hide);

            hDisconnected = new HandleDisconnected(hide);

            this.iPresenter.OnOpenConnWindow += hOpenConnWindow;

            this.iPresenter.OnConnected += hConnected;

            this.iPresenter.OnDisconnected += hDisconnected;

        }


        private void connect(object o, RoutedEventArgs r) {

            iPresenter.connect();

        }


        private void cancel(object o, RoutedEventArgs r) {

            iPresenter.disconnect();

        }


        private void onChange(object o, RoutedEventArgs r) {

            receiveTxt.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            destTxt.GetBindingExpression(TextBox.TextProperty).UpdateSource();

        }


        private void show() {

            ShowDialog();

        }


        private void hide() {

            Close();

        }


        protected override void OnClosing(CancelEventArgs c) {

            c.Cancel = true;

            Hide();

        }


        protected override void OnClosed(EventArgs e) {

            iPresenter.OnOpenConnWindow -= hOpenConnWindow;

            iPresenter.OnConnected -= hConnected;

            iPresenter.OnDisconnected -= hDisconnected;

            base.OnClosed(e);

        }


    }


}

