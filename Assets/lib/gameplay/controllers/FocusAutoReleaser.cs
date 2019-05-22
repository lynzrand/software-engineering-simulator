using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sesim.Game.Controllers
{
    public class FocusAutoReleaser : MonoBehaviour
    {
        void LateUpdate()
        {
            if (EventSystem.current.currentSelectedGameObject == this.gameObject && Input.GetKeyDown(KeyCode.Escape))
                EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
