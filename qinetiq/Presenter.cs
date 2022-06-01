

namespace qinetiq {


    public delegate void HandleOpenConnWindow();

    public delegate void HandleConnect();

    public delegate void HandleConnected();

    public delegate void HandleStartSend(string msg);

    public delegate void HandleDisconnect();

    public delegate void HandleDisconnected();

    public delegate void HandleCloseApp();


    public interface IPresenter {

        void openConnWindow();

        void connect();

        void onConnected();

        void onDisconnected();

        void disconnect();

        void sendData();

        void closeApp();

        event HandleOpenConnWindow OnOpenConnWindow;

        event HandleConnect OnConnect;

        event HandleConnected OnConnected;

        event HandleStartSend OnStartSend;

        event HandleDisconnect OnDisconnect;

        event HandleDisconnected OnDisconnected;

        event HandleCloseApp OnCloseApp;

        Model model { get; set; }

    }


    class Presenter : IPresenter {


        public event HandleOpenConnWindow OnOpenConnWindow;

        public event HandleConnect OnConnect;

        public event HandleConnected OnConnected;

        public event HandleStartSend OnStartSend;

        public event HandleDisconnect OnDisconnect;

        public event HandleDisconnected OnDisconnected;

        public event HandleCloseApp OnCloseApp;

        public Model model { get; set; }


        public Presenter(Model model) {
        
            this.model = model;

        }


        public void openConnWindow() {

            OnOpenConnWindow();

        }


        public void connect() {

            model.onConnect();

            OnConnect();

        }


        public void onConnected() {

            OnConnected();

        }


        public void sendData() {

            model.onStartSend();

            OnStartSend(model.message);

        }


        public void disconnect() {

            model.onStartDisconnect();

            OnDisconnect();

        }


        public void onDisconnected() {

            model.onAfterDisconnected();

            OnDisconnected();

        }


        public void closeApp() {

            OnCloseApp();

        }


    }


}

