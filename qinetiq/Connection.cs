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

        private volatile bool listen;

        private const int udpTimeout = 3000;


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

            iPresenter.OnStartSend -= hStartSend;

            iPresenter.OnCloseApp -= hCloseApp;

        }


        private void receiveData() {

            UdpClient? udpClient = null;

            try {

                udpClient = new UdpClient(iPresenter.model.receivePort);

                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(iPresenter.model.ipAddress), iPresenter.model.receivePort);

                while (listen) {

                    byte[] data = udpClient.Receive(ref ipEndPoint);

                    Application.Current.Dispatcher.Invoke(
                        new Action(() => { iPresenter.onDataReceived(Encoding.Default.GetString(data)); })
                    );

                }

                Application.Current.Dispatcher.Invoke(new Action(() => { iPresenter.onDisconnected(); }));

            }

            catch (Exception e) when (
                e is SocketException ||
                e is ObjectDisposedException ||
                e is ThreadInterruptedException
            ) {

                listen = false;

                Application.Current.Dispatcher.Invoke(new Action(() => { iPresenter.onReceiveError(e.GetType); }));

            }

            if (udpClient != null) udpClient.Close();

        }


        private void sendData(string msg) {

            Socket? socket = null;

            try {

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                socket.SendTimeout = udpTimeout;

                byte[] sendBytes = Encoding.UTF8.GetBytes(msg);

                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(iPresenter.model.ipAddress), iPresenter.model.destPort);

                socket.SendTo(sendBytes, ipEndPoint);

                Application.Current.Dispatcher.Invoke(new Action(() => { iPresenter.onDataSent(); }));

            }

            catch (Exception e) when (e is SocketException || e is ThreadInterruptedException) {

                Application.Current.Dispatcher.Invoke(new Action(() => { iPresenter.onSendError(); }));
            
            }

            if (socket != null) socket.Close();

        }


    }


}

