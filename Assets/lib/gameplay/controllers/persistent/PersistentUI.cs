using System;
using UnityEngine;

namespace Sesim.Game.Controllers
{
    public class PersistentUI : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {

        }

    }
}