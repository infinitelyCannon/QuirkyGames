Shader "Custom/Outline"
{
	Properties //Variables
	{
		_MainTex("Main Texture (RBG)", 2D) = "white" {}//allows for a texture property
		_Color("Color", Color) = (1,1,1,1)//Allows for a color property

		_OutlineTex("Outline Texture", 2D) = "white"{}
		_OutlineColor("Outline Color", Color) = (1,1,1,0)
		_OutlineWidth("Outline Width", Range(1.0, 10.0)) = 1.1

		_XTex("Xray Texture (RBG)", 2D) = "white" {}//allows for a texture property
		_XColor("Xray Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
		}
		Pass
		{
			Name "OUTLINE"

			ZWrite Off

			CGPROGRAM//Allows talk between two languages: shader lab and nvidia c for graphics

			//Function Defines - defines the name for the vertex and fragment funcitons

			#pragma vertex vert//Define for the buillding function
			#pragma fragment frag//Define for coloring function

			//Includes
			#include "UnityCG.cginc"//Built in shader functions

			//Structures - can get data like - vertices's, normal, color, uv.

			//How the vertex gets its information
			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
		//How the fragment gets its information
		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
		};

		//Imports - Re-Import property from shader lab to nvidia cg

		float _OutlineWidth;
		float4 _OutlineColor;
		sampler2D _OutlineTex;

		//Vertex Function - Builds the object

		v2f vert(appdata IN)
		{
			IN.vertex.xyz *= _OutlineWidth;
			v2f OUT;

			OUT.pos = UnityObjectToClipPos(IN.vertex);
			OUT.uv = IN.uv;

			return OUT;
		}

		//Fragment Function - Color it in

		fixed4 frag(v2f IN) : SV_Target
		{
			float4 textColor = tex2D(_OutlineTex, IN.uv);
			return textColor * _OutlineColor;
		}

			ENDCG
		}
		Pass
		{
			NAME "OBJECT"
			CGPROGRAM//Allows talk between two languages: shader lab and nvidia c for graphics

			//Function Defines - defines the name for the vertex and fragment funcitons

			#pragma vertex vert//Define for the buillding function
			#pragma fragment frag//Define for coloring function

			//Includes
			#include "UnityCG.cginc"//Built in shader functions

			//Structures - can get data like - vertices's, normal, color, uv.

			//How the vertex gets its information
			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			//How the fragment gets its information
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			//Imports - Re-Import property from shader lab to nvidia cg

			float4 _Color;
			sampler2D _MainTex;

			//Vertex Function - Builds the object

			v2f vert(appdata IN)
			{
				v2f OUT;

				OUT.pos = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
			}

			//Fragment Function - Color it in

			fixed4 frag(v2f IN) : SV_Target
			{
				float4 textColor = tex2D(_MainTex, IN.uv);
				return textColor * _Color;
			}

				ENDCG
		}
		
			//Xray
			ZTEST GREATER

			CGPROGRAM
			#pragma surface surf Unlit

			sampler2D _XTex;
			struct Input
			{
				float2 uv_OutlineTex;
			};

			fixed4 _XColor;

			half4 LightingUnlit(SurfaceOutput s, half3 lightDir, half atten)
			{
				half4 col;
				col.rgb = s.Albedo;
				col.a = s.Alpha;

				return col;

			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_XTex, IN.uv_OutlineTex) * _XColor;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}

			ENDCG
	}
}
