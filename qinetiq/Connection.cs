using System.Threading;


//one thread to receive, another to send on a different port
//only ONE instance of Connection as destPort and receivePort have to be different
namespace qinetiq {


    public class Connection {


        private IPresenter iPresenter;


        public Connection(IPresenter iPresenter) {

            this.iPresenter = iPresenter;

        }


        public void receive() {

            Thread receiveThread = new Thread(receiveData);

            receiveThread.IsBackground = true;

            receiveThread.Start();

        }


        public void send(string msg) {

            Thread sendThread = new Thread(() => sendData(msg));

            sendThread.IsBackground = true;

            sendThread.Start();

        }


        private void receiveData() {



        }


        private void sendData(string msg) {

            

        }


    }


}

