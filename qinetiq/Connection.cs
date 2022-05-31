using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;


namespace qinetiq {


    public class Connection {


        private IPresenter iPresenter;

        private HandleConnect hConnect;

        private HandleStartSend hStartSend;

        private HandleDisconnect hDisconnect;

        private HandleCloseApp hCloseApp;

        private volatile bool listen;

        private const int udpTimeout = 3000;


        public Connection(IPresenter iPresenter) {

            this.iPresenter = iPresenter;

            hConnect = new HandleConnect(receive);

            hStartSend = new HandleStartSend(send);

            hDisconnect = new HandleDisconnect(disconnect);

            hCloseApp = new HandleCloseApp(close);

            this.iPresenter.OnConnect += hConnect;

            this.iPresenter.OnStartSend += hStartSend;

            this.iPresenter.OnDisconnect += hDisconnect;

            this.iPresenter.OnCloseApp += hCloseApp;

        }


        private void receive() {

            listen = true;

            Thread receiveThread = new Thread(receiveData);

            receiveThread.IsBackground = true;

            receiveThread.Start();

        }


        private void send(string msg) {

            Thread sendThread = new Thread(() => sendData(msg));

            sendThread.IsBackground = true;

            sendThread.Start();

        }


        private void disconnect() {

            listen = false;

        }


        private void close() {

            iPresenter.OnConnect -= hConnect;

            iPresenter.OnStartSend -= hStartSend;

            iPresenter.OnDisconnect -= hDisconnect;

            iPresenter.OnCloseApp -= hCloseApp;

        }


        private void receiveData() {

            UdpClient? udpClient = null;

            try {

                udpClient = new UdpClient(iPresenter.model.receivePort);

                udpClient.Client.ReceiveTimeout = udpTimeout;

                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(iPresenter.model.ipAddress), iPresenter.model.receivePort);

                while (listen) {

                    try {

                        byte[] data = udpClient.Receive(ref ipEndPoint);

                        Application.Current.Dispatcher.Invoke(
                            new Action(() => { iPresenter.model.onDataReceived(Encoding.Default.GetString(data)); })
                        );

                    }

                    catch (SocketException e) { if (e.SocketErrorCode != SocketError.TimedOut) throw; }

                }

                Application.Current.Dispatcher.Invoke(new Action(() => { iPresenter.model.onDisconnected(); }));

            }

            catch (Exception e) when (
                e is SocketException ||
                e is ObjectDisposedException ||
                e is ThreadInterruptedException
            ) {

                listen = false;

                Application.Current.Dispatcher.Invoke(new Action(() => {
                    iPresenter.model.onReceiveError(e.GetType().ToString());
                }));

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

                Application.Current.Dispatcher.Invoke(new Action(() => { iPresenter.model.onDataSent(msg); }));

            }

            catch (Exception e) when (e is SocketException || e is ThreadInterruptedException) {

                Application.Current.Dispatcher.Invoke(new Action(() => {
                    iPresenter.model.onSendError(e.GetType().ToString());
                }));
            
            }

            if (socket != null) socket.Close();

        }


    }


}

