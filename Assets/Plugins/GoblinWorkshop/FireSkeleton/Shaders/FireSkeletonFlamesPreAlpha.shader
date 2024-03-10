// Upgrade NOTE: upgraded instancing buffer '_propBlock' to new syntax.

Shader "Custom/GW_FireSkeletonFlamesPreAlpha" {
Properties {
	[MaterialToggle] _UseMotion("UseMotion", Float) = 0 //Use data sent script GW_FireSkeletonManager.cs 
	_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5) //Main flame color
	_SecondaryColor("Secondar Color", Color) = (0.5,0.5,0.5,0.5) //Secondary flame color

	_GlowIntensity("Glow Intensity", Range(0,100)) = 1 //Useful to fine tune while using bloom screen effects
	_FlameTransition("Flame Transition", Range(0,1)) = 0 //force a transition between normal flame based on texture sample and full flames

	_FlameTex("Flame texture (RGB)", 2D) = "white" {} //sample texture
	
	_FlameSpeed("Flame Speed", Range(-50,50)) = -5 //flame speed
	_FlameSize("Flame Size", Range(-1,1)) = 0.05 //controls the amount of vertex distortion on flames, attenuated by vertex color

	_BaseVector("Base Vector", Vector) = (0,1,0,0)//neutral flame direction, can be used to simulate wind
	_MoveVector("MovementVector", Vector) = (0,1,0,0)//motion calculated on GW_FireSkeletonManager.cs provides a natural movement to flames based on GO average velocity
}

Category{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off Lighting Off ZWrite Off

	SubShader {

		Pass {
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#include "UnityCG.cginc"

			sampler2D _FlameTex;
			float4 _FlameTex_ST;

			fixed4 _TintColor;
			fixed4 _SecondaryColor;

			fixed _UseMotion;
			fixed _GlowIntensity;
			fixed _FlameSpeed;
			fixed _FlameSize;
			fixed _FlameTransition;
			float3 _MoveVector;
			float3 _BaseVector;

	
			struct appdata {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;

				UNITY_FOG_COORDS(1)
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			
			

			UNITY_INSTANCING_BUFFER_START(_propBlock)
			UNITY_DEFINE_INSTANCED_PROP(float, _UseMotionProp)
#define _UseMotionProp_arr _propBlock
			UNITY_DEFINE_INSTANCED_PROP(float3, _MoveVectorProp)
#define _MoveVectorProp_arr _propBlock
			UNITY_DEFINE_INSTANCED_PROP(float3, _BaseVectorProp)
#define _BaseVectorProp_arr _propBlock
			UNITY_INSTANCING_BUFFER_END(_propBlock)

			v2f vert (appdata v)
			{

				v2f o = (v2f)0;
				
				UNITY_SETUP_INSTANCE_ID(v);
				
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				
				fixed yScrollValue = _FlameSpeed * _Time;
				fixed2 scrolledUV = fixed2(0, yScrollValue);
				
				//Checkbox has higher priority than data flow
				
				_UseMotion = UNITY_ACCESS_INSTANCED_PROP(_UseMotionProp_arr, _UseMotionProp) * _UseMotion;
				_MoveVector = UNITY_ACCESS_INSTANCED_PROP(_MoveVectorProp_arr, _MoveVectorProp) * _UseMotion + _MoveVector * (1-_UseMotion);
				_BaseVector = UNITY_ACCESS_INSTANCED_PROP(_BaseVectorProp_arr, _BaseVectorProp) * _UseMotion + _BaseVector * (1-_UseMotion);


				float4 firePatternLarge = tex2Dlod(_FlameTex, float4(scrolledUV*0.3 + v.texcoord.xy, 0, 0));
				float firePatternNoise = tex2Dlod(_FlameTex, float4(scrolledUV*2 + v.vertex.xy * 1 + float2(firePatternLarge.a, firePatternLarge.a)* 1 , 0, 0)).a;
				float colorVal = saturate(firePatternLarge.g + _FlameTransition + firePatternNoise * 0.5 * (firePatternLarge.g + _FlameTransition) ) * (firePatternNoise * v.color.r);

				float varNormal = 1 - colorVal;
				float3 sn = mul((float3x3)unity_WorldToObject, float3(lerp(_MoveVector.x, _BaseVector.x, varNormal), lerp(_MoveVector.y, _BaseVector.y, varNormal), lerp(_MoveVector.z, _BaseVector.z, varNormal))).xyz;
				float normalAtt = (dot(normalize(v.normal), normalize(sn)) + 1) ;
				
				//Flame curvature
				float3 posVariation = v.normal * ((2 - normalAtt) ) * 0.8 * (colorVal + 0.5*colorVal)*0.3;

				//flame height
				posVariation = posVariation + sn * normalAtt * normalAtt  * 0.5 * colorVal;

				//Add and attenuate based on vertex color and fire size
				v.vertex.xyz = v.vertex.xyz + posVariation * _FlameSize;

				//Color keeps track of most of vertex calculations that might be useful for fragment
				o.color = float4(colorVal, firePatternLarge.g*firePatternLarge.g, firePatternNoise * firePatternNoise, v.color.r * normalAtt);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _FlameTex);
				
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed yScrollValue = _FlameSpeed * _Time;
				fixed2 scrolledUV = fixed2(0, yScrollValue);

				fixed4 firePattern = tex2D(_FlameTex , fixed2(i.texcoord.x, i.texcoord.y*0.01) * 30 + scrolledUV * 10 + fixed2(i.color.b, i.color.r) );
				fixed attenuation = i.color.r*i.color.r;
				
				fixed alpha = 0.5 * i.color.a * attenuation;
				fixed4 col = (lerp(_TintColor, _SecondaryColor, attenuation * 2 * (firePattern.a)) * firePattern.a + (fixed4(1, 1, 1, 1)  * i.color.b*i.color.b * 0.2 + firePattern.a * 0.02) ) * 16 * _GlowIntensity;
				
				col.a = alpha;
				

				UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0));
				return col;
			}
			ENDCG 
		}
		
	}	
}
}
