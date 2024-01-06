using ClassExtensions.Scripts.Utilities;
using ServerLocator.Scripts.Components;
using ServerLocator.Scripts.Interfaces;
using ServerLocator.Scripts.Services;
using UnityEngine;

namespace ServerLocator.Scripts.Tests
{
    public class Player : MonoBehaviour
    {
        [SerializeField] string playerName = "The Nameless One";

        private ILocalization _localization;
        private ISerializer _serializer;
        private IAuthentication _authentication;
        private IGameService _gameService;

        private void Awake()
        {
            ServiceLocator.Global.Register<IGameService>(_gameService = new MockGameService());
            ServiceLocator.ForSceneOf(this).Register<ILocalization>(_localization = new MockLocalization());
            ServiceLocator.For(this).Register<ISerializer>(_serializer = new MockSerializer());
            ServiceLocator.ForSceneOf(this)
                .Register(_authentication = gameObject.GetOrAdd<MockAuthentication>());
        }

        private void Start()
        {
            Debug.Log("=== Player Start ===");
            ServiceLocator.For(this)
                .Get(out _serializer)
                .Get(out _localization)
                .Get(out _gameService)
                .Get(out _authentication);
        
            Debug.Log(_localization.GetLocalizedWord(playerName));
            _serializer.Serialize();
            _authentication.Login();
            _gameService.StartGame();
        }
    }
}
