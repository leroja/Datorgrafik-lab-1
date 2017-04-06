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
        public void Update(GameTime gameTime)
        {

            var chaseCameras = ComponentManager.Instance.GetAllEntitiesWithComponentType<ChaseCamComponent>();

            if (chaseCameras == null)
                return;

            foreach (var cameraId in chaseCameras)
            {
                var chaseCam = ComponentManager.Instance.GetEntityComponent<ChaseCamComponent>(cameraId);
                var baseCam = ComponentManager.Instance.GetEntityComponent<CameraComponent>(cameraId);
                var transform = ComponentManager.Instance.GetEntityComponent<TransformComponent>(cameraId);


                var mat = Matrix.CreateFromQuaternion(transform.QuaternionRotation);
                var camPosition = Vector3.Transform(chaseCam.OffSet, mat);
                camPosition += transform.Position;

                baseCam.UpVector = Vector3.Transform(Vector3.Up, mat);

                baseCam.Position = camPosition;
                baseCam.LookAt = transform.Position;
            }

        }
    }
}
