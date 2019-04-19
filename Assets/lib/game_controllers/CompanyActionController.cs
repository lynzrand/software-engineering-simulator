using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sesim.Library.Models;
using ProtoBuf;

namespace Sesim.Game.Controllers
{
    public class CompanyActionController : MonoBehaviour
    {
        public Company company;

        public Camera cam;


        // Start is called before the first frame update
        void Start()
        {
            company = new Company();

        }

        // Update is called once per frame
        void Update()
        {

            switch (key)
            {
                case KeyCode.A:
                    // cam.x += 5.0f;
                    break;

                default:
                    // noop
                    break;
            }
        }


    }
}