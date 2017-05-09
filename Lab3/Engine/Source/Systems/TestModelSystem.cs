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
    public class TestModelSystem : IRender
    {
        // The size of the shadow map
        // The larger the size the more detail we will have for our entire scene
        //const int shadowMapWidthHeight = 2048;
        const int shadowMapWidthHeight = 4096;
        //const int shadowMapWidthHeight = 8192;

        CameraComponent defaultCam;
        ShaderComponent CustomShader;
        Vector3 lightDir = new Vector3(-0.3333333f, 0.6666667f, 0.6666667f);

        // ViewProjection matrix from the lights perspective
        Matrix lightViewProjection;
        RenderTarget2D shadowRenderTarget;

        private GraphicsDevice Device;

        public TestModelSystem(GraphicsDevice device)
        {
            shadowRenderTarget = new RenderTarget2D(device,
                                            shadowMapWidthHeight,
                                            shadowMapWidthHeight,
                                            false,
                                            SurfaceFormat.Single,
                                            DepthFormat.Depth24);
            this.Device = device;
        }

        public override void Draw(GameTime gameTime)
        {
            //Check for all entities with a camera
            List<int> entitiesWithCamera = ComponentManager.Instance.GetAllEntitiesWithComponentType<CameraComponent>();
            //pick one
            defaultCam = ComponentManager.Instance.GetEntityComponent<CameraComponent>(entitiesWithCamera.First());
            Dictionary<int, IComponent> mc = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<ModelComponent>();
            if (mc == null)
                return;

            lightViewProjection = CreateLightViewProjectionMatrix();

            CreateShadowMap(mc);
            DrawWithShadowMap(mc);
            

            //foreach (var entity in mc)
            //{
            //    //Hämta ut Modelcomponenten
            //    ModelComponent mcp = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity.Key);
            //    //Hämta ut Transformcomponenten
            //    TransformComponent tfc = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity.Key);
            //    //Check if model has it's own camera, if not use default
            //    if (ComponentManager.Instance.CheckIfEntityHasComponent<CameraComponent>(entity.Key))
            //        defaultCam = ComponentManager.Instance.GetEntityComponent<CameraComponent>(entity.Key);

            //    if (ComponentManager.Instance.CheckIfEntityHasComponent<ShaderComponent>(entity.Key))
            //        CustomShader = ComponentManager.Instance.GetEntityComponent<ShaderComponent>(entity.Key);


            //    var sphere = GetModelBoundingSphere(mcp, entity.Key);
            //    if (defaultCam.CameraFrustrum.Intersects(sphere))
            //    {
            //        if (CustomShader == null)
            //        {
            //            DrawModelsWithoutShader(mcp, tfc);
            //        }
            //        else
            //        {
            //            DrawWithShaders(CustomShader, defaultCam, mcp, tfc);
            //        }

            //    }

            //}
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

        private void CreateShadowMap(Dictionary<int, IComponent> mc)
        {
            // Set our render target to our floating point render target
            Device.SetRenderTarget(shadowRenderTarget);

            // Clear the render target to white or all 1's
            // We set the clear to white since that represents the 
            // furthest the object could be away
            Device.Clear(Color.White);

            foreach (var entity in mc)
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
                    if (CustomShader == null)
                    {
                        DrawModelsWithoutShader(mcp, tfc, true);
                    }
                    else
                    {
                        DrawWithShaders(CustomShader, defaultCam, mcp, tfc, true);
                    }
                }
            }

            //// Draw any occluders in our case that is just the dude model

            //// Set the models world matrix so it will rotate
            //world = Matrix.CreateRotationY(MathHelper.ToRadians(rotateDude));
            //// Draw the dude model
            //DrawModel(dudeModel, true);



            // Set render target back to the back buffer
            Device.SetRenderTarget(null);
        }

        private void DrawWithShadowMap(Dictionary<int, IComponent> mc)
        {
            //vet inte om jag behöver göra detta här, tror inte det
            //Device.Clear(Color.CornflowerBlue);

            Device.SamplerStates[1] = SamplerState.PointClamp;

            foreach (var entity in mc)
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

                //var i = 0;
                var sphere = GetModelBoundingSphere(mcp, entity.Key);
                if (defaultCam.CameraFrustrum.Intersects(sphere))
                {
                    //i++;
                    if (CustomShader == null)
                    {
                        DrawModelsWithoutShader(mcp, tfc, false);
                    }
                    else
                    {
                        DrawWithShaders(CustomShader, defaultCam, mcp, tfc, false);
                    }
                }
                //Console.WriteLine(i);
            }

            // Draw the grid
            //world = Matrix.Identity;
            //DrawModel(gridModel, false);

            // Draw the dude model
            //world = Matrix.CreateRotationY(MathHelper.ToRadians(rotateDude));
            //DrawModel(dudeModel, false);
        }

        private void DrawModelsWithoutShader(ModelComponent mcp, TransformComponent tfc, bool createShadowMap)
        {
            for (int index = 0; index < mcp.Model.Meshes.Count; index++)
            {
                ModelMesh mesh = mcp.Model.Meshes[index];

                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mcp.IsTextured)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = mcp.ModelTexture;
                    }

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    if (mcp.MeshWorldMatrices != null)
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

        private void DrawWithShaders(ShaderComponent shader, CameraComponent cmp, ModelComponent mcp, TransformComponent tfc, bool createShadowMap)
        {
            //Matrix world;
            //Effect effect = shader.ShaderEffect;

            //for (int index = 0; index < mcp.Model.Meshes.Count; index++)
            //{
            //    ModelMesh mesh = mcp.Model.Meshes[index];

            //    if (mcp.MeshWorldMatrices != null)
            //        world = mesh.ParentBone.Transform * mcp.MeshWorldMatrices[index] * tfc.ObjectMatrix;
            //    else
            //        world = mesh.ParentBone.Transform * tfc.ObjectMatrix;

            //    foreach (EffectTechnique pass in effect.Techniques)
            //    {
            //        foreach (ModelMeshPart part in mesh.MeshParts)
            //        {
            //            part.Effect = effect;
            //            effect.Parameters["World"].SetValue(world);
            //            effect.Parameters["View"].SetValue(defaultCam.ViewMatrix);
            //            effect.Parameters["Projection"].SetValue(defaultCam.ProjectionMatrix);
            //            effect.Parameters["AmbientColor"].SetValue(shader.AmbientColor);
            //            effect.Parameters["AmbientIntensity"].SetValue(shader.AmbientIntensity);
            //            effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(mesh.ParentBone.Transform * world));
            //            effect.Parameters["ViewVector"].SetValue(defaultCam.ViewVector);
            //            effect.Parameters["DiffuseLightDirection"].SetValue(shader.DiffuseLightDirection);
            //            effect.Parameters["DiffuseColor"].SetValue(shader.Diffusecolor);
            //            effect.Parameters["DiffuseIntensity"].SetValue(shader.DiffuseIntensity);
            //            effect.Parameters["ModelTexture"].SetValue(mcp.ModelTexture);
            //            effect.Parameters["CameraPosition"].SetValue(defaultCam.Position);
            //            if (shader.FogEnabled)
            //            {
            //                effect.Parameters["FogStart"].SetValue(shader.FogStart);
            //                effect.Parameters["FogEnd"].SetValue(shader.FogEnd);
            //                effect.Parameters["FogColor"].SetValue(shader.FogColor);
            //                effect.Parameters["FogEnabled"].SetValue(true);
            //            }
            //            effect.Parameters["Shininess"].SetValue(shader.Shininess);
            //            effect.Parameters["SpecularColor"].SetValue(shader.SpecularColor);
            //            effect.Parameters["SpecularIntensity"].SetValue(shader.SpecularIntensity);

            //        }
            //        mesh.Draw();
            //    }
            //}

            string techniqueName = createShadowMap ? "CreateShadowMap" : "DrawWithShadowMap";

            Matrix[] transforms = new Matrix[mcp.Model.Bones.Count];
            mcp.Model.CopyAbsoluteBoneTransformsTo(transforms);

            // Loop over meshs in the model
            foreach (ModelMesh mesh in mcp.Model.Meshes)
            {
                // Loop over effects in the mesh
                foreach (Effect effect in mesh.Effects)
                {
                    // Set the currest values for the effect
                    effect.CurrentTechnique = effect.Techniques[techniqueName];
                    effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * tfc.ObjectMatrix/* * Matrix.CreateScale(10,10,10)*/);
                    effect.Parameters["View"].SetValue(defaultCam.ViewMatrix);
                    effect.Parameters["Projection"].SetValue(defaultCam.ProjectionMatrix);
                    effect.Parameters["LightDirection"].SetValue(lightDir);
                    effect.Parameters["LightViewProj"].SetValue(lightViewProjection);
                    effect.Parameters["ShadowStrenght"].SetValue(1f);
                    effect.Parameters["DepthBias"].SetValue(0.001f);

                    if (!createShadowMap)
                        effect.Parameters["ShadowMap"].SetValue(shadowRenderTarget);
                    
                    //effect.Parameters["World"].SetValue(world);
                    //effect.Parameters["View"].SetValue(defaultCam.ViewMatrix);
                    //effect.Parameters["Projection"].SetValue(defaultCam.ProjectionMatrix);

                    effect.Parameters["AmbientColor"].SetValue(shader.AmbientColor);
                    effect.Parameters["AmbientIntensity"].SetValue(shader.AmbientIntensity);
                    effect.Parameters["ViewVector"].SetValue(defaultCam.ViewVector);
                    //effect.Parameters["DiffuseLightDirection"].SetValue(shader.DiffuseLightDirection);
                    effect.Parameters["DiffuseLightDirection"].SetValue(lightDir); // todo
                    effect.Parameters["DiffuseColor"].SetValue(shader.Diffusecolor);
                    effect.Parameters["DiffuseIntensity"].SetValue(shader.DiffuseIntensity);
                    effect.Parameters["TextureEnabled"].SetValue(mcp.IsTextured);
                    if (mcp.IsTextured)
                        effect.Parameters["Texture"].SetValue(mcp.ModelTexture);
                    effect.Parameters["CameraPosition"].SetValue(defaultCam.Position);
                    if (shader.FogEnabled)
                    {
                        effect.Parameters["FogStart"].SetValue(shader.FogStart);
                        effect.Parameters["FogEnd"].SetValue(shader.FogEnd);
                        effect.Parameters["FogColor"].SetValue(shader.FogColor);
                        effect.Parameters["FogEnabled"].SetValue(true);
                    }
                    effect.Parameters["Shininess"].SetValue(shader.Shininess);
                    effect.Parameters["SpecularColor"].SetValue(shader.SpecularColor);
                    effect.Parameters["SpecularIntensity"].SetValue(shader.SpecularIntensity);
                    
                }
                // Draw the mesh
                mesh.Draw();
            }
        }
        /// <summary>
        /// Creates the WorldViewProjection matrix from the perspective of the 
        /// light using the cameras bounding frustum to determine what is visible 
        /// in the scene.
        /// </summary>
        /// <returns>The WorldViewProjection for the light</returns>
        private Matrix CreateLightViewProjectionMatrix()
        {
            // Matrix with that will rotate in points the direction of the light
            Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero,
                                                       -lightDir,
                                                       Vector3.Up);

            // Get the corners of the frustum
            Vector3[] frustumCorners = defaultCam.CameraFrustrum.GetCorners();

            // Transform the positions of the corners into the direction of the light
            for (int i = 0; i < frustumCorners.Length; i++)
            {
                frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
            }

            // Find the smallest box around the points
            BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            Vector3 boxSize = lightBox.Max - lightBox.Min;
            Vector3 halfBoxSize = boxSize * 0.5f;

            // The position of the light should be in the center of the back
            // pannel of the box. 
            Vector3 lightPosition = lightBox.Min + halfBoxSize;
            lightPosition.Z = lightBox.Min.Z;

            // We need the position back in world coordinates so we transform 
            // the light position by the inverse of the lights rotation
            lightPosition = Vector3.Transform(lightPosition,
                                              Matrix.Invert(lightRotation));

            // Create the view matrix for the light
            Matrix lightView = Matrix.CreateLookAt(lightPosition,
                                                   lightPosition - lightDir,
                                                   Vector3.Up);

            // Create the projection matrix for the light
            // The projection is orthographic since we are using a directional light
            Matrix lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y,
                                                               -boxSize.Z, boxSize.Z);

            return lightView * lightProjection;
        }

    }
}
