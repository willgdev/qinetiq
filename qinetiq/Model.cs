using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;


namespace qinetiq {


    public class Model : IDataErrorInfo {

        public string message { get; set; } = string.Empty;

        public List<string> messages { get; set; }

        public string ipAddress { get; set; } = "0.0.0.0";

        public int receivePort { get; set; } = 11000;

        public int destPort { get; set; } = 11001;

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

            validConn =
                new Dictionary<string, string?> { { "ipAddress", null }, { "receivePort", null }, { "destPort", null } };

        }


        public string this[string id] {
            
            get {

                string? res = valids[id]();

                if (validConn.ContainsKey(id)) validConn[id] = res;
                
                return res;
            
            }
        
        }


        public string Error {
            
            get { return validConn.Values.ToList().Where(x => x != null).DefaultIfEmpty(null).First(); }
        
        }


    }


}

