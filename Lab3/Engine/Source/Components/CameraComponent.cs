using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Source.Components
{
    public class CameraComponent : IComponent
    {
        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }

        public BoundingFrustum CameraFrustrum { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 LookAt { get; set; }
        public Vector3 UpVector { get; set; }

        public float FarPlane { get; set; }
        public float NearPlane { get; set; }
        public float AspectRatio { get; set; }

        public Vector3 ViewVector { get; private set; }

        public CameraComponent(Vector3 pos, Vector3 LookAt, Vector3 UpVector, float FarPlane, float NearPlane, float AspectRatio)
        {
            this.AspectRatio = AspectRatio;
            this.FarPlane = FarPlane;
            this.LookAt = LookAt;
            this.NearPlane = NearPlane;
            this.Position = pos;
            this.UpVector = UpVector;

            //Vector3 cameraLocation = Position * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
            Vector3 cameraTarget = new Vector3(0, 0, 0);
            ViewVector = Vector3.Transform(cameraTarget - Position, Matrix.CreateRotationY(0));
            ViewVector.Normalize();
            ViewMatrix = Matrix.CreateLookAt(Position, cameraTarget, new Vector3(0, 1, 0));

            //ViewMatrix = Matrix.CreateLookAt(Position, LookAt, UpVector);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, AspectRatio, NearPlane, FarPlane);
            CameraFrustrum = new BoundingFrustum(ViewMatrix * ProjectionMatrix);
        }
    }
}
