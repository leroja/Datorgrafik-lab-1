using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Engine.Source.Managers;
using Engine.Source.Components;

namespace Engine.Source.Systems
{
    public class ChaseCamSystem : IUpdate
    {
        public void update(GameTime gameTime)
        {

            var chaseCameras = ComponentManager.Instance.GetAllEntitiesWithComponentType<ChaseCamComponent>();

            if (chaseCameras == null) return;

            foreach (var cameraId in chaseCameras)
            {
                var chaseCam = ComponentManager.Instance.GetEntityComponent<ChaseCamComponent>(cameraId);
                var baseCam = ComponentManager.Instance.GetEntityComponent<CameraComponent>(cameraId);
                var transform = ComponentManager.Instance.GetEntityComponent<TransformComponent>(cameraId);

                var camPosition = chaseCam.OffSet;
                // var mat = Matrix.CreateFromQuaternion(transform.QuaternionRotation);
                //camPosition = Vector3.Transform(camPosition, mat);
                camPosition += transform.Position;

                //baseCam.UpVector = Vector3.Transform(Vector3.Up, mat);

                baseCam.Position = camPosition;
                baseCam.LookAt = transform.Position;
            }

        }
    }
}
