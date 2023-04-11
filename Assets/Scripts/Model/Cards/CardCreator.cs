using System;
using System.Collections.Generic;
using Configurations;
using Zenject;

namespace Model.Cards
{
    public class CardCreator
    {
        [Inject]
        public CardCreator(CardsConfiguration cardsConfiguration, MergeConfiguration mergeConfiguration)
        {
            _configuration = cardsConfiguration;
            _mergeConfiguration = mergeConfiguration;
        }

        private readonly CardsConfiguration _configuration;
        private readonly MergeConfiguration _mergeConfiguration;

        public Card InstantiateCard(int id)
        {
            var info = _configuration[id];
            return new Card(info.Id, info.Name, TimeSpan.FromSeconds(info.MergeSeconds), info.Price);
        }

        public Card InstantiateCardByMerge(List<Card> cards)
        {
            var resultID = _mergeConfiguration.GetResultCardID(cards);

            if (resultID == -1)
                return null;

            return InstantiateCard(resultID);
        }
    }
}