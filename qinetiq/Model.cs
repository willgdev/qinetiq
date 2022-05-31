using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace qinetiq {


    public interface IConnection {

        void onConnect();

        void onDataReceived(string msg);

        void onReceiveError(string error);

        void onStartSend();

        void onDataSent(string msg);

        void onSendError(string msg);

        void onStartDisconnect();

        void onDisconnected();

    }


    public class Model : IDataErrorInfo, IConnection, INotifyPropertyChanged {

        public string message { get; set; } = string.Empty;

        public List<string> messages { get; set; } = new List<string>();

        public string ipAddress { get; set; } = "192.168.1.255";

        public int receivePort { get; set; } = 11000;

        public int destPort { get; set; } = 11001;

        public string Error { get { return validConn.Values.ToList().Where(x => x != null).DefaultIfEmpty(null).First(); } }

        public bool allowConnect { get { return Error == null && isNotConnected; } }

        public bool allowDisconnect {
            
            get { return _allowDisconnect; }
            
            set {

                _allowDisconnect = value;

                OnPropertyChanged("allowDisconnect");
                    
            }
        
        }

        public bool allowSend { get { return !isNotConnected && !sendingInProgress; } }

        public bool isNotConnected { get; set; } = true;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _allowDisconnect = false;

        private bool sendingInProgress = false;

        private Dictionary<string, Func<string?>> valids;

        private Dictionary<string, string?> validConn;

        private Regex ipRegex = new Regex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");

        private const int udpMax = 65535;


        public Model () {

            valids = new Dictionary<string, Func<string?>> {
                {"message", () => message.Trim().Length > 0 ? null : "Enter text."},
                {"messages", () => null},
                {"ipAddress", () => ipRegex.IsMatch(ipAddress) ? null : "Invalid IP Address"},
                {"receivePort", () => receivePort > 0 && receivePort <= udpMax && receivePort != destPort 
                    ? null
                    : String.Format("Port must be < %d and cannot be the same as the destination port.", udpMax)
                },
                {"destPort", () => destPort > 0 && destPort <= udpMax && receivePort != destPort
                    ? null
                    : String.Format("Port must be < %d and cannot be the same as the receive port.", udpMax)
                }
            };

            validConn = new Dictionary<string, string?> { { "ipAddress", null }, { "receivePort", null }, { "destPort", null } };

        }


        public string this[string id] {
            
            get {

                string? res = valids[id]();

                if (validConn.ContainsKey(id)) validConn[id] = res;

                OnPropertyChanged("allowConnect");

                return res;
            
            }
        
        }


        public void onConnect() {

            isNotConnected = false;

            allowDisconnect = true;

            OnPropertyChanged("allowConnect");

            messages.Add(String.Format("Connected: %s:%s", ipAddress, receivePort));

        }


        public void onDataReceived(string msg) { messages.Add(String.Format("Received: %s", msg)); }


        public void onReceiveError(string msg) {

            isNotConnected = true;

            allowDisconnect = false;

            OnPropertyChanged("allowConnect");

            messages.Add(String.Format("Disconnected [Receive Error: %s]", msg));

        }


        public void onStartSend() { sendingInProgress = true; }


        public void onDataSent(string msg) {

            sendingInProgress = false;

            messages.Add(String.Format("Sent: %s", msg));

        }


        public void onStartDisconnect() { allowDisconnect = false; }


        public void onDisconnected() {

            isNotConnected = true;

            OnPropertyChanged("allowConnect");

            messages.Add("Disconnected");

        }


        public void onSendError(string msg) {

            sendingInProgress = false;

            messages.Add(String.Format("Sending Error: %s", msg));

        }


        public void OnPropertyChanged([CallerMemberName] string name=null) {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }


    }


}

