using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShanHai_IsolatedCity.Inventory
{
    public class ActionBarButton : MonoBehaviour
    {
        public KeyCode key;
        private SlotUI slotUI;
        private bool canUse;

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }

        private void OnEnable()
        {
            EventHandler.UpdateGameStateEvent += OnUpdateGameStateEvent;
        }

        private void OnDisable()
        {
            EventHandler.UpdateGameStateEvent -= OnUpdateGameStateEvent;
        }

        private void OnUpdateGameStateEvent(GameState gameState)
        {
            canUse = gameState == GameState.GamePlay;
        }
    }
}

