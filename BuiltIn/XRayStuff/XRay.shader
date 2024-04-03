// Made with Amplify Shader Editor v1.9.2.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "XRay"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		_DecalColorMap("_DecalColorMap", 2D) = "black" {}
		[Toggle(_SKINNED_ON)] _Skinned("Skinned", Float) = 0
		_DistanceFade("DistanceFade", Range( 0 , 20)) = 15
		_BaseColor("BaseColor", Color) = (0,0.9407032,1,0.5019608)
		[Toggle(_PENETRATION_DEFORMATION_DETAIL_ON)] _PENETRATION_DEFORMATION_DETAIL("_PENETRATION_DEFORMATION_DETAIL", Float) = 0
		[Toggle(_PENETRATION_DEFORMATION_ON)] _PENETRATION_DEFORMATION("_PENETRATION_DEFORMATION", Float) = 0
		[HideInInspector]_WorldDickPosition("WorldDickPosition", Vector) = (0,0,0,0)
		[HideInInspector]_WorldDickNormal("WorldDickNormal", Vector) = (0,1,0,0)
		[HideInInspector]_WorldDickBinormal("WorldDickBinormal", Vector) = (0,0,1,0)
		[Toggle(_COCKVORESQUISHENABLED_ON)] _CockVoreSquishEnabled("CockVoreSquishEnabled", Float) = 0
		[HideInInspector]_Angle("Angle", Range( 0 , 89)) = 45
		[HideInInspector]_TipRadius("TipRadius", Range( 0 , 1)) = 0.1
		_CompressibleDistance("CompressibleDistance", Range( 0 , 1)) = 0.15
		_Smoothness("Smoothness", Range( 0 , 10)) = 1

		[HideInInspector] _RenderQueueType("Render Queue Type", Float) = 5
		[HideInInspector][ToggleUI] _AddPrecomputedVelocity("Add Precomputed Velocity", Float) = 1
		//[HideInInspector] _ShadowMatteFilter("Shadow Matte Filter", Float) = 2.006836
		[HideInInspector] _StencilRef("Stencil Ref", Int) = 0 // StencilUsage.Clear
		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
		[HideInInspector] _StencilRefDepth("Stencil Ref Depth", Int) = 0 // Nothing
		[HideInInspector] _StencilWriteMaskDepth("Stencil Write Mask Depth", Int) = 8 // StencilUsage.TraceReflectionRay
		[HideInInspector] _StencilRefMV("Stencil Ref MV", Int) = 32 // StencilUsage.ObjectMotionVector
		[HideInInspector] _StencilWriteMaskMV("Stencil Write Mask MV", Int) = 32 // StencilUsage.ObjectMotionVector
		[HideInInspector] _StencilRefDistortionVec("Stencil Ref Distortion Vec", Int) = 2 // StencilUsage.DistortionVectors
		[HideInInspector] _StencilWriteMaskDistortionVec("Stencil Write Mask Distortion Vec", Int) = 2 // StencilUsage.DistortionVectors
		[HideInInspector] _StencilWriteMaskGBuffer("Stencil Write Mask GBuffer", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
		[HideInInspector] _StencilRefGBuffer("Stencil Ref GBuffer", Int) = 2 // StencilUsage.RequiresDeferredLighting
		[HideInInspector] _ZTestGBuffer("ZTest GBuffer", Int) = 4
		[HideInInspector][ToggleUI] _RequireSplitLighting("Require Split Lighting", Float) = 0
		[HideInInspector][ToggleUI] _ReceivesSSR("Receives SSR", Float) = 1
		[HideInInspector] _SurfaceType("Surface Type", Float) = 0
		[HideInInspector] _BlendMode("Blend Mode", Float) = 0
		[HideInInspector] _SrcBlend("Src Blend", Float) = 1
		[HideInInspector] _DstBlend("Dst Blend", Float) = 0
		[HideInInspector] _AlphaSrcBlend("Alpha Src Blend", Float) = 1
		[HideInInspector] _AlphaDstBlend("Alpha Dst Blend", Float) = 0
		[HideInInspector][ToggleUI] _ZWrite("ZWrite", Float) = 1
		[HideInInspector][ToggleUI] _TransparentZWrite("Transparent ZWrite", Float) = 0
		[HideInInspector] _CullMode("Cull Mode", Float) = 2
		[HideInInspector] _TransparentSortPriority("Transparent Sort Priority", Float) = 0
		[HideInInspector][ToggleUI] _EnableFogOnTransparent("Enable Fog", Float) = 1
		[HideInInspector] _CullModeForward("Cull Mode Forward", Float) = 2 // This mode is dedicated to Forward to correctly handle backface then front face rendering thin transparent
		[HideInInspector][Enum(UnityEditor.Rendering.HighDefinition.TransparentCullMode)] _TransparentCullMode("Transparent Cull Mode", Int) = 2 // Back culling by default
		[HideInInspector] _ZTestDepthEqualForOpaque("ZTest Depth Equal For Opaque", Int) = 4 // Less equal
		[HideInInspector][Enum(UnityEngine.Rendering.CompareFunction)] _ZTestTransparent("ZTest Transparent", Int) = 4 // Less equal
		[HideInInspector][ToggleUI] _TransparentBackfaceEnable("Transparent Backface Enable", Float) = 0
		[HideInInspector][ToggleUI] _AlphaCutoffEnable("Alpha Cutoff Enable", Float) = 0
		[HideInInspector][ToggleUI] _UseShadowThreshold("Use Shadow Threshold", Float) = 0
		[HideInInspector][ToggleUI] _DoubleSidedEnable("Double Sided Enable", Float) = 0
		[HideInInspector][Enum(Flip, 0, Mirror, 1, None, 2)] _DoubleSidedNormalMode("Double Sided Normal Mode", Float) = 2
		[HideInInspector] _DoubleSidedConstants("DoubleSidedConstants", Vector) = (1,1,-1,0)
		[HideInInspector] _DistortionEnable("_DistortionEnable",Float) = 0
		[HideInInspector] _DistortionOnly("_DistortionOnly",Float) = 0

		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25

		[HideInInspector][ToggleUI] _TransparentWritingMotionVec("Transparent Writing MotionVec", Float) = 0
		[HideInInspector][Enum(UnityEditor.Rendering.HighDefinition.OpaqueCullMode)] _OpaqueCullMode("Opaque Cull Mode", Int) = 2 // Back culling by default
		[HideInInspector][ToggleUI] _SupportDecals("Support Decals", Float) = 1
		[HideInInspector][ToggleUI] _ReceivesSSRTransparent("Receives SSR Transparent", Float) = 0
		[HideInInspector] _EmissionColor("Color", Color) = (1, 1, 1)
		[HideInInspector] _UnlitColorMap_MipInfo("_UnlitColorMap_MipInfo", Vector) = (0, 0, 0, 0)

		[HideInInspector][Enum(Auto, 0, On, 1, Off, 2)] _DoubleSidedGIMode("Double sided GI mode", Float) = 0 //DoubleSidedGIMode added in api 12x and higher
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="HDRenderPipeline" "RenderType"="Opaque" "Queue"="Geometry" }

		HLSLINCLUDE
		#pragma target 4.5
		#pragma exclude_renderers glcore gles gles3 ps4 ps5 

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}

		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlaneASE (float3 pos, float4 plane)
		{
			return dot (float4(pos,1.0f), plane);
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlaneASE(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlaneASE(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlaneASE(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlaneASE(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			
			Name "Forward Unlit"
			Tags { "LightMode"="ForwardOnly" }

			Blend [_SrcBlend] [_DstBlend], [_AlphaSrcBlend] [_AlphaDstBlend]

			Cull [_CullModeForward]
			ZTest [_ZTestDepthEqualForOpaque]
			ZWrite [_ZWrite]

			ColorMask [_ColorMaskTransparentVel] 1

			Stencil
			{
				Ref [_StencilRef]
				WriteMask [_StencilWriteMask]
				Comp Always
				Pass Replace
			}


			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 140008


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile _ DEBUG_DISPLAY

			#pragma vertex Vert
			#pragma fragment Frag

	        #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
	        #define _WRITE_TRANSPARENT_MOTION_VECTOR
	        #endif

			#define SHADERPASS SHADERPASS_FORWARD_UNLIT
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			#if defined(_ENABLE_SHADOW_MATTE) && SHADERPASS == SHADERPASS_FORWARD_UNLIT
				#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
				#define HAS_LIGHTLOOP
				#define SHADOW_OPTIMIZE_REGISTER_USAGE 1

				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonLighting.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Shadow/HDShadowContext.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/HDShadow.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/PunctualLightCommon.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/HDShadowLoop.hlsl"
			#endif

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_RELATIVE_WORLD_POS
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#pragma shader_feature_local _PENETRATION_DEFORMATION_DETAIL_ON
			#pragma multi_compile_local __ _PENETRATION_DEFORMATION_ON
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#pragma shader_feature_local _SKINNED_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START( UnityPerMaterial )
			float4 _BaseColor;
			float3 _WorldDickNormal;
			float3 _WorldDickBinormal;
			float3 _WorldDickPosition;
			float _TipRadius;
			float _Angle;
			float _CompressibleDistance;
			float _Smoothness;
			float _DistanceFade;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _DecalColorMap;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			float4x4 Inverse4x4(float4x4 input)
			{
				#define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
				float4x4 cofactors = float4x4(
				minor( _22_23_24, _32_33_34, _42_43_44 ),
				-minor( _21_23_24, _31_33_34, _41_43_44 ),
				minor( _21_22_24, _31_32_34, _41_42_44 ),
				-minor( _21_22_23, _31_32_33, _41_42_43 ),
			
				-minor( _12_13_14, _32_33_34, _42_43_44 ),
				minor( _11_13_14, _31_33_34, _41_43_44 ),
				-minor( _11_12_14, _31_32_34, _41_42_44 ),
				minor( _11_12_13, _31_32_33, _41_42_43 ),
			
				minor( _12_13_14, _22_23_24, _42_43_44 ),
				-minor( _11_13_14, _21_23_24, _41_43_44 ),
				minor( _11_12_14, _21_22_24, _41_42_44 ),
				-minor( _11_12_13, _21_22_23, _41_42_43 ),
			
				-minor( _12_13_14, _22_23_24, _32_33_34 ),
				minor( _11_13_14, _21_23_24, _31_33_34 ),
				-minor( _11_12_14, _21_22_24, _31_32_34 ),
				minor( _11_12_13, _21_22_23, _31_32_33 ));
				#undef minor
				return transpose( cofactors ) / determinant( input );
			}
			
			float3 MyCustomExpression32_g274( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			
			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			

			struct SurfaceDescription
			{
				float3 Color;
				float3 Emission;
				float4 ShadowTint;
				float Alpha;
				float AlphaClipThreshold;
				float4 VTPackedFeedback;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
				surfaceData.color = surfaceDescription.Color;
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription , FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#if _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);

				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif

				#if defined(_ENABLE_SHADOW_MATTE) && SHADERPASS == SHADERPASS_FORWARD_UNLIT
					HDShadowContext shadowContext = InitShadowContext();
					float shadow;
					float3 shadow3;
					posInput = GetPositionInput(fragInputs.positionSS.xy, _ScreenSize.zw, fragInputs.positionSS.z, UNITY_MATRIX_I_VP, UNITY_MATRIX_V);
					float3 normalWS = normalize(fragInputs.tangentToWorld[1]);
					uint renderingLayers = _EnableLightLayers ? asuint(unity_RenderingLayer.x) : DEFAULT_LIGHT_LAYERS;
					ShadowLoopMin(shadowContext, posInput, normalWS, asuint(_ShadowMatteFilter), renderingLayers, shadow3);
					shadow = dot(shadow3, float3(1.0f/3.0f, 1.0f/3.0f, 1.0f/3.0f));

					float4 shadowColor = (1 - shadow)*surfaceDescription.ShadowTint.rgba;
					float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);

					#ifdef _SURFACE_TYPE_TRANSPARENT
						surfaceData.color = lerp(shadowColor.rgb*surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow), surfaceDescription.Alpha);
					#else
						surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow);
					#endif
					localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
					surfaceDescription.Alpha = localAlpha;
				#endif

				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity = surfaceDescription.Alpha;

				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				#ifdef UNITY_VIRTUAL_TEXTURING
                builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif

				#if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif

                ApplyDebugToBuiltinData(builtinData);
			}

			float GetDeExposureMultiplier()
			{
			#if defined(DISABLE_UNLIT_DEEXPOSURE)
				return 1.0;
			#else
				return _DeExposureMultiplier;
			#endif
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 normalizeResult27_g276 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g276 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g276 = normalize( cross( normalizeResult27_g276 , normalizeResult31_g276 ) );
				float4 appendResult26_g275 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g275 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g275 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g275 = -_WorldDickPosition;
				float4 appendResult29_g275 = (float4(break27_g275.x , break27_g275.y , break27_g275.z , 1.0));
				float4x4 temp_output_30_0_g275 = mul( transpose( float4x4( float4( normalizeResult27_g276 , 0.0 ).x,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).x,float4( normalizeResult29_g276 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g276 , 0.0 ).y,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).y,float4( normalizeResult29_g276 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g276 , 0.0 ).z,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).z,float4( normalizeResult29_g276 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g276 , 0.0 ).w,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).w,float4( normalizeResult29_g276 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g275.x,appendResult28_g275.x,appendResult31_g275.x,appendResult29_g275.x,appendResult26_g275.y,appendResult28_g275.y,appendResult31_g275.y,appendResult29_g275.y,appendResult26_g275.z,appendResult28_g275.z,appendResult31_g275.z,appendResult29_g275.z,appendResult26_g275.w,appendResult28_g275.w,appendResult31_g275.w,appendResult29_g275.w ) );
				float4x4 invertVal44_g275 = Inverse4x4( temp_output_30_0_g275 );
				float4 appendResult27_g274 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g274 = mul( GetObjectToWorldMatrix(), appendResult27_g274 ).xyz;
				float3 localMyCustomExpression32_g274 = MyCustomExpression32_g274( pos32_g274 );
				float4 appendResult32_g275 = (float4(localMyCustomExpression32_g274 , 1.0));
				float4 break35_g275 = mul( temp_output_30_0_g275, appendResult32_g275 );
				float temp_output_124_0_g275 = _TipRadius;
				float2 appendResult36_g275 = (float2(break35_g275.y , break35_g275.z));
				float2 normalizeResult41_g275 = normalize( appendResult36_g275 );
				float temp_output_120_0_g275 = sqrt( max( break35_g275.x , 0.0 ) );
				float temp_output_48_0_g275 = tan( radians( _Angle ) );
				float temp_output_125_0_g275 = ( temp_output_124_0_g275 + ( temp_output_120_0_g275 * temp_output_48_0_g275 ) );
				float temp_output_37_0_g275 = length( appendResult36_g275 );
				float temp_output_114_0_g275 = ( ( temp_output_125_0_g275 - temp_output_37_0_g275 ) + 1.0 );
				float lerpResult102_g275 = lerp( temp_output_125_0_g275 , temp_output_37_0_g275 , saturate( temp_output_114_0_g275 ));
				float lerpResult130_g275 = lerp( 0.0 , lerpResult102_g275 , saturate( ( -( -temp_output_124_0_g275 - break35_g275.x ) / temp_output_124_0_g275 ) ));
				float2 break43_g275 = ( normalizeResult41_g275 * lerpResult130_g275 );
				float4 appendResult40_g275 = (float4(max( break35_g275.x , -temp_output_124_0_g275 ) , break43_g275.x , break43_g275.y , 1.0));
				float4 appendResult28_g274 = (float4(((mul( invertVal44_g275, appendResult40_g275 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g274 = appendResult28_g274;
				(localWorldVar29_g274).xyz = GetCameraRelativePositionWS((localWorldVar29_g274).xyz);
				float4 transform29_g274 = mul(GetWorldToObjectMatrix(),localWorldVar29_g274);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g274 = (transform29_g274).xyz;
				#else
				float3 staticSwitch13_g274 = inputMesh.positionOS;
				#endif
				float3 temp_output_229_0 = staticSwitch13_g274;
				float3 temp_output_10_0_g277 = temp_output_229_0;
				float localGetDeformationFromPenetrators_float8_g277 = ( 0.0 );
				float4 appendResult17_g277 = (float4(temp_output_10_0_g277 , 1.0));
				float4 transform16_g277 = mul(GetObjectToWorldMatrix(),appendResult17_g277);
				transform16_g277.xyz = GetAbsolutePositionWS((transform16_g277).xyz);
				float3 worldPosition8_g277 = (transform16_g277).xyz;
				float4 texCoord3_g277 = inputMesh.ase_texcoord2;
				texCoord3_g277.xy = inputMesh.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float4 uv28_g277 = texCoord3_g277;
				float compressibleDistance8_g277 = _CompressibleDistance;
				float smoothness8_g277 = _Smoothness;
				float3 deformedPosition8_g277 = float3( 0,0,0 );
				{
				GetDeformationFromPenetrators_float(worldPosition8_g277,uv28_g277,compressibleDistance8_g277,smoothness8_g277,deformedPosition8_g277);
				}
				float4 appendResult21_g277 = (float4(deformedPosition8_g277 , 1.0));
				float4 localWorldVar19_g277 = appendResult21_g277;
				(localWorldVar19_g277).xyz = GetCameraRelativePositionWS((localWorldVar19_g277).xyz);
				float4 transform19_g277 = mul(GetWorldToObjectMatrix(),localWorldVar19_g277);
				#ifdef _PENETRATION_DEFORMATION_ON
				float3 staticSwitch24_g277 = (transform19_g277).xyz;
				#else
				float3 staticSwitch24_g277 = temp_output_10_0_g277;
				#endif
				float3 lerpResult275 = lerp( temp_output_229_0 , staticSwitch24_g277 , inputMesh.ase_color.r);
				
				float3 temp_output_50_0_g274 = inputMesh.normalOS;
				float2 break146_g275 = normalizeResult41_g275;
				float4 appendResult139_g275 = (float4(temp_output_48_0_g275 , break146_g275.x , break146_g275.y , 0.0));
				float3 normalizeResult144_g275 = normalize( (mul( invertVal44_g275, appendResult139_g275 )).xyz );
				float3 lerpResult44_g274 = lerp( normalizeResult144_g275 , temp_output_50_0_g274 , saturate( sign( temp_output_114_0_g275 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g274 = lerpResult44_g274;
				#else
				float3 staticSwitch17_g274 = temp_output_50_0_g274;
				#endif
				
				float3 vertexToFrag28_g270 = mul( UNITY_MATRIX_M, float4( inputMesh.positionOS , 0.0 ) ).xyz;
				o.ase_texcoord2.xyz = vertexToFrag28_g270;
				
				float3 vertexPos11_g269 = inputMesh.positionOS;
				float4 ase_clipPos11_g269 = TransformWorldToHClip( TransformObjectToWorld(vertexPos11_g269));
				float4 screenPos11_g269 = ComputeScreenPos( ase_clipPos11_g269 , _ProjectionParams.x );
				o.ase_texcoord3 = screenPos11_g269;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				o.ase_texcoord4.xyz = ase_worldNormal;
				
				o.ase_texcoord1.xy = inputMesh.ase_texcoord1.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				o.ase_texcoord2.w = 0;
				o.ase_texcoord4.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = lerpResult275;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = staticSwitch17_g274;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				o.positionRWS = positionRWS;
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord2 = v.ase_texcoord2;
				o.ase_color = v.ase_color;
				o.ase_texcoord1 = v.ase_texcoord1;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord2 = patch[0].ase_texcoord2 * bary.x + patch[1].ase_texcoord2 * bary.y + patch[2].ase_texcoord2 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#ifdef UNITY_VIRTUAL_TEXTURING
			#define VT_BUFFER_TARGET SV_Target1
			#define EXTRA_BUFFER_TARGET SV_Target2
			#else
			#define EXTRA_BUFFER_TARGET SV_Target1
			#endif

			void Frag( VertexOutput packedInput,
						out float4 outColor : SV_Target0
						#ifdef UNITY_VIRTUAL_TEXTURING
						,out float4 outVTFeedback : VT_BUFFER_TARGET
						#endif
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				float3 positionRWS = packedInput.positionRWS;

				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir( input.positionRWS );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float4 temp_output_1_0_g270 = _BaseColor;
				float4 color20_g270 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
				float3 vertexToFrag28_g270 = packedInput.ase_texcoord2.xyz;
				#ifdef _SKINNED_ON
				float3 staticSwitch26_g270 = float3( packedInput.ase_texcoord1.xy ,  0.0 );
				#else
				float3 staticSwitch26_g270 = vertexToFrag28_g270;
				#endif
				float simplePerlin3D9_g270 = snoise( ( ( staticSwitch26_g270 * float3( 2,2,2 ) ) + float3( 0,0,0 ) ) );
				simplePerlin3D9_g270 = simplePerlin3D9_g270*0.5 + 0.5;
				float temp_output_16_0_g270 = ( ( simplePerlin3D9_g270 - 0.5 ) * 0.5 );
				float temp_output_12_0_g270 = ( tex2D( _DecalColorMap, packedInput.ase_texcoord1.xy ).r + temp_output_16_0_g270 );
				float lerpResult22_g270 = lerp( ( temp_output_12_0_g270 + temp_output_16_0_g270 ) , 0.7 , 0.8);
				float4 lerpResult19_g270 = lerp( temp_output_1_0_g270 , color20_g270 , ( 1.0 - lerpResult22_g270 ));
				float temp_output_3_0_g271 = ( 0.5 - temp_output_12_0_g270 );
				float temp_output_10_0_g270 = ( 1.0 - saturate( ( temp_output_3_0_g271 / fwidth( temp_output_3_0_g271 ) ) ) );
				float4 lerpResult18_g270 = lerp( temp_output_1_0_g270 , lerpResult19_g270 , temp_output_10_0_g270);
				float4 temp_output_276_18 = lerpResult18_g270;
				
				float3 ase_worldPos = GetAbsolutePositionWS( positionRWS );
				float4 screenPos11_g269 = packedInput.ase_texcoord3;
				float4 ase_screenPosNorm11 = screenPos11_g269 / screenPos11_g269.w;
				ase_screenPosNorm11.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm11.z : ase_screenPosNorm11.z * 0.5 + 0.5;
				float screenDepth11_g269 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm11.xy ),_ZBufferParams);
				float distanceDepth11_g269 = saturate( abs( ( screenDepth11_g269 - LinearEyeDepth( ase_screenPosNorm11.z,_ZBufferParams ) ) / ( 2.0 ) ) );
				float3 ase_worldNormal = packedInput.ase_texcoord4.xyz;
				float dotResult3_g269 = dot( V , ase_worldNormal );
				
				surfaceDescription.Color = temp_output_276_18.rgb;
				surfaceDescription.Emission = temp_output_276_18.rgb;
				surfaceDescription.Alpha = ( _BaseColor.a * ( 1.0 - saturate( ( distance( _WorldSpaceCameraPos , ase_worldPos ) / _DistanceFade ) ) ) * saturate( distanceDepth11_g269 ) * saturate( dotResult3_g269 ) );
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				surfaceDescription.ShadowTint = float4( 0, 0 ,0 ,1 );
				float2 Distortion = float2 ( 0, 0 );
				float DistortionBlur = 0;

				surfaceDescription.VTPackedFeedback = float4(1.0f,1.0f,1.0f,1.0f);
				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData( input.positionSS.xy, surfaceData );

				#if defined(_ENABLE_SHADOW_MATTE)
				bsdfData.color *= GetScreenSpaceAmbientOcclusion(input.positionSS.xy);
				#endif


			#ifdef DEBUG_DISPLAY
				if (_DebugLightingMode >= DEBUGLIGHTINGMODE_DIFFUSE_LIGHTING && _DebugLightingMode <= DEBUGLIGHTINGMODE_EMISSIVE_LIGHTING)
				{
					if (_DebugLightingMode != DEBUGLIGHTINGMODE_EMISSIVE_LIGHTING)
					{
						builtinData.emissiveColor = 0.0;
					}
					else
					{
						bsdfData.color = 0.0;
					}
				}
			#endif

				float4 outResult = ApplyBlendMode(bsdfData.color * GetDeExposureMultiplier() + builtinData.emissiveColor * GetCurrentExposureMultiplier(), builtinData.opacity);
				outResult = EvaluateAtmosphericScattering(posInput, V, outResult);

				#ifdef DEBUG_DISPLAY
					int bufferSize = int(_DebugViewMaterialArray[0].x);
					for (int index = 1; index <= bufferSize; index++)
					{
						int indexMaterialProperty = int(_DebugViewMaterialArray[index].x);
						if (indexMaterialProperty != 0)
						{
							float3 result = float3(1.0, 0.0, 1.0);
							bool needLinearToSRGB = false;

							GetPropertiesDataDebug(indexMaterialProperty, result, needLinearToSRGB);
							GetVaryingsDataDebug(indexMaterialProperty, input, result, needLinearToSRGB);
							GetBuiltinDataDebug(indexMaterialProperty, builtinData, posInput, result, needLinearToSRGB);
							GetSurfaceDataDebug(indexMaterialProperty, surfaceData, result, needLinearToSRGB);
							GetBSDFDataDebug(indexMaterialProperty, bsdfData, result, needLinearToSRGB);

							if (!needLinearToSRGB)
								result = SRGBToLinear(max(0, result));

							outResult = float4(result, 1.0);
						}
					}

					if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_TRANSPARENCY_OVERDRAW)
					{
						float4 result = _DebugTransparencyOverdrawWeight * float4(TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_A);
						outResult = result;
					}
				#endif

				outColor = outResult;

				#ifdef _DEPTHOFFSET_ON
					outputDepth = posInput.deviceDepth;
				#endif

				#ifdef UNITY_VIRTUAL_TEXTURING
					outVTFeedback = builtinData.vtPackedFeedback;
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			Cull [_CullMode]
			ZWrite On
			ZClip [_ZClip]
			ColorMask 0

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 140008


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma vertex Vert
			#pragma fragment Frag

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			#define SHADERPASS SHADERPASS_SHADOWS
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _PENETRATION_DEFORMATION_DETAIL_ON
			#pragma multi_compile_local __ _PENETRATION_DEFORMATION_ON
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START( UnityPerMaterial )
			float4 _BaseColor;
			float3 _WorldDickNormal;
			float3 _WorldDickBinormal;
			float3 _WorldDickPosition;
			float _TipRadius;
			float _Angle;
			float _CompressibleDistance;
			float _Smoothness;
			float _DistanceFade;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			float4x4 Inverse4x4(float4x4 input)
			{
				#define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
				float4x4 cofactors = float4x4(
				minor( _22_23_24, _32_33_34, _42_43_44 ),
				-minor( _21_23_24, _31_33_34, _41_43_44 ),
				minor( _21_22_24, _31_32_34, _41_42_44 ),
				-minor( _21_22_23, _31_32_33, _41_42_43 ),
			
				-minor( _12_13_14, _32_33_34, _42_43_44 ),
				minor( _11_13_14, _31_33_34, _41_43_44 ),
				-minor( _11_12_14, _31_32_34, _41_42_44 ),
				minor( _11_12_13, _31_32_33, _41_42_43 ),
			
				minor( _12_13_14, _22_23_24, _42_43_44 ),
				-minor( _11_13_14, _21_23_24, _41_43_44 ),
				minor( _11_12_14, _21_22_24, _41_42_44 ),
				-minor( _11_12_13, _21_22_23, _41_42_43 ),
			
				-minor( _12_13_14, _22_23_24, _32_33_34 ),
				minor( _11_13_14, _21_23_24, _31_33_34 ),
				-minor( _11_12_14, _21_22_24, _31_32_34 ),
				minor( _11_12_13, _21_22_23, _31_32_33 ));
				#undef minor
				return transpose( cofactors ) / determinant( input );
			}
			
			float3 MyCustomExpression32_g274( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
				#endif

				#if _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);

				ZERO_INITIALIZE (BuiltinData, builtinData);
				builtinData.opacity = surfaceDescription.Alpha;

				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

				#ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif

                ApplyDebugToBuiltinData(builtinData);
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 normalizeResult27_g276 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g276 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g276 = normalize( cross( normalizeResult27_g276 , normalizeResult31_g276 ) );
				float4 appendResult26_g275 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g275 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g275 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g275 = -_WorldDickPosition;
				float4 appendResult29_g275 = (float4(break27_g275.x , break27_g275.y , break27_g275.z , 1.0));
				float4x4 temp_output_30_0_g275 = mul( transpose( float4x4( float4( normalizeResult27_g276 , 0.0 ).x,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).x,float4( normalizeResult29_g276 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g276 , 0.0 ).y,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).y,float4( normalizeResult29_g276 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g276 , 0.0 ).z,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).z,float4( normalizeResult29_g276 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g276 , 0.0 ).w,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).w,float4( normalizeResult29_g276 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g275.x,appendResult28_g275.x,appendResult31_g275.x,appendResult29_g275.x,appendResult26_g275.y,appendResult28_g275.y,appendResult31_g275.y,appendResult29_g275.y,appendResult26_g275.z,appendResult28_g275.z,appendResult31_g275.z,appendResult29_g275.z,appendResult26_g275.w,appendResult28_g275.w,appendResult31_g275.w,appendResult29_g275.w ) );
				float4x4 invertVal44_g275 = Inverse4x4( temp_output_30_0_g275 );
				float4 appendResult27_g274 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g274 = mul( GetObjectToWorldMatrix(), appendResult27_g274 ).xyz;
				float3 localMyCustomExpression32_g274 = MyCustomExpression32_g274( pos32_g274 );
				float4 appendResult32_g275 = (float4(localMyCustomExpression32_g274 , 1.0));
				float4 break35_g275 = mul( temp_output_30_0_g275, appendResult32_g275 );
				float temp_output_124_0_g275 = _TipRadius;
				float2 appendResult36_g275 = (float2(break35_g275.y , break35_g275.z));
				float2 normalizeResult41_g275 = normalize( appendResult36_g275 );
				float temp_output_120_0_g275 = sqrt( max( break35_g275.x , 0.0 ) );
				float temp_output_48_0_g275 = tan( radians( _Angle ) );
				float temp_output_125_0_g275 = ( temp_output_124_0_g275 + ( temp_output_120_0_g275 * temp_output_48_0_g275 ) );
				float temp_output_37_0_g275 = length( appendResult36_g275 );
				float temp_output_114_0_g275 = ( ( temp_output_125_0_g275 - temp_output_37_0_g275 ) + 1.0 );
				float lerpResult102_g275 = lerp( temp_output_125_0_g275 , temp_output_37_0_g275 , saturate( temp_output_114_0_g275 ));
				float lerpResult130_g275 = lerp( 0.0 , lerpResult102_g275 , saturate( ( -( -temp_output_124_0_g275 - break35_g275.x ) / temp_output_124_0_g275 ) ));
				float2 break43_g275 = ( normalizeResult41_g275 * lerpResult130_g275 );
				float4 appendResult40_g275 = (float4(max( break35_g275.x , -temp_output_124_0_g275 ) , break43_g275.x , break43_g275.y , 1.0));
				float4 appendResult28_g274 = (float4(((mul( invertVal44_g275, appendResult40_g275 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g274 = appendResult28_g274;
				(localWorldVar29_g274).xyz = GetCameraRelativePositionWS((localWorldVar29_g274).xyz);
				float4 transform29_g274 = mul(GetWorldToObjectMatrix(),localWorldVar29_g274);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g274 = (transform29_g274).xyz;
				#else
				float3 staticSwitch13_g274 = inputMesh.positionOS;
				#endif
				float3 temp_output_229_0 = staticSwitch13_g274;
				float3 temp_output_10_0_g277 = temp_output_229_0;
				float localGetDeformationFromPenetrators_float8_g277 = ( 0.0 );
				float4 appendResult17_g277 = (float4(temp_output_10_0_g277 , 1.0));
				float4 transform16_g277 = mul(GetObjectToWorldMatrix(),appendResult17_g277);
				transform16_g277.xyz = GetAbsolutePositionWS((transform16_g277).xyz);
				float3 worldPosition8_g277 = (transform16_g277).xyz;
				float4 texCoord3_g277 = inputMesh.ase_texcoord2;
				texCoord3_g277.xy = inputMesh.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float4 uv28_g277 = texCoord3_g277;
				float compressibleDistance8_g277 = _CompressibleDistance;
				float smoothness8_g277 = _Smoothness;
				float3 deformedPosition8_g277 = float3( 0,0,0 );
				{
				GetDeformationFromPenetrators_float(worldPosition8_g277,uv28_g277,compressibleDistance8_g277,smoothness8_g277,deformedPosition8_g277);
				}
				float4 appendResult21_g277 = (float4(deformedPosition8_g277 , 1.0));
				float4 localWorldVar19_g277 = appendResult21_g277;
				(localWorldVar19_g277).xyz = GetCameraRelativePositionWS((localWorldVar19_g277).xyz);
				float4 transform19_g277 = mul(GetWorldToObjectMatrix(),localWorldVar19_g277);
				#ifdef _PENETRATION_DEFORMATION_ON
				float3 staticSwitch24_g277 = (transform19_g277).xyz;
				#else
				float3 staticSwitch24_g277 = temp_output_10_0_g277;
				#endif
				float3 lerpResult275 = lerp( temp_output_229_0 , staticSwitch24_g277 , inputMesh.ase_color.r);
				
				float3 temp_output_50_0_g274 = inputMesh.normalOS;
				float2 break146_g275 = normalizeResult41_g275;
				float4 appendResult139_g275 = (float4(temp_output_48_0_g275 , break146_g275.x , break146_g275.y , 0.0));
				float3 normalizeResult144_g275 = normalize( (mul( invertVal44_g275, appendResult139_g275 )).xyz );
				float3 lerpResult44_g274 = lerp( normalizeResult144_g275 , temp_output_50_0_g274 , saturate( sign( temp_output_114_0_g275 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g274 = lerpResult44_g274;
				#else
				float3 staticSwitch17_g274 = temp_output_50_0_g274;
				#endif
				
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				o.ase_texcoord.xyz = ase_worldPos;
				float3 vertexPos11_g269 = inputMesh.positionOS;
				float4 ase_clipPos11_g269 = TransformWorldToHClip( TransformObjectToWorld(vertexPos11_g269));
				float4 screenPos11_g269 = ComputeScreenPos( ase_clipPos11_g269 , _ProjectionParams.x );
				o.ase_texcoord1 = screenPos11_g269;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				o.ase_texcoord2.xyz = ase_worldNormal;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord2.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = lerpResult275;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = staticSwitch17_g274;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord2 = v.ase_texcoord2;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord2 = patch[0].ase_texcoord2 * bary.x + patch[1].ase_texcoord2 * bary.y + patch[2].ase_texcoord2 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( VertexOutput packedInput
						#ifdef WRITE_MSAA_DEPTH
						, out float4 depthColor : SV_Target0
							#ifdef WRITE_NORMAL_BUFFER
							, out float4 outNormalBuffer : SV_Target1
							#endif
						#else
							#ifdef WRITE_NORMAL_BUFFER
							, out float4 outNormalBuffer : SV_Target0
							#endif
						#endif
						#if defined(_DEPTHOFFSET_ON)
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float3 ase_worldPos = packedInput.ase_texcoord.xyz;
				float4 screenPos11_g269 = packedInput.ase_texcoord1;
				float4 ase_screenPosNorm11 = screenPos11_g269 / screenPos11_g269.w;
				ase_screenPosNorm11.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm11.z : ase_screenPosNorm11.z * 0.5 + 0.5;
				float screenDepth11_g269 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm11.xy ),_ZBufferParams);
				float distanceDepth11_g269 = saturate( abs( ( screenDepth11_g269 - LinearEyeDepth( ase_screenPosNorm11.z,_ZBufferParams ) ) / ( 2.0 ) ) );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = packedInput.ase_texcoord2.xyz;
				float dotResult3_g269 = dot( ase_worldViewDir , ase_worldNormal );
				
				surfaceDescription.Alpha = ( _BaseColor.a * ( 1.0 - saturate( ( distance( _WorldSpaceCameraPos , ase_worldPos ) / _DistanceFade ) ) ) * saturate( distanceDepth11_g269 ) * saturate( dotResult3_g269 ) );
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				#if defined(_DEPTHOFFSET_ON)
				outputDepth = posInput.deviceDepth;
				float bias = max(abs(ddx(posInput.deviceDepth)), abs(ddy(posInput.deviceDepth))) * _SlopeScaleDepthBias;
				outputDepth += bias;
				#endif

				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.vmesh.positionCS.z;

				#ifdef _ALPHATOMASK_ON
				depthColor.a = SharpenAlpha(builtinData.opacity, builtinData.alphaClipTreshold);
				#endif
				#endif

				#if defined(WRITE_NORMAL_BUFFER)
				EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), outNormalBuffer);
				#endif

				#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
				DecalPrepassData decalPrepassData;
				decalPrepassData.geomNormalWS = surfaceData.geomNormalWS;
				decalPrepassData.decalLayerMask = GetMeshRenderingDecalLayer();
				EncodeIntoDecalPrepassBuffer(decalPrepassData, outDecalBuffer);
				#endif
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "META"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 140008


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma shader_feature EDITOR_VISUALIZATION

			#pragma vertex Vert
			#pragma fragment Frag

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			#define SHADERPASS SHADERPASS_LIGHT_TRANSPORT
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			CBUFFER_START( UnityPerMaterial )
			float4 _BaseColor;
			float3 _WorldDickNormal;
			float3 _WorldDickBinormal;
			float3 _WorldDickPosition;
			float _TipRadius;
			float _Angle;
			float _CompressibleDistance;
			float _Smoothness;
			float _DistanceFade;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _DecalColorMap;


            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _PENETRATION_DEFORMATION_DETAIL_ON
			#pragma multi_compile_local __ _PENETRATION_DEFORMATION_ON
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#pragma shader_feature_local _SKINNED_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 uv3 : TEXCOORD3;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				#ifdef EDITOR_VISUALIZATION
				float2 VizUV : TEXCOORD0;
				float4 LightCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};


			float4x4 Inverse4x4(float4x4 input)
			{
				#define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
				float4x4 cofactors = float4x4(
				minor( _22_23_24, _32_33_34, _42_43_44 ),
				-minor( _21_23_24, _31_33_34, _41_43_44 ),
				minor( _21_22_24, _31_32_34, _41_42_44 ),
				-minor( _21_22_23, _31_32_33, _41_42_43 ),
			
				-minor( _12_13_14, _32_33_34, _42_43_44 ),
				minor( _11_13_14, _31_33_34, _41_43_44 ),
				-minor( _11_12_14, _31_32_34, _41_42_44 ),
				minor( _11_12_13, _31_32_33, _41_42_43 ),
			
				minor( _12_13_14, _22_23_24, _42_43_44 ),
				-minor( _11_13_14, _21_23_24, _41_43_44 ),
				minor( _11_12_14, _21_22_24, _41_42_44 ),
				-minor( _11_12_13, _21_22_23, _41_42_43 ),
			
				-minor( _12_13_14, _22_23_24, _32_33_34 ),
				minor( _11_13_14, _21_23_24, _31_33_34 ),
				-minor( _11_12_14, _21_22_24, _31_32_34 ),
				minor( _11_12_13, _21_22_23, _31_32_33 ));
				#undef minor
				return transpose( cofactors ) / determinant( input );
			}
			
			float3 MyCustomExpression32_g274( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			
			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			

			struct SurfaceDescription
			{
				float3 Color;
				float3 Emission;
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData( FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData )
			{
				ZERO_INITIALIZE( SurfaceData, surfaceData );
				surfaceData.color = surfaceDescription.Color;

				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif
			}

			void GetSurfaceAndBuiltinData( SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData )
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#if _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

				BuildSurfaceData( fragInputs, surfaceDescription, V, surfaceData );
				ZERO_INITIALIZE( BuiltinData, builtinData );
				builtinData.opacity = surfaceDescription.Alpha;
				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

				#ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				#if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif


                ApplyDebugToBuiltinData(builtinData);
			}

			#define SCENEPICKINGPASS
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/MetaPass.hlsl"

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID( inputMesh );
				UNITY_TRANSFER_INSTANCE_ID( inputMesh, o );

				float3 normalizeResult27_g276 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g276 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g276 = normalize( cross( normalizeResult27_g276 , normalizeResult31_g276 ) );
				float4 appendResult26_g275 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g275 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g275 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g275 = -_WorldDickPosition;
				float4 appendResult29_g275 = (float4(break27_g275.x , break27_g275.y , break27_g275.z , 1.0));
				float4x4 temp_output_30_0_g275 = mul( transpose( float4x4( float4( normalizeResult27_g276 , 0.0 ).x,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).x,float4( normalizeResult29_g276 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g276 , 0.0 ).y,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).y,float4( normalizeResult29_g276 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g276 , 0.0 ).z,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).z,float4( normalizeResult29_g276 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g276 , 0.0 ).w,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).w,float4( normalizeResult29_g276 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g275.x,appendResult28_g275.x,appendResult31_g275.x,appendResult29_g275.x,appendResult26_g275.y,appendResult28_g275.y,appendResult31_g275.y,appendResult29_g275.y,appendResult26_g275.z,appendResult28_g275.z,appendResult31_g275.z,appendResult29_g275.z,appendResult26_g275.w,appendResult28_g275.w,appendResult31_g275.w,appendResult29_g275.w ) );
				float4x4 invertVal44_g275 = Inverse4x4( temp_output_30_0_g275 );
				float4 appendResult27_g274 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g274 = mul( GetObjectToWorldMatrix(), appendResult27_g274 ).xyz;
				float3 localMyCustomExpression32_g274 = MyCustomExpression32_g274( pos32_g274 );
				float4 appendResult32_g275 = (float4(localMyCustomExpression32_g274 , 1.0));
				float4 break35_g275 = mul( temp_output_30_0_g275, appendResult32_g275 );
				float temp_output_124_0_g275 = _TipRadius;
				float2 appendResult36_g275 = (float2(break35_g275.y , break35_g275.z));
				float2 normalizeResult41_g275 = normalize( appendResult36_g275 );
				float temp_output_120_0_g275 = sqrt( max( break35_g275.x , 0.0 ) );
				float temp_output_48_0_g275 = tan( radians( _Angle ) );
				float temp_output_125_0_g275 = ( temp_output_124_0_g275 + ( temp_output_120_0_g275 * temp_output_48_0_g275 ) );
				float temp_output_37_0_g275 = length( appendResult36_g275 );
				float temp_output_114_0_g275 = ( ( temp_output_125_0_g275 - temp_output_37_0_g275 ) + 1.0 );
				float lerpResult102_g275 = lerp( temp_output_125_0_g275 , temp_output_37_0_g275 , saturate( temp_output_114_0_g275 ));
				float lerpResult130_g275 = lerp( 0.0 , lerpResult102_g275 , saturate( ( -( -temp_output_124_0_g275 - break35_g275.x ) / temp_output_124_0_g275 ) ));
				float2 break43_g275 = ( normalizeResult41_g275 * lerpResult130_g275 );
				float4 appendResult40_g275 = (float4(max( break35_g275.x , -temp_output_124_0_g275 ) , break43_g275.x , break43_g275.y , 1.0));
				float4 appendResult28_g274 = (float4(((mul( invertVal44_g275, appendResult40_g275 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g274 = appendResult28_g274;
				(localWorldVar29_g274).xyz = GetCameraRelativePositionWS((localWorldVar29_g274).xyz);
				float4 transform29_g274 = mul(GetWorldToObjectMatrix(),localWorldVar29_g274);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g274 = (transform29_g274).xyz;
				#else
				float3 staticSwitch13_g274 = inputMesh.positionOS;
				#endif
				float3 temp_output_229_0 = staticSwitch13_g274;
				float3 temp_output_10_0_g277 = temp_output_229_0;
				float localGetDeformationFromPenetrators_float8_g277 = ( 0.0 );
				float4 appendResult17_g277 = (float4(temp_output_10_0_g277 , 1.0));
				float4 transform16_g277 = mul(GetObjectToWorldMatrix(),appendResult17_g277);
				transform16_g277.xyz = GetAbsolutePositionWS((transform16_g277).xyz);
				float3 worldPosition8_g277 = (transform16_g277).xyz;
				float4 texCoord3_g277 = inputMesh.uv2;
				texCoord3_g277.xy = inputMesh.uv2.xy * float2( 1,1 ) + float2( 0,0 );
				float4 uv28_g277 = texCoord3_g277;
				float compressibleDistance8_g277 = _CompressibleDistance;
				float smoothness8_g277 = _Smoothness;
				float3 deformedPosition8_g277 = float3( 0,0,0 );
				{
				GetDeformationFromPenetrators_float(worldPosition8_g277,uv28_g277,compressibleDistance8_g277,smoothness8_g277,deformedPosition8_g277);
				}
				float4 appendResult21_g277 = (float4(deformedPosition8_g277 , 1.0));
				float4 localWorldVar19_g277 = appendResult21_g277;
				(localWorldVar19_g277).xyz = GetCameraRelativePositionWS((localWorldVar19_g277).xyz);
				float4 transform19_g277 = mul(GetWorldToObjectMatrix(),localWorldVar19_g277);
				#ifdef _PENETRATION_DEFORMATION_ON
				float3 staticSwitch24_g277 = (transform19_g277).xyz;
				#else
				float3 staticSwitch24_g277 = temp_output_10_0_g277;
				#endif
				float3 lerpResult275 = lerp( temp_output_229_0 , staticSwitch24_g277 , inputMesh.ase_color.r);
				
				float3 temp_output_50_0_g274 = inputMesh.normalOS;
				float2 break146_g275 = normalizeResult41_g275;
				float4 appendResult139_g275 = (float4(temp_output_48_0_g275 , break146_g275.x , break146_g275.y , 0.0));
				float3 normalizeResult144_g275 = normalize( (mul( invertVal44_g275, appendResult139_g275 )).xyz );
				float3 lerpResult44_g274 = lerp( normalizeResult144_g275 , temp_output_50_0_g274 , saturate( sign( temp_output_114_0_g275 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g274 = lerpResult44_g274;
				#else
				float3 staticSwitch17_g274 = temp_output_50_0_g274;
				#endif
				
				float3 vertexToFrag28_g270 = mul( UNITY_MATRIX_M, float4( inputMesh.positionOS , 0.0 ) ).xyz;
				o.ase_texcoord3.xyz = vertexToFrag28_g270;
				
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				o.ase_texcoord4.xyz = ase_worldPos;
				float3 vertexPos11_g269 = inputMesh.positionOS;
				float4 ase_clipPos11_g269 = TransformWorldToHClip( TransformObjectToWorld(vertexPos11_g269));
				float4 screenPos11_g269 = ComputeScreenPos( ase_clipPos11_g269 , _ProjectionParams.x );
				o.ase_texcoord5 = screenPos11_g269;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				o.ase_texcoord6.xyz = ase_worldNormal;
				
				o.ase_texcoord2.xy = inputMesh.uv1.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				o.ase_texcoord3.w = 0;
				o.ase_texcoord4.w = 0;
				o.ase_texcoord6.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = lerpResult275;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = staticSwitch17_g274;

			#ifdef EDITOR_VISUALIZATION
				float2 vizUV = 0;
				float4 lightCoord = 0;
				UnityEditorVizData(inputMesh.positionOS.xyz, inputMesh.uv0.xy, inputMesh.uv1.xy, inputMesh.uv2.xy, vizUV, lightCoord);
			#endif

				float2 uv = float2( 0.0, 0.0 );
				if( unity_MetaVertexControl.x )
				{
					uv = inputMesh.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				}
				else if( unity_MetaVertexControl.y )
				{
					uv = inputMesh.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				}

				#ifdef EDITOR_VISUALIZATION
					o.VizUV.xy = vizUV;
					o.LightCoord = lightCoord;
				#endif

				o.positionCS = float4( uv * 2.0 - 1.0, inputMesh.positionOS.z > 0 ? 1.0e-4 : 0.0, 1.0 );
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 uv3 : TEXCOORD3;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.uv0 = v.uv0;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				o.uv3 = v.uv3;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.uv0 = patch[0].uv0 * bary.x + patch[1].uv0 * bary.y + patch[2].uv0 * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				o.uv3 = patch[0].uv3 * bary.x + patch[1].uv3 * bary.y + patch[2].uv3 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			float4 Frag( VertexOutput packedInput  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE( FragInputs, input );
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput( input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS );

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float4 temp_output_1_0_g270 = _BaseColor;
				float4 color20_g270 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
				float3 vertexToFrag28_g270 = packedInput.ase_texcoord3.xyz;
				#ifdef _SKINNED_ON
				float3 staticSwitch26_g270 = float3( packedInput.ase_texcoord2.xy ,  0.0 );
				#else
				float3 staticSwitch26_g270 = vertexToFrag28_g270;
				#endif
				float simplePerlin3D9_g270 = snoise( ( ( staticSwitch26_g270 * float3( 2,2,2 ) ) + float3( 0,0,0 ) ) );
				simplePerlin3D9_g270 = simplePerlin3D9_g270*0.5 + 0.5;
				float temp_output_16_0_g270 = ( ( simplePerlin3D9_g270 - 0.5 ) * 0.5 );
				float temp_output_12_0_g270 = ( tex2D( _DecalColorMap, packedInput.ase_texcoord2.xy ).r + temp_output_16_0_g270 );
				float lerpResult22_g270 = lerp( ( temp_output_12_0_g270 + temp_output_16_0_g270 ) , 0.7 , 0.8);
				float4 lerpResult19_g270 = lerp( temp_output_1_0_g270 , color20_g270 , ( 1.0 - lerpResult22_g270 ));
				float temp_output_3_0_g271 = ( 0.5 - temp_output_12_0_g270 );
				float temp_output_10_0_g270 = ( 1.0 - saturate( ( temp_output_3_0_g271 / fwidth( temp_output_3_0_g271 ) ) ) );
				float4 lerpResult18_g270 = lerp( temp_output_1_0_g270 , lerpResult19_g270 , temp_output_10_0_g270);
				float4 temp_output_276_18 = lerpResult18_g270;
				
				float3 ase_worldPos = packedInput.ase_texcoord4.xyz;
				float4 screenPos11_g269 = packedInput.ase_texcoord5;
				float4 ase_screenPosNorm11 = screenPos11_g269 / screenPos11_g269.w;
				ase_screenPosNorm11.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm11.z : ase_screenPosNorm11.z * 0.5 + 0.5;
				float screenDepth11_g269 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm11.xy ),_ZBufferParams);
				float distanceDepth11_g269 = saturate( abs( ( screenDepth11_g269 - LinearEyeDepth( ase_screenPosNorm11.z,_ZBufferParams ) ) / ( 2.0 ) ) );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = packedInput.ase_texcoord6.xyz;
				float dotResult3_g269 = dot( ase_worldViewDir , ase_worldNormal );
				
				surfaceDescription.Color = temp_output_276_18.rgb;
				surfaceDescription.Emission = temp_output_276_18.rgb;
				surfaceDescription.Alpha = ( _BaseColor.a * ( 1.0 - saturate( ( distance( _WorldSpaceCameraPos , ase_worldPos ) / _DistanceFade ) ) ) * saturate( distanceDepth11_g269 ) * saturate( dotResult3_g269 ) );
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData( surfaceDescription,input, V, posInput, surfaceData, builtinData );

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData( input.positionSS.xy, surfaceData );
				LightTransportData lightTransportData = GetLightTransportData( surfaceData, builtinData, bsdfData );

				float4 res = float4( 0.0, 0.0, 0.0, 1.0 );
				UnityMetaInput metaInput;
				metaInput.Albedo = lightTransportData.diffuseColor.rgb;
				metaInput.Emission = lightTransportData.emissiveColor;
			#ifdef EDITOR_VISUALIZATION
				metaInput.VizUV = packedInput.VizUV;
				metaInput.LightCoord = packedInput.LightCoord;
			#endif
				res = UnityMetaFragment(metaInput);

				return res;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }

			Cull Off

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 140008


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma editor_sync_compilation

			#pragma vertex Vert
			#pragma fragment Frag

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define SCENESELECTIONPASS 1
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			int _ObjectId;
			int _PassValue;

			CBUFFER_START( UnityPerMaterial )
			float4 _BaseColor;
			float3 _WorldDickNormal;
			float3 _WorldDickBinormal;
			float3 _WorldDickPosition;
			float _TipRadius;
			float _Angle;
			float _CompressibleDistance;
			float _Smoothness;
			float _DistanceFade;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _PENETRATION_DEFORMATION_DETAIL_ON
			#pragma multi_compile_local __ _PENETRATION_DEFORMATION_ON
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};


			float4x4 Inverse4x4(float4x4 input)
			{
				#define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
				float4x4 cofactors = float4x4(
				minor( _22_23_24, _32_33_34, _42_43_44 ),
				-minor( _21_23_24, _31_33_34, _41_43_44 ),
				minor( _21_22_24, _31_32_34, _41_42_44 ),
				-minor( _21_22_23, _31_32_33, _41_42_43 ),
			
				-minor( _12_13_14, _32_33_34, _42_43_44 ),
				minor( _11_13_14, _31_33_34, _41_43_44 ),
				-minor( _11_12_14, _31_32_34, _41_42_44 ),
				minor( _11_12_13, _31_32_33, _41_42_43 ),
			
				minor( _12_13_14, _22_23_24, _42_43_44 ),
				-minor( _11_13_14, _21_23_24, _41_43_44 ),
				minor( _11_12_14, _21_22_24, _41_42_44 ),
				-minor( _11_12_13, _21_22_23, _41_42_43 ),
			
				-minor( _12_13_14, _22_23_24, _32_33_34 ),
				minor( _11_13_14, _21_23_24, _31_33_34 ),
				-minor( _11_12_14, _21_22_24, _31_32_34 ),
				minor( _11_12_13, _21_22_23, _31_32_33 ));
				#undef minor
				return transpose( cofactors ) / determinant( input );
			}
			
			float3 MyCustomExpression32_g274( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);
				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity =  surfaceDescription.Alpha;

				#ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				#if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif


                ApplyDebugToBuiltinData(builtinData);
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 normalizeResult27_g276 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g276 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g276 = normalize( cross( normalizeResult27_g276 , normalizeResult31_g276 ) );
				float4 appendResult26_g275 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g275 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g275 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g275 = -_WorldDickPosition;
				float4 appendResult29_g275 = (float4(break27_g275.x , break27_g275.y , break27_g275.z , 1.0));
				float4x4 temp_output_30_0_g275 = mul( transpose( float4x4( float4( normalizeResult27_g276 , 0.0 ).x,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).x,float4( normalizeResult29_g276 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g276 , 0.0 ).y,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).y,float4( normalizeResult29_g276 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g276 , 0.0 ).z,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).z,float4( normalizeResult29_g276 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g276 , 0.0 ).w,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).w,float4( normalizeResult29_g276 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g275.x,appendResult28_g275.x,appendResult31_g275.x,appendResult29_g275.x,appendResult26_g275.y,appendResult28_g275.y,appendResult31_g275.y,appendResult29_g275.y,appendResult26_g275.z,appendResult28_g275.z,appendResult31_g275.z,appendResult29_g275.z,appendResult26_g275.w,appendResult28_g275.w,appendResult31_g275.w,appendResult29_g275.w ) );
				float4x4 invertVal44_g275 = Inverse4x4( temp_output_30_0_g275 );
				float4 appendResult27_g274 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g274 = mul( GetObjectToWorldMatrix(), appendResult27_g274 ).xyz;
				float3 localMyCustomExpression32_g274 = MyCustomExpression32_g274( pos32_g274 );
				float4 appendResult32_g275 = (float4(localMyCustomExpression32_g274 , 1.0));
				float4 break35_g275 = mul( temp_output_30_0_g275, appendResult32_g275 );
				float temp_output_124_0_g275 = _TipRadius;
				float2 appendResult36_g275 = (float2(break35_g275.y , break35_g275.z));
				float2 normalizeResult41_g275 = normalize( appendResult36_g275 );
				float temp_output_120_0_g275 = sqrt( max( break35_g275.x , 0.0 ) );
				float temp_output_48_0_g275 = tan( radians( _Angle ) );
				float temp_output_125_0_g275 = ( temp_output_124_0_g275 + ( temp_output_120_0_g275 * temp_output_48_0_g275 ) );
				float temp_output_37_0_g275 = length( appendResult36_g275 );
				float temp_output_114_0_g275 = ( ( temp_output_125_0_g275 - temp_output_37_0_g275 ) + 1.0 );
				float lerpResult102_g275 = lerp( temp_output_125_0_g275 , temp_output_37_0_g275 , saturate( temp_output_114_0_g275 ));
				float lerpResult130_g275 = lerp( 0.0 , lerpResult102_g275 , saturate( ( -( -temp_output_124_0_g275 - break35_g275.x ) / temp_output_124_0_g275 ) ));
				float2 break43_g275 = ( normalizeResult41_g275 * lerpResult130_g275 );
				float4 appendResult40_g275 = (float4(max( break35_g275.x , -temp_output_124_0_g275 ) , break43_g275.x , break43_g275.y , 1.0));
				float4 appendResult28_g274 = (float4(((mul( invertVal44_g275, appendResult40_g275 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g274 = appendResult28_g274;
				(localWorldVar29_g274).xyz = GetCameraRelativePositionWS((localWorldVar29_g274).xyz);
				float4 transform29_g274 = mul(GetWorldToObjectMatrix(),localWorldVar29_g274);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g274 = (transform29_g274).xyz;
				#else
				float3 staticSwitch13_g274 = inputMesh.positionOS;
				#endif
				float3 temp_output_229_0 = staticSwitch13_g274;
				float3 temp_output_10_0_g277 = temp_output_229_0;
				float localGetDeformationFromPenetrators_float8_g277 = ( 0.0 );
				float4 appendResult17_g277 = (float4(temp_output_10_0_g277 , 1.0));
				float4 transform16_g277 = mul(GetObjectToWorldMatrix(),appendResult17_g277);
				transform16_g277.xyz = GetAbsolutePositionWS((transform16_g277).xyz);
				float3 worldPosition8_g277 = (transform16_g277).xyz;
				float4 texCoord3_g277 = inputMesh.ase_texcoord2;
				texCoord3_g277.xy = inputMesh.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float4 uv28_g277 = texCoord3_g277;
				float compressibleDistance8_g277 = _CompressibleDistance;
				float smoothness8_g277 = _Smoothness;
				float3 deformedPosition8_g277 = float3( 0,0,0 );
				{
				GetDeformationFromPenetrators_float(worldPosition8_g277,uv28_g277,compressibleDistance8_g277,smoothness8_g277,deformedPosition8_g277);
				}
				float4 appendResult21_g277 = (float4(deformedPosition8_g277 , 1.0));
				float4 localWorldVar19_g277 = appendResult21_g277;
				(localWorldVar19_g277).xyz = GetCameraRelativePositionWS((localWorldVar19_g277).xyz);
				float4 transform19_g277 = mul(GetWorldToObjectMatrix(),localWorldVar19_g277);
				#ifdef _PENETRATION_DEFORMATION_ON
				float3 staticSwitch24_g277 = (transform19_g277).xyz;
				#else
				float3 staticSwitch24_g277 = temp_output_10_0_g277;
				#endif
				float3 lerpResult275 = lerp( temp_output_229_0 , staticSwitch24_g277 , inputMesh.ase_color.r);
				
				float3 temp_output_50_0_g274 = inputMesh.normalOS;
				float2 break146_g275 = normalizeResult41_g275;
				float4 appendResult139_g275 = (float4(temp_output_48_0_g275 , break146_g275.x , break146_g275.y , 0.0));
				float3 normalizeResult144_g275 = normalize( (mul( invertVal44_g275, appendResult139_g275 )).xyz );
				float3 lerpResult44_g274 = lerp( normalizeResult144_g275 , temp_output_50_0_g274 , saturate( sign( temp_output_114_0_g275 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g274 = lerpResult44_g274;
				#else
				float3 staticSwitch17_g274 = temp_output_50_0_g274;
				#endif
				
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				o.ase_texcoord.xyz = ase_worldPos;
				float3 vertexPos11_g269 = inputMesh.positionOS;
				float4 ase_clipPos11_g269 = TransformWorldToHClip( TransformObjectToWorld(vertexPos11_g269));
				float4 screenPos11_g269 = ComputeScreenPos( ase_clipPos11_g269 , _ProjectionParams.x );
				o.ase_texcoord1 = screenPos11_g269;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				o.ase_texcoord2.xyz = ase_worldNormal;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord2.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  lerpResult275;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = staticSwitch17_g274;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord2 = v.ase_texcoord2;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord2 = patch[0].ase_texcoord2 * bary.x + patch[1].ase_texcoord2 * bary.y + patch[2].ase_texcoord2 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( VertexOutput packedInput
					, out float4 outColor : SV_Target0
					#ifdef _DEPTHOFFSET_ON
					, out float outputDepth : SV_Depth
					#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceData surfaceData;
				BuiltinData builtinData;
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float3 ase_worldPos = packedInput.ase_texcoord.xyz;
				float4 screenPos11_g269 = packedInput.ase_texcoord1;
				float4 ase_screenPosNorm11 = screenPos11_g269 / screenPos11_g269.w;
				ase_screenPosNorm11.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm11.z : ase_screenPosNorm11.z * 0.5 + 0.5;
				float screenDepth11_g269 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm11.xy ),_ZBufferParams);
				float distanceDepth11_g269 = saturate( abs( ( screenDepth11_g269 - LinearEyeDepth( ase_screenPosNorm11.z,_ZBufferParams ) ) / ( 2.0 ) ) );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = packedInput.ase_texcoord2.xyz;
				float dotResult3_g269 = dot( ase_worldViewDir , ase_worldNormal );
				
				surfaceDescription.Alpha = ( _BaseColor.a * ( 1.0 - saturate( ( distance( _WorldSpaceCameraPos , ase_worldPos ) / _DistanceFade ) ) ) * saturate( distanceDepth11_g269 ) * saturate( dotResult3_g269 ) );
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;

				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthForwardOnly"
			Tags { "LightMode"="DepthForwardOnly" }

			Cull [_CullMode]
			ZWrite On
			Stencil
			{
				Ref [_StencilRefDepth]
				WriteMask [_StencilWriteMaskDepth]
				Comp Always
				Pass Replace
			}


			ColorMask 0 0

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 140008


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile _ WRITE_MSAA_DEPTH

			#pragma vertex Vert
			#pragma fragment Frag

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			CBUFFER_START( UnityPerMaterial )
			float4 _BaseColor;
			float3 _WorldDickNormal;
			float3 _WorldDickBinormal;
			float3 _WorldDickPosition;
			float _TipRadius;
			float _Angle;
			float _CompressibleDistance;
			float _Smoothness;
			float _DistanceFade;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _PENETRATION_DEFORMATION_DETAIL_ON
			#pragma multi_compile_local __ _PENETRATION_DEFORMATION_ON
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float4x4 Inverse4x4(float4x4 input)
			{
				#define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
				float4x4 cofactors = float4x4(
				minor( _22_23_24, _32_33_34, _42_43_44 ),
				-minor( _21_23_24, _31_33_34, _41_43_44 ),
				minor( _21_22_24, _31_32_34, _41_42_44 ),
				-minor( _21_22_23, _31_32_33, _41_42_43 ),
			
				-minor( _12_13_14, _32_33_34, _42_43_44 ),
				minor( _11_13_14, _31_33_34, _41_43_44 ),
				-minor( _11_12_14, _31_32_34, _41_42_44 ),
				minor( _11_12_13, _31_32_33, _41_42_43 ),
			
				minor( _12_13_14, _22_23_24, _42_43_44 ),
				-minor( _11_13_14, _21_23_24, _41_43_44 ),
				minor( _11_12_14, _21_22_24, _41_42_44 ),
				-minor( _11_12_13, _21_22_23, _41_42_43 ),
			
				-minor( _12_13_14, _22_23_24, _32_33_34 ),
				minor( _11_13_14, _21_23_24, _31_33_34 ),
				-minor( _11_12_14, _21_22_24, _31_32_34 ),
				minor( _11_12_13, _21_22_23, _31_32_33 ));
				#undef minor
				return transpose( cofactors ) / determinant( input );
			}
			
			float3 MyCustomExpression32_g274( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#if _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);
				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity =  surfaceDescription.Alpha;

				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				#if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif

                ApplyDebugToBuiltinData(builtinData);
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 normalizeResult27_g276 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g276 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g276 = normalize( cross( normalizeResult27_g276 , normalizeResult31_g276 ) );
				float4 appendResult26_g275 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g275 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g275 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g275 = -_WorldDickPosition;
				float4 appendResult29_g275 = (float4(break27_g275.x , break27_g275.y , break27_g275.z , 1.0));
				float4x4 temp_output_30_0_g275 = mul( transpose( float4x4( float4( normalizeResult27_g276 , 0.0 ).x,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).x,float4( normalizeResult29_g276 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g276 , 0.0 ).y,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).y,float4( normalizeResult29_g276 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g276 , 0.0 ).z,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).z,float4( normalizeResult29_g276 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g276 , 0.0 ).w,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).w,float4( normalizeResult29_g276 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g275.x,appendResult28_g275.x,appendResult31_g275.x,appendResult29_g275.x,appendResult26_g275.y,appendResult28_g275.y,appendResult31_g275.y,appendResult29_g275.y,appendResult26_g275.z,appendResult28_g275.z,appendResult31_g275.z,appendResult29_g275.z,appendResult26_g275.w,appendResult28_g275.w,appendResult31_g275.w,appendResult29_g275.w ) );
				float4x4 invertVal44_g275 = Inverse4x4( temp_output_30_0_g275 );
				float4 appendResult27_g274 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g274 = mul( GetObjectToWorldMatrix(), appendResult27_g274 ).xyz;
				float3 localMyCustomExpression32_g274 = MyCustomExpression32_g274( pos32_g274 );
				float4 appendResult32_g275 = (float4(localMyCustomExpression32_g274 , 1.0));
				float4 break35_g275 = mul( temp_output_30_0_g275, appendResult32_g275 );
				float temp_output_124_0_g275 = _TipRadius;
				float2 appendResult36_g275 = (float2(break35_g275.y , break35_g275.z));
				float2 normalizeResult41_g275 = normalize( appendResult36_g275 );
				float temp_output_120_0_g275 = sqrt( max( break35_g275.x , 0.0 ) );
				float temp_output_48_0_g275 = tan( radians( _Angle ) );
				float temp_output_125_0_g275 = ( temp_output_124_0_g275 + ( temp_output_120_0_g275 * temp_output_48_0_g275 ) );
				float temp_output_37_0_g275 = length( appendResult36_g275 );
				float temp_output_114_0_g275 = ( ( temp_output_125_0_g275 - temp_output_37_0_g275 ) + 1.0 );
				float lerpResult102_g275 = lerp( temp_output_125_0_g275 , temp_output_37_0_g275 , saturate( temp_output_114_0_g275 ));
				float lerpResult130_g275 = lerp( 0.0 , lerpResult102_g275 , saturate( ( -( -temp_output_124_0_g275 - break35_g275.x ) / temp_output_124_0_g275 ) ));
				float2 break43_g275 = ( normalizeResult41_g275 * lerpResult130_g275 );
				float4 appendResult40_g275 = (float4(max( break35_g275.x , -temp_output_124_0_g275 ) , break43_g275.x , break43_g275.y , 1.0));
				float4 appendResult28_g274 = (float4(((mul( invertVal44_g275, appendResult40_g275 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g274 = appendResult28_g274;
				(localWorldVar29_g274).xyz = GetCameraRelativePositionWS((localWorldVar29_g274).xyz);
				float4 transform29_g274 = mul(GetWorldToObjectMatrix(),localWorldVar29_g274);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g274 = (transform29_g274).xyz;
				#else
				float3 staticSwitch13_g274 = inputMesh.positionOS;
				#endif
				float3 temp_output_229_0 = staticSwitch13_g274;
				float3 temp_output_10_0_g277 = temp_output_229_0;
				float localGetDeformationFromPenetrators_float8_g277 = ( 0.0 );
				float4 appendResult17_g277 = (float4(temp_output_10_0_g277 , 1.0));
				float4 transform16_g277 = mul(GetObjectToWorldMatrix(),appendResult17_g277);
				transform16_g277.xyz = GetAbsolutePositionWS((transform16_g277).xyz);
				float3 worldPosition8_g277 = (transform16_g277).xyz;
				float4 texCoord3_g277 = inputMesh.ase_texcoord2;
				texCoord3_g277.xy = inputMesh.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float4 uv28_g277 = texCoord3_g277;
				float compressibleDistance8_g277 = _CompressibleDistance;
				float smoothness8_g277 = _Smoothness;
				float3 deformedPosition8_g277 = float3( 0,0,0 );
				{
				GetDeformationFromPenetrators_float(worldPosition8_g277,uv28_g277,compressibleDistance8_g277,smoothness8_g277,deformedPosition8_g277);
				}
				float4 appendResult21_g277 = (float4(deformedPosition8_g277 , 1.0));
				float4 localWorldVar19_g277 = appendResult21_g277;
				(localWorldVar19_g277).xyz = GetCameraRelativePositionWS((localWorldVar19_g277).xyz);
				float4 transform19_g277 = mul(GetWorldToObjectMatrix(),localWorldVar19_g277);
				#ifdef _PENETRATION_DEFORMATION_ON
				float3 staticSwitch24_g277 = (transform19_g277).xyz;
				#else
				float3 staticSwitch24_g277 = temp_output_10_0_g277;
				#endif
				float3 lerpResult275 = lerp( temp_output_229_0 , staticSwitch24_g277 , inputMesh.ase_color.r);
				
				float3 temp_output_50_0_g274 = inputMesh.normalOS;
				float2 break146_g275 = normalizeResult41_g275;
				float4 appendResult139_g275 = (float4(temp_output_48_0_g275 , break146_g275.x , break146_g275.y , 0.0));
				float3 normalizeResult144_g275 = normalize( (mul( invertVal44_g275, appendResult139_g275 )).xyz );
				float3 lerpResult44_g274 = lerp( normalizeResult144_g275 , temp_output_50_0_g274 , saturate( sign( temp_output_114_0_g275 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g274 = lerpResult44_g274;
				#else
				float3 staticSwitch17_g274 = temp_output_50_0_g274;
				#endif
				
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				o.ase_texcoord.xyz = ase_worldPos;
				float3 vertexPos11_g269 = inputMesh.positionOS;
				float4 ase_clipPos11_g269 = TransformWorldToHClip( TransformObjectToWorld(vertexPos11_g269));
				float4 screenPos11_g269 = ComputeScreenPos( ase_clipPos11_g269 , _ProjectionParams.x );
				o.ase_texcoord1 = screenPos11_g269;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				o.ase_texcoord2.xyz = ase_worldNormal;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord2.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  lerpResult275;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = staticSwitch17_g274;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord2 = v.ase_texcoord2;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord2 = patch[0].ase_texcoord2 * bary.x + patch[1].ase_texcoord2 * bary.y + patch[2].ase_texcoord2 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( VertexOutput packedInput
						#ifdef WRITE_MSAA_DEPTH
						, out float4 depthColor : SV_Target0
							#ifdef WRITE_NORMAL_BUFFER
							, out float4 outNormalBuffer : SV_Target1
							#endif
						#else
							#ifdef WRITE_NORMAL_BUFFER
							, out float4 outNormalBuffer : SV_Target0
							#endif
						#endif
						#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float3 ase_worldPos = packedInput.ase_texcoord.xyz;
				float4 screenPos11_g269 = packedInput.ase_texcoord1;
				float4 ase_screenPosNorm11 = screenPos11_g269 / screenPos11_g269.w;
				ase_screenPosNorm11.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm11.z : ase_screenPosNorm11.z * 0.5 + 0.5;
				float screenDepth11_g269 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm11.xy ),_ZBufferParams);
				float distanceDepth11_g269 = saturate( abs( ( screenDepth11_g269 - LinearEyeDepth( ase_screenPosNorm11.z,_ZBufferParams ) ) / ( 2.0 ) ) );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = packedInput.ase_texcoord2.xyz;
				float dotResult3_g269 = dot( ase_worldViewDir , ase_worldNormal );
				
				surfaceDescription.Alpha = ( _BaseColor.a * ( 1.0 - saturate( ( distance( _WorldSpaceCameraPos , ase_worldPos ) / _DistanceFade ) ) ) * saturate( distanceDepth11_g269 ) * saturate( dotResult3_g269 ) );
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_MSAA_DEPTH
					depthColor = packedInput.positionCS.z;
					#ifdef _ALPHATOMASK_ON
					depthColor.a = SharpenAlpha(builtinData.opacity, builtinData.alphaClipTreshold);
					#endif
				#endif

				#if defined(WRITE_NORMAL_BUFFER)
					EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), outNormalBuffer);
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "MotionVectors"
			Tags { "LightMode"="MotionVectors" }

			Cull [_CullMode]

			ZWrite On

			Stencil
			{
				Ref [_StencilRefMV]
				WriteMask [_StencilWriteMaskMV]
				Comp Always
				Pass Replace
			}


			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 140008


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile _ WRITE_MSAA_DEPTH

			#pragma vertex Vert
			#pragma fragment Frag

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			#define SHADERPASS SHADERPASS_MOTION_VECTORS
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			CBUFFER_START( UnityPerMaterial )
			float4 _BaseColor;
			float3 _WorldDickNormal;
			float3 _WorldDickBinormal;
			float3 _WorldDickPosition;
			float _TipRadius;
			float _Angle;
			float _CompressibleDistance;
			float _Smoothness;
			float _DistanceFade;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
			#ifdef ASE_TESSELLATION
			float _TessPhongStrength;
			float _TessValue;
			float _TessMin;
			float _TessMax;
			float _TessEdgeLength;
			float _TessMaxDisp;
			#endif
			CBUFFER_END

			

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#pragma shader_feature_local _PENETRATION_DEFORMATION_DETAIL_ON
			#pragma multi_compile_local __ _PENETRATION_DEFORMATION_ON
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float3 previousPositionOS : TEXCOORD4;
				float3 precomputedVelocity : TEXCOORD5;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 vmeshPositionCS : SV_Position;
				float3 vmeshInterp00 : TEXCOORD0;
				float3 vpassInterpolators0 : TEXCOORD1; //interpolators0
				float3 vpassInterpolators1 : TEXCOORD2; //interpolators1
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float4x4 Inverse4x4(float4x4 input)
			{
				#define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
				float4x4 cofactors = float4x4(
				minor( _22_23_24, _32_33_34, _42_43_44 ),
				-minor( _21_23_24, _31_33_34, _41_43_44 ),
				minor( _21_22_24, _31_32_34, _41_42_44 ),
				-minor( _21_22_23, _31_32_33, _41_42_43 ),
			
				-minor( _12_13_14, _32_33_34, _42_43_44 ),
				minor( _11_13_14, _31_33_34, _41_43_44 ),
				-minor( _11_12_14, _31_32_34, _41_42_44 ),
				minor( _11_12_13, _31_32_33, _41_42_43 ),
			
				minor( _12_13_14, _22_23_24, _42_43_44 ),
				-minor( _11_13_14, _21_23_24, _41_43_44 ),
				minor( _11_12_14, _21_22_24, _41_42_44 ),
				-minor( _11_12_13, _21_22_23, _41_42_43 ),
			
				-minor( _12_13_14, _22_23_24, _32_33_34 ),
				minor( _11_13_14, _21_23_24, _31_33_34 ),
				-minor( _11_12_14, _21_22_24, _31_32_34 ),
				minor( _11_12_13, _21_22_23, _31_32_33 ));
				#undef minor
				return transpose( cofactors ) / determinant( input );
			}
			
			float3 MyCustomExpression32_g274( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#if _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);
				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity =  surfaceDescription.Alpha;

				#if defined(DEBUG_DISPLAY)
                    builtinData.renderingLayers = GetMeshRenderingLightLayer();
                #endif


                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif


                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif

                ApplyDebugToBuiltinData(builtinData);
			}

			VertexInput ApplyMeshModification(VertexInput inputMesh, float3 timeParameters, inout VertexOutput o )
			{
				_TimeParameters.xyz = timeParameters;
				float3 normalizeResult27_g276 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g276 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g276 = normalize( cross( normalizeResult27_g276 , normalizeResult31_g276 ) );
				float4 appendResult26_g275 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g275 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g275 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g275 = -_WorldDickPosition;
				float4 appendResult29_g275 = (float4(break27_g275.x , break27_g275.y , break27_g275.z , 1.0));
				float4x4 temp_output_30_0_g275 = mul( transpose( float4x4( float4( normalizeResult27_g276 , 0.0 ).x,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).x,float4( normalizeResult29_g276 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g276 , 0.0 ).y,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).y,float4( normalizeResult29_g276 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g276 , 0.0 ).z,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).z,float4( normalizeResult29_g276 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g276 , 0.0 ).w,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).w,float4( normalizeResult29_g276 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g275.x,appendResult28_g275.x,appendResult31_g275.x,appendResult29_g275.x,appendResult26_g275.y,appendResult28_g275.y,appendResult31_g275.y,appendResult29_g275.y,appendResult26_g275.z,appendResult28_g275.z,appendResult31_g275.z,appendResult29_g275.z,appendResult26_g275.w,appendResult28_g275.w,appendResult31_g275.w,appendResult29_g275.w ) );
				float4x4 invertVal44_g275 = Inverse4x4( temp_output_30_0_g275 );
				float4 appendResult27_g274 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g274 = mul( GetObjectToWorldMatrix(), appendResult27_g274 ).xyz;
				float3 localMyCustomExpression32_g274 = MyCustomExpression32_g274( pos32_g274 );
				float4 appendResult32_g275 = (float4(localMyCustomExpression32_g274 , 1.0));
				float4 break35_g275 = mul( temp_output_30_0_g275, appendResult32_g275 );
				float temp_output_124_0_g275 = _TipRadius;
				float2 appendResult36_g275 = (float2(break35_g275.y , break35_g275.z));
				float2 normalizeResult41_g275 = normalize( appendResult36_g275 );
				float temp_output_120_0_g275 = sqrt( max( break35_g275.x , 0.0 ) );
				float temp_output_48_0_g275 = tan( radians( _Angle ) );
				float temp_output_125_0_g275 = ( temp_output_124_0_g275 + ( temp_output_120_0_g275 * temp_output_48_0_g275 ) );
				float temp_output_37_0_g275 = length( appendResult36_g275 );
				float temp_output_114_0_g275 = ( ( temp_output_125_0_g275 - temp_output_37_0_g275 ) + 1.0 );
				float lerpResult102_g275 = lerp( temp_output_125_0_g275 , temp_output_37_0_g275 , saturate( temp_output_114_0_g275 ));
				float lerpResult130_g275 = lerp( 0.0 , lerpResult102_g275 , saturate( ( -( -temp_output_124_0_g275 - break35_g275.x ) / temp_output_124_0_g275 ) ));
				float2 break43_g275 = ( normalizeResult41_g275 * lerpResult130_g275 );
				float4 appendResult40_g275 = (float4(max( break35_g275.x , -temp_output_124_0_g275 ) , break43_g275.x , break43_g275.y , 1.0));
				float4 appendResult28_g274 = (float4(((mul( invertVal44_g275, appendResult40_g275 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g274 = appendResult28_g274;
				(localWorldVar29_g274).xyz = GetCameraRelativePositionWS((localWorldVar29_g274).xyz);
				float4 transform29_g274 = mul(GetWorldToObjectMatrix(),localWorldVar29_g274);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g274 = (transform29_g274).xyz;
				#else
				float3 staticSwitch13_g274 = inputMesh.positionOS;
				#endif
				float3 temp_output_229_0 = staticSwitch13_g274;
				float3 temp_output_10_0_g277 = temp_output_229_0;
				float localGetDeformationFromPenetrators_float8_g277 = ( 0.0 );
				float4 appendResult17_g277 = (float4(temp_output_10_0_g277 , 1.0));
				float4 transform16_g277 = mul(GetObjectToWorldMatrix(),appendResult17_g277);
				transform16_g277.xyz = GetAbsolutePositionWS((transform16_g277).xyz);
				float3 worldPosition8_g277 = (transform16_g277).xyz;
				float4 texCoord3_g277 = inputMesh.ase_texcoord2;
				texCoord3_g277.xy = inputMesh.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float4 uv28_g277 = texCoord3_g277;
				float compressibleDistance8_g277 = _CompressibleDistance;
				float smoothness8_g277 = _Smoothness;
				float3 deformedPosition8_g277 = float3( 0,0,0 );
				{
				GetDeformationFromPenetrators_float(worldPosition8_g277,uv28_g277,compressibleDistance8_g277,smoothness8_g277,deformedPosition8_g277);
				}
				float4 appendResult21_g277 = (float4(deformedPosition8_g277 , 1.0));
				float4 localWorldVar19_g277 = appendResult21_g277;
				(localWorldVar19_g277).xyz = GetCameraRelativePositionWS((localWorldVar19_g277).xyz);
				float4 transform19_g277 = mul(GetWorldToObjectMatrix(),localWorldVar19_g277);
				#ifdef _PENETRATION_DEFORMATION_ON
				float3 staticSwitch24_g277 = (transform19_g277).xyz;
				#else
				float3 staticSwitch24_g277 = temp_output_10_0_g277;
				#endif
				float3 lerpResult275 = lerp( temp_output_229_0 , staticSwitch24_g277 , inputMesh.ase_color.r);
				
				float3 temp_output_50_0_g274 = inputMesh.normalOS;
				float2 break146_g275 = normalizeResult41_g275;
				float4 appendResult139_g275 = (float4(temp_output_48_0_g275 , break146_g275.x , break146_g275.y , 0.0));
				float3 normalizeResult144_g275 = normalize( (mul( invertVal44_g275, appendResult139_g275 )).xyz );
				float3 lerpResult44_g274 = lerp( normalizeResult144_g275 , temp_output_50_0_g274 , saturate( sign( temp_output_114_0_g275 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g274 = lerpResult44_g274;
				#else
				float3 staticSwitch17_g274 = temp_output_50_0_g274;
				#endif
				
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				o.ase_texcoord3.xyz = ase_worldPos;
				float3 vertexPos11_g269 = inputMesh.positionOS;
				float4 ase_clipPos11_g269 = TransformWorldToHClip( TransformObjectToWorld(vertexPos11_g269));
				float4 screenPos11_g269 = ComputeScreenPos( ase_clipPos11_g269 , _ProjectionParams.x );
				o.ase_texcoord4 = screenPos11_g269;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				o.ase_texcoord5.xyz = ase_worldNormal;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.w = 0;
				o.ase_texcoord5.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = lerpResult275;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS = staticSwitch17_g274;
				return inputMesh;
			}

			VertexOutput VertexFunction(VertexInput inputMesh)
			{
				VertexOutput o = (VertexOutput)0;
				VertexInput defaultMesh = inputMesh;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				inputMesh = ApplyMeshModification( inputMesh, _TimeParameters.xyz, o);

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);

				float3 VMESHpositionRWS = positionRWS;
				float4 VMESHpositionCS = TransformWorldToHClip(positionRWS);

				//#if defined(UNITY_REVERSED_Z)
				//	VMESHpositionCS.z -= unity_MotionVectorsParams.z * VMESHpositionCS.w;
				//#else
				//	VMESHpositionCS.z += unity_MotionVectorsParams.z * VMESHpositionCS.w;
				//#endif

				float4 VPASSpreviousPositionCS;
				float4 VPASSpositionCS = mul(UNITY_MATRIX_UNJITTERED_VP, float4(VMESHpositionRWS, 1.0));

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if (forceNoMotion)
				{
					VPASSpreviousPositionCS = float4(0.0, 0.0, 0.0, 1.0);
				}
				else
				{
					bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
					float3 effectivePositionOS = (hasDeformation ? inputMesh.previousPositionOS : defaultMesh.positionOS);
					#if defined(_ADD_PRECOMPUTED_VELOCITY)
					effectivePositionOS -= inputMesh.precomputedVelocity;
					#endif

					#if defined(HAVE_MESH_MODIFICATION)
						VertexInput previousMesh = defaultMesh;
						previousMesh.positionOS = effectivePositionOS ;
						VertexOutput test = (VertexOutput)0;
						float3 curTime = _TimeParameters.xyz;
						previousMesh = ApplyMeshModification(previousMesh, _LastTimeParameters.xyz, test);
						_TimeParameters.xyz = curTime;
						float3 previousPositionRWS = TransformPreviousObjectToWorld(previousMesh.positionOS);
					#else
						float3 previousPositionRWS = TransformPreviousObjectToWorld(effectivePositionOS);
					#endif

					#ifdef ATTRIBUTES_NEED_NORMAL
						float3 normalWS = TransformPreviousObjectToWorldNormal(defaultMesh.normalOS);
					#else
						float3 normalWS = float3(0.0, 0.0, 0.0);
					#endif

					#if defined(HAVE_VERTEX_MODIFICATION)
						ApplyVertexModification(inputMesh, normalWS, previousPositionRWS, _LastTimeParameters.xyz);
					#endif

					VPASSpreviousPositionCS = mul(UNITY_MATRIX_PREV_VP, float4(previousPositionRWS, 1.0));
				}

				o.vmeshPositionCS = VMESHpositionCS;
				o.vmeshInterp00.xyz = VMESHpositionRWS;

				o.vpassInterpolators0 = float3(VPASSpositionCS.xyw);
				o.vpassInterpolators1 = float3(VPASSpreviousPositionCS.xyw);
				return o;
			}

			#if ( 0 ) // TEMPORARY: defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float3 previousPositionOS : TEXCOORD4;
				float3 precomputedVelocity : TEXCOORD5;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.previousPositionOS = v.previousPositionOS;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					o.precomputedVelocity = v.precomputedVelocity;
				#endif
				o.ase_texcoord2 = v.ase_texcoord2;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.previousPositionOS = patch[0].previousPositionOS * bary.x + patch[1].previousPositionOS * bary.y + patch[2].previousPositionOS * bary.z;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					o.precomputedVelocity = patch[0].precomputedVelocity * bary.x + patch[1].precomputedVelocity * bary.y + patch[2].precomputedVelocity * bary.z;
				#endif
				o.ase_texcoord2 = patch[0].ase_texcoord2 * bary.x + patch[1].ase_texcoord2 * bary.y + patch[2].ase_texcoord2 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(WRITE_DECAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_NORMAL SV_Target3
			#elif defined(WRITE_DECAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_NORMAL SV_Target2
			#else
			#define SV_TARGET_NORMAL SV_Target1
			#endif

			void Frag( VertexOutput packedInput
						#ifdef WRITE_MSAA_DEPTH
						, out float4 depthColor : SV_Target0
						, out float4 outMotionVector : SV_Target1
							#ifdef WRITE_DECAL_BUFFER
							, out float4 outDecalBuffer : SV_Target2
							#endif
						#else
						, out float4 outMotionVector : SV_Target0
							#ifdef WRITE_DECAL_BUFFER
							, out float4 outDecalBuffer : SV_Target1
							#endif
						#endif

						#ifdef WRITE_NORMAL_BUFFER
						, out float4 outNormalBuffer : SV_TARGET_NORMAL
						#endif

						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.vmeshPositionCS;
				input.positionRWS = packedInput.vmeshInterp00.xyz;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float3 ase_worldPos = packedInput.ase_texcoord3.xyz;
				float4 screenPos11_g269 = packedInput.ase_texcoord4;
				float4 ase_screenPosNorm11 = screenPos11_g269 / screenPos11_g269.w;
				ase_screenPosNorm11.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm11.z : ase_screenPosNorm11.z * 0.5 + 0.5;
				float screenDepth11_g269 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm11.xy ),_ZBufferParams);
				float distanceDepth11_g269 = saturate( abs( ( screenDepth11_g269 - LinearEyeDepth( ase_screenPosNorm11.z,_ZBufferParams ) ) / ( 2.0 ) ) );
				float3 ase_worldNormal = packedInput.ase_texcoord5.xyz;
				float dotResult3_g269 = dot( V , ase_worldNormal );
				
				surfaceDescription.Alpha = ( _BaseColor.a * ( 1.0 - saturate( ( distance( _WorldSpaceCameraPos , ase_worldPos ) / _DistanceFade ) ) ) * saturate( distanceDepth11_g269 ) * saturate( dotResult3_g269 ) );
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				float4 VPASSpositionCS = float4(packedInput.vpassInterpolators0.xy, 0.0, packedInput.vpassInterpolators0.z);
				float4 VPASSpreviousPositionCS = float4(packedInput.vpassInterpolators1.xy, 0.0, packedInput.vpassInterpolators1.z);

				#ifdef _DEPTHOFFSET_ON
				VPASSpositionCS.w += builtinData.depthOffset;
				VPASSpreviousPositionCS.w += builtinData.depthOffset;
				#endif

				float2 motionVector = CalculateMotionVector( VPASSpositionCS, VPASSpreviousPositionCS );
				EncodeMotionVector( motionVector * 0.5, outMotionVector );

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if( forceNoMotion )
					outMotionVector = float4( 2.0, 0.0, 0.0, 0.0 );

				// Depth and Alpha to coverage
				#ifdef WRITE_MSAA_DEPTH
					// In case we are rendering in MSAA, reading the an MSAA depth buffer is way too expensive. To avoid that, we export the depth to a color buffer
					depthColor = packedInput.vmeshPositionCS.z;

					// Alpha channel is used for alpha to coverage
					depthColor.a = SharpenAlpha(builtinData.opacity, builtinData.alphaClipTreshold);
				#endif

				// Normal Buffer Processing
				#ifdef WRITE_NORMAL_BUFFER
					EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), outNormalBuffer);
				#endif

				#if defined(WRITE_DECAL_BUFFER)
					DecalPrepassData decalPrepassData;
					#ifdef _DISABLE_DECALS
					ZERO_INITIALIZE(DecalPrepassData, decalPrepassData);
					#else
					decalPrepassData.geomNormalWS = surfaceData.geomNormalWS;
					decalPrepassData.decalLayerMask = GetMeshRenderingDecalLayer();
					#endif
					EncodeIntoDecalPrepassBuffer(decalPrepassData, outDecalBuffer);

					// make sure we don't overwrite light layers
					outDecalBuffer.w = (GetMeshRenderingLightLayer() & 0x000000FF) / 255.0;
				#endif

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}

			ENDHLSL
		}

		
        Pass
		{
			
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }

            Cull [_CullMode]

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 140008


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _TRANSPARENT_WRITES_MOTION_VEC

			#pragma editor_sync_compilation

			#pragma vertex Vert
			#pragma fragment Frag

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define VARYINGS_NEED_TANGENT_TO_WORLD

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define SCENEPICKINGPASS 1

			#define SHADER_UNLIT

			float4 _SelectionID;

            CBUFFER_START( UnityPerMaterial )
			float4 _BaseColor;
			float3 _WorldDickNormal;
			float3 _WorldDickBinormal;
			float3 _WorldDickPosition;
			float _TipRadius;
			float _Angle;
			float _CompressibleDistance;
			float _Smoothness;
			float _DistanceFade;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _PENETRATION_DEFORMATION_DETAIL_ON
			#pragma multi_compile_local __ _PENETRATION_DEFORMATION_ON
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float3 normalWS : TEXCOORD0;
				float4 tangentWS : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float4x4 Inverse4x4(float4x4 input)
			{
				#define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
				float4x4 cofactors = float4x4(
				minor( _22_23_24, _32_33_34, _42_43_44 ),
				-minor( _21_23_24, _31_33_34, _41_43_44 ),
				minor( _21_22_24, _31_32_34, _41_42_44 ),
				-minor( _21_22_23, _31_32_33, _41_42_43 ),
			
				-minor( _12_13_14, _32_33_34, _42_43_44 ),
				minor( _11_13_14, _31_33_34, _41_43_44 ),
				-minor( _11_12_14, _31_32_34, _41_42_44 ),
				minor( _11_12_13, _31_32_33, _41_42_43 ),
			
				minor( _12_13_14, _22_23_24, _42_43_44 ),
				-minor( _11_13_14, _21_23_24, _41_43_44 ),
				minor( _11_12_14, _21_22_24, _41_42_44 ),
				-minor( _11_12_13, _21_22_23, _41_42_43 ),
			
				-minor( _12_13_14, _22_23_24, _32_33_34 ),
				minor( _11_13_14, _21_23_24, _31_33_34 ),
				-minor( _11_12_14, _21_22_24, _31_32_34 ),
				minor( _11_12_13, _21_22_23, _31_32_33 ));
				#undef minor
				return transpose( cofactors ) / determinant( input );
			}
			
			float3 MyCustomExpression32_g274( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			

            struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};


            void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                #ifdef LOD_FADE_CROSSFADE
			        LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif

                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif


				ZERO_INITIALIZE(SurfaceData, surfaceData);

				ZERO_BUILTIN_INITIALIZE(builtinData);
				builtinData.opacity = surfaceDescription.Alpha;

				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif

                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif


                ApplyDebugToBuiltinData(builtinData);

            }


			VertexOutput VertexFunction(VertexInput inputMesh  )
			{

				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o );

				float3 normalizeResult27_g276 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g276 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g276 = normalize( cross( normalizeResult27_g276 , normalizeResult31_g276 ) );
				float4 appendResult26_g275 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g275 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g275 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g275 = -_WorldDickPosition;
				float4 appendResult29_g275 = (float4(break27_g275.x , break27_g275.y , break27_g275.z , 1.0));
				float4x4 temp_output_30_0_g275 = mul( transpose( float4x4( float4( normalizeResult27_g276 , 0.0 ).x,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).x,float4( normalizeResult29_g276 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g276 , 0.0 ).y,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).y,float4( normalizeResult29_g276 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g276 , 0.0 ).z,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).z,float4( normalizeResult29_g276 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g276 , 0.0 ).w,float4( cross( normalizeResult29_g276 , normalizeResult27_g276 ) , 0.0 ).w,float4( normalizeResult29_g276 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g275.x,appendResult28_g275.x,appendResult31_g275.x,appendResult29_g275.x,appendResult26_g275.y,appendResult28_g275.y,appendResult31_g275.y,appendResult29_g275.y,appendResult26_g275.z,appendResult28_g275.z,appendResult31_g275.z,appendResult29_g275.z,appendResult26_g275.w,appendResult28_g275.w,appendResult31_g275.w,appendResult29_g275.w ) );
				float4x4 invertVal44_g275 = Inverse4x4( temp_output_30_0_g275 );
				float4 appendResult27_g274 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g274 = mul( GetObjectToWorldMatrix(), appendResult27_g274 ).xyz;
				float3 localMyCustomExpression32_g274 = MyCustomExpression32_g274( pos32_g274 );
				float4 appendResult32_g275 = (float4(localMyCustomExpression32_g274 , 1.0));
				float4 break35_g275 = mul( temp_output_30_0_g275, appendResult32_g275 );
				float temp_output_124_0_g275 = _TipRadius;
				float2 appendResult36_g275 = (float2(break35_g275.y , break35_g275.z));
				float2 normalizeResult41_g275 = normalize( appendResult36_g275 );
				float temp_output_120_0_g275 = sqrt( max( break35_g275.x , 0.0 ) );
				float temp_output_48_0_g275 = tan( radians( _Angle ) );
				float temp_output_125_0_g275 = ( temp_output_124_0_g275 + ( temp_output_120_0_g275 * temp_output_48_0_g275 ) );
				float temp_output_37_0_g275 = length( appendResult36_g275 );
				float temp_output_114_0_g275 = ( ( temp_output_125_0_g275 - temp_output_37_0_g275 ) + 1.0 );
				float lerpResult102_g275 = lerp( temp_output_125_0_g275 , temp_output_37_0_g275 , saturate( temp_output_114_0_g275 ));
				float lerpResult130_g275 = lerp( 0.0 , lerpResult102_g275 , saturate( ( -( -temp_output_124_0_g275 - break35_g275.x ) / temp_output_124_0_g275 ) ));
				float2 break43_g275 = ( normalizeResult41_g275 * lerpResult130_g275 );
				float4 appendResult40_g275 = (float4(max( break35_g275.x , -temp_output_124_0_g275 ) , break43_g275.x , break43_g275.y , 1.0));
				float4 appendResult28_g274 = (float4(((mul( invertVal44_g275, appendResult40_g275 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g274 = appendResult28_g274;
				(localWorldVar29_g274).xyz = GetCameraRelativePositionWS((localWorldVar29_g274).xyz);
				float4 transform29_g274 = mul(GetWorldToObjectMatrix(),localWorldVar29_g274);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g274 = (transform29_g274).xyz;
				#else
				float3 staticSwitch13_g274 = inputMesh.positionOS;
				#endif
				float3 temp_output_229_0 = staticSwitch13_g274;
				float3 temp_output_10_0_g277 = temp_output_229_0;
				float localGetDeformationFromPenetrators_float8_g277 = ( 0.0 );
				float4 appendResult17_g277 = (float4(temp_output_10_0_g277 , 1.0));
				float4 transform16_g277 = mul(GetObjectToWorldMatrix(),appendResult17_g277);
				transform16_g277.xyz = GetAbsolutePositionWS((transform16_g277).xyz);
				float3 worldPosition8_g277 = (transform16_g277).xyz;
				float4 texCoord3_g277 = inputMesh.ase_texcoord2;
				texCoord3_g277.xy = inputMesh.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float4 uv28_g277 = texCoord3_g277;
				float compressibleDistance8_g277 = _CompressibleDistance;
				float smoothness8_g277 = _Smoothness;
				float3 deformedPosition8_g277 = float3( 0,0,0 );
				{
				GetDeformationFromPenetrators_float(worldPosition8_g277,uv28_g277,compressibleDistance8_g277,smoothness8_g277,deformedPosition8_g277);
				}
				float4 appendResult21_g277 = (float4(deformedPosition8_g277 , 1.0));
				float4 localWorldVar19_g277 = appendResult21_g277;
				(localWorldVar19_g277).xyz = GetCameraRelativePositionWS((localWorldVar19_g277).xyz);
				float4 transform19_g277 = mul(GetWorldToObjectMatrix(),localWorldVar19_g277);
				#ifdef _PENETRATION_DEFORMATION_ON
				float3 staticSwitch24_g277 = (transform19_g277).xyz;
				#else
				float3 staticSwitch24_g277 = temp_output_10_0_g277;
				#endif
				float3 lerpResult275 = lerp( temp_output_229_0 , staticSwitch24_g277 , inputMesh.ase_color.r);
				
				float3 temp_output_50_0_g274 = inputMesh.normalOS;
				float2 break146_g275 = normalizeResult41_g275;
				float4 appendResult139_g275 = (float4(temp_output_48_0_g275 , break146_g275.x , break146_g275.y , 0.0));
				float3 normalizeResult144_g275 = normalize( (mul( invertVal44_g275, appendResult139_g275 )).xyz );
				float3 lerpResult44_g274 = lerp( normalizeResult144_g275 , temp_output_50_0_g274 , saturate( sign( temp_output_114_0_g275 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g274 = lerpResult44_g274;
				#else
				float3 staticSwitch17_g274 = temp_output_50_0_g274;
				#endif
				
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				o.ase_texcoord2.xyz = ase_worldPos;
				float3 vertexPos11_g269 = inputMesh.positionOS;
				float4 ase_clipPos11_g269 = TransformWorldToHClip( TransformObjectToWorld(vertexPos11_g269));
				float4 screenPos11_g269 = ComputeScreenPos( ase_clipPos11_g269 , _ProjectionParams.x );
				o.ase_texcoord3 = screenPos11_g269;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  lerpResult275;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = staticSwitch17_g274;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				o.positionCS = TransformWorldToHClip(positionRWS);
				o.normalWS.xyz =  normalWS;
				o.tangentWS.xyzw =  tangentWS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.ase_texcoord2 = v.ase_texcoord2;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.ase_texcoord2 = patch[0].ase_texcoord2 * bary.x + patch[1].ase_texcoord2 * bary.y + patch[2].ase_texcoord2 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag(	VertexOutput packedInput
						, out float4 outColor : SV_Target0
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
				UNITY_SETUP_INSTANCE_ID(packedInput);

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.tangentToWorld = BuildTangentToWorld(packedInput.tangentWS.xyzw, packedInput.normalWS.xyz);

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float3 ase_worldPos = packedInput.ase_texcoord2.xyz;
				float4 screenPos11_g269 = packedInput.ase_texcoord3;
				float4 ase_screenPosNorm11 = screenPos11_g269 / screenPos11_g269.w;
				ase_screenPosNorm11.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm11.z : ase_screenPosNorm11.z * 0.5 + 0.5;
				float screenDepth11_g269 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm11.xy ),_ZBufferParams);
				float distanceDepth11_g269 = saturate( abs( ( screenDepth11_g269 - LinearEyeDepth( ase_screenPosNorm11.z,_ZBufferParams ) ) / ( 2.0 ) ) );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float dotResult3_g269 = dot( ase_worldViewDir , packedInput.normalWS );
				
				surfaceDescription.Alpha = ( _BaseColor.a * ( 1.0 - saturate( ( distance( _WorldSpaceCameraPos , ase_worldPos ) / _DistanceFade ) ) ) * saturate( distanceDepth11_g269 ) * saturate( dotResult3_g269 ) );
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;


				float3 V = float3(1.0, 1.0, 1.0);

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);
				outColor = _SelectionID;
			}

            ENDHLSL
        }

		Pass
		{

            Name "FullScreenDebug"
            Tags 
			{ 
				"LightMode" = "FullScreenDebug" 
			}

            Cull [_CullMode]
			ZTest LEqual
			ZWrite Off

			HLSLPROGRAM

			/*ase_pragma_before*/

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer

			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _TRANSPARENT_WRITES_MOTION_VEC

			#pragma vertex Vert
			#pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT

            #define SHADERPASS SHADERPASS_FULL_SCREEN_DEBUG
			#define SHADER_UNLIT

            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
            #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

            struct AttributesMesh
			{
				 float3 positionOS : POSITION;
				 float3 normalOS : NORMAL;
				 float4 tangentOS : TANGENT;
				#if UNITY_ANY_INSTANCING_ENABLED
				 uint instanceID : INSTANCEID_SEMANTIC;
				#endif
			};

			struct VaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
				#if UNITY_ANY_INSTANCING_ENABLED
				 uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
			};

			struct VertexDescriptionInputs
			{
				 float3 ObjectSpaceNormal;
				 float3 ObjectSpaceTangent;
				 float3 ObjectSpacePosition;
			};

			struct SurfaceDescriptionInputs
			{
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
				#if UNITY_ANY_INSTANCING_ENABLED
				 uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
			};

			PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
			{
				PackedVaryingsMeshToPS output;
				ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
				output.positionCS = input.positionCS;
				#if UNITY_ANY_INSTANCING_ENABLED
				output.instanceID = input.instanceID;
				#endif
				return output;
			}

			VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
			{
				VaryingsMeshToPS output;
				output.positionCS = input.positionCS;
				#if UNITY_ANY_INSTANCING_ENABLED
				output.instanceID = input.instanceID;
				#endif
				return output;
			}

            struct VertexDescription
			{
				float3 Position;
				float3 Normal;
				float3 Tangent;
			};

			VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
			{
				VertexDescription description = (VertexDescription)0;
				description.Position = IN.ObjectSpacePosition;
				description.Normal = IN.ObjectSpaceNormal;
				description.Tangent = IN.ObjectSpaceTangent;
				return description;
			}


            struct SurfaceDescription
			{
				float3 BaseColor;
				float3 Emission;
				float Alpha;
			};

			SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
			{
				SurfaceDescription surface = (SurfaceDescription)0;
				surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
				surface.Emission = float3(0, 0, 0);
				surface.Alpha = 1;
				return surface;
			}



			VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
			{
				VertexDescriptionInputs output;
				ZERO_INITIALIZE(VertexDescriptionInputs, output);
				output.ObjectSpaceNormal =                          input.normalOS;
				output.ObjectSpaceTangent =                         input.tangentOS.xyz;
				output.ObjectSpacePosition =                        input.positionOS;
				return output;
			}

			AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters )
			{
				VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
				VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
				input.positionOS = vertexDescription.Position;
				input.normalOS = vertexDescription.Normal;
				input.tangentOS.xyz = vertexDescription.Tangent;
				return input;
			}

			FragInputs BuildFragInputs(VaryingsMeshToPS input)
			{
				FragInputs output;
				ZERO_INITIALIZE(FragInputs, output);

				output.tangentToWorld = k_identity3x3;
				output.positionSS = input.positionCS;

				return output;
			}


			FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
			{
				UNITY_SETUP_INSTANCE_ID(input);
				VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
				return BuildFragInputs(unpacked);
			}
				SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
			{
				SurfaceDescriptionInputs output;
				ZERO_INITIALIZE(SurfaceDescriptionInputs, output);


        		return output;
			}

			#define DEBUG_DISPLAY
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/FullScreenDebug.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/VertMesh.hlsl"

			PackedVaryingsType Vert(AttributesMesh inputMesh)
			{
				VaryingsType varyingsType;
				varyingsType.vmesh = VertMesh(inputMesh);
				return PackVaryingsType(varyingsType);
			}

			#if !defined(_DEPTHOFFSET_ON)
			[earlydepthstencil]
			#endif
			void Frag(PackedVaryingsToPS packedInput)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
				FragInputs input = UnpackVaryingsToFragInputs(packedInput);

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS.xyz);

			#ifdef PLATFORM_SUPPORTS_PRIMITIVE_ID_IN_PIXEL_SHADER
				if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_QUAD_OVERDRAW)
				{
					IncrementQuadOverdrawCounter(posInput.positionSS.xy, input.primitiveID);
				}
			#endif
			}

            ENDHLSL
        }
		
	}
	
	CustomEditor "Rendering.HighDefinition.HDUnlitGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=19201
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;263;1614.556,-72.17564;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;ShadowCaster;0;1;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;264;1614.556,-72.17564;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;META;0;2;META;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;265;1614.556,-72.17564;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;SceneSelectionPass;0;3;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;266;1614.556,-72.17564;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;DepthForwardOnly;0;4;DepthForwardOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;True;True;0;True;_StencilRefDepth;255;False;;255;True;_StencilWriteMaskDepth;7;False;;3;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;False;False;True;1;LightMode=DepthForwardOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;267;1614.556,-72.17564;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;MotionVectors;0;5;MotionVectors;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;True;0;True;_StencilRefMV;255;False;;255;True;_StencilWriteMaskMV;7;False;;3;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;False;False;True;1;LightMode=MotionVectors;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;268;1614.556,-72.17564;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;DistortionVectors;0;6;DistortionVectors;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;4;1;False;;1;False;;4;1;False;;1;False;;True;1;False;;1;False;;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;True;0;True;_StencilRefDistortionVec;255;False;;255;True;_StencilWriteMaskDistortionVec;7;False;;3;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;False;True;1;LightMode=DistortionVectors;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;269;1614.556,-72.17564;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;ScenePickingPass;0;7;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;True;3;False;;False;True;1;LightMode=Picking;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;262;2295.612,103.7815;Float;False;True;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;XRay;7f5cb9c3ea6481f469fdd856555439ef;True;Forward Unlit;0;0;Forward Unlit;9;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;1;0;True;_SrcBlend;0;True;_DstBlend;1;0;True;_AlphaSrcBlend;0;True;_AlphaDstBlend;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullModeForward;False;False;False;True;True;True;True;True;0;True;_ColorMaskTransparentVel;False;False;False;False;False;True;True;0;True;_StencilRef;255;False;;255;True;_StencilWriteMask;7;False;;3;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;0;True;_ZWrite;True;0;True;_ZTestDepthEqualForOpaque;False;True;1;LightMode=ForwardOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;31;Surface Type;0;0;  Rendering Pass ;0;0;  Rendering Pass;1;0;  Blending Mode;0;0;  Receive Fog;1;0;  Distortion;0;0;    Distortion Mode;0;0;    Distortion Only;1;0;  Depth Write;1;0;  Cull Mode;0;0;  Depth Test;4;0;Double-Sided;0;0;Alpha Clipping;0;0;Receive Decals;1;0;Motion Vectors;1;0;  Add Precomputed Velocity;0;0;Shadow Matte;0;0;Cast Shadows;1;0;DOTS Instancing;0;0;GPU Instancing;1;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,;0;  Type;0;0;  Tess;16,False,;0;  Min;10,False,;0;  Max;25,False,;0;  Edge Length;16,False,;0;  Max Displacement;25,False,;0;Vertex Position,InvertActionOnDeselection;0;638352032933031592;LOD CrossFade;0;0;0;8;True;True;True;True;True;True;False;True;False;;False;0
Node;AmplifyShaderEditor.FunctionNode;276;1700.642,44.71197;Inherit;False;XRayAlpha;0;;269;ae038c7b9c7a9ca46880701b6b51a204;0;0;2;COLOR;18;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;270;668.1749,758.0126;Inherit;False;Property;_CompressibleDistance;CompressibleDistance;16;0;Create;True;0;0;0;False;0;False;0.15;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;271;653.1749,854.0126;Inherit;False;Property;_Smoothness;Smoothness;17;0;Create;True;0;0;0;False;0;False;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;274;1215.948,913.3509;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;229;638.2106,333.2184;Inherit;False;CockVoreSlurpFunction;9;;274;48b4b4d4c94c1d341abf875fe96b8fe0;0;2;49;FLOAT3;0,0,0;False;50;FLOAT3;0,0,0;False;2;FLOAT3;42;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;275;1738.592,456.7726;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;272;1069.906,530.5699;Inherit;False;PenetrableDeformation;6;;277;7ff1b70ed2c7b9e43aecbec8a912cc8c;0;4;10;FLOAT3;0,0,0;False;11;FLOAT4;0,0,0,0;False;12;FLOAT;0;False;13;FLOAT;0;False;1;FLOAT3;0
WireConnection;262;0;276;18
WireConnection;262;1;276;18
WireConnection;262;2;276;0
WireConnection;262;6;275;0
WireConnection;262;7;229;42
WireConnection;275;0;229;0
WireConnection;275;1;272;0
WireConnection;275;2;274;1
WireConnection;272;10;229;0
WireConnection;272;12;270;0
WireConnection;272;13;271;0
ASEEND*/
//CHKSM=08F165C7CE9DEFA2B5578ECC5D606BD8BCE0EEAF