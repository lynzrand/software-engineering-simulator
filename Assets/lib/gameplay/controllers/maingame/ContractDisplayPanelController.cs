using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Sesim.Game.Controllers;

namespace Sesim.Game.Controllers.MainGame
{
    public class ContractDisplayPanelController : MonoBehaviour
    {
        public Font font;

        // Start is called before the first frame update
        void Start()
        {
            var display = this.GetComponentInChildren<KeyValueDisplayer>();
            Debug.Log(display.name);
            display.Construct(new List<KeyValueDisplayElement>()
            {
                new KeyValueDisplayElement(){
                    Title="test",
                    font=font
                }
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
