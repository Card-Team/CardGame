using System;
using System.Collections.Generic;
using CardGameEngine.EventSystem.Events;

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
        private Dictionary<Type, List<IEventHandler<Event>>> _eventHandlersDict;


        /// <summary>
        /// Abonne le délégué fourni à l'évènement T donné
        /// </summary>
        /// <param name="deleg">Le délégué qui veut écouter</param>
        /// <param name="evenIfCancelled">Écoute même si l'évènement est annulé (défaut = false)</param>
        /// <param name="postEvent">Veut recevoir l'information <i>après</i> l'exécution (défaut = false)</param>
        /// <typeparam name="T">Le type d'évènement à écouter</typeparam>
        /// <seealso cref="Event"/>
        public void SubscribeToEvent<T>(OnEvent<T> deleg, bool evenIfCancelled = false, bool postEvent = false)
            where T : Event
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Désabonne le délégué fourni de l'évènement T 
        /// </summary>
        /// <param name="deleg">Le délégué à désinscrire</param>
        /// <param name="postEvent">Se désabonner de l'évènement post (défaut = false)</param>
        /// <typeparam name="T">Le type d'évènement à retirer</typeparam>
        /// <seealso cref="Event"/>
        public void UnsubscribeFromEvent<T>(OnEvent<T> deleg, bool postEvent = false) where T : Event
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Déclenche l'évènement donné
        /// </summary>
        /// <param name="evt">L'évènement à déclencher</param>
        /// <param name="postEvent">Indique si vient de se finir (défaut = false)</param>
        /// <typeparam name="T">Le type d'évènement</typeparam>
        /// <returns></returns>
        /// <seealso cref="Event"/>
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