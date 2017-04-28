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
               
                if (ComponentManager.Instance.CheckIfEntityHasComponent<TransformComponent>(entity.Key))
                {
                    //Hämta ut Modelcomponenten
                    ModelComponent mcp = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity.Key);
                    //Hämta ut Transformcomponenten
                    TransformComponent tfc = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity.Key);
                    //Check if model has it's own camera, if not use default
                    if (ComponentManager.Instance.CheckIfEntityHasComponent<CameraComponent>(entity.Key))
                        defaultCam = ComponentManager.Instance.GetEntityComponent<CameraComponent>(entity.Key);
                    
                    // onödigt att göra detta varje gång för statiska object
                    var sphere = GetModelBoundingSphere(mcp, entity.Key);
                    if (defaultCam.CameraFrustrum.Intersects(sphere))
                    {
                        if (mcp.MeshWorldMatrices != null)
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
                                    
                                    effect.World = mesh.ParentBone.Transform * mcp.MeshWorldMatrices[index] * tfc.ObjectMatrix;
                                    effect.View = defaultCam.ViewMatrix;
                                    effect.Projection = defaultCam.ProjectionMatrix;
                                }
                                mesh.Draw();
                            }
                        }
                        else
                        {
                            foreach (ModelMesh modelMesh in mcp.Model.Meshes)
                            {
                                foreach (BasicEffect effect in modelMesh.Effects)
                                {
                                    if (mcp.isTextured)
                                    {
                                        effect.TextureEnabled = true;
                                        effect.Texture = mcp.modelTexture;
                                    }

                                    effect.EnableDefaultLighting();
                                    effect.PreferPerPixelLighting = true;
                                    
                                    Matrix objectWorld = tfc.ObjectMatrix;
                                    effect.World = modelMesh.ParentBone.Transform * objectWorld;

                                    effect.View = defaultCam.ViewMatrix;
                                    effect.Projection = defaultCam.ProjectionMatrix;
                                    
                                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                                    {
                                        pass.Apply();
                                        modelMesh.Draw();
                                    }
                                }
                            }
                        }
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
    }
}
