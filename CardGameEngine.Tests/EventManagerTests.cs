using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events;
using NUnit.Framework;

namespace CardGameTests
{
    [TestFixture(Author = "Bilel",
        Category = "Events",
        TestOf = typeof(EventManager),
        TestName = "Tests du système d'envoi et de réception des évènements")]
    public class EventManagerTests
    {
        private EventManager _eventManager = null!;

        [SetUp]
        public void SetUp()
        {
            _eventManager = new EventManager();
        }

        [TearDown]
        public void TearDown()
        {
            _eventManager = null!;
        }

        private class TestEventType : Event
        {
        }

        private class OtherTestEventType : Event
        {
        }

        [Test(Description = "Vérification du déclenchement et de la réception d'un évènement simple")]
        public void TestSimpleEventDispatch()
        {
            var called = false;
            EventManager.OnEvent<TestEventType> onTestEvent = evt => called = true;

            _eventManager.SubscribeToEvent(onTestEvent);

            _eventManager.SendEvent(new TestEventType());

            Assert.That(called, Is.True, "L'évènement n'a pas été reçu");
        }

        [Test(Description = "Vérification du désabonnement")]
        public void TestUnsubscription()
        {
            bool called = false;
            EventManager.OnEvent<TestEventType> onTestEvent = evt => called = true;

            var handler = _eventManager.SubscribeToEvent(onTestEvent);

            _eventManager.UnsubscribeFromEvent(handler);

            _eventManager.SendEvent(new TestEventType());

            Assert.That(called, Is.False, "Un évènement a été reçu alors que l'écouteur s'est désabonné");
        }

        [Test(Description = "Vérification des données reçues lors de la réception d'un évènement")]
        public void TestEventData()
        {
            var originalEvent = new TestEventType();
            TestEventType? receivedEvent = null;
            EventManager.OnEvent<TestEventType> onTestEvent = evt => receivedEvent = evt;

            _eventManager.SubscribeToEvent(onTestEvent);

            var postSender = _eventManager.SendEvent(originalEvent);

            Assert.That(receivedEvent, Is.SameAs(originalEvent),
                "L'évènement reçu par l'écouteur n'est pas celui qui a été émis");
            Assert.That(postSender.Event, Is.SameAs(originalEvent),
                "L'évènement renvoyé n'est pas celui qui a été émis");
        }


        [Test(Description = "Vérification de l'envoi des bon évènements lorsque plusieurs écouteurs sont abonnés ")]
        public void TestMultipleEventTypes()
        {
            var originalEvent = new TestEventType();
            var originalOtherEvent = new OtherTestEventType();

            TestEventType? receivedEvent = null;
            OtherTestEventType? receivedOtherEvent = null;

            EventManager.OnEvent<TestEventType> onTestEvent = evt => receivedEvent = evt;
            EventManager.OnEvent<OtherTestEventType> onOtherTestEvent = evt => receivedOtherEvent = evt;

            _eventManager.SubscribeToEvent(onTestEvent);
            _eventManager.SubscribeToEvent(onOtherTestEvent);

            _eventManager.SendEvent(originalEvent);
            _eventManager.SendEvent(originalOtherEvent);


            var message = "L'évènement reçu n'est pas celui envoyé";
            Assert.That(receivedEvent, Is.SameAs(originalEvent), message);
            Assert.That(receivedOtherEvent, Is.SameAs(originalOtherEvent), message);
        }

        private class SuperClassEvent : Event
        {
        }

        private class SubClassEvent : SuperClassEvent
        {
        }

        [Test(Description = "Vérification de l'écoute de super-classe")]
        public void TestSubscribeSuperclass()
        {
            var originalSubClassEvent = new SubClassEvent();
            SuperClassEvent? receivedEvent = null;
            Event? megaReceived = null;
            Event? postReceived = null;

            EventManager.OnEvent<SuperClassEvent> superHandler = evt => receivedEvent = evt;
            EventManager.OnEvent<Event> megaHandler = evt => megaReceived = evt;
            EventManager.OnEvent<Event> postHandler = evt => postReceived = evt;

            _eventManager.SubscribeToEvent(superHandler);
            _eventManager.SubscribeToEvent(megaHandler);
            _eventManager.SubscribeToEvent(postHandler, postEvent: true);

            var postsender = _eventManager.SendEvent(originalSubClassEvent);
            postsender.Dispose();

            Assert.That(receivedEvent, Is.Not.Null.And.SameAs(megaReceived).And.SameAs(originalSubClassEvent),
                "L'évènement de type classe fille n'est pas reçu par un écouteur de la classe parente");

            Assert.That(postReceived, Is.Not.Null.And.SameAs(originalSubClassEvent),
                "L'évenement POST de type classe fille n'est pas recu par un écouteur POST de la classe parente");
        }


