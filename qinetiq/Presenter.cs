

namespace qinetiq {


    public delegate void HandleOpenConnWindow();

    public delegate void HandleConnect();

    public delegate void HandleDataReceived(string msg);

    public delegate void HandleDisconnected();

    public delegate void HandleReceiveError(string error);

    public delegate void HandleStartSend(string msg);

    public delegate void HandleDataSent();

    public delegate void HandleSendError();

    public delegate void HandleCloseApp();


    public interface IPresenter {

        void openConnWindow();

        void connect();

        void onDataReceived(string msg);

        void onDisconnected();

        void onReceiveError(string error);

        void sendData();

        void onDataSent();

        void onSendError();

        void closeApp();

        event HandleOpenConnWindow OnOpenConnWindow;

        event HandleConnect OnConnect;

        event HandleDataReceived OnDataReceived;

        event HandleDisconnected OnDisconnected;

        event HandleReceiveError OnReceiveError;

        event HandleStartSend OnStartSend;

        event HandleSendError OnSendError;

        event HandleDataSent OnDataSent;

        event HandleCloseApp OnCloseApp;

        Model model { get; set; }

    }


    class Presenter : IPresenter {


        public event HandleOpenConnWindow OnOpenConnWindow;

        public event HandleConnect OnConnect;

        public event HandleDataReceived OnDataReceived;

        public event HandleDisconnected OnDisconnected;

        public event HandleReceiveError OnReceiveError;

        public event HandleStartSend OnStartSend;

        public event HandleDataSent OnDataSent;

        public event HandleSendError OnSendError;

        public event HandleCloseApp OnCloseApp;

        public Model model { get; set; }

        private enum State { DISCONNECTED, CONNECTED, SENDING, CONNECTING }

        private State state;


        public Presenter(Model model) {
        
            this.model = model;

        }


        public void openConnWindow() {

            OnOpenConnWindow();

        }


        public void connect() {

            OnConnect();

        }


        public void onDataReceived(string msg) {

            OnDataReceived(msg);
        
        }


        public void onDisconnected() {

            OnDisconnected();

        }


        public void onReceiveError(string error) {

            onReceiveError(error);

        }


        public void sendData() {

            OnStartSend(model.message);

        }


        public void onDataSent() {

            OnDataSent();

        }


        public void onSendError() {

            OnSendError();

        }


        public void closeApp() {

            OnCloseApp();

        }


    }


}

