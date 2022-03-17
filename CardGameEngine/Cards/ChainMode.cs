namespace CardGameEngine.Cards
{
    /// <summary>
    ///     Le mode de chainage de la carte
    /// </summary>
    public enum ChainMode
    {
        /// <summary>
        ///     Non chainable, Déclenche une chaine
        /// </summary>
        StartChain,

        /// <summary>
        ///     Chainable (continue la chaine), ne Déclenche pas de chaine
        /// </summary>
        MiddleChain,

        /// <summary>
        ///     Chainable (continue la chaine), Déclenche une chaine
        /// </summary>
        StartOrMiddleChain,

        /// <summary>
        ///     Chainable (termine la chaine), ne déclenche pas de chaine
        /// </summary>
        EndChain,

        /// <summary>
        ///     Non chainable, ne déclenche pas de chaine
        /// </summary>
        NoChain
    }
}