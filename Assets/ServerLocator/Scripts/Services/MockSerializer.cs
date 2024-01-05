using ServerLocator.Scripts.Interfaces;
using UnityEngine;

namespace ServerLocator.Scripts.Services
{
    public class MockSerializer : ISerializer
    {
        public void Serialize()
        {
            Debug.Log("MockSerializer.Serialize() called.");
        }
    }
}