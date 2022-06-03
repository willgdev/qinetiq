using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace qinetiq.Tests {


    class FakeConnOk {


        private IPresenter iPresenter;

        private HandleStartSend hStartSend;

        public bool sent;


        public FakeConnOk(IPresenter iPresenter) {

            this.iPresenter = iPresenter;

            hStartSend = new HandleStartSend(send);

            this.iPresenter.OnStartSend += hStartSend;

        }


        private void send(string msg) {

            iPresenter.model.onDataSent(msg);

            sent = true;

        }


    }


    class FakeConnFail {


        private IPresenter iPresenter;

        private HandleStartSend hStartSend;


        public FakeConnFail(IPresenter iPresenter) {

            this.iPresenter = iPresenter;

            hStartSend = new HandleStartSend(send);

            this.iPresenter.OnStartSend += hStartSend;

        }


        private void send(string msg) {

            iPresenter.model.onSendError("Test error message.");

        }


    }


    [TestClass()]
    public class Tests {


        [TestMethod()]
        public void testSendDataOk() {

            Model model = new Model() { message="test message" };

            Presenter presenter = new Presenter(model);

            FakeConnOk conn = new FakeConnOk(presenter);

            presenter.sendData();

            Assert.IsTrue(
                conn.sent &&
                model.message == string.Empty &&
                model.messages.Last() == "Sent: test message"
            );

        }


        [TestMethod()]
        public void testSendDataFail() {

            Model model = new Model() { message="test message" };

            Presenter presenter = new Presenter(model);

            FakeConnFail conn = new FakeConnFail(presenter);

            presenter.sendData();

            Assert.IsTrue(
                model.message == "test message" &&
                model.messages.Last() == "Sending Error: Test error message."
            );

        }


    }


}