        [Test(Description = "Vérification de l'ordre de réception des évènements")]
        public void TestEventReceiverOrder()
        {
            TestEventType? firstEvent = null;
            bool? wasFirstEventNotNull = null;

            EventManager.OnEvent<TestEventType> onEventFirst = evt => firstEvent = evt;
            EventManager.OnEvent<TestEventType> onEventSecond = evt => wasFirstEventNotNull = firstEvent != null;

            _eventManager.SubscribeToEvent(onEventFirst);
            _eventManager.SubscribeToEvent(onEventSecond);

            _eventManager.SendEvent(new TestEventType());

            Assert.That(firstEvent, Is.Not.Null, "L'évènement n'a pas été reçu par le premier écouteur");
            Assert.That(wasFirstEventNotNull, Is.Not.Null, "L'évènement n'a pas été reçu par le deuxième écouteur");

            Assert.That(wasFirstEventNotNull,
                Is.True, "Le premier évènement n'a pas été reçu avant le second évènement");
        }

        [Test(Description = "Vérification qu'un même écouteur reçoit plusieurs évènements jusqu'au désabonnement")]
        public void TestMultipleEvent()
        {
            var eventCount = 0;
            const int expectedEventCount = 50;

            EventManager.OnEvent<TestEventType> onEventFirst = evt => eventCount++;

            var handler = _eventManager.SubscribeToEvent(onEventFirst);

            for (var i = 0; i < expectedEventCount; i++)
            {
                _eventManager.SendEvent(new TestEventType());
            }

            _eventManager.UnsubscribeFromEvent(handler);

            //normalement non reçu
            for (var i = 0; i < expectedEventCount; i++)
            {
                _eventManager.SendEvent(new TestEventType());
            }

            Assert.That(eventCount, Is.EqualTo(expectedEventCount),
                "Le nombre d'évènements reçu ne correspond pas au nombre d'évènements envoyés pendant l'abonnement");
        }

        [Test(Description = "Vérification que le post event soit bien envoyé et reçu")]
        public void TestSimplePostEvent()
        {
            var expectedEvent = new TestEventType();
            TestEventType? receivedEvent = null;
            EventManager.OnEvent<TestEventType> onEventFirstPost = evt => receivedEvent = evt;

            _eventManager.SubscribeToEvent(onEventFirstPost, postEvent: true);

            var postEventSender = _eventManager.SendEvent(expectedEvent);

            Assert.That(receivedEvent, Is.Null,
                "L'évènement pré a été reçu alors que l'écouteur est abonné pour les post");

            postEventSender.Dispose();

            Assert.That(receivedEvent, Is.SameAs(expectedEvent), "L'évènement post n'a pas été reçu");
        }

        [Test(Description = "Vérification que l'évent post n'est pas reçu par les écouteurs pré")]
        public void TestPreDontReceivePost()
        {
            var expectedEvent = new TestEventType();
            TestEventType? receivedEvent = null;
            EventManager.OnEvent<TestEventType> onEventFirstPre = evt => receivedEvent = evt;

            _eventManager.SubscribeToEvent(onEventFirstPre);

            var postEventSender = _eventManager.SendEvent(expectedEvent);

            if (receivedEvent == null)
            {
                Assert.Inconclusive("L'évènement pré n'a pas été reçu, impossible de vérifier l'évènement post");
            }

            receivedEvent = null;

            postEventSender.Dispose();

            Assert.That(receivedEvent, Is.Null, "L'évènement POST a été reçu par un écouteur PRÉ");
        }


