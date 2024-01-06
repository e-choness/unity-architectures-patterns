using UnityEngine;

namespace FluentBuilder.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public float Speed { get; private set; }
        public int Damage { get; private set; }
        public bool IsBoos { get; private set; }
    
        public class Builder
        {
            private string _name = "DefaultName";
            private int _health = 10;
            private float _speed = 5.0f;
            private int _damage = 20;
            private bool _isBoss = false;

            public Builder WithName(string name)
            {
                _name = name;
                return this;
            }

            public Builder WithHealth(int health)
            {
                _health = health;
                return this;
            }

            public Builder WithSpeed(float speed)
            {
                _speed = speed;
                return this;
            }

            public Builder WithDamange(int damange)
            {
                _damage = damange;
                return this;
            }

            public Builder WithIsBoss(bool isBoss)
            {
                _isBoss = isBoss;
                return this;
            }

            public Enemy Build()
            {
                var enemy = new GameObject($"Enemy{(string.IsNullOrEmpty(_name) ? "Nameless" : _name)}")
                    .AddComponent<Enemy>();
                enemy.Name = _name;
                enemy.Health = _health;
                enemy.Speed = _speed;
                enemy.Damage = _damage;
                enemy.IsBoos = _isBoss;
                return enemy;
            }
        }
    }
}


