﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Source.Components
{
    public class ShaderComponent : IComponent
    {
        public Effect ShaderEffect { private set; get; }
        public Vector4 AmbientColor { set; get; }
        public float AmbientIntensity { set; get; }
        public Vector3 DiffuseLightDirection { get; set; }
        public Vector4 Diffusecolor { get; set; }
        public float DiffuseIntensity { get; set; }
        public float FogStart { get; set; }
        public float FogEnd { get; set; }
        public Vector4 FogColor { get; set; }
        public bool FogEnabled { get; set; }
        public float Shininess { get; set; }
        public Vector4 SpecularColor { get; set; }
        public float SpecularIntensity { get; set; }
        public float ShadowStrenght { get; set; }

        public ShaderComponent(Effect effect)
        {
            ShaderEffect = effect;
            AmbientColor = Color.White.ToVector4();
            AmbientIntensity = 0.1f;
            Diffusecolor = Color.White.ToVector4();
            DiffuseLightDirection = new Vector3(0, 0.5f, 0);
            DiffuseIntensity = 0.1f;
            FogEnabled = false;
            FogStart = 0;
            FogEnd = 0;
            FogColor = Color.White.ToVector4();
            Shininess = 50;
            SpecularColor = Color.White.ToVector4();
            SpecularIntensity = 0;
            ShadowStrenght = 0.5f;
        }

        public void AmbientLightSettings(Color AmbientColor, float AmbientIntensity)
        {
            this.AmbientColor = AmbientColor.ToVector4();
            this.AmbientIntensity = AmbientIntensity;
        }
        public void DirectionalLightSettings(Vector3 DiffuseLightDirection, Color DiffuseColor, float DiffuseIntensity)
        {
            this.DiffuseLightDirection = DiffuseLightDirection;
            Diffusecolor = DiffuseColor.ToVector4();
            this.DiffuseIntensity = DiffuseIntensity;
        }
        public void ActivateAppropriateFogSettings()
        {
            FogEnabled = true;
            FogEnd = 600;
            FogStart = 400;
            FogColor = Color.Black.ToVector4();
        }
        public void AddCustomFog(float FogEnd, float FogStart, Color FogColor)
        {
            this.FogColor = FogColor.ToVector4();
            this.FogEnd = FogEnd;
            this.FogStart = FogStart;
            FogEnabled = true;
        }

        public void SpecularSettings(float Shininess, Color Specularcolor, float SpecularIntensity)
        {
            this.Shininess = Shininess;
            this.SpecularIntensity = SpecularIntensity;
            SpecularColor = Specularcolor.ToVector4();
        }

        public void FunnyTheme()
        {
            AmbientLightSettings(Color.ForestGreen, 1f);
            DirectionalLightSettings(new Vector3(0.2f, 0.2f, 0),Color.BlueViolet,1f);
            SpecularSettings(200, Color.DarkMagenta, 1f);
            AddCustomFog(400, 50, Color.DeepPink);
        }

        public void RealisticSettings()
        {
            AmbientLightSettings(Color.White, 0.1f);
            DirectionalLightSettings(new Vector3(-1f, 0f, 0), Color.White, 0.8f);
            SpecularSettings(80, Color.WhiteSmoke, 0.4f);
            ActivateAppropriateFogSettings();
        }
    }
}
