using System.Collections;
using System.Collections.Generic;
using ShanHai_IsolatedCity.Skill;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

namespace ShanHai_IsolatedCity.Inventory
{
    [RequireComponent(typeof(SlotUI))]
    public class ShowItemToolTip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        public SlotUI slotUI;
        //private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private Vector3 mousePos;

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }

        
        private void OnDisable()
        {
            //inventoryUI.itemToolTip.gameObject.SetActive(false);
            //inventoryUI.skillToolTip.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }



        public void OnPointerExit(PointerEventData eventData)
        {
           
        }

        private void UpdateToolTipPosition(RectTransform pos)
        {
            mousePos = Input.mousePosition;
            Vector3[] corners = new Vector3[4];
            
            pos.GetWorldCorners(corners);
            
            float width = corners[3].x - corners[0].x;
            
            pos.pivot = new Vector2(-0.06f, 0.5f);

            if (Screen.width - mousePos.x >= width)
                pos.position = mousePos + Vector3.right * width * 0.5f;
            else
                pos.position = mousePos + Vector3.left * width * 0.5f;
            pos.position = new Vector3(transform.position.x, Screen.height * 0.5f, transform.position.z);
        }
    }
}
