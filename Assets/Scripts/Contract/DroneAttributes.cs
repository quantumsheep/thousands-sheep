using System;
using System.Collections.Generic;

namespace CryptoDrones
{
    [Serializable]
    public class DroneAttributes
    {
        public string id;
        public List<string> elements;
        public int attacksPerSecond;
        public int attackDamages;
        public int attackRange;
    }
}
