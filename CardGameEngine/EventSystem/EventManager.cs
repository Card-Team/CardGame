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
        public IEventHandler<T> SubscribeToEvent<T>(OnEvent<T> deleg, bool evenIfCancelled = false, bool postEvent = false)
            where T : Event
        {
            throw new NotImplementedException();
        }
        //todo subscribe lua
        //todo retourner interface sans methodes pour éviter l'envoi manuel

        /// <summary>
        /// Désabonne le délégué fourni de l'évènement T 
        /// </summary>
        /// <param name="deleg">Le délégué à désinscrire</param>
        /// <typeparam name="T">Le type d'évènement à retirer</typeparam>
        /// <seealso cref="Event"/>
        public void UnsubscribeFromEvent<T>(IEventHandler<T> deleg) where T : Event
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Déclenche l'évènement donné
        /// </summary>
        /// <param name="evt">L'évènement à déclencher</param>
        /// <typeparam name="T">Le type d'évènement</typeparam>
        /// <returns></returns>
        /// <seealso cref="Event"/>
        public IPostEventSender SendEvent<T>(T evt) where T : Event
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Déclenche l'évènement donné en mode POST
        /// </summary>
        /// <param name="evt">L'évènement à déclencher</param>
        /// <typeparam name="T">Le type d'évènement</typeparam>
        /// <returns></returns>
        /// <seealso cref="Event"/>
        private void SendEventPost<T>(T evt) where T: Event
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Interface qui permet d'empaqueter les délégués d'évenements avec comme parametre générique <see cref="Event"/>.<br/>
        /// <see cref="T"/> est contravariant et il est donc
        /// possible de faire :
        /// <code>
        /// IEventHandler&lt;Event&gt; handler = new EventHandlerImpl&lt;CardNameChangeEvent&gt;();
        /// </code>
        /// </summary>
        /// <typeparam name="T">Le sous type de <see cref="Event"/> que le délégué demande</typeparam>
        public interface IEventHandler<out T> where T : Event
        {
            /// <value>
            /// <see cref="EventManager.SubscribeToEvent{T}"/>
            /// </value>
            public bool EvenIfCancelled { get; }
            /// <value>
            /// <see cref="EventManager.SubscribeToEvent{T}"/>
            /// </value>
            public bool PostEvent { get; }
            /// <summary>
            /// Envoi l'évent <paramref name="evt"/> au délégué
            /// </summary>
            /// <param name="evt">L'event a envoyer au délégué</param>
            public void HandleEvent(Event evt);
        }

        /// <inheritdoc cref="IEventHandler{T}"/>
        private class EventHandlerImpl<T> : IEventHandler<T> where T : Event
        {
            private OnEvent<T> _evt;

            public EventHandlerImpl(OnEvent<T> evt)
            {
                _evt = evt;
            }
            
            public bool EvenIfCancelled { get; }
            public bool PostEvent { get; }

            public void HandleEvent(Event evt)
            {
                _evt.Invoke((T) evt);
            }
        }


        /// <summary>
        /// Classe qui permet d'envoyer un event en version "post" plus simplement
        /// </summary>
        public interface IPostEventSender : IDisposable
        {
            /// <value>L'event a renvoyer</value>
            public Event Event { get; }
        }

        /// <inheritdoc cref="IPostEventSender"/>
        private class PostEventSenderImpl : IPostEventSender
        {
            public Event Event { get; }

            private EventManager _eventManager;

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