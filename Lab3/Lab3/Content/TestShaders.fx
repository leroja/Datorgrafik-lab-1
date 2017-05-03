#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix World;
matrix View;
matrix Projection;
matrix WorldInverseTranspose;

float4 AmbientColor = float4(0.2f, 0.2f, 0.2f, 0.2f);
float AmbientIntensity = 0.1f;

float3 DiffuseLightDirection = float3(0, 1, 0);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0;

float Shininess = 200;
float4 SpecularColor = float4(1, 1, 1, 1);
float SpecularIntensity = 1;
float3 ViewVector = float3(1, 0, 0);

//Parameters for the fog
float fogStart;
float fogEnd;
Texture2D shaderTexture;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float3 Normal : TEXCOORD0;
};

struct VertexInputType
{
    float4 position : POSITION0;
    float2 tex : TEXCOORD0;
};

struct PixelInputType
{
    float4 position : POSITION0;
    float2 tex : TEXCOORD0;
    float fogFactor : FOG;
};

SamplerState SampleType
{
    //Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float4 normal = mul(input.Normal, WorldInverseTranspose);
	float lightIntensity = dot(normal.xyz, DiffuseLightDirection);
	output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);
	output.Normal = normal;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 light = normalize(DiffuseLightDirection);
	float3 normal = normalize(input.Normal);
	float3 r = normalize(2 * dot(light, normal) * normal - light);
	float3 v = normalize(mul(normalize(ViewVector), World));

	float dotProduct = dot(r, v);
	float4 specular = SpecularIntensity * SpecularColor * max(pow(dotProduct, Shininess), 0) * length(input.Color);

	return saturate(input.Color + AmbientColor * AmbientIntensity + specular);
}

PixelInputType FogVertexShader(VertexInputType input)
{
    PixelInputType output;
    float4 cameraPosition;

    
    // Change the position vector to be 4 units for proper matrix calculations.
    input.position.w = 1.0f;

    // Calculate the position of the vertex against the world, view, and projection matrices.
    output.position = mul(input.position, World);
    output.position = mul(output.position, View);
    output.position = mul(output.position, Projection);
    
    // Store the texture coordinates for the pixel shader.
    output.tex = input.tex;
    
    // Calculate the camera position.
    cameraPosition = mul(input.position, World);
    cameraPosition = mul(cameraPosition, View);

    // Calculate linear fog.    
    output.fogFactor = saturate((fogEnd - cameraPosition.z) / (fogEnd - fogStart));

    return output;
}

float4 FogPixelShader(PixelInputType input) : SV_Target
{
    float4 textureColor;
    float4 fogColor;
    float4 finalColor;
	
	
    // Sample the texture pixel at this location.
    textureColor = shaderTexture.Sample(SampleType, input.tex);
    
    // Set the color of the fog to grey.
    fogColor = float4(0.5f, 0.5f, 0.5f, 1.0f);


    // Calculate the final color using the fog effect equation.
    finalColor = input.fogFactor * textureColor + (1.0 - input.fogFactor) * fogColor;
   	
    return finalColor;
}

technique Ambient
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODELVertexShaderFunction();
        PixelShader = compile PS_SHADERMODELPixelShaderFunction();
    }
}

technique Fog
{
    pass Pass2
    {
        VertexShader = compile VS_SHADERMODEL FogVertexShader();
        PixelShader = compile PS_SHADERMODEL FogPixelShader();
    }
}

