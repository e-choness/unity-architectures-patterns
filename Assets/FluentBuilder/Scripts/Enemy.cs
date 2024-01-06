using FluentBuilder.Scripts.Interfaces;
using Unity.VisualScripting;
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

            public Builder WithDamage(int damage)
            {
                _damage = damage;
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
                Debug.Log($"Enemy build - name: {enemy.Name} health: {enemy.Health} speed: {enemy.Speed} damage: {enemy.Damage} Is a boss: {enemy.IsBoos}");
                return enemy;
            }
        }

        public class ComponentBuilder
        {
            private Enemy _enemy = new GameObject("Enemy").AddComponent<Enemy>();

            public ComponentBuilder AddWeaponComponent()
            {
                _enemy.gameObject.AddComponent<Weapon>();
                return this;
            }

            public ComponentBuilder AddHealthComponent()
            {
                _enemy.gameObject.AddComponent<Health>();
                return this;
            }

            public ComponentBuilder AddArmorComponent()
            {
                _enemy.gameObject.AddComponent<Armor>();
                return this;
            }

            public Enemy Build()
            {
                var builtEnemy = _enemy;
                // reset enemy for the next build process;
                _enemy = new GameObject("Enemy").AddComponent<Enemy>();
                return builtEnemy;
            }
        }

        public class InterfaceBuilder : IEnemyBuilder, IWeaponEnemyBuilder, IHealthEnemyBuilder, IFinalEnemyBuilder
        {
            private Enemy _enemy = new GameObject("Enemy").AddComponent<Enemy>();


            public IWeaponEnemyBuilder AddArmorComponent()
            {
                _enemy.gameObject.AddComponent<Armor>();
                return this;
            }

            public IHealthEnemyBuilder AddWeaponComponent()
            {
                _enemy.gameObject.AddComponent<Weapon>();
                return this;
            }

            public IFinalEnemyBuilder AddHealthComponent()
            {
                _enemy.gameObject.AddComponent<Health>();
                return this;
            }

            public Enemy Build()
            {
                var builtEnemy = _enemy;
                _enemy = new GameObject("Enemy").AddComponent<Enemy>();
                return builtEnemy;
            }
        }
    }
}


