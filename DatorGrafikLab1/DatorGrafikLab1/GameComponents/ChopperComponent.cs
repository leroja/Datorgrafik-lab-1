using Engine.Source.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatorGrafikLab1.GameComponents
{
    public class ChopperComponent : IComponent
    {

        public float MainRotorAngle { get; set; }
        public float TailRotorAngle { get; set; }

    }
}
