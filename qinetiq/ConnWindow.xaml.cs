using System;
using System.Windows;
using System.ComponentModel;


namespace qinetiq {


    public partial class ConnWindow : Window, IDisposable
    {


        private IPresenter iPresenter;

        private HandleOpenConnWindow hOpenConnWindow;


        public ConnWindow(IPresenter iPresenter) {

            InitializeComponent();

            this.iPresenter = iPresenter;

            hOpenConnWindow = new HandleOpenConnWindow(show);

            this.iPresenter.OnOpenConnWindow += hOpenConnWindow;

        }


        private void show() {

            ShowDialog();

        }


        protected override void OnClosing(CancelEventArgs c) {

            c.Cancel = true;

            Hide();

        }


        public void Dispose() {

            iPresenter.OnOpenConnWindow -= hOpenConnWindow;

        }


    }


}

