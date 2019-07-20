Shader "Overboard/Water"
{
	Properties
	{
		[Header(General Settings)]
		_MainTex ("Foam Texture (BW)", 2D) = "white" {}
		_FoamNoiseTexture("Foam Noise Texture (BW)", 2D) = "white" {}
		_FoamNoiseStep("Foam Noise Step", float) = 1
		_WaterTexSpeed("Water Texture Speed (X,Y)", Vector) = (1,1,0,0)
		_Color("Water Color", Color) = (1,1,1,1)
		[Header(Render Texture Settings (RTCameraUpdater.cs needed))]
		_RenderTexture("Render Texture (RT)", 2D) = "white" {}


		[Header(Water Settings)]
		_WaveHeight("Wave Height", float) = 0.5
		_WaveDistance("Wave Distance", float) = 1
		_WaveSpeed("Wave Speed", float) = 1


	}
	SubShader
	{
        Tags
		{ 
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}
		LOD 100

		Pass
		{
			ZWrite Off
        	Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _WaterTexSpeed;
			sampler2D _FoamNoiseTexture;
			float _FoamNoiseStep;

			// RT Settings
			sampler2D _RenderTexture;
			float4 _RTCameraPosition;
			float _RTCameraSize;

			// Water Settings
			float _WaveHeight;
			float _WaveDistance;
			float _WaveSpeed;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;
				v.vertex.y += sin(_Time.y * _WaveSpeed + (o.worldPos.x * _WaveDistance)) * _WaveHeight;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//o.vertex.y += sin(_Time*_WaveSpeed)*_WaveHeight;
				//o.vertex.x += cos(_Time*_WaveSpeed)*_WaveHeight;


				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{				
				fixed4 noiseTexture = tex2D(_FoamNoiseTexture, i.uv + _Time.x);
				//smoothstepping the noisetexture so that it isnt too random
				noiseTexture.rgb = step(_FoamNoiseStep, noiseTexture.r);
				noiseTexture.a = 0;


				float2 texPos = float2(i.worldPos.x - _RTCameraPosition.x, i.worldPos.z - _RTCameraPosition.z) / (_RTCameraSize * 2) + 0.5;

				// sampling textures
				fixed4 col = tex2D(_MainTex, i.uv + float2(_WaterTexSpeed.x  * _Time.x, _WaterTexSpeed.y  * _Time.x));
				fixed4 renderTextureColor = tex2D(_RenderTexture, texPos);

				// reapplying the textures and just changing the alpha value. this is possible since its just black and white pictures.
				renderTextureColor = float4(renderTextureColor.r * 1000, renderTextureColor.g * 1000, renderTextureColor.b * 1000, renderTextureColor.r * 1000);
				col = float4(col.r,col.g,col.b,col.r);


				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return (renderTextureColor) + col + _Color;
			}
			ENDCG
		}
	}
}
