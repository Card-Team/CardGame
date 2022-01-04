using System;
using System.Diagnostics;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace CardGameTests
{
    [TestFixture(Author = "Bilel",
        Category = "Events",
        TestOf = typeof(EventManager),
        TestName = "Tests du systeme d'envoi et de récéption des évenements")]
    public class EventmanagerTests
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

        [Test(Description = "Verification du déclencheent et de la récéption d'un évenement simple")]
        public void TestSimpleEventDispatch()
        {
            var called = false;
            EventManager.OnEvent<TestEventType> onTestEvent = evt => called = true;

            _eventManager.SubscribeToEvent(onTestEvent);

            _eventManager.SendEvent(new TestEventType());

            Assert.That(called, Is.True, "L'évenement n'a pas été recu");
        }

        [Test(Description = "Vérification du désabonnement")]
        public void TestUnsubscription()
        {
            bool called = false;
            EventManager.OnEvent<TestEventType> onTestEvent = evt => called = true;

            var handler = _eventManager.SubscribeToEvent(onTestEvent);

            _eventManager.UnsubscribeFromEvent(handler);

            _eventManager.SendEvent(new TestEventType());

            Assert.That(called, Is.False, "Un évenement a été recu alors que l'écouteur s'est désabonné");
        }

        [Test(Description = "Verification des données recues lors de la récéption d'un évenement")]
        public void TestEventData()
        {
            var originalEvent = new TestEventType();
            TestEventType? receivedEvent = null;
            EventManager.OnEvent<TestEventType> onTestEvent = evt => receivedEvent = evt;

            _eventManager.SubscribeToEvent(onTestEvent);

            var postSender = _eventManager.SendEvent(originalEvent);

            Assert.That(receivedEvent, Is.SameAs(originalEvent),
                "L'évenement recu par l'écouteur n'est pas celui qui a été émis");
            Assert.That(postSender.Event, Is.SameAs(originalEvent),
                "L'évenement renvoyé n'est pas celui qui a été émis");
        }


        [Test(Description = "Vérification de l'envoi des bon evenements lorsque plusieurs écouteurs sont abbonés ")]
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


            var message = "L'evenement recu n'est pas celui envoyé";
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

            EventManager.OnEvent<SuperClassEvent> superHandler = evt => receivedEvent = evt;

            _eventManager.SubscribeToEvent(superHandler);

            _eventManager.SendEvent(originalSubClassEvent);

            Assert.That(receivedEvent, Is.Not.Null.And.SameAs(originalSubClassEvent),
                "L'évenement de type classe fille n'est pas recu par un écouteur de la classe Parente");
        }


        [Test(Description = "Verification de l'ordre de récéption des évenements")]
        public void TestEventReceiverOrder()
        {
            DateTime? firstEventTime = null;
            DateTime? secondEventTime = null;

            EventManager.OnEvent<TestEventType> onEventFirst = evt => firstEventTime = DateTime.Now;
            EventManager.OnEvent<TestEventType> onEventSecond = evt => secondEventTime = DateTime.Now;

            _eventManager.SubscribeToEvent(onEventFirst);
            _eventManager.SubscribeToEvent(onEventSecond);

            _eventManager.SendEvent(new TestEventType());

            Assert.That(firstEventTime, Is.Not.Null, "L'évenement n'a pas été recu par le premier écouteur");
            Assert.That(secondEventTime, Is.Not.Null, "L'évenement n'a pas été recu par le deuxieme écouteur");


            //< 0 singifie que firstEvent est avant secondEvent
            Assert.That(DateTime.Compare(firstEventTime!.Value, secondEventTime!.Value),
                Is.Negative, "Le premier évenement n'a pas été recu avant le second évenement");
        }

        [Test(Description = "Vérification qu'un meme écouteur recoit plusieurs évenements jusqu'au désabonnement")]
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

            //normalement non recu
            for (var i = 0; i < expectedEventCount; i++)
            {
                _eventManager.SendEvent(new TestEventType());
            }

            Assert.That(eventCount, Is.EqualTo(expectedEventCount),
                "Le nombre d'évenements recu ne correspond pas au nombre d'évenements envoyés pendant l'abonnement");
        }

        [Test(Description = "Verification que le post event soit bien envoyé et recu")]
        public void TestSimplePostEvent()
        {
            var expectedEvent = new TestEventType();
            TestEventType? receivedEvent = null;
            EventManager.OnEvent<TestEventType> onEventFirstPost = evt => receivedEvent = evt;

            _eventManager.SubscribeToEvent(onEventFirstPost, postEvent: true);

            var postEventSender = _eventManager.SendEvent(expectedEvent);

            Assert.That(receivedEvent, Is.Null,
                "L'évenement pré a été recu alors que l'écouteur est abonné pour les post");

            postEventSender.Dispose();

            Assert.That(receivedEvent, Is.SameAs(expectedEvent), "L'évenement post n'a pas été recu");
        }

        [Test(Description = "Verification que l'évent post n'est pas recu par les écouteurs pré")]
        public void TestPreDontReceivePost()
        {
            var expectedEvent = new TestEventType();
            TestEventType? receivedEvent = null;
            EventManager.OnEvent<TestEventType> onEventFirstPre = evt => receivedEvent = evt;

            _eventManager.SubscribeToEvent(onEventFirstPre);

            var postEventSender = _eventManager.SendEvent(expectedEvent);

            if (receivedEvent == null)
            {
                Assert.Inconclusive("L'évenement pré n'a pas été recu, impossible de verifier l'évenement post");
            }

            receivedEvent = null;

            postEventSender.Dispose();

            Assert.That(receivedEvent, Is.Null, "L'évenement POST a été recu par un écouteur PRE");
        }


        private class TestCancellableEvent : CancellableEvent
        {
        }

        [Test(Description = "Verification de l'annulation d'un évenement")]
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
                "Un évenement annulé en amont a été recu par un écouteur qui ne veut pas les évenements annulés");
        }

        [Test(Description = "Verification de l'écoute des évenements annulés")]
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
                "Un écouteur d'évenements annulé n'a pas recu un évenement annulé");
        }


        [Test(Description = "Verification de la dé-annulation")]
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
            _eventManager.SubscribeToEvent(eventUnCancellator);
            _eventManager.SubscribeToEvent(eventConsumer);

            _eventManager.SendEvent(expected);

            Assert.That(received, Is.SameAs(expected),
                "Un écouteur d'évenements n'a pas recu l'évenement apres ca dé-annulation");
        }

        [Test(Description = "Verification qu'un évenement annulé a la fin ne déclenche pas de post")]
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

            Assert.That(received, Is.Null, "Un évent annulé a quand meme envoyé un postEvent");
        }

        [Test(Description = "Vérification que deux évenements a la suite recoivent la meme instance")]
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
                "Les deux écouteurs a la chaine n'ont pas recu le meme object Evenement");
        }
    }
}