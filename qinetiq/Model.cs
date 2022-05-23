using System.Collections.Generic;


namespace qinetiq {


    public class Model {

        public string message { get; set; }

        public List<string> messages { get; set; }

        public string ipAddress { get; set; }

        public int receivePort { get; set; }

        public int destPort { get; set; }

    }


}

