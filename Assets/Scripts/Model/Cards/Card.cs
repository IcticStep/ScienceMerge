using System;

namespace Model.Cards
{
    public class Card
    {
        public Card(int id, string name, TimeSpan mergeTime, int price)
        {
            Id = id;
            Name = name;
            MergeTime = mergeTime;
            Price = price;
        }

        public int Id { get; }
        public string Name { get; }
        public TimeSpan MergeTime { get; }
        public int Price { get; }

        public override string ToString() => 
            $"[{Name}] Time: {MergeTime.ToString()} Price: {Price}";
    }
}
