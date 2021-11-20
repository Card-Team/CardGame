namespace Dynamic
{
    public class DynamicValue<T>
    {
        public T InitialValue { get; }

        public T CurrentValue;

        public bool HasValueChanged { get; }
        
        //TODO ajouter des méthodes pour réagir à un changement
    }
}