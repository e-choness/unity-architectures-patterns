using ServerLocator.Scripts.Interfaces;
using UnityEngine;

namespace ServerLocator.Scripts.Services
{
    public class MockAuthentication : MonoBehaviour, IAuthentication
    {
        public void Login()
        {
            Debug.Log("MockAuthentication.Login() called.");
        }
    }
}