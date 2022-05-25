

namespace qinetiq {


    public interface IPresenter {

        void openConnWindow();

        void sendData();

        void onDataSent();

        void onSendError();

        void onSentClosed();

        void closeApp();

        event HandleOpenConnWindow OnOpenConnWindow;

        event HandleStartSend OnStartSend;

        event HandleSendError OnSendError;

        event HandleDataSent OnDataSent;

        event HandleSendClosed OnSendClosed;

        event HandleCloseApp OnCloseApp;

        Model model { get; set; }

    }


    public delegate void HandleOpenConnWindow();

    public delegate void HandleStartSend(string msg);

    public delegate void HandleDataSent();

    public delegate void HandleSendError();

    public delegate void HandleSendClosed();

    public delegate void HandleCloseApp();


    class Presenter : IPresenter {


        public event HandleOpenConnWindow OnOpenConnWindow;

        public event HandleStartSend OnStartSend;

        public event HandleDataSent OnDataSent;

        public event HandleSendError OnSendError;

        public event HandleSendClosed OnSendClosed;

        public event HandleCloseApp OnCloseApp;

        public Model model { get; set; }

        private enum State { IDLE, SENDING, CONNECTING, SETTINGS }

        private State state;


        public Presenter(Model model) {
        
            this.model = model;

            state = State.IDLE;

        }


        public void openConnWindow() {

            OnOpenConnWindow();

        }


        public void sendData() {

            state = State.SENDING;

            OnStartSend(model.message);

        }


        public void onDataSent() {

            OnDataSent();

            state = State.IDLE;

        }


        public void onSendError() {

            OnSendError();

            state = State.IDLE;

        }


        public void onSentClosed() {

            OnSendClosed();

        }


        public void closeApp() {

            OnCloseApp();

        } 


    }


}

