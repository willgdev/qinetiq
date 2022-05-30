

namespace qinetiq {


    public delegate void HandleOpenConnWindow();

    public delegate void HandleConnect();

    public delegate void HandleStartSend(string msg);

    public delegate void HandleDisconnect();

    public delegate void HandleCloseApp();


    public interface IPresenter {

        void openConnWindow();

        void connect();

        void disconnect();

        void sendData();

        void closeApp();

        event HandleOpenConnWindow OnOpenConnWindow;

        event HandleConnect OnConnect;

        event HandleStartSend OnStartSend;

        event HandleDisconnect OnDisconnect;

        event HandleCloseApp OnCloseApp;

        Model model { get; set; }

    }


    class Presenter : IPresenter {


        public event HandleOpenConnWindow OnOpenConnWindow;

        public event HandleConnect OnConnect;

        public event HandleStartSend OnStartSend;

        public event HandleDisconnect OnDisconnect;

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


        public void sendData() {

            model.onStartSend();

            OnStartSend(model.message);

        }


        public void disconnect() {

            OnDisconnect();

        }


        public void closeApp() {

            OnCloseApp();

        }


    }


}

