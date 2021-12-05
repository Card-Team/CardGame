using System;
using System.Collections.Generic;
using CardGameEngine.EventSystem.Events;
using CardGameEngine.EventSystem.Events.CardEvents;

namespace CardGameEngine.EventSystem
{
    public class EventManager
    {
        public delegate void OnEvent<in T>(T evt) where T : Event;

        private Dictionary<Type, List<IEventHandler<Event>>> _eventHandlersDict;


        public void SubscribeToEvent<T>(OnEvent<T> deleg, bool eventCancelled = false, bool postEvent = false)
            where T : Event
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeFromEvent<T>(OnEvent<T> deleg, bool postEvent = false) where T : Event
        {
            throw new NotImplementedException();
        }

        public IPostEventSender SendEvent<T>(T evt, bool postEvent = false) where T : Event
        {
            throw new NotImplementedException();
        }

        private interface IEventHandler<out T> where T : Event
        {
            public bool ReceiveCancelled { get; }
            public bool IsPost { get; }
            public void HandleEvent(Event evt);
        }

        private class EventHandlerImpl<T> : IEventHandler<T> where T : Event
        {
            private OnEvent<T> _evt;

            public EventHandlerImpl(OnEvent<T> evt)
            {
                _evt = evt;
            }

            public bool ReceiveCancelled { get; }
            public bool IsPost { get; }

            public void HandleEvent(Event evt)
            {
                _evt.Invoke((T) evt);
            }
        }


        public interface IPostEventSender : IDisposable
        {
            public Event Event { get; }
        }

        private class PostEventSenderImpl : IPostEventSender
        {
            public Event Event { get; }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}