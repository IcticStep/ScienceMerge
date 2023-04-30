using System;
using UnityEngine;

namespace Configurations
{
    [Serializable]
    public class CardSettings
    {
        public CardSettings() { }
        public CardSettings(int id) => _id = id;
        
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private int _mergeSeconds;
        [SerializeField] private int _price;
        [SerializeField] private Sprite _sprite;

        public int Id => _id;
        public string Name => _name;
        public int MergeSeconds => _mergeSeconds;
        public int Price => _price;
        public Sprite Sprite => _sprite;
    }
}