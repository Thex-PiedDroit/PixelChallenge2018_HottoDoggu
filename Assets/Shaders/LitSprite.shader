Shader "Custom/LitSprite"	// To enable shadowcasting on sprite renderers, set inspector mode to "debug"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_AlphaCutout("Alpha Cutout", Range(0, 1)) = 0.5
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Geometry"
			"RenderType" = "TransparentCutout"
		}

		LOD 200
		Cull Off

		CGPROGRAM

		#pragma surface Surf Lambert addshadow fullforwardshadows
		#pragma target 5.0


		sampler2D _MainTex;
		fixed4 _Color;
		half _AlphaCutout;

		struct Input
		{
			half2 uv_MainTex	: TEXCOORD0;
			half4 vertexColor	: COLOR;
		};

		void Surf(Input tIn, inout SurfaceOutput tOut)
		{
			fixed4 tColor = tex2D(_MainTex, tIn.uv_MainTex) * (tIn.vertexColor * _Color);
			tOut.Albedo = tColor.rgb;
			tOut.Alpha = tColor.a;
			clip(tOut.Alpha - _AlphaCutout);
		}

		ENDCG
	}

	FallBack "Sprites/Default"
}
