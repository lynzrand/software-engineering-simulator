using System;
using Hocon;
using ProtoBuf;
using Sesim.Helpers.Config;
using Sesim.Models.Exceptions;
using UnityEngine;

namespace Sesim.Models
{
    public class ContractFactory : IConfDeserializable
    {
        public string name;
        public string category;
        public string title;
        public string description;
        AnimationCurve abundancyCurve;

        public ContractFactory()
        {

        }

        public void DeserializeFromHocon(IHoconElement rootNode)
        {
            if (!(rootNode is HoconObject)) throw new DeformedObjectException();
            throw new NotImplementedException();
        }
    }

    [ProtoContract]
    public partial class Contract
    {
        [ProtoMember(16)]
        public long id;
    }
}