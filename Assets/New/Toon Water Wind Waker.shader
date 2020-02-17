// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Toon Water 1"
{
	Properties
	{
		_WaveGuide("Wave Guide", 2D) = "white" {}
		_WaveSpeed("Wave Speed", Range( 0 , 5)) = 0.4512808
		_FoamColour("Foam Colour", Color) = (0.5518868,0.9261317,1,1)
		_WaterColour("Water Colour", Color) = (0,0.6295598,0.7264151,1)
		_WaveHeight("Wave Height", Range( 0 , 5)) = 1.001184
		_Zoom("Zoom", Float) = 1.21
		_WaterTexture("Water Texture", 2D) = "white" {}
		_DistanceFader("Distance Fader", Range( 0 , 1)) = 0.1
		_Opacity("Opacity", Range( 0 , 1)) = 0.8
		_ScrollSpeed("Scroll Speed", Float) = 0.06
		_Texture1("Texture 1", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _WaveGuide;
		uniform float _WaveSpeed;
		uniform float _WaveHeight;
		uniform float4 _WaterColour;
		uniform float4 _FoamColour;
		uniform sampler2D _WaterTexture;
		uniform float _ScrollSpeed;
		uniform float _Zoom;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DistanceFader;
		uniform sampler2D _TextureSample0;
		uniform sampler2D _Texture1;
		uniform float _Opacity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 speed40 = ( _Time * _WaveSpeed );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float2 uv_TexCoord26 = v.texcoord.xy + ( speed40 + (ase_vertex3Pos).y ).xy;
			float3 ase_vertexNormal = v.normal.xyz;
			float3 VertexAnimation33 = ( ( tex2Dlod( _WaveGuide, float4( uv_TexCoord26, 0, 1.0) ).r - 0.5 ) * ( ase_vertexNormal * _WaveHeight ) );
			v.vertex.xyz += VertexAnimation33;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 appendResult15 = (float2(0.0 , _ScrollSpeed));
			float2 panner13 = ( 1.0 * _Time.y * appendResult15 + ( i.uv_texcoord * _Zoom ));
			float4 lerpResult72 = lerp( _WaterColour , _FoamColour , tex2D( _WaterTexture, panner13 ));
			o.Albedo = lerpResult72.rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth151 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos )));
			float distanceDepth151 = abs( ( screenDepth151 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DistanceFader ) );
			float clampResult153 = clamp( ( 1.0 - distanceDepth151 ) , 0.0 , 1.0 );
			float2 temp_output_1_0_g2 = float2( 1,1 );
			float2 appendResult10_g2 = (float2(( (temp_output_1_0_g2).x * i.uv_texcoord.x ) , ( i.uv_texcoord.y * (temp_output_1_0_g2).y )));
			float2 temp_output_11_0_g2 = float2( 0,0 );
			float2 panner18_g2 = ( ( (temp_output_11_0_g2).x * _Time.y ) * float2( 1,0 ) + i.uv_texcoord);
			float2 panner19_g2 = ( ( _Time.y * (temp_output_11_0_g2).y ) * float2( 0,1 ) + i.uv_texcoord);
			float2 appendResult24_g2 = (float2((panner18_g2).x , (panner19_g2).y));
			float mulTime162 = _Time.y * 0.11;
			float2 temp_cast_1 = (sin( ( 0.001 * mulTime162 ) )).xx;
			float2 temp_output_47_0_g2 = temp_cast_1;
			float2 uv_TexCoord78_g2 = i.uv_texcoord * float2( 2,2 );
			float2 temp_output_31_0_g2 = ( uv_TexCoord78_g2 - float2( 1,1 ) );
			float2 appendResult39_g2 = (float2(frac( ( atan2( (temp_output_31_0_g2).x , (temp_output_31_0_g2).y ) / 6.28318548202515 ) ) , length( temp_output_31_0_g2 )));
			float2 panner54_g2 = ( ( (temp_output_47_0_g2).x * _Time.y ) * float2( 1,0 ) + appendResult39_g2);
			float2 panner55_g2 = ( ( _Time.y * (temp_output_47_0_g2).y ) * float2( 0,1 ) + appendResult39_g2);
			float2 appendResult58_g2 = (float2((panner54_g2).x , (panner55_g2).y));
			float4 Emission118 = ( clampResult153 * tex2D( _TextureSample0, ( ( (tex2D( _Texture1, ( appendResult10_g2 + appendResult24_g2 ) )).rg * 1.0 ) + ( float2( 1,1 ) * appendResult58_g2 ) ) ) );
			o.Emission = Emission118.rgb;
			o.Alpha = _Opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16800
