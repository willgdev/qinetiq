using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;


namespace qinetiq {


    public partial class ConnWindow : Window {


        private IPresenter iPresenter;

        private HandleOpenConnWindow hOpenConnWindow;


        public ConnWindow(IPresenter iPresenter) {

            InitializeComponent();

            this.iPresenter = iPresenter;

            DataContext = iPresenter.model;

            hOpenConnWindow = new HandleOpenConnWindow(show);

            this.iPresenter.OnOpenConnWindow += hOpenConnWindow;

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


        protected override void OnClosing(CancelEventArgs c) {

            c.Cancel = true;

            Hide();

        }


        protected override void OnClosed(EventArgs e) {

            iPresenter.OnOpenConnWindow -= hOpenConnWindow;

            base.OnClosed(e);

        }


    }


}

