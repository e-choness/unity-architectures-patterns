using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

namespace FluentBuilder.Scripts
{
    public enum Type
    {
        Sword,
        Dagger,
        Bow
    }
    public class Weapon : MonoBehaviour
    {
        public string Name { get; set; } = "None";

        private Dictionary<string, Data> WeaponDataSet = new WeaponDataBuilder()
            .AddSword("sword", 11, 200.0f)
            .AddDagger("dagger", 13, 400.0f)
            .AddBow("bow", 10, 100.0f)
            .Build();


        public class Builder
        {
            private string _name;

            public Builder WithName(string name)
            {
                _name = name;
                return this;
            }

            public Weapon Build()
            {
                var weapon = new GameObject($"Weapon{_name}").AddComponent<Weapon>();
                return weapon;
            }
        }
        
        
    }
    public class Data
    {
        private int _damage;
        private float _price;
        private Type _type;

        public Data(int damage, float price, Type type)
        {
            _damage = damage;
            _price = price;
            _type = type;
        }
    }
        
    public class WeaponDataBuilder
    {
        private readonly Dictionary<string, Data> _dataSet = new();

        public WeaponDataBuilder AddSword(string key, int damage, float price)
        {
            _dataSet.Add(key, new Data(damage, price,Type.Sword));
            return this;
        }

        public WeaponDataBuilder AddDagger(string key, int damage, float price)
        {
            _dataSet.Add(key, new Data(damage, price, Type.Dagger));
            return this;
        }

        public WeaponDataBuilder AddBow(string key, int damage, float price)
        {
            _dataSet.Add(key, new Data(damage, price, Type.Bow));
            return this;
        }

        public Dictionary<string, Data> Build()
        {
            return _dataSet;
        }
    }
}