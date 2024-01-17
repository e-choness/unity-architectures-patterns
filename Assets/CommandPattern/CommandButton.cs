﻿using CommandPattern.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace CommandPattern
{
    public class CommandButton : MonoBehaviour
    {
        private ICommand _command = ICommand.Null;
        [SerializeField] private Button button;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);    
        }

        public void SetCommand(ICommand commandIn)
        {
            _command = commandIn;
        }

        public void ResetCommand()
        {
            _command = ICommand.Null;
        }

        private void OnClick()
        {
            Debug.Log($"Button Clicked {_command.GetType().Name}");
            _command.Run();
        }
    }
}