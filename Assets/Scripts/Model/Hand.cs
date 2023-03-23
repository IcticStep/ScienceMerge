namespace Model
{
    public class Hand
    {
        private Card _card;

        public Card Card
        {
            get => _card;
            set => _card = value;
        }

        public bool HasCard => Card is not null;
    }
}