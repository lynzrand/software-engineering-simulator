using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sesim.Models;
using ProtoBuf;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sesim.Game.Controllers
{
    public class CompanyActionController : MonoBehaviour
    {
        public Company company;
        public Text warpDisplayer;
        public Text timeDisplayer;
        public Camera cam;


        // Start is called before the first frame update
        void Start()
        {
            company = new Company();
        }

        // Update is called once per frame
        void Update()
        {
            var key = Event.current;

            if (Input.GetKey(KeyCode.A))
                cam.transform.Translate(new Vector3(-1f, 0f, 0f));
            if (Input.GetKey(KeyCode.W))
                cam.transform.Translate(new Vector3(0f, 1f, 0f));
            if (Input.GetKey(KeyCode.S))
                cam.transform.Translate(new Vector3(0f, -1f, 0f));
            if (Input.GetKey(KeyCode.D))
                cam.transform.Translate(new Vector3(1f, 0f, 0f));
        }


    }
}