using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Source.Components
{
    public class ChaseCamComponent : IComponent
    {
        public Vector3 OffSet { get; set; }

        public bool IsDrunk { get; set; }
    }
}
