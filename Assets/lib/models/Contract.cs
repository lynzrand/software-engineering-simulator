using System;
using Hocon;
using MessagePack;
using Sesim.Helpers.Config;
using Sesim.Models.Exceptions;
using UnityEngine;
using Wiry.Base32;

namespace Sesim.Models
{
    public class ContractFactory : IConfDeserializable
    {
        public string name;
        public string category;
        public string title;
        public string description;
        AnimationCurve abundanceCurve;

        public ContractFactory()
        {

        }

        public void DeserializeFromHocon(IHoconElement rootNode)
        {
            if (!(rootNode is HoconObject)) throw new DeformedObjectException();
            throw new NotImplementedException();
        }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public partial class Contract
    {
        public long id;

        public string Base32String
        {
            get
            {
                return Base32Encoding.ZBase32.GetString(new byte[]{
                (byte)(id>>56), (byte)(id>>48), (byte)(id>>40), (byte)(id>>32),
                (byte)(id>>24), (byte)(id>>16), (byte)(id>>8), (byte)id
            });
            }
        }
    }
}