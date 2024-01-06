using UnityEngine;

namespace FluentBuilder.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            var enemy = new Enemy.Builder()
                .WithName("Kobolt")
                .WithDamage(10)
                .WithSpeed(3f)
                .WithHealth(20)
                .WithIsBoss(false)
                .Build();
            
            var weapon = new Weapon.Builder()
                .WithName("Arc")
                .Build();
            // Instantiate creates a clone
            // Instantiate(enemy,Vector3.zero,Quaternion.identity);

            var enemyWithComponents = new Enemy.ComponentBuilder()
                .AddArmorComponent()
                .AddWeaponComponent()
                .AddHealthComponent()
                .Build();
        }
    }
}
