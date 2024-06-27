using UnityEngine;
using UnityEngine.UI;
using ShanHai_IsolatedCity.Inventory;

namespace ShanHai_IsolatedCity.Buff
{
    public class BuffSlotUI : MonoBehaviour
    {
        [SerializeField] private Buff_SO buffData;
        [SerializeField] private Text buffName;
        public Image BG;
        

        public void UpdateBuffSlot(Buff_SO buff)
        {
            buffData = buff;
            buffName.text = buffData.buffName;
            BG.color = InventoryManager.Instance.GetBasicQualityColor(buff.buffQuality);
        }
    }
}

