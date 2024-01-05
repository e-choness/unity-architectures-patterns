using ServerLocator.Scripts.Interfaces;
using UnityEngine;

namespace ServerLocator.Scripts.Services
{
    public class MockGameService : IGameService
    {
        public void StartGame()
        {
            Debug.Log("MockGameService.StartGame() called.");
        }
    }
}