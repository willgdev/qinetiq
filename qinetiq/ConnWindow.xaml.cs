using System;
using System.Windows;
using System.ComponentModel;


//TextChanged + Validation.GetHasError(ctrl) for each input and set [connect/cancel/text boxes].IsEnabled accordingly
namespace qinetiq {


    public partial class ConnWindow : Window {


        private IPresenter iPresenter;

        private HandleOpenConnWindow hOpenConnWindow;

        private enum S { INVALID, VALID, CONNECTED }

        private S state;


        public ConnWindow(IPresenter iPresenter) {

            state = S.INVALID;

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

            //iPresenter.cancel();

        }


        private void onChange(object o, RoutedEventArgs r) {

            //

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

