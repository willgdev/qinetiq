using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;


namespace qinetiq {


    public class Model : IDataErrorInfo, INotifyPropertyChanged {

        public string message {

            get { return _message; }

            set {

                _message = value;

                OnPropertyChanged();

            }

        }

        public ObservableCollection<string> messages { get; set; } = new ObservableCollection<string>();

        public string ipAddress { get; set; } = "192.168.1.255";

        public int receivePort { get; set; } = 11000;

        public int destPort { get; set; } = 11001;

        public string Error { get { return validConn.Values.ToList().Where(x => x != null).DefaultIfEmpty(null).First(); } }

        public bool allowConnect { get { return Error == null && isNotConnected; } }

        public bool allowDisconnect {

            get { return _allowDisconnect; }

            private set {

                _allowDisconnect = value;

                OnPropertyChanged();

            }

        }

        public bool allowSend { get { return !isNotConnected && !sendingInProgress && valids["message"]() == null; } }

        public bool isNotConnected {

            get { return _isNotConnected; }

            private set {

                _isNotConnected = value;

                OnPropertyChanged();

                OnPropertyChanged("allowSend");

            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int maxMsgLength { get; } = 100;

        private string _message = string.Empty;

        private bool _allowDisconnect = false;

        private bool _isNotConnected = true;

        private bool sendingInProgress = false;

        private Dictionary<string, Func<string?>> valids;

        private Dictionary<string, string?> validConn;

        private Regex ipRegex = new Regex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");

        private const int udpMax = 65535;


        public Model () {

            valids = new Dictionary<string, Func<string?>> {
                {"message", () => message.Trim().Length > 0 && message.Length < maxMsgLength ? null : "Enter text."},
                {"messages", () => null},
                {"ipAddress", () => ipRegex.IsMatch(ipAddress) ? null : "Invalid IP Address"},
                {"receivePort", () => receivePort > 0 && receivePort <= udpMax && receivePort != destPort 
                    ? null
                    : string.Format("Port must be < {0} and cannot be the same as the destination port.", udpMax)
                },
                {"destPort", () => destPort > 0 && destPort <= udpMax && receivePort != destPort
                    ? null
                    : string.Format("Port must be < {0} and cannot be the same as the receive port.", udpMax)
                }
            };

            validConn = new Dictionary<string, string?> { { "ipAddress", null }, { "receivePort", null }, { "destPort", null } };

        }


        public string this[string id] {
            
            get {

                string? res = valids[id]();

                if (validConn.ContainsKey(id)) validConn[id] = res;

                OnPropertyChanged("allowConnect");

                OnPropertyChanged("allowSend");

                return res;
            
            }
        
        }


        public void onConnect() {

            isNotConnected = false;

            allowDisconnect = true;

            OnPropertyChanged("allowConnect");

            messages.Add(string.Format("Connected: {0}:{1}", ipAddress, receivePort));

        }


        public void onDataReceived(string msg) { messages.Add(string.Format("Received: {0}", msg)); }


        public void onAfterReceiveError(string msg) {

            isNotConnected = true;

            allowDisconnect = false;

            OnPropertyChanged("allowConnect");

            messages.Add(string.Format("Disconnected [Receive Error: {0}]", msg));

        }


        public void onStartSend() {
            
            sendingInProgress = true;

            OnPropertyChanged("allowSend");

        }


        public void onDataSent(string msg) {

            sendingInProgress = false;

            OnPropertyChanged("allowSend");

            messages.Add(string.Format("Sent: {0}", msg));

            message = string.Empty;

        }


        public void onStartDisconnect() {
            
            allowDisconnect = false;

            messages.Add("Disconnecting...");
        
        }


        public void onAfterDisconnected() {

            isNotConnected = true;

            OnPropertyChanged("allowConnect");

            messages.Add("Disconnected");

        }


        public void onSendError(string msg) {

            sendingInProgress = false;

            OnPropertyChanged("allowSend");

            messages.Add(string.Format("Sending Error: {0}", msg));

        }


        public void OnPropertyChanged([CallerMemberName] string name=null) {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }


    }


}

