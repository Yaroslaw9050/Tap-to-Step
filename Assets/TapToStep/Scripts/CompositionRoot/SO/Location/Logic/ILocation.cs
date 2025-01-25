using System.Collections.Generic;
using UnityEngine;

namespace SO.Location.Logic
{
    public interface ILocation
    {
        public Queue<GameObject> GetLocations();
    }
}