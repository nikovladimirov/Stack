﻿using System;
using DefaultNamespace.Enums;
using UnityEngine;

namespace DefaultNamespace.Behaviours
{
    public class MenuUIBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.Instance.GameStateChanged +=GameStateChanged;
            GameStateChanged(GameState.Menu);
        }

        private void GameStateChanged(GameState state)
        {
            if (state != GameState.Menu)
            {
                gameObject.SetActive(false);
                return;
            }
            
            gameObject.SetActive(true);
        }
    }
}