// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "AE/Alpha FX Alpha"
{
	
	Properties
	{
		_Color		("Direct Color", Color) = (1.0, 0, 0, 1.0)
		_AlphaTex	("AlphaPad (RGBA)", 2D)    = "" {}
	}
	
	SubShader
	{
		
		
		
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
			"IgnoreProjector" = "True"
		}
		
		
		
		Pass
		{
			
			Colormask A
			AlphaTest Less 0.5
			
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			
			uniform sampler2D _AlphaTex;
			
			
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv  : TEXCOORD0;
			};
			
			
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv  = v.texcoord.xy;
				return o;
			}
			
			
			
			half4 frag (v2f i) : COLOR
			{
				half4 sample = tex2D( _AlphaTex, i.uv );
				
				return sample;
			}
			
			
			
			ENDCG
			
		}
		
		
		
		
		
	}
	
	Fallback "VertexLit"
	
}