        private class TestCancellableEvent : CancellableEvent
        {
        }

        [Test(Description = "Vérification de l'annulation d'un évènement")]
        public void TestCancelEvent()
        {
            var notExpected = new TestCancellableEvent();
            TestCancellableEvent? received = null;

            EventManager.OnEvent<TestCancellableEvent> eventCancellator = evt => evt.Cancelled = true;
            EventManager.OnEvent<TestCancellableEvent> eventConsumer = evt => received = evt;

            _eventManager.SubscribeToEvent(eventCancellator);
            _eventManager.SubscribeToEvent(eventConsumer);

            _eventManager.SendEvent(notExpected);

            Assert.That(received, Is.Null,
                "Un évènement annulé en amont a été reçu par un écouteur qui ne veut pas les évènements annulés");
        }

        [Test(Description = "Vérification de l'écoute des évènements annulés")]
        public void TestReceiveCancelEvent()
        {
            var expected = new TestCancellableEvent();
            TestCancellableEvent? received = null;

            EventManager.OnEvent<TestCancellableEvent> eventCancellator = evt => evt.Cancelled = true;
            EventManager.OnEvent<TestCancellableEvent> eventConsumer = evt => received = evt;

            _eventManager.SubscribeToEvent(eventCancellator);
            _eventManager.SubscribeToEvent(eventConsumer, true);

            _eventManager.SendEvent(expected);

            Assert.That(received, Is.SameAs(expected),
                "Un écouteur d'évènements annulé n'a pas reçu un évènement annulé");
        }


        [Test(Description = "Vérification de la dé-annulation")]
        public void TestUnCancelEvent()
        {
            var expected = new TestCancellableEvent();
            TestCancellableEvent? received = null;

            EventManager.OnEvent<TestCancellableEvent> eventCancellator = evt => evt.Cancelled = true;
            EventManager.OnEvent<TestCancellableEvent> eventMisser = evt => received = evt;
            EventManager.OnEvent<TestCancellableEvent> eventUnCancellator = evt => evt.Cancelled = false;
            EventManager.OnEvent<TestCancellableEvent> eventConsumer = evt => received = evt;

            _eventManager.SubscribeToEvent(eventCancellator);
            _eventManager.SubscribeToEvent(eventMisser);
            _eventManager.SubscribeToEvent(eventUnCancellator, true);
            _eventManager.SubscribeToEvent(eventConsumer);

            _eventManager.SendEvent(expected);

            Assert.That(received, Is.SameAs(expected),
                "Un écouteur d'évènements n'a pas reçu l'évènement après sa dé-annulation");
        }

        [Test(Description = "Vérification qu'un évènement annulé à la fin ne déclenche pas de post")]
        public void TestCancelledDoesntSendPost()
        {
            var notExpected = new TestCancellableEvent();
            TestCancellableEvent? received = null;

            EventManager.OnEvent<TestCancellableEvent> eventCancellator = evt => evt.Cancelled = true;
            EventManager.OnEvent<TestCancellableEvent> postMisser = evt => received = evt;

            _eventManager.SubscribeToEvent(eventCancellator);
            _eventManager.SubscribeToEvent(postMisser, postEvent: true);

            var postSender = _eventManager.SendEvent(notExpected);
            postSender.Dispose();

            Assert.That(received, Is.Null, "Un évent annulé a quand même envoyé un postEvent");
        }

        [Test(Description = "Vérification que deux évènements à la suite reçoivent la même instance")]
        public void TestChainEventsSameInstance()
        {
            var expectedEvent = new TestEventType();

            TestEventType? firstReceived = null;
            TestEventType? secondReceived = null;

            EventManager.OnEvent<TestEventType> firstConsumer = evt => firstReceived = evt;
            EventManager.OnEvent<TestEventType> secondConsumer = evt => secondReceived = evt;

            _eventManager.SubscribeToEvent(firstConsumer);
            _eventManager.SubscribeToEvent(secondConsumer);

            _eventManager.SendEvent(expectedEvent);

            Assert.That(firstReceived, Is.Not.Null.And.SameAs(secondReceived),
                "Les deux écouteurs à la chaine n'ont pas reçu le même object évènement");
        }
    }
}