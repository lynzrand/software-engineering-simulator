using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using Sesim.Library.Models;

namespace Sesim.Game.Controllers
{
    public class CompanyActionController : MonoBehaviour
    {
        public Company company;
        // Start is called before the first frame update
        void Start()
        {
            company = new Company();

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}