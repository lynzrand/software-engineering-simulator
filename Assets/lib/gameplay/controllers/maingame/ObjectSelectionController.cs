using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;


namespace Sesim.Game.Controllers.MainGame
{
    public class ObjectSelectionController : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData data)
        {
            Debug.Log(data);
        }
    }
}
