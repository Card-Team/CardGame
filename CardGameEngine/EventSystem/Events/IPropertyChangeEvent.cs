namespace CardGameEngine.EventSystem.Events
{
    /// <summary>
    /// Interface représentant le changement de valeur d'une propriété
    /// </summary>
    /// <typeparam name="S">Type de l'objet qui subit le changement</typeparam>
    /// <typeparam name="T">Le type de la valeur changée</typeparam>
    public interface IPropertyChangeEvent<S, T>
    {
        /// <summary>
        /// L'objet qui subit le changement
        /// </summary>
        internal S Sender { get; set; }

        /// <summary>
        /// Nouvelle valeur de la propriété
        /// </summary>
        internal T NewValue { get; set; }

        /// <summary>
        /// Ancienne valeur de la propriété
        /// </summary>
        internal T OldValue { get; set; }
    }
}