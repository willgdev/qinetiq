using System.Windows;


namespace qinetiq {


    public interface IPresenter {

        void openConnWindow();

        event HandleOpenConnWindow OnOpenConnWindow;

    }


    public delegate void HandleOpenConnWindow();


    class Presenter : IPresenter {


        public event HandleOpenConnWindow OnOpenConnWindow;


        public void openConnWindow() {

            OnOpenConnWindow();

        }


    }


}

