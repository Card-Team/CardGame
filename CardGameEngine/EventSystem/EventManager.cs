using System;
using System.Collections.Generic;
using System.Linq;
using CardGameEngine.EventSystem.Events;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace CardGameEngine.EventSystem
{
    /// <summary>
    /// Classe représentant un gestionnaire d'évènement
    /// </summary>
    public class EventManager
    {
        /// <summary>
        /// Format d'une fonction d'écouteur
        /// </summary>
        /// <typeparam name="T">Event</typeparam>
        /// <seealso cref="Event"/>
        public delegate void OnEvent<in T>(T evt) where T : Event;

        /// <summary>
        /// Dictionnaire contenant tous les évènements
        /// </summary>
        private Dictionary<Type, List<IEventHandler>> _eventHandlersDict =
            new Dictionary<Type, List<IEventHandler>>();

        internal bool Disabled { get; set; }

        /// <summary>
        /// Abonne le délégué fourni à l'évènement T donné
        /// </summary>
        /// <param name="deleg">Le délégué qui veut écouter</param>
        /// <param name="evenIfCancelled">Écoute même si l'évènement est annulé (défaut = false)</param>
        /// <param name="postEvent">Veut recevoir l'information <i>après</i> l'exécution (défaut = false)</param>
        /// <typeparam name="T">Le type d'évènement à écouter</typeparam>
        /// <seealso cref="Event"/>
        public IEventHandler SubscribeToEvent<T>(OnEvent<T> deleg, bool evenIfCancelled = false,
            bool postEvent = false)
            where T : Event
        {
            return SubscribeToEvent(typeof(T), new EngineEventHandler<T>(deleg, evenIfCancelled, postEvent));
        }

        internal IEventHandler LuaSubscribeToEvent(Type eventType, Closure closure, bool? evenIfCancelled,
            bool? postEvent)
        {
            evenIfCancelled ??= false;
            postEvent ??= false;

            return SubscribeToEvent(eventType,
                new LuaEventHandler(eventType, closure, evenIfCancelled.Value, postEvent.Value));
        }

        private IEventHandler SubscribeToEvent(Type type, IEventHandler handler)
        {
            if (Disabled) return null!;
            if (!_eventHandlersDict.ContainsKey(type))
                _eventHandlersDict.Add(type, new List<IEventHandler>());

            _eventHandlersDict[type].Add(handler);

            return handler;
        }

        /// <summary>
        /// Désabonne le délégué fourni de l'évènement T 
        /// </summary>
        /// <param name="deleg">Le délégué à désinscrire</param>
        /// <typeparam name="T">Le type d'évènement à retirer</typeparam>
        /// <seealso cref="Event"/>
        public void UnsubscribeFromEvent(IEventHandler deleg)
        {
            UnsubscribeFromEvent(deleg.EventType, deleg);
        }

        private void UnsubscribeFromEvent(Type eventType, IEventHandler listener)
        {
            if (_eventHandlersDict.ContainsKey(eventType))
                _eventHandlersDict[eventType].Remove(listener);
        }

        internal void LuaUnsubscribeFromEvent(IEventHandler listener)
        {
            UnsubscribeFromEvent(listener.EventType, listener);
        }


        /// <summary>
        /// Déclenche l'évènement donné
        /// </summary>
        /// <param name="evt">L'évènement à déclencher</param>
        /// <typeparam name="T">Le type d'évènement</typeparam>
        /// <returns></returns>
        /// <seealso cref="Event"/>
        internal IPostEventSender<T> SendEvent<T>(T evt) where T : Event
        {
            foreach (var eventHandler in GetHandlerOfAssignableFrom<T>())
            {
                if (!eventHandler.PostEvent && (evt is CancellableEvent == false ||
                                                (evt is CancellableEvent cancelled && (!cancelled.Cancelled ||
                                                    eventHandler.EvenIfCancelled))))
                {
                    if(!Disabled)eventHandler.HandleEvent(evt);
                }
            }
            return new PostEventSenderImpl<T>(evt, this);
        }

        /// <summary>
        /// Récupère la liste des <see cref="IEventHandler"/> de la classe <see cref="T"/> ainsi que de ses classes parentes
        /// </summary>
        /// <typeparam name="T">Le type demandé</typeparam>
        /// <returns>Un <see cref="IEnumerable{T}"/> contenant les  <see cref="IEventHandler"/> de <see cref="T"/> et de ses classes parentes</returns>
        private IEnumerable<IEventHandler> GetHandlerOfAssignableFrom<T>() where T : Event
        {
            var currentType = typeof(T);

            do
            {
                if (_eventHandlersDict.TryGetValue(currentType, out var handlerList))
                {
                    foreach (var eventHandler in handlerList.ToList())
                    {
                        yield return eventHandler;
                    }
                }

                currentType = currentType.BaseType;
            } while (currentType != null &&
                     currentType != typeof(object));
        }

        /// <summary>
        /// Déclenche l'évènement donné en mode POST
        /// </summary>
        /// <param name="evt">L'évènement à déclencher</param>
        /// <typeparam name="T">Le type d'évènement</typeparam>
        /// <returns></returns>
        /// <seealso cref="Event"/>
        private void SendEventPost<T>(T evt) where T : Event
        {
            foreach (var eventHandler in GetHandlerOfAssignableFrom<T>().Where(eventHandler => eventHandler.PostEvent))
            {
                if (evt is CancellableEvent == false || (evt is CancellableEvent cancelled &&
                                                         (!cancelled.Cancelled || eventHandler.EvenIfCancelled)))
                {
                    eventHandler.HandleEvent(evt);
                    
                }
            }
        }

        /// <summary>
        /// Interface qui permet d'empaqueter les délégués d'évènements avec comme paramètre générique <see cref="Event"/>.<br/>
        /// <see cref="T"/> est contravariant et il est donc
        /// possible de faire :
        /// <code>
        /// IEventHandler&lt;Event&gt; handler = new EventHandlerBase&lt;CardNameChangeEvent&gt;();
        /// </code>
        /// </summary>
        /// <typeparam name="T">Le sous-type de <see cref="Event"/> que le délégué demande</typeparam>
        // ReSharper disable once UnusedTypeParameter
        public interface IEventHandler
        {
            /// <value>
            /// <see cref="EventManager.SubscribeToEvent{T}"/>
            /// </value>
            public bool EvenIfCancelled { get; }

            /// <value>
            /// <see cref="EventManager.SubscribeToEvent{T}"/>
            /// </value>
            public bool PostEvent { get; }

            Type EventType { get; }

            /// <summary>
            /// Envoi l'évent <paramref name="evt"/> au délégué
            /// </summary>
            /// <param name="evt">L'event à envoyer au délégué</param>
            public void HandleEvent(Event evt);
        }

        /// <inheritdoc cref="IEventHandler"/>
        private abstract class EventHandlerBase : IEventHandler
        {
            protected EventHandlerBase(bool evenIfCancelled, bool postEvent)
            {
                EvenIfCancelled = evenIfCancelled;
                PostEvent = postEvent;
            }

            public bool EvenIfCancelled { get; }
            public bool PostEvent { get; }
            public abstract Type EventType { get; }
            public abstract void HandleEvent(Event evt);
        }

        private class EngineEventHandler<T> : EventHandlerBase where T : Event
        {
            private readonly OnEvent<T> _evt;

            public EngineEventHandler(OnEvent<T> evt, bool evenIfCancelled, bool postEvent) : base(evenIfCancelled,
                postEvent)
            {
                _evt = evt;
            }

            public override Type EventType => typeof(T);

            public override void HandleEvent(Event evt)
            {
                _evt.Invoke((T)evt);
            }
        }

        private class LuaEventHandler : EventHandlerBase
        {
            public override Type EventType { get; }
            
            private readonly Closure _evt;

            public LuaEventHandler(Type eventType, Closure closure, bool evenIfCancelled, bool postEvent) : base(
                evenIfCancelled,
                postEvent)
            {
                EventType = eventType;
                _evt = closure;
            }
            
            public override void HandleEvent(Event evt)
            {
                _evt.Call(Convert.ChangeType(evt, EventType),this);
            }
        }


        /// <summary>
        /// Classe qui permet d'envoyer un event en version "post" plus simplement
        /// </summary>
        internal interface IPostEventSender<out T> : IDisposable where T : Event
        {
            /// <value>L'event a renvoyer</value>
            public T Event { get; }
        }

        /// <inheritdoc cref="IPostEventSender{T}"/>
        private class PostEventSenderImpl<T> : IPostEventSender<T> where T : Event
        {
            public T Event { get; }

            private EventManager _eventManager;

            public PostEventSenderImpl(T evt, EventManager eventManager)
            {
                Event = evt;
                _eventManager = eventManager;
            }

            /// <summary>
            /// Envoi l'event post
            /// </summary>
            public void Dispose()
            {
                _eventManager.SendEventPost(Event);
            }
        }
    }
}