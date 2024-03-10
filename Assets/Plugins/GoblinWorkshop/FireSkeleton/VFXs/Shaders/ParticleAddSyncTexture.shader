Shader "Custom/Particles/ParticleAddSyncTexture" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_Sync("Sync Texture", 2D) = "white" {}
	
	_DistortionFrequency("_ScrollYSpeedY", Range(-50,50)) = 3
	_OffsetX("X", Range(0,1)) = 0.5
	_OffsetY("Y", Range(0,1)) = 0.5

	_OffsetX2("X2", Range(0,1)) = 0.5
	_OffsetY2("Y2", Range(0,1)) = 0.5
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
	Blend SrcAlpha One
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_particles
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _Sync;
			fixed4 _TintColor;
			

			fixed _DistortionFrequency;
			fixed _OffsetX;
			fixed _OffsetY;
			fixed _OffsetX2;
			fixed _OffsetY2;

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				fixed yScrollValue = _DistortionFrequency * _Time;
				
				float _SyncTexture = tex2Dlod(_Sync, float4(float2(_OffsetX, _OffsetY + yScrollValue * 0.3) , 0, 0) ).g;
				float _SyncTexture2 = tex2Dlod(_Sync, float4(float2(_OffsetX2, _OffsetY2 + yScrollValue * 0.3), 0, 0)).g;

				o.vertex = UnityObjectToClipPos(v.vertex);
				
				//o.color = float4(v.color.r, v.color.r, v.color.r, v.color.r);

				//o.color = v.color * saturate(_SyncTexture * 20 * v.color.r) + v.color * (1 - v.color.r);
				
				float4 tempColor = v.color;
				float intensity = _SyncTexture + _SyncTexture2;

				o.color = tempColor * intensity;

				//o.color = v.color;
				
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
				UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0)); // fog towards black due to our blend mode
				return col;
			}
			ENDCG 
		}
	}	
}
}
