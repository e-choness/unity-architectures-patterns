using ServerLocator.Scripts.Interfaces;
using UnityEngine;

namespace ServerLocator.Scripts.Services
{
    public class MockLocalization : ILocalization
    {
        const string Message = "MockLocalization.GetLocalizedWord with word ";
        public string GetLocalizedWord(string key)
        {
            var word = $"{Message} {key}";
            Debug.Log(word);
            return word;
        }
    }
}