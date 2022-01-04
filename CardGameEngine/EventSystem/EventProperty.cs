using CardGameEngine.EventSystem.Events;

namespace CardGameEngine.EventSystem
{
    /// <summary>
    /// Classe représentant un évènement
    /// </summary>
    /// <typeparam name="S">Type de l'objet (Artefact/Card/Keyword)</typeparam>
    /// <typeparam name="T">Type de la valeur (int, string, ...)</typeparam>
    /// <typeparam name="ET">IPropertyChangeEvent</typeparam>
    /// <seealso cref="IPropertyChangeEvent{S,T}"/>
    public class EventProperty<S, T, ET> where ET : IPropertyChangeEvent<S, T>
    {
        /// <summary>
        /// Objet lié à l'évènement
        /// </summary>
        private S _sender;

        /// <summary>
        /// Valeur de la propriété
        /// </summary>
        public T Value { get; private set; }


        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="sender">L'objet lié à l'évènement</param>
        public EventProperty(S sender)
        {
            //todo passer eventManager et creer dans les constructeurs du coup
            _sender = sender;
        }

        /// <summary>
        /// Essaye de changer la valeur de la propriété par la nouvelle entrée
        /// </summary>
        /// <param name="newVal">La nouvelle valeur</param>
        public void TryChangeValue(T newVal)
        {
            throw new System.NotImplementedException();
        }
    }
}