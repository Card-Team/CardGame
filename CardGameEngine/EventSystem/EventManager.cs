using System;
using System.Collections.Generic;
using CardGameEngine.EventSystem.Events;

namespace CardGameEngine.EventSystem
{
    public class EventManager
    {
        public delegate void OnEvent<T>(T evt) where T : Event;

        private Dictionary<Type, List<OnEvent<Event>>> _eventHandlersDict;


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