using System;
using Configurations;
using Zenject;

namespace Model
{
    public class CardCreator
    {
        [Inject]
        public CardCreator(CardsConfiguration cardsConfiguration) => 
            _configuration = cardsConfiguration;

        public Card InstantiateCard(int id)
        {
            var info = _configuration[id];
            return new Card(info.Id, info.Name, new TimeSpan(0, 0, info.MergeSeconds), info.Price);
        } 
        
        private readonly CardsConfiguration _configuration;
    }
}