using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;


//one thread to receive, another to send on a different port
//only ONE instance of Connection as destPort and receivePort have to be different
namespace qinetiq {


    public class Connection {


        private IPresenter iPresenter;

        private HandleStartSend hStartSend;

        private HandleCloseApp hCloseApp;


        public Connection(IPresenter iPresenter) {

            this.iPresenter = iPresenter;

            hStartSend = new HandleStartSend(send);

            hCloseApp = new HandleCloseApp(close);

            this.iPresenter.OnStartSend += hStartSend;

            this.iPresenter.OnCloseApp += hCloseApp;

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


        public void close() {

            this.iPresenter.OnStartSend -= hStartSend;

            this.iPresenter.OnCloseApp -= hCloseApp;

        }


        private void receiveData() {



        }


        private void sendData(string msg) {

            try {

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                byte[] sendBytes = Encoding.UTF8.GetBytes(msg);

                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(iPresenter.model.ipAddress), iPresenter.model.destPort);

                socket.SendTo(sendBytes, ipEndPoint);

                Application.Current.Dispatcher.Invoke(new Action(() => { iPresenter.onDataSent(); }));

                socket.Shutdown(SocketShutdown.Both);

                socket.Close();

                Application.Current.Dispatcher.Invoke(new Action(() => { iPresenter.onSentClosed(); }));

            }

            catch (Exception e) when (e is SocketException || e is ThreadInterruptedException) {

                Application.Current.Dispatcher.Invoke(new Action(() => { iPresenter.onSendError(); }));
            
            }

        }


    }


}

