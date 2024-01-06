using ServerLocator.Scripts.Components;
using ServerLocator.Scripts.Interfaces;
using ServerLocator.Scripts.Services;
using UnityEngine;

namespace ServerLocator.Scripts.Tests
{
    public class MiniMap : MonoBehaviour
    {
        private ILocalization _localization;
        private IGameService _gameService;

        private void Awake()
        {
            // ServiceLocator.Global.Register<ILocalization>(_localization = new MockLocalization());
            ServiceLocator.ForSceneOf(this).Register(_gameService = new MockGameService());
        }

        private void Start()
        {
            Debug.Log("=== Mini Map Start ===");
            ServiceLocator.For(this)
                .Get(out _localization)
                .Get(out _gameService);
        
            _localization.GetLocalizedWord(gameObject.name);
            _gameService.StartGame();
        }
    }
}