218;413;1174;646;2026.003;-1672.287;1.6;True;False
Node;AmplifyShaderEditor.CommentaryNode;36;-1231.796,1346.019;Float;False;914.394;362.5317;Comment;4;40;39;38;37;Wave Speed;1,1,1,1;0;0
Node;AmplifyShaderEditor.TimeNode;38;-1110.899,1396.02;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;-1181.796,1593.552;Float;False;Property;_WaveSpeed;Wave Speed;1;0;Create;True;0;0;False;0;0.4512808;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-781.8617,1531.041;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;52;-1230.74,771.2521;Float;False;2307.46;426.9862;Comment;12;34;23;24;25;26;29;28;27;30;31;32;33;Vertex Animation;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;34;-1180.74,920.7151;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;164;-1474.699,3124.365;Float;False;Constant;_Float4;Float 4;13;0;Create;True;0;0;False;0;0.11;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-560.4017,1438.695;Float;False;speed;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;24;-941.9764,939.9871;Float;False;False;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;23;-857.1194,860.4338;Float;False;40;speed;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleTimeNode;162;-1289.707,3163.388;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;160;-1270.912,3057.838;Float;False;Constant;_Float3;Float 3;13;0;Create;True;0;0;False;0;0.001;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;150;-710.3673,2305.523;Float;False;Property;_DistanceFader;Distance Fader;9;0;Create;True;0;0;False;0;0.1;0.194;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-656.2974,857.1514;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;163;-1111.342,3110.227;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;161;-962.168,3060.924;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;1;-1036.72,-340.6348;Float;False;2032.086;1020.818;;10;19;18;15;14;13;12;10;5;2;72;GOOD WATER;1,1,1,1;0;0
Node;AmplifyShaderEditor.DepthFade;151;-467.9389,2159.01;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;159;-1176.554,2843.588;Float;True;Property;_Texture1;Texture 1;13;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-467.6862,821.2521;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;152;-227.5114,2118.664;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;27;-188.2233,827.2737;Float;True;Property;_WaveGuide;Wave Guide;0;0;Create;True;0;0;False;0;None;9e0295241b4e8ec4d889c1394b856600;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;158;-798.0227,2849.621;Float;True;RadialUVDistortion;-1;;2;051d65e7699b41a4c800363fd0e822b2;0;7;60;SAMPLER2D;0.0;False;1;FLOAT2;1,1;False;11;FLOAT2;0,0;False;65;FLOAT;1;False;68;FLOAT2;1,1;False;47;FLOAT2;1,1;False;29;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-950.3464,495.7102;Float;False;Property;_ScrollSpeed;Scroll Speed;12;0;Create;True;0;0;False;0;0.06;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-969.2521,377.8921;Float;False;Property;_Zoom;Zoom;6;0;Create;True;0;0;False;0;1.21;7.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-986.7204,239.569;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;28;166.9988,927.4282;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;122.6827,1083.238;Float;False;Property;_WaveHeight;Wave Height;4;0;Create;True;0;0;False;0;1.001184;1.001184;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;31;499.3577,854.8445;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;494.0527,1008.51;Float;False;2;2;0;FLOAT3;1,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;149;-326.9527,2667.108;Float;True;Property;_TextureSample0;Texture Sample 0;14;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-656.1543,344.8202;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;-651.0012,468.6285;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;153;-60.42639,2008.859;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;13;-437.3851,386.9385;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;154;71.5889,2323.73;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;661.3619,987.3748;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;12;37.29755,-32.19386;Float;False;Property;_FoamColour;Foam Colour;2;0;Create;True;0;0;False;0;0.5518868,0.9261317,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-195.3851,238.6559;Float;True;Property;_WaterTexture;Water Texture;7;0;Create;True;0;0;False;0;None;4a9389be908bcae469e71a725a6d06f3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;132.2272,375.8846;Float;False;Property;_WaterColour;Water Colour;3;0;Create;True;0;0;False;0;0,0.6295598,0.7264151,1;0,0.8702703,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;41;-188.9101,1325.511;Float;False;1244.412;443.4576;Comment;9;50;49;48;47;46;45;44;43;42;Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;814.7205,979.3967;Float;False;VertexAnimation;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;118;318.7827,2352.238;Float;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;70;1151.197,139.761;Float;False;-1;;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;104;1148.571,400.7514;Float;False;Property;_Opacity;Opacity;10;0;Create;True;0;0;False;0;0.8;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;47;462.5849,1670.342;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;51;1211.792,301.3152;Float;False;118;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DdxOpNode;43;30.58491,1574.342;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;35;1142.386,504.1846;Float;False;33;VertexAnimation;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;326.5889,1640.591;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;1E-09,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;72;364.7105,180.6408;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CrossProductOpNode;45;158.5849,1606.342;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;148;-1281.009,2292.702;Float;True;Property;_Texture0;Texture 0;11;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.DdyOpNode;44;30.58491,1670.342;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;147;-984.9291,2459.834;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;50;812.5017,1588.739;Float;False;Normal;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;146;-727.5624,2456.906;Float;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;42;-177.4151,1590.342;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;48;185.2049,1375.511;Float;True;Property;_NormalOnlyNoPolyMode;Normal (Only No Poly Mode);8;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;49;609.7289,1577.734;Float;False;Property;_LowPoly;Low Poly;5;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;155;-701.6239,2702.385;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;156;-999.1762,2745.991;Float;False;Constant;_Float0;Float 0;12;0;Create;True;0;0;False;0;0.47;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;157;-1278.059,2642.264;Float;False;Constant;_Float1;Float 1;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1468.487,158.4648;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Toon Water 1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;39;0;38;0
WireConnection;39;1;37;0
WireConnection;40;0;39;0
WireConnection;24;0;34;0
WireConnection;162;0;164;0
WireConnection;25;0;23;0
WireConnection;25;1;24;0
WireConnection;163;0;160;0
WireConnection;163;1;162;0
WireConnection;161;0;163;0
WireConnection;151;0;150;0
WireConnection;26;1;25;0
WireConnection;152;0;151;0
WireConnection;27;1;26;0
WireConnection;158;60;159;0
WireConnection;158;47;161;0
WireConnection;31;0;27;1
WireConnection;30;0;28;0
WireConnection;30;1;29;0
WireConnection;149;1;158;0
WireConnection;19;0;14;0
WireConnection;19;1;5;0
WireConnection;15;1;18;0
WireConnection;153;0;152;0
WireConnection;13;0;19;0
WireConnection;13;2;15;0
WireConnection;154;0;153;0
WireConnection;154;1;149;0
WireConnection;32;0;31;0
WireConnection;32;1;30;0
WireConnection;10;1;13;0
WireConnection;33;0;32;0
WireConnection;118;0;154;0
WireConnection;47;0;46;0
WireConnection;43;0;42;0
WireConnection;46;0;45;0
WireConnection;72;0;2;0
WireConnection;72;1;12;0
WireConnection;72;2;10;0
WireConnection;45;0;43;0
WireConnection;45;1;44;0
WireConnection;44;0;42;0
WireConnection;147;2;148;0
WireConnection;50;0;49;0
WireConnection;49;0;48;0
WireConnection;49;1;47;0
WireConnection;155;0;147;0
WireConnection;0;0;72;0
WireConnection;0;2;51;0
WireConnection;0;9;104;0
WireConnection;0;11;35;0
ASEEND*/
//CHKSM=A2A4DDE5B1ABE9A9EC0D15796943CD4199C26D74