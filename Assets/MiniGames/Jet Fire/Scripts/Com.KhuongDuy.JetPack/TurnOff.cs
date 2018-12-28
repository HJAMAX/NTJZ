using UnityEngine;
using System.Collections;

namespace Com.KhuongDuy.JetPack
{
    public class TurnOff : MonoBehaviour
    {
        // Animation Event
        public void Off()
        {
            this.gameObject.SetActive(false);
        }
    }

}
