using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Source.Components
{
    public class TransformComponent : IComponent
    {
        //Holds data such as position, rotation and scaling.

        public Vector3 position { get; set; }
        public Matrix rotation { get; set; }
        public Vector3 scale { get; set; }
        public Matrix objectMatrix { get; set; }
        public TransformComponent(Vector3 position, Vector3 scale)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = Matrix.Identity;
        }




    }
}
