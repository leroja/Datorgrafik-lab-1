using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Engine.Source.Components;
using Engine.Source.Managers;

namespace Engine.Source.Systems
{
    public class TransformSystem : IUpdate
    {

        /// <summary>
        /// the system for this component should handle the objects world matrix calculation
        /// i.e objectWorld = Matrix.CreateScale(scale) * rotation * Matrix.CreateTranslation(position);
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            Dictionary<int, IComponent> mc = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<TransformComponent>();
            foreach (var entity in mc)
            {
                TransformComponent tfc = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity.Key);
                var rotationQuaternion = Quaternion.CreateFromYawPitchRoll(tfc.Rotation.X, tfc.Rotation.Y, tfc.Rotation.Z);

                tfc.QuaternionRotation *= rotationQuaternion;
                tfc.Forward = Vector3.Transform(Vector3.Forward, tfc.QuaternionRotation);
                tfc.Up = Vector3.Transform(Vector3.Up, tfc.QuaternionRotation);
                tfc.Right = Vector3.Transform(Vector3.Right, tfc.QuaternionRotation);

                tfc.ObjectMatrix = Matrix.CreateScale(tfc.Scale) * Matrix.CreateFromQuaternion(tfc.QuaternionRotation) * Matrix.CreateTranslation(tfc.Position);
            }

        }
    }
}
