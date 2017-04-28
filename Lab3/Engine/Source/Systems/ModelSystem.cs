using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Source.Components;
using Engine.Source.Managers;

namespace Engine.Source.Systems
{
    public class ModelSystem : IRender
    {
        CameraComponent defaultCam;
        ShaderComponent CustomShader;
        public override void Draw(GameTime gameTime)
        {
            //Check for all entities with a camera
            List<int> entitiesWithCamera = ComponentManager.Instance.GetAllEntitiesWithComponentType<CameraComponent>();
            //pick one
            defaultCam = ComponentManager.Instance.GetEntityComponent<CameraComponent>(entitiesWithCamera.First());
            Dictionary<int,IComponent> mc = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<ModelComponent>();
            if (mc == null)
                return;
            foreach(var entity in mc)
            {
                //Hämta ut Modelcomponenten
                ModelComponent mcp = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity.Key);
                //Hämta ut Transformcomponenten
                TransformComponent tfc = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity.Key);
                //Check if model has it's own camera, if not use default
                if (ComponentManager.Instance.CheckIfEntityHasComponent<CameraComponent>(entity.Key))
                    defaultCam = ComponentManager.Instance.GetEntityComponent<CameraComponent>(entity.Key);

                if (ComponentManager.Instance.CheckIfEntityHasComponent<ShaderComponent>(entity.Key))
                    CustomShader = ComponentManager.Instance.GetEntityComponent<ShaderComponent>(entity.Key);


                var sphere = GetModelBoundingSphere(mcp, entity.Key);
                if (defaultCam.CameraFrustrum.Intersects(sphere))
                {
                    if(CustomShader == null)
                    {
                        DrawModelsWithoutShader(mcp, tfc);
                    }
                    else
                    {
                        DrawWithShaders(CustomShader, defaultCam, mcp, tfc);
                    }
                        
                }
                
            }
        }

        private BoundingSphere GetModelBoundingSphere(ModelComponent modelComp, int entityId)
        {
            var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(entityId);
            var sphere = new BoundingSphere(transformComp.Position, 0);

            var boneTransforms = new Matrix[modelComp.Model.Bones.Count];
            modelComp.Model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (var mesh in modelComp.Model.Meshes)
            {
                var meshTransform = boneTransforms[mesh.ParentBone.Index] * transformComp.ObjectMatrix;

                var s = mesh.BoundingSphere;
                s = s.Transform(meshTransform);
                sphere = BoundingSphere.CreateMerged(sphere, s);
            }
            return sphere;
        }

        private void DrawModelsWithoutShader(ModelComponent mcp, TransformComponent tfc)
        {
            for (int index = 0; index < mcp.Model.Meshes.Count; index++)
            {
                ModelMesh mesh = mcp.Model.Meshes[index];
                
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mcp.isTextured)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = mcp.modelTexture;
                    }

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    if(mcp.MeshWorldMatrices != null)
                        effect.World = mesh.ParentBone.Transform * mcp.MeshWorldMatrices[index] * tfc.ObjectMatrix;
                    else
                        effect.World = mesh.ParentBone.Transform * tfc.ObjectMatrix;

                    effect.View = defaultCam.ViewMatrix;
                    effect.Projection = defaultCam.ProjectionMatrix;

                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        mesh.Draw();
                    }
                }
            }
        }

        private void DrawWithShaders(ShaderComponent shader, CameraComponent cmp, ModelComponent mcp, TransformComponent tfc)
        {
            Matrix world;
            Effect effect = shader.ShaderEffect;

            for (int index = 0; index < mcp.Model.Meshes.Count; index++)
            {
                ModelMesh mesh = mcp.Model.Meshes[index];

                if (mcp.MeshWorldMatrices != null)
                    world = mesh.ParentBone.Transform * mcp.MeshWorldMatrices[index] * tfc.ObjectMatrix;
                else
                    world = mesh.ParentBone.Transform * tfc.ObjectMatrix;

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(world);
                    effect.Parameters["View"].SetValue(defaultCam.ViewMatrix);
                    effect.Parameters["Projection"].SetValue(defaultCam.ProjectionMatrix);
                    effect.Parameters["AmbientColor"].SetValue(Color.DeepPink.ToVector4());
                    effect.Parameters["AmbientIntensity"].SetValue(0.5f);
                    effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(mesh.ParentBone.Transform * world));
                    effect.Parameters["ViewVector"].SetValue(defaultCam.ViewVector);
                }
                mesh.Draw();
            }
            

        }

        
    }
}
