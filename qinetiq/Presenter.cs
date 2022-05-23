using System.Collections.Generic;


namespace qinetiq {


    public interface IPresenter {

        void openConnWindow();

        event HandleOpenConnWindow OnOpenConnWindow;

        Model model { get; set; }

    }


    public delegate void HandleOpenConnWindow();


    class Presenter : IPresenter {


        public event HandleOpenConnWindow OnOpenConnWindow;

        public Model model { get; set; }


        public Presenter(Model model) {
        
            this.model = model;

        }


        public void openConnWindow() {

            OnOpenConnWindow();

        }


    }


}

