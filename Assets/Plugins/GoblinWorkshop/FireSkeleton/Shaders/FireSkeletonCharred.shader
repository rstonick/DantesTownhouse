Shader "Custom/GW_FireSkeletonCharred" {
	Properties{
		_FlameTransition("Flame Transition", Range(0,1)) = 0.2 //force a transition between normal flame based on texture sample and full flames
		
		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5) //Main flame color
		_SecondaryColor("Secondar Color", Color) = (0.5,0.5,0.5,0.5) //Secondary flame color

		_GlowIntensity("Glow Intensity", Range(0,100)) = 1 //Useful to fine tune while using bloom screen effects
		_MainTex("Albedo (RGB)", 2D) = "white" {} //Charred Albedo Texture
		_Emission("BaseEmissionTex (RGB)", 2D) = "white" {} //base emissive texture
		
		_FlameTex("Flame texture (RGB)", 2D) = "white" {} //sample texture
		
		_NormalTex("Normal Map", 2D) = "bump" {} //Charred Normal map
		
		_FireTextureDistortion("FireIntensity", Range(0,2)) = 1 //Distortion intensity on sampling texture UVs
		_FlameSpeed("Flame Speed", Range(-50,50)) = -5 //flame speed
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalTex;
		sampler2D _Emission;
		sampler2D _FlameTex;
		
		fixed4 _TintColor;
		fixed4 _SecondaryColor;
		fixed _FlameTransition;
		fixed _GlowIntensity;
		fixed _FlameSpeed;
		fixed _FireTextureDistortion;

		struct Input {
			float2 uv_MainTex;
			float2 uv2_FlameTex;
			half4 color : COLOR;
			float3 worldNormal;
			INTERNAL_DATA
		};


		void surf(Input IN, inout SurfaceOutputStandard o) {

			fixed yScrollValue = _FlameSpeed * _Time;
			fixed2 scrolledUV = fixed2(0, yScrollValue);

			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = fixed3(c.r, c.r, c.r);
			o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_MainTex));

			fixed firePatternNoise = tex2D(_FlameTex, scrolledUV*1.5 + IN.uv2_FlameTex * 10).a;
			fixed distortion = ((firePatternNoise - 0.5) * 1)* 0.01 * c.g * c.b;

			fixed2 firePatternLarge = tex2D(_FlameTex, scrolledUV*0.3 + IN.uv2_FlameTex * 1 + (fixed2(distortion, distortion)) * 2 * _FireTextureDistortion).rg;
			fixed2 firePattern = tex2D(_FlameTex,scrolledUV * 0.3 + IN.uv2_FlameTex + (fixed2(distortion, distortion)) * 2 * _FireTextureDistortion).rg;
			fixed3 BaseEmissionTex = tex2D(_Emission, IN.uv_MainTex + (fixed2(distortion, distortion))*2*_FireTextureDistortion);
			
			fixed sampleNoise = (firePattern.r * firePattern.g * firePatternNoise);
			fixed cracks = saturate((_FlameTransition + 0.4 + ( firePatternLarge.g*firePatternLarge.g + firePatternLarge.r*firePatternLarge.g  * 1.0 - c.a)) * saturate(_GlowIntensity));
			
			fixed4 colorBase = (lerp(_TintColor, _SecondaryColor, BaseEmissionTex.g) + BaseEmissionTex.b) * BaseEmissionTex.r;
			colorBase = lerp(colorBase, fixed4(1, 1, 0, 1) + fixed4(1, 1, 1, 1)*sampleNoise*sampleNoise, sampleNoise * 1.5 * colorBase.r*colorBase.r);
			colorBase = colorBase * 0.75 + colorBase * sampleNoise;

			colorBase = colorBase * saturate(cracks*cracks * 2);
			fixed4 color = colorBase * (_GlowIntensity + 1);
			
			o.Metallic = 0;
			o.Smoothness = cracks * 1.0 * colorBase.r;
			o.Emission = color.rgb;

		}
		ENDCG
	}
		FallBack "Diffuse"
}
