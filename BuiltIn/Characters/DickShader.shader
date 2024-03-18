// Made with Amplify Shader Editor v1.9.2.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DickShader"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector]_PenetratorRootWorld("PenetratorRootWorld", Vector) = (0,0,0,0)
		[HideInInspector]_PenetratorStartWorld("PenetratorStartWorld", Vector) = (0,0,0,0)
		[HideInInspector]_PenetratorForwardWorld("PenetratorForwardWorld", Vector) = (0,0,0,0)
		[HideInInspector]_PenetratorRightWorld("PenetratorRightWorld", Vector) = (0,0,0,0)
		[HideInInspector]_PenetratorUpWorld("PenetratorUpWorld", Vector) = (0,0,0,0)
		[HideInInspector]_TruncateLength("TruncateLength", Float) = 999
		[HideInInspector]_SquashStretchCorrection("SquashStretchCorrection", Float) = 1
		[HideInInspector]_DistanceToHole("DistanceToHole", Float) = 0
		[HideInInspector]_PenetratorWorldLength("PenetratorWorldLength", Float) = 1
		[HideInInspector]_PenetratorOffsetLength("PenetratorOffsetLength", Float) = 0
		[Toggle(_TRUNCATESPHERIZE_ON)] _TruncateSpherize("TruncateSpherize", Float) = 0
		_GirthRadius("GirthRadius", Float) = 0.1
		_BaseColorMap("BaseColorMap", 2D) = "white" {}
		_MaskMap("MaskMap", 2D) = "gray" {}
		_NormalMap("NormalMap", 2D) = "bump" {}
		_BulgeBlend("BulgeBlend", Range( 0 , 1)) = 1
		_BulgeRadius("BulgeRadius", Range( 0 , 1)) = 0.58
		_WorldDickPosition("WorldDickPosition", Vector) = (0,0,0,0)
		_WorldDickNormal("WorldDickNormal", Vector) = (0,1,0,0)
		_WorldDickBinormal("WorldDickBinormal", Vector) = (0,0,1,0)
		[Toggle(_COCKVORESQUISHENABLED_ON)] _CockVoreSquishEnabled("CockVoreSquishEnabled", Float) = 0
		_Angle("Angle", Range( 0 , 89)) = 45
		_TipRadius("TipRadius", Range( 0 , 1)) = 0.1
		_DickForward("DickForward", Vector) = (0,0,1,0)
		_DickOffset("DickOffset", Vector) = (0,0,0,0)
		_BulgeProgress("BulgeProgress", Range( -1 , 3)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		[HideInInspector] _RenderQueueType("Render Queue Type", Float) = 1
		[HideInInspector][ToggleUI] _AddPrecomputedVelocity("Add Precomputed Velocity", Float) = 1
		[HideInInspector][ToggleUI] _SupportDecals("Support Decals", Float) = 1.0
		[HideInInspector] _StencilRef("Stencil Ref", Int) = 0 // StencilUsage.Clear
		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
		[HideInInspector] _StencilRefDepth("Stencil Ref Depth", Int) = 0 // Nothing
		[HideInInspector] _StencilWriteMaskDepth("Stencil Write Mask Depth", Int) = 8 // StencilUsage.TraceReflectionRay
		[HideInInspector] _StencilRefMV("Stencil Ref MV", Int) = 32 // StencilUsage.ObjectMotionVector
		[HideInInspector] _StencilWriteMaskMV("Stencil Write Mask MV", Int) = 32 // StencilUsage.ObjectMotionVector
		[HideInInspector] _StencilRefDistortionVec("Stencil Ref Distortion Vec", Int) = 4 				// DEPRECATED
		[HideInInspector] _StencilWriteMaskDistortionVec("Stencil Write Mask Distortion Vec", Int) = 4	// DEPRECATED
		[HideInInspector] _StencilWriteMaskGBuffer("Stencil Write Mask GBuffer", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
		[HideInInspector] _StencilRefGBuffer("Stencil Ref GBuffer", Int) = 2 // StencilUsage.RequiresDeferredLighting
		[HideInInspector] _ZTestGBuffer("ZTest GBuffer", Int) = 4
		[HideInInspector][ToggleUI] _RequireSplitLighting("Require Split Lighting", Float) = 0
		[HideInInspector][ToggleUI] _ReceivesSSR("Receives SSR", Float) = 1
		[HideInInspector][ToggleUI] _ReceivesSSRTransparent("Receives SSR Transparent", Float) = 0
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

		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25

		[HideInInspector][ToggleUI] _TransparentWritingMotionVec("Transparent Writing MotionVec", Float) = 0
		[HideInInspector][Enum(UnityEditor.Rendering.HighDefinition.OpaqueCullMode)] _OpaqueCullMode("Opaque Cull Mode", Int) = 2 // Back culling by default
		[HideInInspector][ToggleUI] _EnableBlendModePreserveSpecularLighting("Enable Blend Mode Preserve Specular Lighting", Float) = 1
		[HideInInspector] _EmissionColor("Color", Color) = (1, 1, 1)

		[HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}

		[HideInInspector][Enum(Auto, 0, On, 1, Off, 2)] _DoubleSidedGIMode("Double sided GI mode", Float) = 0 //DoubleSidedGIMode added in api 12x and higher

		[HideInInspector][ToggleUI] _AlphaToMaskInspectorValue("_AlphaToMaskInspectorValue", Float) = 0 // Property used to save the alpha to mask state in the inspector
        [HideInInspector][ToggleUI] _AlphaToMask("__alphaToMask", Float) = 0
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

		struct GlobalSurfaceDescription // GBuffer Forward META TransparentBackface
		{
			float3 BaseColor;
			float3 Normal;
			float3 BentNormal;
			float3 Specular;
			float CoatMask;
			float Metallic;
			float3 Emission;
			float Smoothness;
			float Occlusion;
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float AlphaClipThresholdDepthPrepass;
			float AlphaClipThresholdDepthPostpass;
			float SpecularAAScreenSpaceVariance;
			float SpecularAAThreshold;
			float SpecularOcclusion;
			float DepthOffset;
			//Refraction
			float RefractionIndex;
			float3 RefractionColor;
			float RefractionDistance;
			//SSS/Translucent
			float DiffusionProfile;
			float TransmissionMask;
			// Transmission + Diffusion Profile
			float Thickness;
			float SubsurfaceMask;
			//Anisotropy
			float Anisotropy;
			float3 Tangent;
			//Iridescent
			float IridescenceMask;
			float IridescenceThickness;
			//BakedGI
			float3 BakedGI;
			float3 BakedBackGI;
			//Virtual Texturing
			float4 VTPackedFeedback;
		};

		struct AlphaSurfaceDescription // ShadowCaster
		{
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float DepthOffset;
		};

		struct SceneSurfaceDescription // SceneSelection
		{
			float Alpha;
			float AlphaClipThreshold;
			float DepthOffset;
		};

		struct PrePassSurfaceDescription // DepthPrePass
		{
			float3 Normal;
			float Smoothness;
			float Alpha;
			float AlphaClipThresholdDepthPrepass;
			float DepthOffset;
		};

		struct PostPassSurfaceDescription //DepthPostPass
		{
			float Alpha;
			float AlphaClipThresholdDepthPostpass;
			float DepthOffset;
		};

		struct SmoothSurfaceDescription // MotionVectors DepthOnly
		{
			float3 Normal;
			float Smoothness;
			float Alpha;
			float AlphaClipThreshold;
			float DepthOffset;
		};

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
			
			Name "GBuffer"
			Tags { "LightMode"="GBuffer" }

			Cull [_CullMode]
			ZTest [_ZTestGBuffer]

			Stencil
			{
				Ref [_StencilRefGBuffer]
				WriteMask [_StencilWriteMaskGBuffer]
				Comp Always
				Pass Replace
			}


			ColorMask [_LightLayersMaskBuffer4] 4
			ColorMask [_LightLayersMaskBuffer5] 5

			HLSLPROGRAM

            #define _SPECULAR_OCCLUSION_FROM_AO 1
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #define ASE_ABSOLUTE_VERTEX_POS 1
            #define _AMBIENT_OCCLUSION 1
            #define HAVE_MESH_MODIFICATION
            #define ASE_SRP_VERSION 140008


            #pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _TRANSPARENT_WRITES_MOTION_VEC
            #pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile_fragment _ SHADOWS_SHADOWMASK
			#pragma multi_compile_fragment _ LIGHT_LAYERS
			#pragma multi_compile_fragment PROBE_VOLUMES_OFF PROBE_VOLUMES_L1 PROBE_VOLUMES_L2
			#pragma multi_compile _ DEBUG_DISPLAY
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile_fragment DECALS_OFF DECALS_3RT DECALS_4RT
			#pragma multi_compile_fragment _ DECAL_SURFACE_GRADIENT

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

			#define SHADERPASS SHADERPASS_GBUFFER

			#ifndef SHADER_UNLIT
			#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
			#define VARYINGS_NEED_CULLFACE
			#endif
			#endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

		    #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
			#undef  _REFRACTION_PLANE
			#undef  _REFRACTION_SPHERE
			#define _REFRACTION_THIN
		    #endif

			#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
			#if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
				#define WRITE_NORMAL_BUFFER
			#endif
			#endif

			#ifndef DEBUG_DISPLAY
				#if !defined(_SURFACE_TYPE_TRANSPARENT)
					#if SHADERPASS == SHADERPASS_FORWARD
					#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
					#elif SHADERPASS == SHADERPASS_GBUFFER
					#define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
					#endif
				#endif
			#endif

			#if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define _DEFERRED_CAPABLE_MATERIAL
			#endif

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			CBUFFER_START( UnityPerMaterial )
			float4 _MaskMap_ST;
			float4 _BaseColorMap_ST;
			float4 _NormalMap_ST;
			float3 _PenetratorStartWorld;
			float3 _DickForward;
			float3 _PenetratorForwardWorld;
			float3 _PenetratorUpWorld;
			float3 _DickOffset;
			float3 _PenetratorRootWorld;
			float3 _WorldDickPosition;
			float3 _WorldDickBinormal;
			float3 _PenetratorRightWorld;
			float3 _WorldDickNormal;
			float _BulgeProgress;
			float _Angle;
			float _TipRadius;
			float _BulgeRadius;
			float _BulgeBlend;
			float _SquashStretchCorrection;
			float _DistanceToHole;
			float _PenetratorWorldLength;
			float _TruncateLength;
			float _GirthRadius;
			float _PenetratorOffsetLength;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			float _EnableBlendModePreserveSpecularLighting;
            #endif
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
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			float _TessPhongStrength;
			float _TessValue;
			float _TessMin;
			float _TessMax;
			float _TessEdgeLength;
			float _TessMaxDisp;
			#endif
			CBUFFER_END

		    // Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

			// Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			sampler2D _BaseColorMap;
			sampler2D _NormalMap;
			sampler2D _MaskMap;


            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			// Setup DECALS_OFF so the shader stripper can remove variants
            #define HAVE_DECALS ( (defined(DECALS_3RT) || defined(DECALS_4RT)) && !defined(_DISABLE_DECALS) )
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_TANGENT
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#pragma multi_compile_local __ _TRUNCATESPHERIZE_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				float3 normalWS : TEXCOORD1;
				float4 tangentWS : TEXCOORD2;
				float4 uv1 : TEXCOORD3;
				float4 uv2 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
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
			
			float3 MyCustomExpression32_g265( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			
			float3x3 ChangeOfBasis169_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			
			float3x3 ChangeOfBasis9_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.baseColor =					surfaceDescription.BaseColor;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif
				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness =					surfaceDescription.Thickness;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.transmissionMask =			surfaceDescription.TransmissionMask;
				#endif
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.ior = surfaceDescription.RefractionIndex;
					surfaceData.transmittanceColor = surfaceDescription.RefractionColor;
					surfaceData.atDistance = surfaceDescription.RefractionDistance;

					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
                #ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				surfaceData.normalWS = float3(0, 1, 0);
                #endif
				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[ 2 ];
				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );

				// decals
			#ifdef DECAL_NORMAL_BLENDING
				if (_EnableDecals)
				{
					#ifndef SURFACE_GRADIENT
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);
					#endif

					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData, normalTS);
				}

				GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
			#else
				GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

				#if HAVE_DECALS
				if (_EnableDecals)
				{
					// Both uses and modifies 'surfaceData.normalWS'.
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif
			#endif
				
				bentNormalWS = surfaceData.normalWS;

				#ifdef ASE_BENT_NORMAL
				GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.tangentWS = TransformTangentToWorld( surfaceDescription.Tangent, fragInputs.tangentToWorld );
				#endif
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceData.perceptualSmoothness = GeometricNormalFiltering( surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[ 2 ], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				#endif

				#ifdef _ALPHATEST_ON
				builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				#ifdef UNITY_VIRTUAL_TEXTURING
                builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif

				#ifdef ASE_BAKEDGI
				builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif

				#ifdef ASE_BAKEDBACKGI
				builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				float3 normalizeResult27_g267 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g267 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g267 = normalize( cross( normalizeResult27_g267 , normalizeResult31_g267 ) );
				float4 appendResult26_g266 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g266 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g266 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g266 = -_WorldDickPosition;
				float4 appendResult29_g266 = (float4(break27_g266.x , break27_g266.y , break27_g266.z , 1.0));
				float4x4 temp_output_30_0_g266 = mul( transpose( float4x4( float4( normalizeResult27_g267 , 0.0 ).x,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).x,float4( normalizeResult29_g267 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g267 , 0.0 ).y,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).y,float4( normalizeResult29_g267 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g267 , 0.0 ).z,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).z,float4( normalizeResult29_g267 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g267 , 0.0 ).w,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).w,float4( normalizeResult29_g267 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g266.x,appendResult28_g266.x,appendResult31_g266.x,appendResult29_g266.x,appendResult26_g266.y,appendResult28_g266.y,appendResult31_g266.y,appendResult29_g266.y,appendResult26_g266.z,appendResult28_g266.z,appendResult31_g266.z,appendResult29_g266.z,appendResult26_g266.w,appendResult28_g266.w,appendResult31_g266.w,appendResult29_g266.w ) );
				float4x4 invertVal44_g266 = Inverse4x4( temp_output_30_0_g266 );
				float4 appendResult27_g265 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g265 = mul( GetObjectToWorldMatrix(), appendResult27_g265 ).xyz;
				float3 localMyCustomExpression32_g265 = MyCustomExpression32_g265( pos32_g265 );
				float4 appendResult32_g266 = (float4(localMyCustomExpression32_g265 , 1.0));
				float4 break35_g266 = mul( temp_output_30_0_g266, appendResult32_g266 );
				float temp_output_124_0_g266 = _TipRadius;
				float2 appendResult36_g266 = (float2(break35_g266.y , break35_g266.z));
				float2 normalizeResult41_g266 = normalize( appendResult36_g266 );
				float temp_output_120_0_g266 = sqrt( max( break35_g266.x , 0.0 ) );
				float temp_output_48_0_g266 = tan( radians( _Angle ) );
				float temp_output_125_0_g266 = ( temp_output_124_0_g266 + ( temp_output_120_0_g266 * temp_output_48_0_g266 ) );
				float temp_output_37_0_g266 = length( appendResult36_g266 );
				float temp_output_114_0_g266 = ( ( temp_output_125_0_g266 - temp_output_37_0_g266 ) + 1.0 );
				float lerpResult102_g266 = lerp( temp_output_125_0_g266 , temp_output_37_0_g266 , saturate( temp_output_114_0_g266 ));
				float lerpResult130_g266 = lerp( 0.0 , lerpResult102_g266 , saturate( ( -( -temp_output_124_0_g266 - break35_g266.x ) / temp_output_124_0_g266 ) ));
				float2 break43_g266 = ( normalizeResult41_g266 * lerpResult130_g266 );
				float4 appendResult40_g266 = (float4(max( break35_g266.x , -temp_output_124_0_g266 ) , break43_g266.x , break43_g266.y , 1.0));
				float4 appendResult28_g265 = (float4(((mul( invertVal44_g266, appendResult40_g266 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g265 = appendResult28_g265;
				(localWorldVar29_g265).xyz = GetCameraRelativePositionWS((localWorldVar29_g265).xyz);
				float4 transform29_g265 = mul(GetWorldToObjectMatrix(),localWorldVar29_g265);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g265 = (transform29_g265).xyz;
				#else
				float3 staticSwitch13_g265 = inputMesh.positionOS;
				#endif
				float3 temp_output_48_0 = staticSwitch13_g265;
				float localToCatmullRomSpace_float56_g1 = ( 0.0 );
				float3 penetratorRootWorld122_g1 = _PenetratorRootWorld;
				float3 worldPenetratorRootPos56_g1 = penetratorRootWorld122_g1;
				float3 penetratorRightWorld139_g1 = _PenetratorRightWorld;
				float3 right169_g1 = penetratorRightWorld139_g1;
				float3 penetratorUpWorld134_g1 = _PenetratorUpWorld;
				float3 up169_g1 = penetratorUpWorld134_g1;
				float3 penetratorForwardWorld126_g1 = _PenetratorForwardWorld;
				float3 forward169_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis169_g1 = ChangeOfBasis169_g1( right169_g1 , up169_g1 , forward169_g1 );
				float3 right9_g1 = penetratorRightWorld139_g1;
				float3 up9_g1 = penetratorUpWorld134_g1;
				float3 forward9_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis9_g1 = ChangeOfBasis9_g1( right9_g1 , up9_g1 , forward9_g1 );
				float3 normalizeResult37 = normalize( _DickForward );
				float3 temp_output_36_0 = ( ( _BulgeProgress * normalizeResult37 ) + _DickOffset );
				float3 temp_output_3_0_g2 = temp_output_36_0;
				float3 normalizeResult6_g2 = normalize( ( temp_output_48_0 - temp_output_3_0_g2 ) );
				float3 temp_output_28_0 = ( temp_output_48_0 - temp_output_36_0 );
				float temp_output_41_0 = ( saturate( ( ( _BulgeRadius - length( temp_output_28_0 ) ) * 10.0 ) ) * inputMesh.ase_color.g * _BulgeBlend );
				float3 lerpResult33 = lerp( temp_output_48_0 , ( ( normalizeResult6_g2 * _BulgeRadius ) + temp_output_3_0_g2 ) , temp_output_41_0);
				float4 appendResult67_g1 = (float4(lerpResult33 , 1.0));
				float4 transform66_g1 = mul(GetObjectToWorldMatrix(),appendResult67_g1);
				transform66_g1.xyz = GetAbsolutePositionWS((transform66_g1).xyz);
				float3 localPenetratorSpaceVertexPosition142_g1 = ( (transform66_g1).xyz - ( _PenetratorStartWorld - penetratorRootWorld122_g1 ) );
				float3 temp_output_12_0_g1 = mul( localChangeOfBasis9_g1, ( localPenetratorSpaceVertexPosition142_g1 - penetratorRootWorld122_g1 ) );
				float3 break15_g1 = temp_output_12_0_g1;
				float temp_output_18_0_g1 = ( break15_g1.z * _SquashStretchCorrection );
				float3 appendResult26_g1 = (float3(break15_g1.x , break15_g1.y , temp_output_18_0_g1));
				float3 appendResult25_g1 = (float3(( break15_g1.x / _SquashStretchCorrection ) , ( break15_g1.y / _SquashStretchCorrection ) , temp_output_18_0_g1));
				float distanceToHole180_g1 = _DistanceToHole;
				float temp_output_17_0_g1 = ( distanceToHole180_g1 * 0.5 );
				float smoothstepResult23_g1 = smoothstep( 0.0 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float smoothstepResult22_g1 = smoothstep( distanceToHole180_g1 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float3 lerpResult31_g1 = lerp( appendResult26_g1 , appendResult25_g1 , min( smoothstepResult23_g1 , smoothstepResult22_g1 ));
				float3 lerpResult32_g1 = lerp( lerpResult31_g1 , ( temp_output_12_0_g1 + ( ( distanceToHole180_g1 - ( ( distanceToHole180_g1 / ( _SquashStretchCorrection * _PenetratorWorldLength ) ) * _PenetratorWorldLength ) ) * float3(0,0,1) ) ) , step( distanceToHole180_g1 , temp_output_18_0_g1 ));
				float3 squashStretchedPosition44_g1 = lerpResult32_g1;
				float3 temp_output_150_0_g1 = ( float3(0,0,1) * _TruncateLength );
				float3 temp_output_149_0_g1 = ( squashStretchedPosition44_g1 - temp_output_150_0_g1 );
				float3 normalizeResult156_g1 = normalize( temp_output_149_0_g1 );
				float3 lerpResult152_g1 = lerp( temp_output_149_0_g1 , ( normalizeResult156_g1 * min( length( temp_output_149_0_g1 ) , _GirthRadius ) ) , saturate( ( temp_output_149_0_g1.z * ( 1.0 / _GirthRadius ) ) ));
				#ifdef _TRUNCATESPHERIZE_ON
				float3 staticSwitch116_g1 = ( lerpResult152_g1 + temp_output_150_0_g1 );
				#else
				float3 staticSwitch116_g1 = squashStretchedPosition44_g1;
				#endif
				float3 TruncatedPosition147_g1 = ( penetratorRootWorld122_g1 + mul( transpose( localChangeOfBasis169_g1 ), staticSwitch116_g1 ) );
				float3 worldPosition56_g1 = ( TruncatedPosition147_g1 + ( penetratorForwardWorld126_g1 * _PenetratorOffsetLength ) );
				float3 worldPenetratorForward56_g1 = penetratorForwardWorld126_g1;
				float3 worldPenetratorUp56_g1 = penetratorUpWorld134_g1;
				float3 worldPenetratorRight56_g1 = penetratorRightWorld139_g1;
				float3 temp_output_50_0_g265 = inputMesh.normalOS;
				float2 break146_g266 = normalizeResult41_g266;
				float4 appendResult139_g266 = (float4(temp_output_48_0_g266 , break146_g266.x , break146_g266.y , 0.0));
				float3 normalizeResult144_g266 = normalize( (mul( invertVal44_g266, appendResult139_g266 )).xyz );
				float3 lerpResult44_g265 = lerp( normalizeResult144_g266 , temp_output_50_0_g265 , saturate( sign( temp_output_114_0_g266 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g265 = lerpResult44_g265;
				#else
				float3 staticSwitch17_g265 = temp_output_50_0_g265;
				#endif
				float3 temp_output_48_42 = staticSwitch17_g265;
				float3 normalizeResult38 = normalize( temp_output_28_0 );
				float dotResult50 = dot( temp_output_28_0 , normalizeResult37 );
				float3 lerpResult39 = lerp( temp_output_48_42 , normalizeResult38 , ( temp_output_41_0 * ( 1.0 - saturate( abs( dotResult50 ) ) ) ));
				float3 normalizeResult44 = normalize( lerpResult39 );
				float4 appendResult86_g1 = (float4(normalizeResult44 , 0.0));
				float3 normalizeResult87_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult86_g1 )).xyz );
				float3 worldNormal56_g1 = normalizeResult87_g1;
				float4 break93_g1 = inputMesh.tangentOS;
				float4 appendResult89_g1 = (float4(break93_g1.x , break93_g1.y , break93_g1.z , 0.0));
				float3 normalizeResult91_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult89_g1 )).xyz );
				float4 appendResult94_g1 = (float4(normalizeResult91_g1 , break93_g1.w));
				float4 worldTangent56_g1 = appendResult94_g1;
				float3 worldPositionOUT56_g1 = float3( 0,0,0 );
				float3 worldNormalOUT56_g1 = float3( 0,0,0 );
				float4 worldTangentOUT56_g1 = float4( 0,0,0,0 );
				{
				ToCatmullRomSpace_float(worldPenetratorRootPos56_g1,worldPosition56_g1,worldPenetratorForward56_g1,worldPenetratorUp56_g1,worldPenetratorRight56_g1,worldNormal56_g1,worldTangent56_g1,worldPositionOUT56_g1,worldNormalOUT56_g1,worldTangentOUT56_g1);
				}
				float4 appendResult73_g1 = (float4(worldPositionOUT56_g1 , 1.0));
				float4 localWorldVar72_g1 = appendResult73_g1;
				(localWorldVar72_g1).xyz = GetCameraRelativePositionWS((localWorldVar72_g1).xyz);
				float4 transform72_g1 = mul(GetWorldToObjectMatrix(),localWorldVar72_g1);
				float3 lerpResult15 = lerp( temp_output_48_0 , (transform72_g1).xyz , inputMesh.ase_color.r);
				
				float4 appendResult75_g1 = (float4(worldNormalOUT56_g1 , 0.0));
				float3 normalizeResult76_g1 = normalize( (mul( GetWorldToObjectMatrix(), appendResult75_g1 )).xyz );
				float3 lerpResult17 = lerp( temp_output_48_42 , normalizeResult76_g1 , inputMesh.ase_color.r);
				
				float4 break79_g1 = worldTangentOUT56_g1;
				float4 appendResult77_g1 = (float4(break79_g1.x , break79_g1.y , break79_g1.z , 0.0));
				float3 normalizeResult80_g1 = normalize( (mul( GetWorldToObjectMatrix(), appendResult77_g1 )).xyz );
				float4 appendResult83_g1 = (float4(normalizeResult80_g1 , break79_g1.w));
				float4 lerpResult20 = lerp( inputMesh.tangentOS , appendResult83_g1 , inputMesh.ase_color.r);
				
				outputPackedVaryingsMeshToPS.ase_texcoord5.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord5.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = lerpResult15;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = lerpResult17;
				inputMesh.tangentOS = lerpResult20;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.positionRWS.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.normalWS.xyz = normalWS;
				outputPackedVaryingsMeshToPS.tangentWS.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.uv1.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.uv2.xyzw = inputMesh.uv2;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
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
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
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
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput,
						OUTPUT_GBUFFER(outGBuffer)
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
				float3 positionRWS = packedInput.positionRWS.xyz;
				float3 normalWS = packedInput.normalWS.xyz;
				float4 tangentWS = packedInput.tangentWS.xyzw;

				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);
				input.texCoord1 = packedInput.uv1.xyzw;
				input.texCoord2 = packedInput.uv2.xyzw;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);
				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);
				SurfaceData surfaceData;
				BuiltinData builtinData;

				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				float2 uv_BaseColorMap = packedInput.ase_texcoord5.xy * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw;
				
				float2 uv_NormalMap = packedInput.ase_texcoord5.xy * _NormalMap_ST.xy + _NormalMap_ST.zw;
				
				float2 uv_MaskMap = packedInput.ase_texcoord5.xy * _MaskMap_ST.xy + _MaskMap_ST.zw;
				float4 tex2DNode46 = tex2D( _MaskMap, uv_MaskMap );
				
				surfaceDescription.BaseColor = tex2D( _BaseColorMap, uv_BaseColorMap ).rgb;
				surfaceDescription.Normal = UnpackNormalScale( tex2D( _NormalMap, uv_NormalMap ), 1.0f );
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = tex2DNode46.r;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = 0;
				surfaceDescription.Smoothness = tex2DNode46.a;
				surfaceDescription.Occlusion = tex2DNode46.g;
				surfaceDescription.Alpha = 1;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
				surfaceDescription.AlphaClipThresholdShadow = 0.5;
				#endif

				surfaceDescription.AlphaClipThresholdDepthPrepass = 0.5;
				surfaceDescription.AlphaClipThresholdDepthPostpass = 0.5;

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 0;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceDescription.TransmissionMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				#ifdef ASE_BAKEDGI
				surfaceDescription.BakedGI = 0;
				#endif
				#ifdef ASE_BAKEDBACKGI
				surfaceDescription.BakedBackGI = 0;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				#ifdef UNITY_VIRTUAL_TEXTURING
				surfaceDescription.VTPackedFeedback = float4(1.0f,1.0f,1.0f,1.0f);
				#endif

				GetSurfaceAndBuiltinData( surfaceDescription, input, V, posInput, surfaceData, builtinData );
				ENCODE_INTO_GBUFFER( surfaceData, builtinData, posInput.positionSS, outGBuffer );
				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
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

			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _AMBIENT_OCCLUSION 1
			#define HAVE_MESH_MODIFICATION
			#define ASE_SRP_VERSION 140008


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _TRANSPARENT_WRITES_MOTION_VEC
			#pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma shader_feature EDITOR_VISUALIZATION

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

			#define SHADERPASS SHADERPASS_LIGHT_TRANSPORT

			#ifndef SHADER_UNLIT
			#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
			#define VARYINGS_NEED_CULLFACE
			#endif
			#endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

			#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
			#if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
				#define WRITE_NORMAL_BUFFER
			#endif
			#endif

			#ifndef DEBUG_DISPLAY
				#if !defined(_SURFACE_TYPE_TRANSPARENT)
					#if SHADERPASS == SHADERPASS_FORWARD
					#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
					#elif SHADERPASS == SHADERPASS_GBUFFER
					#define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
					#endif
				#endif
			#endif

			#if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define _DEFERRED_CAPABLE_MATERIAL
			#endif

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			CBUFFER_START( UnityPerMaterial )
			float4 _MaskMap_ST;
			float4 _BaseColorMap_ST;
			float4 _NormalMap_ST;
			float3 _PenetratorStartWorld;
			float3 _DickForward;
			float3 _PenetratorForwardWorld;
			float3 _PenetratorUpWorld;
			float3 _DickOffset;
			float3 _PenetratorRootWorld;
			float3 _WorldDickPosition;
			float3 _WorldDickBinormal;
			float3 _PenetratorRightWorld;
			float3 _WorldDickNormal;
			float _BulgeProgress;
			float _Angle;
			float _TipRadius;
			float _BulgeRadius;
			float _BulgeBlend;
			float _SquashStretchCorrection;
			float _DistanceToHole;
			float _PenetratorWorldLength;
			float _TruncateLength;
			float _GirthRadius;
			float _PenetratorOffsetLength;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			float _EnableBlendModePreserveSpecularLighting;
            #endif
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
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			float _TessPhongStrength;
			float _TessValue;
			float _TessMin;
			float _TessMax;
			float _TessEdgeLength;
			float _TessMaxDisp;
			#endif
			CBUFFER_END

			// Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

			// Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			sampler2D _BaseColorMap;
			sampler2D _NormalMap;
			sampler2D _MaskMap;


            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			// Setup DECALS_OFF so the shader stripper can remove variants
            #define HAVE_DECALS ( (defined(DECALS_3RT) || defined(DECALS_4RT)) && !defined(_DISABLE_DECALS) )
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_TANGENT
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#pragma multi_compile_local __ _TRUNCATESPHERIZE_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 uv3 : TEXCOORD3;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_Position;
				#ifdef EDITOR_VISUALIZATION
				float2 VizUV : TEXCOORD0;
				float4 LightCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
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
			
			float3 MyCustomExpression32_g265( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			
			float3x3 ChangeOfBasis169_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			
			float3x3 ChangeOfBasis9_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.baseColor =					surfaceDescription.BaseColor;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif
				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness = 				surfaceDescription.Thickness;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.transmissionMask =			surfaceDescription.TransmissionMask;
				#endif
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.ior = surfaceDescription.RefractionIndex;
					surfaceData.transmittanceColor = surfaceDescription.RefractionColor;
					surfaceData.atDistance = surfaceDescription.RefractionDistance;

					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif

				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
                #ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				surfaceData.normalWS = float3(0, 1, 0);
                #endif
				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );

				// decals
			#ifdef DECAL_NORMAL_BLENDING
				if (_EnableDecals)
				{
					#ifndef SURFACE_GRADIENT
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);
					#endif

					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData, normalTS);
				}

				GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
			#else
				GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

				#if HAVE_DECALS
				if (_EnableDecals)
				{
					// Both uses and modifies 'surfaceData.normalWS'.
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif
			#endif

				bentNormalWS = surfaceData.normalWS;

				#ifdef ASE_BENT_NORMAL
				GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.tangentWS = TransformTangentToWorld( surfaceDescription.Tangent, fragInputs.tangentToWorld );
				#endif
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceData.perceptualSmoothness = GeometricNormalFiltering( surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[ 2 ], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				builtinData.emissiveColor = surfaceDescription.Emission;

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			#if SHADERPASS == SHADERPASS_LIGHT_TRANSPORT
			#define SCENEPICKINGPASS
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/MetaPass.hlsl"

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh  )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);

				float3 normalizeResult27_g267 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g267 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g267 = normalize( cross( normalizeResult27_g267 , normalizeResult31_g267 ) );
				float4 appendResult26_g266 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g266 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g266 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g266 = -_WorldDickPosition;
				float4 appendResult29_g266 = (float4(break27_g266.x , break27_g266.y , break27_g266.z , 1.0));
				float4x4 temp_output_30_0_g266 = mul( transpose( float4x4( float4( normalizeResult27_g267 , 0.0 ).x,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).x,float4( normalizeResult29_g267 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g267 , 0.0 ).y,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).y,float4( normalizeResult29_g267 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g267 , 0.0 ).z,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).z,float4( normalizeResult29_g267 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g267 , 0.0 ).w,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).w,float4( normalizeResult29_g267 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g266.x,appendResult28_g266.x,appendResult31_g266.x,appendResult29_g266.x,appendResult26_g266.y,appendResult28_g266.y,appendResult31_g266.y,appendResult29_g266.y,appendResult26_g266.z,appendResult28_g266.z,appendResult31_g266.z,appendResult29_g266.z,appendResult26_g266.w,appendResult28_g266.w,appendResult31_g266.w,appendResult29_g266.w ) );
				float4x4 invertVal44_g266 = Inverse4x4( temp_output_30_0_g266 );
				float4 appendResult27_g265 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g265 = mul( GetObjectToWorldMatrix(), appendResult27_g265 ).xyz;
				float3 localMyCustomExpression32_g265 = MyCustomExpression32_g265( pos32_g265 );
				float4 appendResult32_g266 = (float4(localMyCustomExpression32_g265 , 1.0));
				float4 break35_g266 = mul( temp_output_30_0_g266, appendResult32_g266 );
				float temp_output_124_0_g266 = _TipRadius;
				float2 appendResult36_g266 = (float2(break35_g266.y , break35_g266.z));
				float2 normalizeResult41_g266 = normalize( appendResult36_g266 );
				float temp_output_120_0_g266 = sqrt( max( break35_g266.x , 0.0 ) );
				float temp_output_48_0_g266 = tan( radians( _Angle ) );
				float temp_output_125_0_g266 = ( temp_output_124_0_g266 + ( temp_output_120_0_g266 * temp_output_48_0_g266 ) );
				float temp_output_37_0_g266 = length( appendResult36_g266 );
				float temp_output_114_0_g266 = ( ( temp_output_125_0_g266 - temp_output_37_0_g266 ) + 1.0 );
				float lerpResult102_g266 = lerp( temp_output_125_0_g266 , temp_output_37_0_g266 , saturate( temp_output_114_0_g266 ));
				float lerpResult130_g266 = lerp( 0.0 , lerpResult102_g266 , saturate( ( -( -temp_output_124_0_g266 - break35_g266.x ) / temp_output_124_0_g266 ) ));
				float2 break43_g266 = ( normalizeResult41_g266 * lerpResult130_g266 );
				float4 appendResult40_g266 = (float4(max( break35_g266.x , -temp_output_124_0_g266 ) , break43_g266.x , break43_g266.y , 1.0));
				float4 appendResult28_g265 = (float4(((mul( invertVal44_g266, appendResult40_g266 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g265 = appendResult28_g265;
				(localWorldVar29_g265).xyz = GetCameraRelativePositionWS((localWorldVar29_g265).xyz);
				float4 transform29_g265 = mul(GetWorldToObjectMatrix(),localWorldVar29_g265);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g265 = (transform29_g265).xyz;
				#else
				float3 staticSwitch13_g265 = inputMesh.positionOS;
				#endif
				float3 temp_output_48_0 = staticSwitch13_g265;
				float localToCatmullRomSpace_float56_g1 = ( 0.0 );
				float3 penetratorRootWorld122_g1 = _PenetratorRootWorld;
				float3 worldPenetratorRootPos56_g1 = penetratorRootWorld122_g1;
				float3 penetratorRightWorld139_g1 = _PenetratorRightWorld;
				float3 right169_g1 = penetratorRightWorld139_g1;
				float3 penetratorUpWorld134_g1 = _PenetratorUpWorld;
				float3 up169_g1 = penetratorUpWorld134_g1;
				float3 penetratorForwardWorld126_g1 = _PenetratorForwardWorld;
				float3 forward169_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis169_g1 = ChangeOfBasis169_g1( right169_g1 , up169_g1 , forward169_g1 );
				float3 right9_g1 = penetratorRightWorld139_g1;
				float3 up9_g1 = penetratorUpWorld134_g1;
				float3 forward9_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis9_g1 = ChangeOfBasis9_g1( right9_g1 , up9_g1 , forward9_g1 );
				float3 normalizeResult37 = normalize( _DickForward );
				float3 temp_output_36_0 = ( ( _BulgeProgress * normalizeResult37 ) + _DickOffset );
				float3 temp_output_3_0_g2 = temp_output_36_0;
				float3 normalizeResult6_g2 = normalize( ( temp_output_48_0 - temp_output_3_0_g2 ) );
				float3 temp_output_28_0 = ( temp_output_48_0 - temp_output_36_0 );
				float temp_output_41_0 = ( saturate( ( ( _BulgeRadius - length( temp_output_28_0 ) ) * 10.0 ) ) * inputMesh.ase_color.g * _BulgeBlend );
				float3 lerpResult33 = lerp( temp_output_48_0 , ( ( normalizeResult6_g2 * _BulgeRadius ) + temp_output_3_0_g2 ) , temp_output_41_0);
				float4 appendResult67_g1 = (float4(lerpResult33 , 1.0));
				float4 transform66_g1 = mul(GetObjectToWorldMatrix(),appendResult67_g1);
				transform66_g1.xyz = GetAbsolutePositionWS((transform66_g1).xyz);
				float3 localPenetratorSpaceVertexPosition142_g1 = ( (transform66_g1).xyz - ( _PenetratorStartWorld - penetratorRootWorld122_g1 ) );
				float3 temp_output_12_0_g1 = mul( localChangeOfBasis9_g1, ( localPenetratorSpaceVertexPosition142_g1 - penetratorRootWorld122_g1 ) );
				float3 break15_g1 = temp_output_12_0_g1;
				float temp_output_18_0_g1 = ( break15_g1.z * _SquashStretchCorrection );
				float3 appendResult26_g1 = (float3(break15_g1.x , break15_g1.y , temp_output_18_0_g1));
				float3 appendResult25_g1 = (float3(( break15_g1.x / _SquashStretchCorrection ) , ( break15_g1.y / _SquashStretchCorrection ) , temp_output_18_0_g1));
				float distanceToHole180_g1 = _DistanceToHole;
				float temp_output_17_0_g1 = ( distanceToHole180_g1 * 0.5 );
				float smoothstepResult23_g1 = smoothstep( 0.0 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float smoothstepResult22_g1 = smoothstep( distanceToHole180_g1 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float3 lerpResult31_g1 = lerp( appendResult26_g1 , appendResult25_g1 , min( smoothstepResult23_g1 , smoothstepResult22_g1 ));
				float3 lerpResult32_g1 = lerp( lerpResult31_g1 , ( temp_output_12_0_g1 + ( ( distanceToHole180_g1 - ( ( distanceToHole180_g1 / ( _SquashStretchCorrection * _PenetratorWorldLength ) ) * _PenetratorWorldLength ) ) * float3(0,0,1) ) ) , step( distanceToHole180_g1 , temp_output_18_0_g1 ));
				float3 squashStretchedPosition44_g1 = lerpResult32_g1;
				float3 temp_output_150_0_g1 = ( float3(0,0,1) * _TruncateLength );
				float3 temp_output_149_0_g1 = ( squashStretchedPosition44_g1 - temp_output_150_0_g1 );
				float3 normalizeResult156_g1 = normalize( temp_output_149_0_g1 );
				float3 lerpResult152_g1 = lerp( temp_output_149_0_g1 , ( normalizeResult156_g1 * min( length( temp_output_149_0_g1 ) , _GirthRadius ) ) , saturate( ( temp_output_149_0_g1.z * ( 1.0 / _GirthRadius ) ) ));
				#ifdef _TRUNCATESPHERIZE_ON
				float3 staticSwitch116_g1 = ( lerpResult152_g1 + temp_output_150_0_g1 );
				#else
				float3 staticSwitch116_g1 = squashStretchedPosition44_g1;
				#endif
				float3 TruncatedPosition147_g1 = ( penetratorRootWorld122_g1 + mul( transpose( localChangeOfBasis169_g1 ), staticSwitch116_g1 ) );
				float3 worldPosition56_g1 = ( TruncatedPosition147_g1 + ( penetratorForwardWorld126_g1 * _PenetratorOffsetLength ) );
				float3 worldPenetratorForward56_g1 = penetratorForwardWorld126_g1;
				float3 worldPenetratorUp56_g1 = penetratorUpWorld134_g1;
				float3 worldPenetratorRight56_g1 = penetratorRightWorld139_g1;
				float3 temp_output_50_0_g265 = inputMesh.normalOS;
				float2 break146_g266 = normalizeResult41_g266;
				float4 appendResult139_g266 = (float4(temp_output_48_0_g266 , break146_g266.x , break146_g266.y , 0.0));
				float3 normalizeResult144_g266 = normalize( (mul( invertVal44_g266, appendResult139_g266 )).xyz );
				float3 lerpResult44_g265 = lerp( normalizeResult144_g266 , temp_output_50_0_g265 , saturate( sign( temp_output_114_0_g266 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g265 = lerpResult44_g265;
				#else
				float3 staticSwitch17_g265 = temp_output_50_0_g265;
				#endif
				float3 temp_output_48_42 = staticSwitch17_g265;
				float3 normalizeResult38 = normalize( temp_output_28_0 );
				float dotResult50 = dot( temp_output_28_0 , normalizeResult37 );
				float3 lerpResult39 = lerp( temp_output_48_42 , normalizeResult38 , ( temp_output_41_0 * ( 1.0 - saturate( abs( dotResult50 ) ) ) ));
				float3 normalizeResult44 = normalize( lerpResult39 );
				float4 appendResult86_g1 = (float4(normalizeResult44 , 0.0));
				float3 normalizeResult87_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult86_g1 )).xyz );
				float3 worldNormal56_g1 = normalizeResult87_g1;
				float4 break93_g1 = inputMesh.tangentOS;
				float4 appendResult89_g1 = (float4(break93_g1.x , break93_g1.y , break93_g1.z , 0.0));
				float3 normalizeResult91_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult89_g1 )).xyz );
				float4 appendResult94_g1 = (float4(normalizeResult91_g1 , break93_g1.w));
				float4 worldTangent56_g1 = appendResult94_g1;
				float3 worldPositionOUT56_g1 = float3( 0,0,0 );
				float3 worldNormalOUT56_g1 = float3( 0,0,0 );
				float4 worldTangentOUT56_g1 = float4( 0,0,0,0 );
				{
				ToCatmullRomSpace_float(worldPenetratorRootPos56_g1,worldPosition56_g1,worldPenetratorForward56_g1,worldPenetratorUp56_g1,worldPenetratorRight56_g1,worldNormal56_g1,worldTangent56_g1,worldPositionOUT56_g1,worldNormalOUT56_g1,worldTangentOUT56_g1);
				}
				float4 appendResult73_g1 = (float4(worldPositionOUT56_g1 , 1.0));
				float4 localWorldVar72_g1 = appendResult73_g1;
				(localWorldVar72_g1).xyz = GetCameraRelativePositionWS((localWorldVar72_g1).xyz);
				float4 transform72_g1 = mul(GetWorldToObjectMatrix(),localWorldVar72_g1);
				float3 lerpResult15 = lerp( temp_output_48_0 , (transform72_g1).xyz , inputMesh.ase_color.r);
				
				float4 appendResult75_g1 = (float4(worldNormalOUT56_g1 , 0.0));
				float3 normalizeResult76_g1 = normalize( (mul( GetWorldToObjectMatrix(), appendResult75_g1 )).xyz );
				float3 lerpResult17 = lerp( temp_output_48_42 , normalizeResult76_g1 , inputMesh.ase_color.r);
				
				float4 break79_g1 = worldTangentOUT56_g1;
				float4 appendResult77_g1 = (float4(break79_g1.x , break79_g1.y , break79_g1.z , 0.0));
				float3 normalizeResult80_g1 = normalize( (mul( GetWorldToObjectMatrix(), appendResult77_g1 )).xyz );
				float4 appendResult83_g1 = (float4(normalizeResult80_g1 , break79_g1.w));
				float4 lerpResult20 = lerp( inputMesh.tangentOS , appendResult83_g1 , inputMesh.ase_color.r);
				
				outputPackedVaryingsMeshToPS.ase_texcoord2.xy = inputMesh.uv0.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord2.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = lerpResult15;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = lerpResult17;
				inputMesh.tangentOS = lerpResult20;

				outputPackedVaryingsMeshToPS.positionCS = UnityMetaVertexPosition(inputMesh.positionOS, inputMesh.uv1.xy, inputMesh.uv2.xy, unity_LightmapST, unity_DynamicLightmapST);


				#ifdef EDITOR_VISUALIZATION
					float2 vizUV = 0;
					float4 lightCoord = 0;
					UnityEditorVizData(inputMesh.positionOS.xyz, inputMesh.uv0.xy, inputMesh.uv1.xy, inputMesh.uv2.xy, vizUV, lightCoord);

					outputPackedVaryingsMeshToPS.VizUV.xy = vizUV;
					outputPackedVaryingsMeshToPS.LightCoord = lightCoord;
				#endif

				return outputPackedVaryingsMeshToPS;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
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

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
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
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
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
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			float4 Frag(PackedVaryingsMeshToPS packedInput  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);
				float3 V = float3(1.0, 1.0, 1.0);

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				float2 uv_BaseColorMap = packedInput.ase_texcoord2.xy * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw;
				
				float2 uv_NormalMap = packedInput.ase_texcoord2.xy * _NormalMap_ST.xy + _NormalMap_ST.zw;
				
				float2 uv_MaskMap = packedInput.ase_texcoord2.xy * _MaskMap_ST.xy + _MaskMap_ST.zw;
				float4 tex2DNode46 = tex2D( _MaskMap, uv_MaskMap );
				
				surfaceDescription.BaseColor = tex2D( _BaseColorMap, uv_BaseColorMap ).rgb;
				surfaceDescription.Normal = UnpackNormalScale( tex2D( _NormalMap, uv_NormalMap ), 1.0f );
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = tex2DNode46.r;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = 0;
				surfaceDescription.Smoothness = tex2DNode46.a;
				surfaceDescription.Occlusion = tex2DNode46.g;
				surfaceDescription.Alpha = 1;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceDescription.TransmissionMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);
				LightTransportData lightTransportData = GetLightTransportData(surfaceData, builtinData, bsdfData);

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
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			Cull [_CullMode]
			ZWrite On
			ZClip [_ZClip]
			ZTest LEqual
			ColorMask 0

			HLSLPROGRAM

			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _AMBIENT_OCCLUSION 1
			#define HAVE_MESH_MODIFICATION
			#define ASE_SRP_VERSION 140008


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _TRANSPARENT_WRITES_MOTION_VEC
			#pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile_fragment _ SHADOWS_SHADOWMASK

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			//#define USE_LEGACY_UNITY_MATRIX_VARIABLES

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

			#define SHADERPASS SHADERPASS_SHADOWS

			#ifndef SHADER_UNLIT
			#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
			#define VARYINGS_NEED_CULLFACE
			#endif
			#endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

		    #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
			#undef  _REFRACTION_PLANE
			#undef  _REFRACTION_SPHERE
			#define _REFRACTION_THIN
		    #endif

			#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
			#if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
			#define WRITE_NORMAL_BUFFER
			#endif
			#endif

			#ifndef DEBUG_DISPLAY
				#if !defined(_SURFACE_TYPE_TRANSPARENT)
					#if SHADERPASS == SHADERPASS_FORWARD
					#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
					#elif SHADERPASS == SHADERPASS_GBUFFER
					#define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
					#endif
				#endif
			#endif

			#if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define _DEFERRED_CAPABLE_MATERIAL
			#endif

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			CBUFFER_START( UnityPerMaterial )
			float4 _MaskMap_ST;
			float4 _BaseColorMap_ST;
			float4 _NormalMap_ST;
			float3 _PenetratorStartWorld;
			float3 _DickForward;
			float3 _PenetratorForwardWorld;
			float3 _PenetratorUpWorld;
			float3 _DickOffset;
			float3 _PenetratorRootWorld;
			float3 _WorldDickPosition;
			float3 _WorldDickBinormal;
			float3 _PenetratorRightWorld;
			float3 _WorldDickNormal;
			float _BulgeProgress;
			float _Angle;
			float _TipRadius;
			float _BulgeRadius;
			float _BulgeBlend;
			float _SquashStretchCorrection;
			float _DistanceToHole;
			float _PenetratorWorldLength;
			float _TruncateLength;
			float _GirthRadius;
			float _PenetratorOffsetLength;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			float _EnableBlendModePreserveSpecularLighting;
            #endif
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
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			float _TessPhongStrength;
			float _TessValue;
			float _TessMin;
			float _TessMax;
			float _TessEdgeLength;
			float _TessMaxDisp;
			#endif
			CBUFFER_END

			// Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

			// Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			

            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			// Setup DECALS_OFF so the shader stripper can remove variants
            #define HAVE_DECALS ( (defined(DECALS_3RT) || defined(DECALS_4RT)) && !defined(_DISABLE_DECALS) )
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#pragma multi_compile_local __ _TRUNCATESPHERIZE_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_tangent : TANGENT;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
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
			
			float3 MyCustomExpression32_g265( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			
			float3x3 ChangeOfBasis169_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			
			float3x3 ChangeOfBasis9_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout AlphaSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
                #ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				surfaceData.normalWS = float3(0, 1, 0);
                #endif
				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );

				// decals
			#ifdef DECAL_NORMAL_BLENDING
				if (_EnableDecals)
				{
					#ifndef SURFACE_GRADIENT
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);
					#endif

					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData, normalTS);
				}

				GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
			#else
				GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

				#if HAVE_DECALS
				if (_EnableDecals)
				{
					// Both uses and modifies 'surfaceData.normalWS'.
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif
			#endif

				bentNormalWS = surfaceData.normalWS;
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(AlphaSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				#ifdef _ALPHATEST_SHADOW_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow );
				#else
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				float3 normalizeResult27_g267 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g267 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g267 = normalize( cross( normalizeResult27_g267 , normalizeResult31_g267 ) );
				float4 appendResult26_g266 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g266 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g266 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g266 = -_WorldDickPosition;
				float4 appendResult29_g266 = (float4(break27_g266.x , break27_g266.y , break27_g266.z , 1.0));
				float4x4 temp_output_30_0_g266 = mul( transpose( float4x4( float4( normalizeResult27_g267 , 0.0 ).x,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).x,float4( normalizeResult29_g267 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g267 , 0.0 ).y,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).y,float4( normalizeResult29_g267 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g267 , 0.0 ).z,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).z,float4( normalizeResult29_g267 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g267 , 0.0 ).w,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).w,float4( normalizeResult29_g267 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g266.x,appendResult28_g266.x,appendResult31_g266.x,appendResult29_g266.x,appendResult26_g266.y,appendResult28_g266.y,appendResult31_g266.y,appendResult29_g266.y,appendResult26_g266.z,appendResult28_g266.z,appendResult31_g266.z,appendResult29_g266.z,appendResult26_g266.w,appendResult28_g266.w,appendResult31_g266.w,appendResult29_g266.w ) );
				float4x4 invertVal44_g266 = Inverse4x4( temp_output_30_0_g266 );
				float4 appendResult27_g265 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g265 = mul( GetObjectToWorldMatrix(), appendResult27_g265 ).xyz;
				float3 localMyCustomExpression32_g265 = MyCustomExpression32_g265( pos32_g265 );
				float4 appendResult32_g266 = (float4(localMyCustomExpression32_g265 , 1.0));
				float4 break35_g266 = mul( temp_output_30_0_g266, appendResult32_g266 );
				float temp_output_124_0_g266 = _TipRadius;
				float2 appendResult36_g266 = (float2(break35_g266.y , break35_g266.z));
				float2 normalizeResult41_g266 = normalize( appendResult36_g266 );
				float temp_output_120_0_g266 = sqrt( max( break35_g266.x , 0.0 ) );
				float temp_output_48_0_g266 = tan( radians( _Angle ) );
				float temp_output_125_0_g266 = ( temp_output_124_0_g266 + ( temp_output_120_0_g266 * temp_output_48_0_g266 ) );
				float temp_output_37_0_g266 = length( appendResult36_g266 );
				float temp_output_114_0_g266 = ( ( temp_output_125_0_g266 - temp_output_37_0_g266 ) + 1.0 );
				float lerpResult102_g266 = lerp( temp_output_125_0_g266 , temp_output_37_0_g266 , saturate( temp_output_114_0_g266 ));
				float lerpResult130_g266 = lerp( 0.0 , lerpResult102_g266 , saturate( ( -( -temp_output_124_0_g266 - break35_g266.x ) / temp_output_124_0_g266 ) ));
				float2 break43_g266 = ( normalizeResult41_g266 * lerpResult130_g266 );
				float4 appendResult40_g266 = (float4(max( break35_g266.x , -temp_output_124_0_g266 ) , break43_g266.x , break43_g266.y , 1.0));
				float4 appendResult28_g265 = (float4(((mul( invertVal44_g266, appendResult40_g266 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g265 = appendResult28_g265;
				(localWorldVar29_g265).xyz = GetCameraRelativePositionWS((localWorldVar29_g265).xyz);
				float4 transform29_g265 = mul(GetWorldToObjectMatrix(),localWorldVar29_g265);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g265 = (transform29_g265).xyz;
				#else
				float3 staticSwitch13_g265 = inputMesh.positionOS;
				#endif
				float3 temp_output_48_0 = staticSwitch13_g265;
				float localToCatmullRomSpace_float56_g1 = ( 0.0 );
				float3 penetratorRootWorld122_g1 = _PenetratorRootWorld;
				float3 worldPenetratorRootPos56_g1 = penetratorRootWorld122_g1;
				float3 penetratorRightWorld139_g1 = _PenetratorRightWorld;
				float3 right169_g1 = penetratorRightWorld139_g1;
				float3 penetratorUpWorld134_g1 = _PenetratorUpWorld;
				float3 up169_g1 = penetratorUpWorld134_g1;
				float3 penetratorForwardWorld126_g1 = _PenetratorForwardWorld;
				float3 forward169_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis169_g1 = ChangeOfBasis169_g1( right169_g1 , up169_g1 , forward169_g1 );
				float3 right9_g1 = penetratorRightWorld139_g1;
				float3 up9_g1 = penetratorUpWorld134_g1;
				float3 forward9_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis9_g1 = ChangeOfBasis9_g1( right9_g1 , up9_g1 , forward9_g1 );
				float3 normalizeResult37 = normalize( _DickForward );
				float3 temp_output_36_0 = ( ( _BulgeProgress * normalizeResult37 ) + _DickOffset );
				float3 temp_output_3_0_g2 = temp_output_36_0;
				float3 normalizeResult6_g2 = normalize( ( temp_output_48_0 - temp_output_3_0_g2 ) );
				float3 temp_output_28_0 = ( temp_output_48_0 - temp_output_36_0 );
				float temp_output_41_0 = ( saturate( ( ( _BulgeRadius - length( temp_output_28_0 ) ) * 10.0 ) ) * inputMesh.ase_color.g * _BulgeBlend );
				float3 lerpResult33 = lerp( temp_output_48_0 , ( ( normalizeResult6_g2 * _BulgeRadius ) + temp_output_3_0_g2 ) , temp_output_41_0);
				float4 appendResult67_g1 = (float4(lerpResult33 , 1.0));
				float4 transform66_g1 = mul(GetObjectToWorldMatrix(),appendResult67_g1);
				transform66_g1.xyz = GetAbsolutePositionWS((transform66_g1).xyz);
				float3 localPenetratorSpaceVertexPosition142_g1 = ( (transform66_g1).xyz - ( _PenetratorStartWorld - penetratorRootWorld122_g1 ) );
				float3 temp_output_12_0_g1 = mul( localChangeOfBasis9_g1, ( localPenetratorSpaceVertexPosition142_g1 - penetratorRootWorld122_g1 ) );
				float3 break15_g1 = temp_output_12_0_g1;
				float temp_output_18_0_g1 = ( break15_g1.z * _SquashStretchCorrection );
				float3 appendResult26_g1 = (float3(break15_g1.x , break15_g1.y , temp_output_18_0_g1));
				float3 appendResult25_g1 = (float3(( break15_g1.x / _SquashStretchCorrection ) , ( break15_g1.y / _SquashStretchCorrection ) , temp_output_18_0_g1));
				float distanceToHole180_g1 = _DistanceToHole;
				float temp_output_17_0_g1 = ( distanceToHole180_g1 * 0.5 );
				float smoothstepResult23_g1 = smoothstep( 0.0 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float smoothstepResult22_g1 = smoothstep( distanceToHole180_g1 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float3 lerpResult31_g1 = lerp( appendResult26_g1 , appendResult25_g1 , min( smoothstepResult23_g1 , smoothstepResult22_g1 ));
				float3 lerpResult32_g1 = lerp( lerpResult31_g1 , ( temp_output_12_0_g1 + ( ( distanceToHole180_g1 - ( ( distanceToHole180_g1 / ( _SquashStretchCorrection * _PenetratorWorldLength ) ) * _PenetratorWorldLength ) ) * float3(0,0,1) ) ) , step( distanceToHole180_g1 , temp_output_18_0_g1 ));
				float3 squashStretchedPosition44_g1 = lerpResult32_g1;
				float3 temp_output_150_0_g1 = ( float3(0,0,1) * _TruncateLength );
				float3 temp_output_149_0_g1 = ( squashStretchedPosition44_g1 - temp_output_150_0_g1 );
				float3 normalizeResult156_g1 = normalize( temp_output_149_0_g1 );
				float3 lerpResult152_g1 = lerp( temp_output_149_0_g1 , ( normalizeResult156_g1 * min( length( temp_output_149_0_g1 ) , _GirthRadius ) ) , saturate( ( temp_output_149_0_g1.z * ( 1.0 / _GirthRadius ) ) ));
				#ifdef _TRUNCATESPHERIZE_ON
				float3 staticSwitch116_g1 = ( lerpResult152_g1 + temp_output_150_0_g1 );
				#else
				float3 staticSwitch116_g1 = squashStretchedPosition44_g1;
				#endif
				float3 TruncatedPosition147_g1 = ( penetratorRootWorld122_g1 + mul( transpose( localChangeOfBasis169_g1 ), staticSwitch116_g1 ) );
				float3 worldPosition56_g1 = ( TruncatedPosition147_g1 + ( penetratorForwardWorld126_g1 * _PenetratorOffsetLength ) );
				float3 worldPenetratorForward56_g1 = penetratorForwardWorld126_g1;
				float3 worldPenetratorUp56_g1 = penetratorUpWorld134_g1;
				float3 worldPenetratorRight56_g1 = penetratorRightWorld139_g1;
				float3 temp_output_50_0_g265 = inputMesh.normalOS;
				float2 break146_g266 = normalizeResult41_g266;
				float4 appendResult139_g266 = (float4(temp_output_48_0_g266 , break146_g266.x , break146_g266.y , 0.0));
				float3 normalizeResult144_g266 = normalize( (mul( invertVal44_g266, appendResult139_g266 )).xyz );
				float3 lerpResult44_g265 = lerp( normalizeResult144_g266 , temp_output_50_0_g265 , saturate( sign( temp_output_114_0_g266 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g265 = lerpResult44_g265;
				#else
				float3 staticSwitch17_g265 = temp_output_50_0_g265;
				#endif
				float3 temp_output_48_42 = staticSwitch17_g265;
				float3 normalizeResult38 = normalize( temp_output_28_0 );
				float dotResult50 = dot( temp_output_28_0 , normalizeResult37 );
				float3 lerpResult39 = lerp( temp_output_48_42 , normalizeResult38 , ( temp_output_41_0 * ( 1.0 - saturate( abs( dotResult50 ) ) ) ));
				float3 normalizeResult44 = normalize( lerpResult39 );
				float4 appendResult86_g1 = (float4(normalizeResult44 , 0.0));
				float3 normalizeResult87_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult86_g1 )).xyz );
				float3 worldNormal56_g1 = normalizeResult87_g1;
				float4 break93_g1 = inputMesh.ase_tangent;
				float4 appendResult89_g1 = (float4(break93_g1.x , break93_g1.y , break93_g1.z , 0.0));
				float3 normalizeResult91_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult89_g1 )).xyz );
				float4 appendResult94_g1 = (float4(normalizeResult91_g1 , break93_g1.w));
				float4 worldTangent56_g1 = appendResult94_g1;
				float3 worldPositionOUT56_g1 = float3( 0,0,0 );
				float3 worldNormalOUT56_g1 = float3( 0,0,0 );
				float4 worldTangentOUT56_g1 = float4( 0,0,0,0 );
				{
				ToCatmullRomSpace_float(worldPenetratorRootPos56_g1,worldPosition56_g1,worldPenetratorForward56_g1,worldPenetratorUp56_g1,worldPenetratorRight56_g1,worldNormal56_g1,worldTangent56_g1,worldPositionOUT56_g1,worldNormalOUT56_g1,worldTangentOUT56_g1);
				}
				float4 appendResult73_g1 = (float4(worldPositionOUT56_g1 , 1.0));
				float4 localWorldVar72_g1 = appendResult73_g1;
				(localWorldVar72_g1).xyz = GetCameraRelativePositionWS((localWorldVar72_g1).xyz);
				float4 transform72_g1 = mul(GetWorldToObjectMatrix(),localWorldVar72_g1);
				float3 lerpResult15 = lerp( temp_output_48_0 , (transform72_g1).xyz , inputMesh.ase_color.r);
				
				float4 appendResult75_g1 = (float4(worldNormalOUT56_g1 , 0.0));
				float3 normalizeResult76_g1 = normalize( (mul( GetWorldToObjectMatrix(), appendResult75_g1 )).xyz );
				float3 lerpResult17 = lerp( temp_output_48_42 , normalizeResult76_g1 , inputMesh.ase_color.r);
				

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = lerpResult15;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = lerpResult17;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.positionRWS.xyz = positionRWS;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_tangent : TANGENT;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_color = v.ase_color;
				o.ase_tangent = v.ase_tangent;
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
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
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
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(WRITE_NORMAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target2
			#elif defined(WRITE_NORMAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target1
			#else
			#define SV_TARGET_DECAL SV_Target0
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
						#if defined(SCENESELECTIONPASS) || defined(SCENEPICKINGPASS)
						, out float4 outColor : SV_Target0
						#else
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

							#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
							, out float4 outDecalBuffer : SV_TARGET_DECAL
							#endif
						#endif

						#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS = packedInput.positionRWS.xyz;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				AlphaSurfaceDescription surfaceDescription = (AlphaSurfaceDescription)0;
				
				surfaceDescription.Alpha = 1;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
				surfaceDescription.AlphaClipThresholdShadow = 0.5;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
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
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }

			Cull Off

			HLSLPROGRAM

            #define _SPECULAR_OCCLUSION_FROM_AO 1
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #define ASE_ABSOLUTE_VERTEX_POS 1
            #define _AMBIENT_OCCLUSION 1
            #define HAVE_MESH_MODIFICATION
            #define ASE_SRP_VERSION 140008


            #pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _TRANSPARENT_WRITES_MOTION_VEC
			#pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma editor_sync_compilation

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
		    #define SCENESELECTIONPASS 1

			#ifndef SHADER_UNLIT
			#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
			#define VARYINGS_NEED_CULLFACE
			#endif
			#endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

		    #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
			#undef  _REFRACTION_PLANE
			#undef  _REFRACTION_SPHERE
			#define _REFRACTION_THIN
		    #endif

			#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
			#if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
				#define WRITE_NORMAL_BUFFER
			#endif
			#endif

			#ifndef DEBUG_DISPLAY
				#if !defined(_SURFACE_TYPE_TRANSPARENT)
					#if SHADERPASS == SHADERPASS_FORWARD
					#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
					#elif SHADERPASS == SHADERPASS_GBUFFER
					#define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
					#endif
				#endif
			#endif

			#if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define _DEFERRED_CAPABLE_MATERIAL
			#endif

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			CBUFFER_START( UnityPerMaterial )
			float4 _MaskMap_ST;
			float4 _BaseColorMap_ST;
			float4 _NormalMap_ST;
			float3 _PenetratorStartWorld;
			float3 _DickForward;
			float3 _PenetratorForwardWorld;
			float3 _PenetratorUpWorld;
			float3 _DickOffset;
			float3 _PenetratorRootWorld;
			float3 _WorldDickPosition;
			float3 _WorldDickBinormal;
			float3 _PenetratorRightWorld;
			float3 _WorldDickNormal;
			float _BulgeProgress;
			float _Angle;
			float _TipRadius;
			float _BulgeRadius;
			float _BulgeBlend;
			float _SquashStretchCorrection;
			float _DistanceToHole;
			float _PenetratorWorldLength;
			float _TruncateLength;
			float _GirthRadius;
			float _PenetratorOffsetLength;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			float _EnableBlendModePreserveSpecularLighting;
            #endif
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
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			float _TessPhongStrength;
			float _TessValue;
			float _TessMin;
			float _TessMax;
			float _TessEdgeLength;
			float _TessMaxDisp;
			#endif
			CBUFFER_END

			// Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

			// Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			

            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			// Setup DECALS_OFF so the shader stripper can remove variants
            #define HAVE_DECALS ( (defined(DECALS_3RT) || defined(DECALS_4RT)) && !defined(_DISABLE_DECALS) )
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#pragma multi_compile_local __ _TRUNCATESPHERIZE_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_tangent : TANGENT;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
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
			
			float3 MyCustomExpression32_g265( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			
			float3x3 ChangeOfBasis169_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			
			float3x3 ChangeOfBasis9_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout SceneSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
                #ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				surfaceData.normalWS = float3(0, 1, 0);
                #endif
				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );

				// decals
			#ifdef DECAL_NORMAL_BLENDING
				if (_EnableDecals)
				{
					#ifndef SURFACE_GRADIENT
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);
					#endif

					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData, normalTS);
				}

				GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
			#else
				GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

				#if HAVE_DECALS
				if (_EnableDecals)
				{
					// Both uses and modifies 'surfaceData.normalWS'.
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif
			#endif

				bentNormalWS = surfaceData.normalWS;
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(SceneSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				float3 normalizeResult27_g267 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g267 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g267 = normalize( cross( normalizeResult27_g267 , normalizeResult31_g267 ) );
				float4 appendResult26_g266 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g266 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g266 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g266 = -_WorldDickPosition;
				float4 appendResult29_g266 = (float4(break27_g266.x , break27_g266.y , break27_g266.z , 1.0));
				float4x4 temp_output_30_0_g266 = mul( transpose( float4x4( float4( normalizeResult27_g267 , 0.0 ).x,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).x,float4( normalizeResult29_g267 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g267 , 0.0 ).y,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).y,float4( normalizeResult29_g267 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g267 , 0.0 ).z,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).z,float4( normalizeResult29_g267 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g267 , 0.0 ).w,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).w,float4( normalizeResult29_g267 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g266.x,appendResult28_g266.x,appendResult31_g266.x,appendResult29_g266.x,appendResult26_g266.y,appendResult28_g266.y,appendResult31_g266.y,appendResult29_g266.y,appendResult26_g266.z,appendResult28_g266.z,appendResult31_g266.z,appendResult29_g266.z,appendResult26_g266.w,appendResult28_g266.w,appendResult31_g266.w,appendResult29_g266.w ) );
				float4x4 invertVal44_g266 = Inverse4x4( temp_output_30_0_g266 );
				float4 appendResult27_g265 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g265 = mul( GetObjectToWorldMatrix(), appendResult27_g265 ).xyz;
				float3 localMyCustomExpression32_g265 = MyCustomExpression32_g265( pos32_g265 );
				float4 appendResult32_g266 = (float4(localMyCustomExpression32_g265 , 1.0));
				float4 break35_g266 = mul( temp_output_30_0_g266, appendResult32_g266 );
				float temp_output_124_0_g266 = _TipRadius;
				float2 appendResult36_g266 = (float2(break35_g266.y , break35_g266.z));
				float2 normalizeResult41_g266 = normalize( appendResult36_g266 );
				float temp_output_120_0_g266 = sqrt( max( break35_g266.x , 0.0 ) );
				float temp_output_48_0_g266 = tan( radians( _Angle ) );
				float temp_output_125_0_g266 = ( temp_output_124_0_g266 + ( temp_output_120_0_g266 * temp_output_48_0_g266 ) );
				float temp_output_37_0_g266 = length( appendResult36_g266 );
				float temp_output_114_0_g266 = ( ( temp_output_125_0_g266 - temp_output_37_0_g266 ) + 1.0 );
				float lerpResult102_g266 = lerp( temp_output_125_0_g266 , temp_output_37_0_g266 , saturate( temp_output_114_0_g266 ));
				float lerpResult130_g266 = lerp( 0.0 , lerpResult102_g266 , saturate( ( -( -temp_output_124_0_g266 - break35_g266.x ) / temp_output_124_0_g266 ) ));
				float2 break43_g266 = ( normalizeResult41_g266 * lerpResult130_g266 );
				float4 appendResult40_g266 = (float4(max( break35_g266.x , -temp_output_124_0_g266 ) , break43_g266.x , break43_g266.y , 1.0));
				float4 appendResult28_g265 = (float4(((mul( invertVal44_g266, appendResult40_g266 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g265 = appendResult28_g265;
				(localWorldVar29_g265).xyz = GetCameraRelativePositionWS((localWorldVar29_g265).xyz);
				float4 transform29_g265 = mul(GetWorldToObjectMatrix(),localWorldVar29_g265);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g265 = (transform29_g265).xyz;
				#else
				float3 staticSwitch13_g265 = inputMesh.positionOS;
				#endif
				float3 temp_output_48_0 = staticSwitch13_g265;
				float localToCatmullRomSpace_float56_g1 = ( 0.0 );
				float3 penetratorRootWorld122_g1 = _PenetratorRootWorld;
				float3 worldPenetratorRootPos56_g1 = penetratorRootWorld122_g1;
				float3 penetratorRightWorld139_g1 = _PenetratorRightWorld;
				float3 right169_g1 = penetratorRightWorld139_g1;
				float3 penetratorUpWorld134_g1 = _PenetratorUpWorld;
				float3 up169_g1 = penetratorUpWorld134_g1;
				float3 penetratorForwardWorld126_g1 = _PenetratorForwardWorld;
				float3 forward169_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis169_g1 = ChangeOfBasis169_g1( right169_g1 , up169_g1 , forward169_g1 );
				float3 right9_g1 = penetratorRightWorld139_g1;
				float3 up9_g1 = penetratorUpWorld134_g1;
				float3 forward9_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis9_g1 = ChangeOfBasis9_g1( right9_g1 , up9_g1 , forward9_g1 );
				float3 normalizeResult37 = normalize( _DickForward );
				float3 temp_output_36_0 = ( ( _BulgeProgress * normalizeResult37 ) + _DickOffset );
				float3 temp_output_3_0_g2 = temp_output_36_0;
				float3 normalizeResult6_g2 = normalize( ( temp_output_48_0 - temp_output_3_0_g2 ) );
				float3 temp_output_28_0 = ( temp_output_48_0 - temp_output_36_0 );
				float temp_output_41_0 = ( saturate( ( ( _BulgeRadius - length( temp_output_28_0 ) ) * 10.0 ) ) * inputMesh.ase_color.g * _BulgeBlend );
				float3 lerpResult33 = lerp( temp_output_48_0 , ( ( normalizeResult6_g2 * _BulgeRadius ) + temp_output_3_0_g2 ) , temp_output_41_0);
				float4 appendResult67_g1 = (float4(lerpResult33 , 1.0));
				float4 transform66_g1 = mul(GetObjectToWorldMatrix(),appendResult67_g1);
				transform66_g1.xyz = GetAbsolutePositionWS((transform66_g1).xyz);
				float3 localPenetratorSpaceVertexPosition142_g1 = ( (transform66_g1).xyz - ( _PenetratorStartWorld - penetratorRootWorld122_g1 ) );
				float3 temp_output_12_0_g1 = mul( localChangeOfBasis9_g1, ( localPenetratorSpaceVertexPosition142_g1 - penetratorRootWorld122_g1 ) );
				float3 break15_g1 = temp_output_12_0_g1;
				float temp_output_18_0_g1 = ( break15_g1.z * _SquashStretchCorrection );
				float3 appendResult26_g1 = (float3(break15_g1.x , break15_g1.y , temp_output_18_0_g1));
				float3 appendResult25_g1 = (float3(( break15_g1.x / _SquashStretchCorrection ) , ( break15_g1.y / _SquashStretchCorrection ) , temp_output_18_0_g1));
				float distanceToHole180_g1 = _DistanceToHole;
				float temp_output_17_0_g1 = ( distanceToHole180_g1 * 0.5 );
				float smoothstepResult23_g1 = smoothstep( 0.0 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float smoothstepResult22_g1 = smoothstep( distanceToHole180_g1 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float3 lerpResult31_g1 = lerp( appendResult26_g1 , appendResult25_g1 , min( smoothstepResult23_g1 , smoothstepResult22_g1 ));
				float3 lerpResult32_g1 = lerp( lerpResult31_g1 , ( temp_output_12_0_g1 + ( ( distanceToHole180_g1 - ( ( distanceToHole180_g1 / ( _SquashStretchCorrection * _PenetratorWorldLength ) ) * _PenetratorWorldLength ) ) * float3(0,0,1) ) ) , step( distanceToHole180_g1 , temp_output_18_0_g1 ));
				float3 squashStretchedPosition44_g1 = lerpResult32_g1;
				float3 temp_output_150_0_g1 = ( float3(0,0,1) * _TruncateLength );
				float3 temp_output_149_0_g1 = ( squashStretchedPosition44_g1 - temp_output_150_0_g1 );
				float3 normalizeResult156_g1 = normalize( temp_output_149_0_g1 );
				float3 lerpResult152_g1 = lerp( temp_output_149_0_g1 , ( normalizeResult156_g1 * min( length( temp_output_149_0_g1 ) , _GirthRadius ) ) , saturate( ( temp_output_149_0_g1.z * ( 1.0 / _GirthRadius ) ) ));
				#ifdef _TRUNCATESPHERIZE_ON
				float3 staticSwitch116_g1 = ( lerpResult152_g1 + temp_output_150_0_g1 );
				#else
				float3 staticSwitch116_g1 = squashStretchedPosition44_g1;
				#endif
				float3 TruncatedPosition147_g1 = ( penetratorRootWorld122_g1 + mul( transpose( localChangeOfBasis169_g1 ), staticSwitch116_g1 ) );
				float3 worldPosition56_g1 = ( TruncatedPosition147_g1 + ( penetratorForwardWorld126_g1 * _PenetratorOffsetLength ) );
				float3 worldPenetratorForward56_g1 = penetratorForwardWorld126_g1;
				float3 worldPenetratorUp56_g1 = penetratorUpWorld134_g1;
				float3 worldPenetratorRight56_g1 = penetratorRightWorld139_g1;
				float3 temp_output_50_0_g265 = inputMesh.normalOS;
				float2 break146_g266 = normalizeResult41_g266;
				float4 appendResult139_g266 = (float4(temp_output_48_0_g266 , break146_g266.x , break146_g266.y , 0.0));
				float3 normalizeResult144_g266 = normalize( (mul( invertVal44_g266, appendResult139_g266 )).xyz );
				float3 lerpResult44_g265 = lerp( normalizeResult144_g266 , temp_output_50_0_g265 , saturate( sign( temp_output_114_0_g266 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g265 = lerpResult44_g265;
				#else
				float3 staticSwitch17_g265 = temp_output_50_0_g265;
				#endif
				float3 temp_output_48_42 = staticSwitch17_g265;
				float3 normalizeResult38 = normalize( temp_output_28_0 );
				float dotResult50 = dot( temp_output_28_0 , normalizeResult37 );
				float3 lerpResult39 = lerp( temp_output_48_42 , normalizeResult38 , ( temp_output_41_0 * ( 1.0 - saturate( abs( dotResult50 ) ) ) ));
				float3 normalizeResult44 = normalize( lerpResult39 );
				float4 appendResult86_g1 = (float4(normalizeResult44 , 0.0));
				float3 normalizeResult87_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult86_g1 )).xyz );
				float3 worldNormal56_g1 = normalizeResult87_g1;
				float4 break93_g1 = inputMesh.ase_tangent;
				float4 appendResult89_g1 = (float4(break93_g1.x , break93_g1.y , break93_g1.z , 0.0));
				float3 normalizeResult91_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult89_g1 )).xyz );
				float4 appendResult94_g1 = (float4(normalizeResult91_g1 , break93_g1.w));
				float4 worldTangent56_g1 = appendResult94_g1;
				float3 worldPositionOUT56_g1 = float3( 0,0,0 );
				float3 worldNormalOUT56_g1 = float3( 0,0,0 );
				float4 worldTangentOUT56_g1 = float4( 0,0,0,0 );
				{
				ToCatmullRomSpace_float(worldPenetratorRootPos56_g1,worldPosition56_g1,worldPenetratorForward56_g1,worldPenetratorUp56_g1,worldPenetratorRight56_g1,worldNormal56_g1,worldTangent56_g1,worldPositionOUT56_g1,worldNormalOUT56_g1,worldTangentOUT56_g1);
				}
				float4 appendResult73_g1 = (float4(worldPositionOUT56_g1 , 1.0));
				float4 localWorldVar72_g1 = appendResult73_g1;
				(localWorldVar72_g1).xyz = GetCameraRelativePositionWS((localWorldVar72_g1).xyz);
				float4 transform72_g1 = mul(GetWorldToObjectMatrix(),localWorldVar72_g1);
				float3 lerpResult15 = lerp( temp_output_48_0 , (transform72_g1).xyz , inputMesh.ase_color.r);
				
				float4 appendResult75_g1 = (float4(worldNormalOUT56_g1 , 0.0));
				float3 normalizeResult76_g1 = normalize( (mul( GetWorldToObjectMatrix(), appendResult75_g1 )).xyz );
				float3 lerpResult17 = lerp( temp_output_48_42 , normalizeResult76_g1 , inputMesh.ase_color.r);
				

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = lerpResult15;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = lerpResult17;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.positionRWS.xyz = positionRWS;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_tangent : TANGENT;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_color = v.ase_color;
				o.ase_tangent = v.ase_tangent;
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
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
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
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(WRITE_NORMAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target2
			#elif defined(WRITE_NORMAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target1
			#else
			#define SV_TARGET_DECAL SV_Target0
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
						, out float4 outColor : SV_Target0
						#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS = packedInput.positionRWS.xyz;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SceneSurfaceDescription surfaceDescription = (SceneSurfaceDescription)0;
				
				surfaceDescription.Alpha = 1;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
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
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			Cull [_CullMode]

			ZWrite On

			Stencil
			{
				Ref [_StencilRefDepth]
				WriteMask [_StencilWriteMaskDepth]
				Comp Always
				Pass Replace
			}


			HLSLPROGRAM

            #define _SPECULAR_OCCLUSION_FROM_AO 1
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #define ASE_ABSOLUTE_VERTEX_POS 1
            #define _AMBIENT_OCCLUSION 1
            #define HAVE_MESH_MODIFICATION
            #define ASE_SRP_VERSION 140008


            #pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _TRANSPARENT_WRITES_MOTION_VEC
			#pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile _ WRITE_NORMAL_BUFFER
			#pragma multi_compile_fragment _ WRITE_MSAA_DEPTH
			#pragma multi_compile _ WRITE_DECAL_BUFFER

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

			#define SHADERPASS SHADERPASS_DEPTH_ONLY

			#ifndef SHADER_UNLIT
			#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
			#define VARYINGS_NEED_CULLFACE
			#endif
			#endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

		    #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
			#undef  _REFRACTION_PLANE
			#undef  _REFRACTION_SPHERE
			#define _REFRACTION_THIN
		    #endif

			#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
			#if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
				#define WRITE_NORMAL_BUFFER
			#endif
			#endif

			#ifndef DEBUG_DISPLAY
				#if !defined(_SURFACE_TYPE_TRANSPARENT)
					#if SHADERPASS == SHADERPASS_FORWARD
					#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
					#elif SHADERPASS == SHADERPASS_GBUFFER
					#define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
					#endif
				#endif
			#endif

			#if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define _DEFERRED_CAPABLE_MATERIAL
			#endif

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			CBUFFER_START( UnityPerMaterial )
			float4 _MaskMap_ST;
			float4 _BaseColorMap_ST;
			float4 _NormalMap_ST;
			float3 _PenetratorStartWorld;
			float3 _DickForward;
			float3 _PenetratorForwardWorld;
			float3 _PenetratorUpWorld;
			float3 _DickOffset;
			float3 _PenetratorRootWorld;
			float3 _WorldDickPosition;
			float3 _WorldDickBinormal;
			float3 _PenetratorRightWorld;
			float3 _WorldDickNormal;
			float _BulgeProgress;
			float _Angle;
			float _TipRadius;
			float _BulgeRadius;
			float _BulgeBlend;
			float _SquashStretchCorrection;
			float _DistanceToHole;
			float _PenetratorWorldLength;
			float _TruncateLength;
			float _GirthRadius;
			float _PenetratorOffsetLength;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			float _EnableBlendModePreserveSpecularLighting;
            #endif
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
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			float _TessPhongStrength;
			float _TessValue;
			float _TessMin;
			float _TessMax;
			float _TessEdgeLength;
			float _TessMaxDisp;
			#endif
			CBUFFER_END

			// Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

			// Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			sampler2D _NormalMap;
			sampler2D _MaskMap;


            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			// Setup DECALS_OFF so the shader stripper can remove variants
            #define HAVE_DECALS ( (defined(DECALS_3RT) || defined(DECALS_4RT)) && !defined(_DISABLE_DECALS) )
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_TANGENT
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#pragma multi_compile_local __ _TRUNCATESPHERIZE_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				float3 normalWS : TEXCOORD1;
				float4 tangentWS : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
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
			
			float3 MyCustomExpression32_g265( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			
			float3x3 ChangeOfBasis169_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			
			float3x3 ChangeOfBasis9_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout SmoothSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
                #ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				surfaceData.normalWS = float3(0, 1, 0);
                #endif
				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );

				// decals
			#ifdef DECAL_NORMAL_BLENDING
				if (_EnableDecals)
				{
					#ifndef SURFACE_GRADIENT
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);
					#endif

					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData, normalTS);
				}

				GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
			#else
				GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

				#if HAVE_DECALS
				if (_EnableDecals)
				{
					// Both uses and modifies 'surfaceData.normalWS'.
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif
			#endif

				bentNormalWS = surfaceData.normalWS;
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(SmoothSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalPrepassBuffer.hlsl"
			#endif
			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				float3 normalizeResult27_g267 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g267 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g267 = normalize( cross( normalizeResult27_g267 , normalizeResult31_g267 ) );
				float4 appendResult26_g266 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g266 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g266 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g266 = -_WorldDickPosition;
				float4 appendResult29_g266 = (float4(break27_g266.x , break27_g266.y , break27_g266.z , 1.0));
				float4x4 temp_output_30_0_g266 = mul( transpose( float4x4( float4( normalizeResult27_g267 , 0.0 ).x,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).x,float4( normalizeResult29_g267 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g267 , 0.0 ).y,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).y,float4( normalizeResult29_g267 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g267 , 0.0 ).z,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).z,float4( normalizeResult29_g267 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g267 , 0.0 ).w,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).w,float4( normalizeResult29_g267 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g266.x,appendResult28_g266.x,appendResult31_g266.x,appendResult29_g266.x,appendResult26_g266.y,appendResult28_g266.y,appendResult31_g266.y,appendResult29_g266.y,appendResult26_g266.z,appendResult28_g266.z,appendResult31_g266.z,appendResult29_g266.z,appendResult26_g266.w,appendResult28_g266.w,appendResult31_g266.w,appendResult29_g266.w ) );
				float4x4 invertVal44_g266 = Inverse4x4( temp_output_30_0_g266 );
				float4 appendResult27_g265 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g265 = mul( GetObjectToWorldMatrix(), appendResult27_g265 ).xyz;
				float3 localMyCustomExpression32_g265 = MyCustomExpression32_g265( pos32_g265 );
				float4 appendResult32_g266 = (float4(localMyCustomExpression32_g265 , 1.0));
				float4 break35_g266 = mul( temp_output_30_0_g266, appendResult32_g266 );
				float temp_output_124_0_g266 = _TipRadius;
				float2 appendResult36_g266 = (float2(break35_g266.y , break35_g266.z));
				float2 normalizeResult41_g266 = normalize( appendResult36_g266 );
				float temp_output_120_0_g266 = sqrt( max( break35_g266.x , 0.0 ) );
				float temp_output_48_0_g266 = tan( radians( _Angle ) );
				float temp_output_125_0_g266 = ( temp_output_124_0_g266 + ( temp_output_120_0_g266 * temp_output_48_0_g266 ) );
				float temp_output_37_0_g266 = length( appendResult36_g266 );
				float temp_output_114_0_g266 = ( ( temp_output_125_0_g266 - temp_output_37_0_g266 ) + 1.0 );
				float lerpResult102_g266 = lerp( temp_output_125_0_g266 , temp_output_37_0_g266 , saturate( temp_output_114_0_g266 ));
				float lerpResult130_g266 = lerp( 0.0 , lerpResult102_g266 , saturate( ( -( -temp_output_124_0_g266 - break35_g266.x ) / temp_output_124_0_g266 ) ));
				float2 break43_g266 = ( normalizeResult41_g266 * lerpResult130_g266 );
				float4 appendResult40_g266 = (float4(max( break35_g266.x , -temp_output_124_0_g266 ) , break43_g266.x , break43_g266.y , 1.0));
				float4 appendResult28_g265 = (float4(((mul( invertVal44_g266, appendResult40_g266 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g265 = appendResult28_g265;
				(localWorldVar29_g265).xyz = GetCameraRelativePositionWS((localWorldVar29_g265).xyz);
				float4 transform29_g265 = mul(GetWorldToObjectMatrix(),localWorldVar29_g265);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g265 = (transform29_g265).xyz;
				#else
				float3 staticSwitch13_g265 = inputMesh.positionOS;
				#endif
				float3 temp_output_48_0 = staticSwitch13_g265;
				float localToCatmullRomSpace_float56_g1 = ( 0.0 );
				float3 penetratorRootWorld122_g1 = _PenetratorRootWorld;
				float3 worldPenetratorRootPos56_g1 = penetratorRootWorld122_g1;
				float3 penetratorRightWorld139_g1 = _PenetratorRightWorld;
				float3 right169_g1 = penetratorRightWorld139_g1;
				float3 penetratorUpWorld134_g1 = _PenetratorUpWorld;
				float3 up169_g1 = penetratorUpWorld134_g1;
				float3 penetratorForwardWorld126_g1 = _PenetratorForwardWorld;
				float3 forward169_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis169_g1 = ChangeOfBasis169_g1( right169_g1 , up169_g1 , forward169_g1 );
				float3 right9_g1 = penetratorRightWorld139_g1;
				float3 up9_g1 = penetratorUpWorld134_g1;
				float3 forward9_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis9_g1 = ChangeOfBasis9_g1( right9_g1 , up9_g1 , forward9_g1 );
				float3 normalizeResult37 = normalize( _DickForward );
				float3 temp_output_36_0 = ( ( _BulgeProgress * normalizeResult37 ) + _DickOffset );
				float3 temp_output_3_0_g2 = temp_output_36_0;
				float3 normalizeResult6_g2 = normalize( ( temp_output_48_0 - temp_output_3_0_g2 ) );
				float3 temp_output_28_0 = ( temp_output_48_0 - temp_output_36_0 );
				float temp_output_41_0 = ( saturate( ( ( _BulgeRadius - length( temp_output_28_0 ) ) * 10.0 ) ) * inputMesh.ase_color.g * _BulgeBlend );
				float3 lerpResult33 = lerp( temp_output_48_0 , ( ( normalizeResult6_g2 * _BulgeRadius ) + temp_output_3_0_g2 ) , temp_output_41_0);
				float4 appendResult67_g1 = (float4(lerpResult33 , 1.0));
				float4 transform66_g1 = mul(GetObjectToWorldMatrix(),appendResult67_g1);
				transform66_g1.xyz = GetAbsolutePositionWS((transform66_g1).xyz);
				float3 localPenetratorSpaceVertexPosition142_g1 = ( (transform66_g1).xyz - ( _PenetratorStartWorld - penetratorRootWorld122_g1 ) );
				float3 temp_output_12_0_g1 = mul( localChangeOfBasis9_g1, ( localPenetratorSpaceVertexPosition142_g1 - penetratorRootWorld122_g1 ) );
				float3 break15_g1 = temp_output_12_0_g1;
				float temp_output_18_0_g1 = ( break15_g1.z * _SquashStretchCorrection );
				float3 appendResult26_g1 = (float3(break15_g1.x , break15_g1.y , temp_output_18_0_g1));
				float3 appendResult25_g1 = (float3(( break15_g1.x / _SquashStretchCorrection ) , ( break15_g1.y / _SquashStretchCorrection ) , temp_output_18_0_g1));
				float distanceToHole180_g1 = _DistanceToHole;
				float temp_output_17_0_g1 = ( distanceToHole180_g1 * 0.5 );
				float smoothstepResult23_g1 = smoothstep( 0.0 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float smoothstepResult22_g1 = smoothstep( distanceToHole180_g1 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float3 lerpResult31_g1 = lerp( appendResult26_g1 , appendResult25_g1 , min( smoothstepResult23_g1 , smoothstepResult22_g1 ));
				float3 lerpResult32_g1 = lerp( lerpResult31_g1 , ( temp_output_12_0_g1 + ( ( distanceToHole180_g1 - ( ( distanceToHole180_g1 / ( _SquashStretchCorrection * _PenetratorWorldLength ) ) * _PenetratorWorldLength ) ) * float3(0,0,1) ) ) , step( distanceToHole180_g1 , temp_output_18_0_g1 ));
				float3 squashStretchedPosition44_g1 = lerpResult32_g1;
				float3 temp_output_150_0_g1 = ( float3(0,0,1) * _TruncateLength );
				float3 temp_output_149_0_g1 = ( squashStretchedPosition44_g1 - temp_output_150_0_g1 );
				float3 normalizeResult156_g1 = normalize( temp_output_149_0_g1 );
				float3 lerpResult152_g1 = lerp( temp_output_149_0_g1 , ( normalizeResult156_g1 * min( length( temp_output_149_0_g1 ) , _GirthRadius ) ) , saturate( ( temp_output_149_0_g1.z * ( 1.0 / _GirthRadius ) ) ));
				#ifdef _TRUNCATESPHERIZE_ON
				float3 staticSwitch116_g1 = ( lerpResult152_g1 + temp_output_150_0_g1 );
				#else
				float3 staticSwitch116_g1 = squashStretchedPosition44_g1;
				#endif
				float3 TruncatedPosition147_g1 = ( penetratorRootWorld122_g1 + mul( transpose( localChangeOfBasis169_g1 ), staticSwitch116_g1 ) );
				float3 worldPosition56_g1 = ( TruncatedPosition147_g1 + ( penetratorForwardWorld126_g1 * _PenetratorOffsetLength ) );
				float3 worldPenetratorForward56_g1 = penetratorForwardWorld126_g1;
				float3 worldPenetratorUp56_g1 = penetratorUpWorld134_g1;
				float3 worldPenetratorRight56_g1 = penetratorRightWorld139_g1;
				float3 temp_output_50_0_g265 = inputMesh.normalOS;
				float2 break146_g266 = normalizeResult41_g266;
				float4 appendResult139_g266 = (float4(temp_output_48_0_g266 , break146_g266.x , break146_g266.y , 0.0));
				float3 normalizeResult144_g266 = normalize( (mul( invertVal44_g266, appendResult139_g266 )).xyz );
				float3 lerpResult44_g265 = lerp( normalizeResult144_g266 , temp_output_50_0_g265 , saturate( sign( temp_output_114_0_g266 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g265 = lerpResult44_g265;
				#else
				float3 staticSwitch17_g265 = temp_output_50_0_g265;
				#endif
				float3 temp_output_48_42 = staticSwitch17_g265;
				float3 normalizeResult38 = normalize( temp_output_28_0 );
				float dotResult50 = dot( temp_output_28_0 , normalizeResult37 );
				float3 lerpResult39 = lerp( temp_output_48_42 , normalizeResult38 , ( temp_output_41_0 * ( 1.0 - saturate( abs( dotResult50 ) ) ) ));
				float3 normalizeResult44 = normalize( lerpResult39 );
				float4 appendResult86_g1 = (float4(normalizeResult44 , 0.0));
				float3 normalizeResult87_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult86_g1 )).xyz );
				float3 worldNormal56_g1 = normalizeResult87_g1;
				float4 break93_g1 = inputMesh.tangentOS;
				float4 appendResult89_g1 = (float4(break93_g1.x , break93_g1.y , break93_g1.z , 0.0));
				float3 normalizeResult91_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult89_g1 )).xyz );
				float4 appendResult94_g1 = (float4(normalizeResult91_g1 , break93_g1.w));
				float4 worldTangent56_g1 = appendResult94_g1;
				float3 worldPositionOUT56_g1 = float3( 0,0,0 );
				float3 worldNormalOUT56_g1 = float3( 0,0,0 );
				float4 worldTangentOUT56_g1 = float4( 0,0,0,0 );
				{
				ToCatmullRomSpace_float(worldPenetratorRootPos56_g1,worldPosition56_g1,worldPenetratorForward56_g1,worldPenetratorUp56_g1,worldPenetratorRight56_g1,worldNormal56_g1,worldTangent56_g1,worldPositionOUT56_g1,worldNormalOUT56_g1,worldTangentOUT56_g1);
				}
				float4 appendResult73_g1 = (float4(worldPositionOUT56_g1 , 1.0));
				float4 localWorldVar72_g1 = appendResult73_g1;
				(localWorldVar72_g1).xyz = GetCameraRelativePositionWS((localWorldVar72_g1).xyz);
				float4 transform72_g1 = mul(GetWorldToObjectMatrix(),localWorldVar72_g1);
				float3 lerpResult15 = lerp( temp_output_48_0 , (transform72_g1).xyz , inputMesh.ase_color.r);
				
				float4 appendResult75_g1 = (float4(worldNormalOUT56_g1 , 0.0));
				float3 normalizeResult76_g1 = normalize( (mul( GetWorldToObjectMatrix(), appendResult75_g1 )).xyz );
				float3 lerpResult17 = lerp( temp_output_48_42 , normalizeResult76_g1 , inputMesh.ase_color.r);
				
				float4 break79_g1 = worldTangentOUT56_g1;
				float4 appendResult77_g1 = (float4(break79_g1.x , break79_g1.y , break79_g1.z , 0.0));
				float3 normalizeResult80_g1 = normalize( (mul( GetWorldToObjectMatrix(), appendResult77_g1 )).xyz );
				float4 appendResult83_g1 = (float4(normalizeResult80_g1 , break79_g1.w));
				float4 lerpResult20 = lerp( inputMesh.tangentOS , appendResult83_g1 , inputMesh.ase_color.r);
				
				outputPackedVaryingsMeshToPS.ase_texcoord3.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = lerpResult15;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = lerpResult17;
				inputMesh.tangentOS = lerpResult20;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.positionRWS.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.normalWS.xyz = normalWS;
				outputPackedVaryingsMeshToPS.tangentWS.xyzw = tangentWS;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
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
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
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
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(WRITE_NORMAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target2
			#elif defined(WRITE_NORMAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target1
			#else
			#define SV_TARGET_DECAL SV_Target0
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
						#if defined(SCENESELECTIONPASS) || defined(SCENEPICKINGPASS)
						, out float4 outColor : SV_Target0
						#else
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

							#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
							, out float4 outDecalBuffer : SV_TARGET_DECAL
							#endif
						#endif

						#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS = packedInput.positionRWS.xyz;
				float3 normalWS = packedInput.normalWS.xyz;
				float4 tangentWS = packedInput.tangentWS.xyzw;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SmoothSurfaceDescription surfaceDescription = (SmoothSurfaceDescription)0;
				float2 uv_NormalMap = packedInput.ase_texcoord3.xy * _NormalMap_ST.xy + _NormalMap_ST.zw;
				
				float2 uv_MaskMap = packedInput.ase_texcoord3.xy * _MaskMap_ST.xy + _MaskMap_ST.zw;
				float4 tex2DNode46 = tex2D( _MaskMap, uv_MaskMap );
				
				surfaceDescription.Normal = UnpackNormalScale( tex2D( _NormalMap, uv_NormalMap ), 1.0f );
				surfaceDescription.Smoothness = tex2DNode46.a;
				surfaceDescription.Alpha = 1;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

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

            #define _SPECULAR_OCCLUSION_FROM_AO 1
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #define ASE_ABSOLUTE_VERTEX_POS 1
            #define _AMBIENT_OCCLUSION 1
            #define HAVE_MESH_MODIFICATION
            #define ASE_SRP_VERSION 140008


            #pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _TRANSPARENT_WRITES_MOTION_VEC
			#pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile _ WRITE_NORMAL_BUFFER
			#pragma multi_compile_fragment _ WRITE_MSAA_DEPTH
			#pragma multi_compile _ WRITE_DECAL_BUFFER

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

			#define SHADERPASS SHADERPASS_MOTION_VECTORS

			#ifndef SHADER_UNLIT
			#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
			#define VARYINGS_NEED_CULLFACE
			#endif
			#endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

		    #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
			#undef  _REFRACTION_PLANE
			#undef  _REFRACTION_SPHERE
			#define _REFRACTION_THIN
		    #endif

			#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
			#if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
				#define WRITE_NORMAL_BUFFER
			#endif
			#endif

			#ifndef DEBUG_DISPLAY
				#if !defined(_SURFACE_TYPE_TRANSPARENT)
					#if SHADERPASS == SHADERPASS_FORWARD
					#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
					#elif SHADERPASS == SHADERPASS_GBUFFER
					#define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
					#endif
				#endif
			#endif

			#if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define _DEFERRED_CAPABLE_MATERIAL
			#endif

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			CBUFFER_START( UnityPerMaterial )
			float4 _MaskMap_ST;
			float4 _BaseColorMap_ST;
			float4 _NormalMap_ST;
			float3 _PenetratorStartWorld;
			float3 _DickForward;
			float3 _PenetratorForwardWorld;
			float3 _PenetratorUpWorld;
			float3 _DickOffset;
			float3 _PenetratorRootWorld;
			float3 _WorldDickPosition;
			float3 _WorldDickBinormal;
			float3 _PenetratorRightWorld;
			float3 _WorldDickNormal;
			float _BulgeProgress;
			float _Angle;
			float _TipRadius;
			float _BulgeRadius;
			float _BulgeBlend;
			float _SquashStretchCorrection;
			float _DistanceToHole;
			float _PenetratorWorldLength;
			float _TruncateLength;
			float _GirthRadius;
			float _PenetratorOffsetLength;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			float _EnableBlendModePreserveSpecularLighting;
            #endif
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
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			float _TessPhongStrength;
			float _TessValue;
			float _TessMin;
			float _TessMax;
			float _TessEdgeLength;
			float _TessMaxDisp;
			#endif
			CBUFFER_END

			// Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

			// Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			sampler2D _NormalMap;
			sampler2D _MaskMap;


            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			// Setup DECALS_OFF so the shader stripper can remove variants
            #define HAVE_DECALS ( (defined(DECALS_3RT) || defined(DECALS_4RT)) && !defined(_DISABLE_DECALS) )
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#pragma multi_compile_local __ _TRUNCATESPHERIZE_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float3 previousPositionOS : TEXCOORD4;
				float3 precomputedVelocity : TEXCOORD5;
				float4 ase_color : COLOR;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 vmeshPositionCS : SV_Position;
				float3 vmeshInterp00 : TEXCOORD0;
				float3 vpassInterpolators0 : TEXCOORD1; //interpolators0
				float3 vpassInterpolators1 : TEXCOORD2; //interpolators1
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
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
			
			float3 MyCustomExpression32_g265( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			
			float3x3 ChangeOfBasis169_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			
			float3x3 ChangeOfBasis9_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout SmoothSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif

				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
                #ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				surfaceData.normalWS = float3(0, 1, 0);
                #endif
				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );

				// decals
			#ifdef DECAL_NORMAL_BLENDING
				if (_EnableDecals)
				{
					#ifndef SURFACE_GRADIENT
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);
					#endif

					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData, normalTS);
				}

				GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
			#else
				GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

				#if HAVE_DECALS
				if (_EnableDecals)
				{
					// Both uses and modifies 'surfaceData.normalWS'.
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif
			#endif

				bentNormalWS = surfaceData.normalWS;
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(SmoothSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			AttributesMesh ApplyMeshModification(AttributesMesh inputMesh, float3 timeParameters, inout PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS )
			{
				_TimeParameters.xyz = timeParameters;
				float3 normalizeResult27_g267 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g267 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g267 = normalize( cross( normalizeResult27_g267 , normalizeResult31_g267 ) );
				float4 appendResult26_g266 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g266 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g266 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g266 = -_WorldDickPosition;
				float4 appendResult29_g266 = (float4(break27_g266.x , break27_g266.y , break27_g266.z , 1.0));
				float4x4 temp_output_30_0_g266 = mul( transpose( float4x4( float4( normalizeResult27_g267 , 0.0 ).x,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).x,float4( normalizeResult29_g267 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g267 , 0.0 ).y,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).y,float4( normalizeResult29_g267 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g267 , 0.0 ).z,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).z,float4( normalizeResult29_g267 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g267 , 0.0 ).w,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).w,float4( normalizeResult29_g267 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g266.x,appendResult28_g266.x,appendResult31_g266.x,appendResult29_g266.x,appendResult26_g266.y,appendResult28_g266.y,appendResult31_g266.y,appendResult29_g266.y,appendResult26_g266.z,appendResult28_g266.z,appendResult31_g266.z,appendResult29_g266.z,appendResult26_g266.w,appendResult28_g266.w,appendResult31_g266.w,appendResult29_g266.w ) );
				float4x4 invertVal44_g266 = Inverse4x4( temp_output_30_0_g266 );
				float4 appendResult27_g265 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g265 = mul( GetObjectToWorldMatrix(), appendResult27_g265 ).xyz;
				float3 localMyCustomExpression32_g265 = MyCustomExpression32_g265( pos32_g265 );
				float4 appendResult32_g266 = (float4(localMyCustomExpression32_g265 , 1.0));
				float4 break35_g266 = mul( temp_output_30_0_g266, appendResult32_g266 );
				float temp_output_124_0_g266 = _TipRadius;
				float2 appendResult36_g266 = (float2(break35_g266.y , break35_g266.z));
				float2 normalizeResult41_g266 = normalize( appendResult36_g266 );
				float temp_output_120_0_g266 = sqrt( max( break35_g266.x , 0.0 ) );
				float temp_output_48_0_g266 = tan( radians( _Angle ) );
				float temp_output_125_0_g266 = ( temp_output_124_0_g266 + ( temp_output_120_0_g266 * temp_output_48_0_g266 ) );
				float temp_output_37_0_g266 = length( appendResult36_g266 );
				float temp_output_114_0_g266 = ( ( temp_output_125_0_g266 - temp_output_37_0_g266 ) + 1.0 );
				float lerpResult102_g266 = lerp( temp_output_125_0_g266 , temp_output_37_0_g266 , saturate( temp_output_114_0_g266 ));
				float lerpResult130_g266 = lerp( 0.0 , lerpResult102_g266 , saturate( ( -( -temp_output_124_0_g266 - break35_g266.x ) / temp_output_124_0_g266 ) ));
				float2 break43_g266 = ( normalizeResult41_g266 * lerpResult130_g266 );
				float4 appendResult40_g266 = (float4(max( break35_g266.x , -temp_output_124_0_g266 ) , break43_g266.x , break43_g266.y , 1.0));
				float4 appendResult28_g265 = (float4(((mul( invertVal44_g266, appendResult40_g266 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g265 = appendResult28_g265;
				(localWorldVar29_g265).xyz = GetCameraRelativePositionWS((localWorldVar29_g265).xyz);
				float4 transform29_g265 = mul(GetWorldToObjectMatrix(),localWorldVar29_g265);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g265 = (transform29_g265).xyz;
				#else
				float3 staticSwitch13_g265 = inputMesh.positionOS;
				#endif
				float3 temp_output_48_0 = staticSwitch13_g265;
				float localToCatmullRomSpace_float56_g1 = ( 0.0 );
				float3 penetratorRootWorld122_g1 = _PenetratorRootWorld;
				float3 worldPenetratorRootPos56_g1 = penetratorRootWorld122_g1;
				float3 penetratorRightWorld139_g1 = _PenetratorRightWorld;
				float3 right169_g1 = penetratorRightWorld139_g1;
				float3 penetratorUpWorld134_g1 = _PenetratorUpWorld;
				float3 up169_g1 = penetratorUpWorld134_g1;
				float3 penetratorForwardWorld126_g1 = _PenetratorForwardWorld;
				float3 forward169_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis169_g1 = ChangeOfBasis169_g1( right169_g1 , up169_g1 , forward169_g1 );
				float3 right9_g1 = penetratorRightWorld139_g1;
				float3 up9_g1 = penetratorUpWorld134_g1;
				float3 forward9_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis9_g1 = ChangeOfBasis9_g1( right9_g1 , up9_g1 , forward9_g1 );
				float3 normalizeResult37 = normalize( _DickForward );
				float3 temp_output_36_0 = ( ( _BulgeProgress * normalizeResult37 ) + _DickOffset );
				float3 temp_output_3_0_g2 = temp_output_36_0;
				float3 normalizeResult6_g2 = normalize( ( temp_output_48_0 - temp_output_3_0_g2 ) );
				float3 temp_output_28_0 = ( temp_output_48_0 - temp_output_36_0 );
				float temp_output_41_0 = ( saturate( ( ( _BulgeRadius - length( temp_output_28_0 ) ) * 10.0 ) ) * inputMesh.ase_color.g * _BulgeBlend );
				float3 lerpResult33 = lerp( temp_output_48_0 , ( ( normalizeResult6_g2 * _BulgeRadius ) + temp_output_3_0_g2 ) , temp_output_41_0);
				float4 appendResult67_g1 = (float4(lerpResult33 , 1.0));
				float4 transform66_g1 = mul(GetObjectToWorldMatrix(),appendResult67_g1);
				transform66_g1.xyz = GetAbsolutePositionWS((transform66_g1).xyz);
				float3 localPenetratorSpaceVertexPosition142_g1 = ( (transform66_g1).xyz - ( _PenetratorStartWorld - penetratorRootWorld122_g1 ) );
				float3 temp_output_12_0_g1 = mul( localChangeOfBasis9_g1, ( localPenetratorSpaceVertexPosition142_g1 - penetratorRootWorld122_g1 ) );
				float3 break15_g1 = temp_output_12_0_g1;
				float temp_output_18_0_g1 = ( break15_g1.z * _SquashStretchCorrection );
				float3 appendResult26_g1 = (float3(break15_g1.x , break15_g1.y , temp_output_18_0_g1));
				float3 appendResult25_g1 = (float3(( break15_g1.x / _SquashStretchCorrection ) , ( break15_g1.y / _SquashStretchCorrection ) , temp_output_18_0_g1));
				float distanceToHole180_g1 = _DistanceToHole;
				float temp_output_17_0_g1 = ( distanceToHole180_g1 * 0.5 );
				float smoothstepResult23_g1 = smoothstep( 0.0 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float smoothstepResult22_g1 = smoothstep( distanceToHole180_g1 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float3 lerpResult31_g1 = lerp( appendResult26_g1 , appendResult25_g1 , min( smoothstepResult23_g1 , smoothstepResult22_g1 ));
				float3 lerpResult32_g1 = lerp( lerpResult31_g1 , ( temp_output_12_0_g1 + ( ( distanceToHole180_g1 - ( ( distanceToHole180_g1 / ( _SquashStretchCorrection * _PenetratorWorldLength ) ) * _PenetratorWorldLength ) ) * float3(0,0,1) ) ) , step( distanceToHole180_g1 , temp_output_18_0_g1 ));
				float3 squashStretchedPosition44_g1 = lerpResult32_g1;
				float3 temp_output_150_0_g1 = ( float3(0,0,1) * _TruncateLength );
				float3 temp_output_149_0_g1 = ( squashStretchedPosition44_g1 - temp_output_150_0_g1 );
				float3 normalizeResult156_g1 = normalize( temp_output_149_0_g1 );
				float3 lerpResult152_g1 = lerp( temp_output_149_0_g1 , ( normalizeResult156_g1 * min( length( temp_output_149_0_g1 ) , _GirthRadius ) ) , saturate( ( temp_output_149_0_g1.z * ( 1.0 / _GirthRadius ) ) ));
				#ifdef _TRUNCATESPHERIZE_ON
				float3 staticSwitch116_g1 = ( lerpResult152_g1 + temp_output_150_0_g1 );
				#else
				float3 staticSwitch116_g1 = squashStretchedPosition44_g1;
				#endif
				float3 TruncatedPosition147_g1 = ( penetratorRootWorld122_g1 + mul( transpose( localChangeOfBasis169_g1 ), staticSwitch116_g1 ) );
				float3 worldPosition56_g1 = ( TruncatedPosition147_g1 + ( penetratorForwardWorld126_g1 * _PenetratorOffsetLength ) );
				float3 worldPenetratorForward56_g1 = penetratorForwardWorld126_g1;
				float3 worldPenetratorUp56_g1 = penetratorUpWorld134_g1;
				float3 worldPenetratorRight56_g1 = penetratorRightWorld139_g1;
				float3 temp_output_50_0_g265 = inputMesh.normalOS;
				float2 break146_g266 = normalizeResult41_g266;
				float4 appendResult139_g266 = (float4(temp_output_48_0_g266 , break146_g266.x , break146_g266.y , 0.0));
				float3 normalizeResult144_g266 = normalize( (mul( invertVal44_g266, appendResult139_g266 )).xyz );
				float3 lerpResult44_g265 = lerp( normalizeResult144_g266 , temp_output_50_0_g265 , saturate( sign( temp_output_114_0_g266 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g265 = lerpResult44_g265;
				#else
				float3 staticSwitch17_g265 = temp_output_50_0_g265;
				#endif
				float3 temp_output_48_42 = staticSwitch17_g265;
				float3 normalizeResult38 = normalize( temp_output_28_0 );
				float dotResult50 = dot( temp_output_28_0 , normalizeResult37 );
				float3 lerpResult39 = lerp( temp_output_48_42 , normalizeResult38 , ( temp_output_41_0 * ( 1.0 - saturate( abs( dotResult50 ) ) ) ));
				float3 normalizeResult44 = normalize( lerpResult39 );
				float4 appendResult86_g1 = (float4(normalizeResult44 , 0.0));
				float3 normalizeResult87_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult86_g1 )).xyz );
				float3 worldNormal56_g1 = normalizeResult87_g1;
				float4 break93_g1 = inputMesh.ase_tangent;
				float4 appendResult89_g1 = (float4(break93_g1.x , break93_g1.y , break93_g1.z , 0.0));
				float3 normalizeResult91_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult89_g1 )).xyz );
				float4 appendResult94_g1 = (float4(normalizeResult91_g1 , break93_g1.w));
				float4 worldTangent56_g1 = appendResult94_g1;
				float3 worldPositionOUT56_g1 = float3( 0,0,0 );
				float3 worldNormalOUT56_g1 = float3( 0,0,0 );
				float4 worldTangentOUT56_g1 = float4( 0,0,0,0 );
				{
				ToCatmullRomSpace_float(worldPenetratorRootPos56_g1,worldPosition56_g1,worldPenetratorForward56_g1,worldPenetratorUp56_g1,worldPenetratorRight56_g1,worldNormal56_g1,worldTangent56_g1,worldPositionOUT56_g1,worldNormalOUT56_g1,worldTangentOUT56_g1);
				}
				float4 appendResult73_g1 = (float4(worldPositionOUT56_g1 , 1.0));
				float4 localWorldVar72_g1 = appendResult73_g1;
				(localWorldVar72_g1).xyz = GetCameraRelativePositionWS((localWorldVar72_g1).xyz);
				float4 transform72_g1 = mul(GetWorldToObjectMatrix(),localWorldVar72_g1);
				float3 lerpResult15 = lerp( temp_output_48_0 , (transform72_g1).xyz , inputMesh.ase_color.r);
				
				float4 appendResult75_g1 = (float4(worldNormalOUT56_g1 , 0.0));
				float3 normalizeResult76_g1 = normalize( (mul( GetWorldToObjectMatrix(), appendResult75_g1 )).xyz );
				float3 lerpResult17 = lerp( temp_output_48_42 , normalizeResult76_g1 , inputMesh.ase_color.r);
				
				outputPackedVaryingsMeshToPS.ase_texcoord3.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = lerpResult15;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS = lerpResult17;
				return inputMesh;
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh)
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS = (PackedVaryingsMeshToPS)0;
				AttributesMesh defaultMesh = inputMesh;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification( inputMesh, _TimeParameters.xyz, outputPackedVaryingsMeshToPS);

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);

				float3 VMESHpositionRWS = positionRWS;
				float4 VMESHpositionCS = TransformWorldToHClip(positionRWS);

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
						AttributesMesh previousMesh = defaultMesh;
						previousMesh.positionOS = effectivePositionOS ;
						PackedVaryingsMeshToPS test = (PackedVaryingsMeshToPS)0;
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

					#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
						if (_TransparentCameraOnlyMotionVectors > 0)
						{
							previousPositionRWS = VMESHpositionRWS.xyz;
						}
					#endif

					VPASSpreviousPositionCS = mul(UNITY_MATRIX_PREV_VP, float4(previousPositionRWS, 1.0));
				}

				outputPackedVaryingsMeshToPS.vmeshPositionCS = VMESHpositionCS;
				outputPackedVaryingsMeshToPS.vmeshInterp00.xyz = VMESHpositionRWS;

				outputPackedVaryingsMeshToPS.vpassInterpolators0 = float3(VPASSpositionCS.xyw);
				outputPackedVaryingsMeshToPS.vpassInterpolators1 = float3(VPASSpreviousPositionCS.xyw);
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalPrepassBuffer.hlsl"
			#endif

			#if ( 0 ) // TEMPORARY: defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float3 previousPositionOS : TEXCOORD4;
				float3 precomputedVelocity : TEXCOORD5;
				float4 ase_color : COLOR;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
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
				o.ase_color = v.ase_color;
				o.ase_tangent = v.ase_tangent;
				o.ase_texcoord = v.ase_texcoord;
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
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.previousPositionOS = patch[0].previousPositionOS * bary.x + patch[1].previousPositionOS * bary.y + patch[2].previousPositionOS * bary.z;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					o.precomputedVelocity = patch[0].precomputedVelocity * bary.x + patch[1].precomputedVelocity * bary.y + patch[2].precomputedVelocity * bary.z;
				#endif
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
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
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
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

			void Frag( PackedVaryingsMeshToPS packedInput
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

				SurfaceData surfaceData;
				BuiltinData builtinData;

				SmoothSurfaceDescription surfaceDescription = (SmoothSurfaceDescription)0;
				float2 uv_NormalMap = packedInput.ase_texcoord3.xy * _NormalMap_ST.xy + _NormalMap_ST.zw;
				
				float2 uv_MaskMap = packedInput.ase_texcoord3.xy * _MaskMap_ST.xy + _MaskMap_ST.zw;
				float4 tex2DNode46 = tex2D( _MaskMap, uv_MaskMap );
				
				surfaceDescription.Normal = UnpackNormalScale( tex2D( _NormalMap, uv_NormalMap ), 1.0f );
				surfaceDescription.Smoothness = tex2DNode46.a;
				surfaceDescription.Alpha = 1;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				GetSurfaceAndBuiltinData( surfaceDescription, input, V, posInput, surfaceData, builtinData );

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
			
			Name "Forward"
			Tags { "LightMode"="Forward" }

			Blend [_SrcBlend] [_DstBlend], [_AlphaSrcBlend] [_AlphaDstBlend]
			Blend 1 SrcAlpha OneMinusSrcAlpha

			Cull [_CullModeForward]
			ZTest [_ZTestDepthEqualForOpaque]
			ZWrite [_ZWrite]

			Stencil
			{
				Ref [_StencilRef]
				WriteMask [_StencilWriteMask]
				Comp Always
				Pass Replace
			}


            ColorMask [_ColorMaskTransparentVelOne] 1
            ColorMask [_ColorMaskTransparentVelTwo] 2

			HLSLPROGRAM

            #define _SPECULAR_OCCLUSION_FROM_AO 1
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #define ASE_ABSOLUTE_VERTEX_POS 1
            #define _AMBIENT_OCCLUSION 1
            #define HAVE_MESH_MODIFICATION
            #define ASE_SRP_VERSION 140008


            #pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _TRANSPARENT_WRITES_MOTION_VEC
			#pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile_fragment _ SHADOWS_SHADOWMASK
			#pragma multi_compile_fragment SHADOW_LOW SHADOW_MEDIUM SHADOW_HIGH
			#pragma multi_compile_fragment AREA_SHADOW_MEDIUM AREA_SHADOW_HIGH
			#pragma multi_compile_fragment PROBE_VOLUMES_OFF PROBE_VOLUMES_L1 PROBE_VOLUMES_L2
            #pragma multi_compile_fragment SCREEN_SPACE_SHADOWS_OFF SCREEN_SPACE_SHADOWS_ON
            #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
			#pragma multi_compile _ DEBUG_DISPLAY
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile_fragment DECALS_OFF DECALS_3RT DECALS_4RT
			#pragma multi_compile_fragment _ DECAL_SURFACE_GRADIENT

			#ifndef SHADER_STAGE_FRAGMENT
			#define SHADOW_LOW
			#define USE_FPTL_LIGHTLIST
			#endif

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

			#define SHADERPASS SHADERPASS_FORWARD
		    #define HAS_LIGHTLOOP 1

			// Setup for Fog Enabled to apply in sky refletions in LightLoopDef.hlsl
            #define APPLY_FOG_ON_SKY_REFLECTIONS

			#ifndef SHADER_UNLIT
			#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
			#define VARYINGS_NEED_CULLFACE
			#endif
			#endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

		    #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
			#undef  _REFRACTION_PLANE
			#undef  _REFRACTION_SPHERE
			#define _REFRACTION_THIN
		    #endif

			#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
			#if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
				#define WRITE_NORMAL_BUFFER
			#endif
			#endif

			#ifndef DEBUG_DISPLAY
				#if !defined(_SURFACE_TYPE_TRANSPARENT)
					#if SHADERPASS == SHADERPASS_FORWARD
					#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
					#elif SHADERPASS == SHADERPASS_GBUFFER
					#define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
					#endif
				#endif
			#endif

			#if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define _DEFERRED_CAPABLE_MATERIAL
			#endif

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			CBUFFER_START( UnityPerMaterial )
			float4 _MaskMap_ST;
			float4 _BaseColorMap_ST;
			float4 _NormalMap_ST;
			float3 _PenetratorStartWorld;
			float3 _DickForward;
			float3 _PenetratorForwardWorld;
			float3 _PenetratorUpWorld;
			float3 _DickOffset;
			float3 _PenetratorRootWorld;
			float3 _WorldDickPosition;
			float3 _WorldDickBinormal;
			float3 _PenetratorRightWorld;
			float3 _WorldDickNormal;
			float _BulgeProgress;
			float _Angle;
			float _TipRadius;
			float _BulgeRadius;
			float _BulgeBlend;
			float _SquashStretchCorrection;
			float _DistanceToHole;
			float _PenetratorWorldLength;
			float _TruncateLength;
			float _GirthRadius;
			float _PenetratorOffsetLength;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			float _EnableBlendModePreserveSpecularLighting;
            #endif
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
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			float _TessPhongStrength;
			float _TessValue;
			float _TessMin;
			float _TessMax;
			float _TessEdgeLength;
			float _TessMaxDisp;
			#endif
			CBUFFER_END

			// Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

			// Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			sampler2D _BaseColorMap;
			sampler2D _NormalMap;
			sampler2D _MaskMap;


            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			// Setup DECALS_OFF so the shader stripper can remove variants
            #define HAVE_DECALS ( (defined(DECALS_3RT) || defined(DECALS_4RT)) && !defined(_DISABLE_DECALS) )
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_TANGENT
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#pragma multi_compile_local __ _TRUNCATESPHERIZE_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float3 previousPositionOS : TEXCOORD4;
				float3 precomputedVelocity : TEXCOORD5;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				float3 normalWS : TEXCOORD1;
				float4 tangentWS : TEXCOORD2;
				float4 uv1 : TEXCOORD3;
				float4 uv2 : TEXCOORD4;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					float3 vpassPositionCS : TEXCOORD5;
					float3 vpassPreviousPositionCS : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
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
			
			float3 MyCustomExpression32_g265( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			
			float3x3 ChangeOfBasis169_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			
			float3x3 ChangeOfBasis9_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.baseColor =                 surfaceDescription.BaseColor;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif
				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness = 				surfaceDescription.Thickness;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.transmissionMask =			surfaceDescription.TransmissionMask;
				#endif
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.ior = surfaceDescription.RefractionIndex;
					surfaceData.transmittanceColor = surfaceDescription.RefractionColor;
					surfaceData.atDistance = surfaceDescription.RefractionDistance;

					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
                #ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				surfaceData.normalWS = float3(0, 1, 0);
                #endif
				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );

				// decals
			#ifdef DECAL_NORMAL_BLENDING
				if (_EnableDecals)
				{
					#ifndef SURFACE_GRADIENT
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);
					#endif

					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData, normalTS);
				}

				GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
			#else
				GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

				#if HAVE_DECALS
				if (_EnableDecals)
				{
					// Both uses and modifies 'surfaceData.normalWS'.
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif
			#endif

				bentNormalWS = surfaceData.normalWS;

				#ifdef ASE_BENT_NORMAL
				GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.tangentWS = TransformTangentToWorld( surfaceDescription.Tangent, fragInputs.tangentToWorld );
				#endif
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );


                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceData.perceptualSmoothness = GeometricNormalFiltering( surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[ 2 ], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				#endif

				#ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				#ifdef UNITY_VIRTUAL_TEXTURING
                builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif

				#ifdef ASE_BAKEDGI
				builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif

				#ifdef ASE_BAKEDBACKGI
				builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			AttributesMesh ApplyMeshModification(AttributesMesh inputMesh, float3 timeParameters, inout PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS )
			{
				_TimeParameters.xyz = timeParameters;
				float3 normalizeResult27_g267 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g267 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g267 = normalize( cross( normalizeResult27_g267 , normalizeResult31_g267 ) );
				float4 appendResult26_g266 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g266 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g266 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g266 = -_WorldDickPosition;
				float4 appendResult29_g266 = (float4(break27_g266.x , break27_g266.y , break27_g266.z , 1.0));
				float4x4 temp_output_30_0_g266 = mul( transpose( float4x4( float4( normalizeResult27_g267 , 0.0 ).x,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).x,float4( normalizeResult29_g267 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g267 , 0.0 ).y,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).y,float4( normalizeResult29_g267 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g267 , 0.0 ).z,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).z,float4( normalizeResult29_g267 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g267 , 0.0 ).w,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).w,float4( normalizeResult29_g267 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g266.x,appendResult28_g266.x,appendResult31_g266.x,appendResult29_g266.x,appendResult26_g266.y,appendResult28_g266.y,appendResult31_g266.y,appendResult29_g266.y,appendResult26_g266.z,appendResult28_g266.z,appendResult31_g266.z,appendResult29_g266.z,appendResult26_g266.w,appendResult28_g266.w,appendResult31_g266.w,appendResult29_g266.w ) );
				float4x4 invertVal44_g266 = Inverse4x4( temp_output_30_0_g266 );
				float4 appendResult27_g265 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g265 = mul( GetObjectToWorldMatrix(), appendResult27_g265 ).xyz;
				float3 localMyCustomExpression32_g265 = MyCustomExpression32_g265( pos32_g265 );
				float4 appendResult32_g266 = (float4(localMyCustomExpression32_g265 , 1.0));
				float4 break35_g266 = mul( temp_output_30_0_g266, appendResult32_g266 );
				float temp_output_124_0_g266 = _TipRadius;
				float2 appendResult36_g266 = (float2(break35_g266.y , break35_g266.z));
				float2 normalizeResult41_g266 = normalize( appendResult36_g266 );
				float temp_output_120_0_g266 = sqrt( max( break35_g266.x , 0.0 ) );
				float temp_output_48_0_g266 = tan( radians( _Angle ) );
				float temp_output_125_0_g266 = ( temp_output_124_0_g266 + ( temp_output_120_0_g266 * temp_output_48_0_g266 ) );
				float temp_output_37_0_g266 = length( appendResult36_g266 );
				float temp_output_114_0_g266 = ( ( temp_output_125_0_g266 - temp_output_37_0_g266 ) + 1.0 );
				float lerpResult102_g266 = lerp( temp_output_125_0_g266 , temp_output_37_0_g266 , saturate( temp_output_114_0_g266 ));
				float lerpResult130_g266 = lerp( 0.0 , lerpResult102_g266 , saturate( ( -( -temp_output_124_0_g266 - break35_g266.x ) / temp_output_124_0_g266 ) ));
				float2 break43_g266 = ( normalizeResult41_g266 * lerpResult130_g266 );
				float4 appendResult40_g266 = (float4(max( break35_g266.x , -temp_output_124_0_g266 ) , break43_g266.x , break43_g266.y , 1.0));
				float4 appendResult28_g265 = (float4(((mul( invertVal44_g266, appendResult40_g266 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g265 = appendResult28_g265;
				(localWorldVar29_g265).xyz = GetCameraRelativePositionWS((localWorldVar29_g265).xyz);
				float4 transform29_g265 = mul(GetWorldToObjectMatrix(),localWorldVar29_g265);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g265 = (transform29_g265).xyz;
				#else
				float3 staticSwitch13_g265 = inputMesh.positionOS;
				#endif
				float3 temp_output_48_0 = staticSwitch13_g265;
				float localToCatmullRomSpace_float56_g1 = ( 0.0 );
				float3 penetratorRootWorld122_g1 = _PenetratorRootWorld;
				float3 worldPenetratorRootPos56_g1 = penetratorRootWorld122_g1;
				float3 penetratorRightWorld139_g1 = _PenetratorRightWorld;
				float3 right169_g1 = penetratorRightWorld139_g1;
				float3 penetratorUpWorld134_g1 = _PenetratorUpWorld;
				float3 up169_g1 = penetratorUpWorld134_g1;
				float3 penetratorForwardWorld126_g1 = _PenetratorForwardWorld;
				float3 forward169_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis169_g1 = ChangeOfBasis169_g1( right169_g1 , up169_g1 , forward169_g1 );
				float3 right9_g1 = penetratorRightWorld139_g1;
				float3 up9_g1 = penetratorUpWorld134_g1;
				float3 forward9_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis9_g1 = ChangeOfBasis9_g1( right9_g1 , up9_g1 , forward9_g1 );
				float3 normalizeResult37 = normalize( _DickForward );
				float3 temp_output_36_0 = ( ( _BulgeProgress * normalizeResult37 ) + _DickOffset );
				float3 temp_output_3_0_g2 = temp_output_36_0;
				float3 normalizeResult6_g2 = normalize( ( temp_output_48_0 - temp_output_3_0_g2 ) );
				float3 temp_output_28_0 = ( temp_output_48_0 - temp_output_36_0 );
				float temp_output_41_0 = ( saturate( ( ( _BulgeRadius - length( temp_output_28_0 ) ) * 10.0 ) ) * inputMesh.ase_color.g * _BulgeBlend );
				float3 lerpResult33 = lerp( temp_output_48_0 , ( ( normalizeResult6_g2 * _BulgeRadius ) + temp_output_3_0_g2 ) , temp_output_41_0);
				float4 appendResult67_g1 = (float4(lerpResult33 , 1.0));
				float4 transform66_g1 = mul(GetObjectToWorldMatrix(),appendResult67_g1);
				transform66_g1.xyz = GetAbsolutePositionWS((transform66_g1).xyz);
				float3 localPenetratorSpaceVertexPosition142_g1 = ( (transform66_g1).xyz - ( _PenetratorStartWorld - penetratorRootWorld122_g1 ) );
				float3 temp_output_12_0_g1 = mul( localChangeOfBasis9_g1, ( localPenetratorSpaceVertexPosition142_g1 - penetratorRootWorld122_g1 ) );
				float3 break15_g1 = temp_output_12_0_g1;
				float temp_output_18_0_g1 = ( break15_g1.z * _SquashStretchCorrection );
				float3 appendResult26_g1 = (float3(break15_g1.x , break15_g1.y , temp_output_18_0_g1));
				float3 appendResult25_g1 = (float3(( break15_g1.x / _SquashStretchCorrection ) , ( break15_g1.y / _SquashStretchCorrection ) , temp_output_18_0_g1));
				float distanceToHole180_g1 = _DistanceToHole;
				float temp_output_17_0_g1 = ( distanceToHole180_g1 * 0.5 );
				float smoothstepResult23_g1 = smoothstep( 0.0 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float smoothstepResult22_g1 = smoothstep( distanceToHole180_g1 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float3 lerpResult31_g1 = lerp( appendResult26_g1 , appendResult25_g1 , min( smoothstepResult23_g1 , smoothstepResult22_g1 ));
				float3 lerpResult32_g1 = lerp( lerpResult31_g1 , ( temp_output_12_0_g1 + ( ( distanceToHole180_g1 - ( ( distanceToHole180_g1 / ( _SquashStretchCorrection * _PenetratorWorldLength ) ) * _PenetratorWorldLength ) ) * float3(0,0,1) ) ) , step( distanceToHole180_g1 , temp_output_18_0_g1 ));
				float3 squashStretchedPosition44_g1 = lerpResult32_g1;
				float3 temp_output_150_0_g1 = ( float3(0,0,1) * _TruncateLength );
				float3 temp_output_149_0_g1 = ( squashStretchedPosition44_g1 - temp_output_150_0_g1 );
				float3 normalizeResult156_g1 = normalize( temp_output_149_0_g1 );
				float3 lerpResult152_g1 = lerp( temp_output_149_0_g1 , ( normalizeResult156_g1 * min( length( temp_output_149_0_g1 ) , _GirthRadius ) ) , saturate( ( temp_output_149_0_g1.z * ( 1.0 / _GirthRadius ) ) ));
				#ifdef _TRUNCATESPHERIZE_ON
				float3 staticSwitch116_g1 = ( lerpResult152_g1 + temp_output_150_0_g1 );
				#else
				float3 staticSwitch116_g1 = squashStretchedPosition44_g1;
				#endif
				float3 TruncatedPosition147_g1 = ( penetratorRootWorld122_g1 + mul( transpose( localChangeOfBasis169_g1 ), staticSwitch116_g1 ) );
				float3 worldPosition56_g1 = ( TruncatedPosition147_g1 + ( penetratorForwardWorld126_g1 * _PenetratorOffsetLength ) );
				float3 worldPenetratorForward56_g1 = penetratorForwardWorld126_g1;
				float3 worldPenetratorUp56_g1 = penetratorUpWorld134_g1;
				float3 worldPenetratorRight56_g1 = penetratorRightWorld139_g1;
				float3 temp_output_50_0_g265 = inputMesh.normalOS;
				float2 break146_g266 = normalizeResult41_g266;
				float4 appendResult139_g266 = (float4(temp_output_48_0_g266 , break146_g266.x , break146_g266.y , 0.0));
				float3 normalizeResult144_g266 = normalize( (mul( invertVal44_g266, appendResult139_g266 )).xyz );
				float3 lerpResult44_g265 = lerp( normalizeResult144_g266 , temp_output_50_0_g265 , saturate( sign( temp_output_114_0_g266 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g265 = lerpResult44_g265;
				#else
				float3 staticSwitch17_g265 = temp_output_50_0_g265;
				#endif
				float3 temp_output_48_42 = staticSwitch17_g265;
				float3 normalizeResult38 = normalize( temp_output_28_0 );
				float dotResult50 = dot( temp_output_28_0 , normalizeResult37 );
				float3 lerpResult39 = lerp( temp_output_48_42 , normalizeResult38 , ( temp_output_41_0 * ( 1.0 - saturate( abs( dotResult50 ) ) ) ));
				float3 normalizeResult44 = normalize( lerpResult39 );
				float4 appendResult86_g1 = (float4(normalizeResult44 , 0.0));
				float3 normalizeResult87_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult86_g1 )).xyz );
				float3 worldNormal56_g1 = normalizeResult87_g1;
				float4 break93_g1 = inputMesh.tangentOS;
				float4 appendResult89_g1 = (float4(break93_g1.x , break93_g1.y , break93_g1.z , 0.0));
				float3 normalizeResult91_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult89_g1 )).xyz );
				float4 appendResult94_g1 = (float4(normalizeResult91_g1 , break93_g1.w));
				float4 worldTangent56_g1 = appendResult94_g1;
				float3 worldPositionOUT56_g1 = float3( 0,0,0 );
				float3 worldNormalOUT56_g1 = float3( 0,0,0 );
				float4 worldTangentOUT56_g1 = float4( 0,0,0,0 );
				{
				ToCatmullRomSpace_float(worldPenetratorRootPos56_g1,worldPosition56_g1,worldPenetratorForward56_g1,worldPenetratorUp56_g1,worldPenetratorRight56_g1,worldNormal56_g1,worldTangent56_g1,worldPositionOUT56_g1,worldNormalOUT56_g1,worldTangentOUT56_g1);
				}
				float4 appendResult73_g1 = (float4(worldPositionOUT56_g1 , 1.0));
				float4 localWorldVar72_g1 = appendResult73_g1;
				(localWorldVar72_g1).xyz = GetCameraRelativePositionWS((localWorldVar72_g1).xyz);
				float4 transform72_g1 = mul(GetWorldToObjectMatrix(),localWorldVar72_g1);
				float3 lerpResult15 = lerp( temp_output_48_0 , (transform72_g1).xyz , inputMesh.ase_color.r);
				
				float4 appendResult75_g1 = (float4(worldNormalOUT56_g1 , 0.0));
				float3 normalizeResult76_g1 = normalize( (mul( GetWorldToObjectMatrix(), appendResult75_g1 )).xyz );
				float3 lerpResult17 = lerp( temp_output_48_42 , normalizeResult76_g1 , inputMesh.ase_color.r);
				
				float4 break79_g1 = worldTangentOUT56_g1;
				float4 appendResult77_g1 = (float4(break79_g1.x , break79_g1.y , break79_g1.z , 0.0));
				float3 normalizeResult80_g1 = normalize( (mul( GetWorldToObjectMatrix(), appendResult77_g1 )).xyz );
				float4 appendResult83_g1 = (float4(normalizeResult80_g1 , break79_g1.w));
				float4 lerpResult20 = lerp( inputMesh.tangentOS , appendResult83_g1 , inputMesh.ase_color.r);
				
				outputPackedVaryingsMeshToPS.ase_texcoord7.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord7.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = lerpResult15;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS = lerpResult17;
				inputMesh.tangentOS = lerpResult20;
				return inputMesh;
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh)
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS = (PackedVaryingsMeshToPS)0;
				AttributesMesh defaultMesh = inputMesh;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification( inputMesh, _TimeParameters.xyz, outputPackedVaryingsMeshToPS);

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
				float4 VPASSpreviousPositionCS;
				float4 VPASSpositionCS = mul(UNITY_MATRIX_UNJITTERED_VP, float4(positionRWS, 1.0));

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
						AttributesMesh previousMesh = defaultMesh;
						previousMesh.positionOS = effectivePositionOS ;
						PackedVaryingsMeshToPS test = (PackedVaryingsMeshToPS)0;
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
				#endif

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.positionRWS.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.normalWS.xyz = normalWS;
				outputPackedVaryingsMeshToPS.tangentWS.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.uv1.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.uv2.xyzw = inputMesh.uv2;

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					outputPackedVaryingsMeshToPS.vpassPositionCS = float3(VPASSpositionCS.xyw);
					outputPackedVaryingsMeshToPS.vpassPreviousPositionCS = float3(VPASSpreviousPositionCS.xyw);
				#endif
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
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
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
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
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

            #ifdef UNITY_VIRTUAL_TEXTURING
                #ifdef OUTPUT_SPLIT_LIGHTING
                   #define DIFFUSE_LIGHTING_TARGET SV_Target2
                   #define SSS_BUFFER_TARGET SV_Target3
                #elif defined(_WRITE_TRANSPARENT_MOTION_VECTOR)
                   #define MOTION_VECTOR_TARGET SV_Target2
            	#endif
            #if defined(SHADER_API_PSSL)
            	#pragma PSSL_target_output_format(target 1 FMT_32_ABGR)
            #endif
            #else
                #ifdef OUTPUT_SPLIT_LIGHTING
                #define DIFFUSE_LIGHTING_TARGET SV_Target1
                #define SSS_BUFFER_TARGET SV_Target2
                #elif defined(_WRITE_TRANSPARENT_MOTION_VECTOR)
                #define MOTION_VECTOR_TARGET SV_Target1
                #endif
            #endif

			void Frag(PackedVaryingsMeshToPS packedInput
				, out float4 outColor:SV_Target0
            #ifdef UNITY_VIRTUAL_TEXTURING
				, out float4 outVTFeedback : SV_Target1
            #endif
            #ifdef OUTPUT_SPLIT_LIGHTING
				, out float4 outDiffuseLighting : DIFFUSE_LIGHTING_TARGET
				, OUTPUT_SSSBUFFER(outSSSBuffer) : SSS_BUFFER_TARGET
            #elif defined(_WRITE_TRANSPARENT_MOTION_VECTOR)
				, out float4 outMotionVec : MOTION_VECTOR_TARGET
            #endif
            #ifdef _DEPTHOFFSET_ON
				, out float outputDepth : DEPTH_OFFSET_SEMANTIC
            #endif
		    
						)
			{
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					outMotionVec = float4(2.0, 0.0, 0.0, 1.0);
				#endif

				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				float3 positionRWS = packedInput.positionRWS.xyz;
				float3 normalWS = packedInput.normalWS.xyz;
				float4 tangentWS = packedInput.tangentWS.xyzw;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);
				input.texCoord1 = packedInput.uv1.xyzw;
				input.texCoord2 = packedInput.uv2.xyzw;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				input.positionSS.xy = _OffScreenRendering > 0 ? (uint2)round(input.positionSS.xy * _OffScreenDownsampleFactor) : input.positionSS.xy;
				uint2 tileIndex = uint2(input.positionSS.xy) / GetTileSize ();

				PositionInputs posInput = GetPositionInput( input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS.xyz, tileIndex );

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				float2 uv_BaseColorMap = packedInput.ase_texcoord7.xy * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw;
				
				float2 uv_NormalMap = packedInput.ase_texcoord7.xy * _NormalMap_ST.xy + _NormalMap_ST.zw;
				
				float2 uv_MaskMap = packedInput.ase_texcoord7.xy * _MaskMap_ST.xy + _MaskMap_ST.zw;
				float4 tex2DNode46 = tex2D( _MaskMap, uv_MaskMap );
				
				surfaceDescription.BaseColor = tex2D( _BaseColorMap, uv_BaseColorMap ).rgb;
				surfaceDescription.Normal = UnpackNormalScale( tex2D( _NormalMap, uv_NormalMap ), 1.0f );
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = tex2DNode46.r;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = 0;
				surfaceDescription.Smoothness = tex2DNode46.a;
				surfaceDescription.Occlusion = tex2DNode46.g;
				surfaceDescription.Alpha = 1;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceDescription.TransmissionMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				#ifdef ASE_BAKEDGI
				surfaceDescription.BakedGI = 0;
				#endif
				#ifdef ASE_BAKEDBACKGI
				surfaceDescription.BakedBackGI = 0;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				#ifdef UNITY_VIRTUAL_TEXTURING
				surfaceDescription.VTPackedFeedback = float4(1.0f,1.0f,1.0f,1.0f);
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);

				PreLightData preLightData = GetPreLightData(V, posInput, bsdfData);

				outColor = float4(0.0, 0.0, 0.0, 0.0);

            #ifdef DEBUG_DISPLAY
            #ifdef OUTPUT_SPLIT_LIGHTING
				outDiffuseLighting = float4(0, 0, 0, 1);
				ENCODE_INTO_SSSBUFFER(surfaceData, posInput.positionSS, outSSSBuffer);
			#endif

				bool viewMaterial = false;
				int bufferSize = _DebugViewMaterialArray[0].x;
				if (bufferSize != 0)
				{
					bool needLinearToSRGB = false;
					float3 result = float3(1.0, 0.0, 1.0);

					for (int index = 1; index <= bufferSize; index++)
					{
						int indexMaterialProperty = _DebugViewMaterialArray[index].x;

						if (indexMaterialProperty != 0)
						{
							viewMaterial = true;

							GetPropertiesDataDebug(indexMaterialProperty, result, needLinearToSRGB);
							GetVaryingsDataDebug(indexMaterialProperty, input, result, needLinearToSRGB);
							GetBuiltinDataDebug(indexMaterialProperty, builtinData, posInput, result, needLinearToSRGB);
							GetSurfaceDataDebug(indexMaterialProperty, surfaceData, result, needLinearToSRGB);
							GetBSDFDataDebug(indexMaterialProperty, bsdfData, result, needLinearToSRGB);
						}
					}

					if (!needLinearToSRGB && _DebugAOVOutput == 0)
						result = SRGBToLinear(max(0, result));

					outColor = float4(result, 1.0);
				}

				if (!viewMaterial)
				{
					if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_VALIDATE_DIFFUSE_COLOR || _DebugFullScreenMode == FULLSCREENDEBUGMODE_VALIDATE_SPECULAR_COLOR)
					{
						float3 result = float3(0.0, 0.0, 0.0);
						GetPBRValidatorDebug(surfaceData, result);
						outColor = float4(result, 1.0f);
					}
					else if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_TRANSPARENCY_OVERDRAW)
					{
						float4 result = _DebugTransparencyOverdrawWeight * float4(TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_A);
						outColor = result;
					}
					else
                #endif
					{
                #ifdef _SURFACE_TYPE_TRANSPARENT
						uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_TRANSPARENT;
                #else
						uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_OPAQUE;
                #endif
						LightLoopOutput lightLoopOutput;
						LightLoop(V, posInput, preLightData, bsdfData, builtinData, featureFlags, lightLoopOutput);

						// Alias
						float3 diffuseLighting = lightLoopOutput.diffuseLighting;
						float3 specularLighting = lightLoopOutput.specularLighting;

						diffuseLighting *= GetCurrentExposureMultiplier();
						specularLighting *= GetCurrentExposureMultiplier();

                #ifdef OUTPUT_SPLIT_LIGHTING
						if (_EnableSubsurfaceScattering != 0 && ShouldOutputSplitLighting(bsdfData))
						{
							outColor = float4(specularLighting, 1.0);
							outDiffuseLighting = float4(TagLightingForSSS(diffuseLighting), 1.0);
						}
						else
						{
							outColor = float4(diffuseLighting + specularLighting, 1.0);
							outDiffuseLighting = float4(0, 0, 0, 1);
						}
						ENCODE_INTO_SSSBUFFER(surfaceData, posInput.positionSS, outSSSBuffer);
                #else
						outColor = ApplyBlendMode(diffuseLighting, specularLighting, builtinData.opacity);
						outColor = EvaluateAtmosphericScattering(posInput, V, outColor);
                #endif

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
						float4 VPASSpositionCS = float4(packedInput.vpassPositionCS.xy, 0.0, packedInput.vpassPositionCS.z);
						float4 VPASSpreviousPositionCS = float4(packedInput.vpassPreviousPositionCS.xy, 0.0, packedInput.vpassPreviousPositionCS.z);

						bool forceNoMotion = any(unity_MotionVectorsParams.yw == 0.0);
                #if defined(HAVE_VFX_MODIFICATION) && !VFX_FEATURE_MOTION_VECTORS
                        forceNoMotion = true;
                #endif
				        if (!forceNoMotion)
						{
							float2 motionVec = CalculateMotionVector(VPASSpositionCS, VPASSpreviousPositionCS);
							EncodeMotionVector(motionVec * 0.5, outMotionVec);
							outMotionVec.zw = 1.0;
						}
				#endif
				}

				#ifdef DEBUG_DISPLAY
				}
				#endif

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef UNITY_VIRTUAL_TEXTURING
					outVTFeedback = builtinData.vtPackedFeedback;
				#endif

                #ifdef UNITY_VIRTUAL_TEXTURING
				    float vtAlphaValue = builtinData.opacity;
                    #if defined(HAS_REFRACTION) && HAS_REFRACTION
					vtAlphaValue = 1.0f - bsdfData.transmittanceMask;
                #endif
				outVTFeedback = PackVTFeedbackWithAlpha(builtinData.vtPackedFeedback, input.positionSS.xy, vtAlphaValue);
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

			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _AMBIENT_OCCLUSION 1
			#define HAVE_MESH_MODIFICATION
			#define ASE_SRP_VERSION 140008


			#pragma editor_sync_compilation

			#pragma vertex Vert
			#pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define VARYINGS_NEED_TANGENT_TO_WORLD

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define SCENEPICKINGPASS 1

			#ifndef SHADER_UNLIT
			#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
		    #define VARYINGS_NEED_CULLFACE
			#endif
			#endif

			#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
			#if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
			#define WRITE_NORMAL_BUFFER
			#endif
			#endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

			#if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define _DEFERRED_CAPABLE_MATERIAL
			#endif

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			float4 _SelectionID;
            CBUFFER_START( UnityPerMaterial )
			float4 _MaskMap_ST;
			float4 _BaseColorMap_ST;
			float4 _NormalMap_ST;
			float3 _PenetratorStartWorld;
			float3 _DickForward;
			float3 _PenetratorForwardWorld;
			float3 _PenetratorUpWorld;
			float3 _DickOffset;
			float3 _PenetratorRootWorld;
			float3 _WorldDickPosition;
			float3 _WorldDickBinormal;
			float3 _PenetratorRightWorld;
			float3 _WorldDickNormal;
			float _BulgeProgress;
			float _Angle;
			float _TipRadius;
			float _BulgeRadius;
			float _BulgeBlend;
			float _SquashStretchCorrection;
			float _DistanceToHole;
			float _PenetratorWorldLength;
			float _TruncateLength;
			float _GirthRadius;
			float _PenetratorOffsetLength;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			float _EnableBlendModePreserveSpecularLighting;
            #endif
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
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			float _TessPhongStrength;
			float _TessValue;
			float _TessMin;
			float _TessMax;
			float _TessEdgeLength;
			float _TessMaxDisp;
			#endif
			CBUFFER_END

			

            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_TANGENT
			#pragma multi_compile_local __ _COCKVORESQUISHENABLED_ON
			#pragma multi_compile_local __ _TRUNCATESPHERIZE_ON
			#include "Packages/com.naelstrof-raliv.dynamic-penetration-for-games/Penetration.cginc"


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float3 normalWS : TEXCOORD0;
				float4 tangentWS : TEXCOORD1;
				
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
			
			float3 MyCustomExpression32_g265( float3 pos )
			{
				return GetAbsolutePositionWS(pos);
			}
			
			float3x3 ChangeOfBasis169_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			
			float3x3 ChangeOfBasis9_g1( float3 right, float3 up, float3 forward )
			{
				float3x3 basisTransform = 0;
				    basisTransform[0][0] = right.x;
				    basisTransform[0][1] = right.y;
				    basisTransform[0][2] = right.z;
				    basisTransform[1][0] = up.x;
				    basisTransform[1][1] = up.y;
				    basisTransform[1][2] = up.z;
				    basisTransform[2][0] = forward.x;
				    basisTransform[2][1] = forward.y;
				    basisTransform[2][2] = forward.z;
				return basisTransform;
			}
			

            struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
				float DepthOffset;
			};

			struct SurfaceDescriptionInputs
			{
				float3 ObjectSpaceNormal;
				float3 WorldSpaceNormal;
				float3 TangentSpaceNormal;
				float3 ObjectSpaceViewDirection;
				float3 WorldSpaceViewDirection;
				float3 ObjectSpacePosition;
			};


            void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {

                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif

                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
				float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
				ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);
                #endif

                #ifdef _ALPHATEST_ON
				float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
				alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                #endif
				GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif

                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
				ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

                #ifdef FRAG_INPUTS_USE_TEXCOORD1
				float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
				float4 lightmapTexCoord1 = float4(0, 0, 0, 0);
                #endif

                #ifdef FRAG_INPUTS_USE_TEXCOORD2
				float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
				float4 lightmapTexCoord2 = float4(0, 0, 0, 0);
                #endif

				//InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);

                //#else
                //BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

                ZERO_INITIALIZE(SurfaceData, surfaceData);

                ZERO_BUILTIN_INITIALIZE(builtinData);
                builtinData.opacity = surfaceDescription.Alpha;

                #if defined(DEBUG_DISPLAY)
				builtinData.renderingLayers = GetMeshRenderingLightLayer();
                #endif

                #ifdef _ALPHATEST_ON
				builtinData.alphaClipTreshold = alphaCutoff;
                #endif

                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif

                #if _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif

                #if (SHADERPASS == SHADERPASS_DISTORTION)
				builtinData.distortion = surfaceDescription.Distortion;
				builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif

                #ifndef SHADER_UNLIT
				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
				ApplyDebugToBuiltinData(builtinData);
                #endif

				RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS

            }


			VertexOutput VertexFunction(VertexInput inputMesh  )
			{

				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o );

				float3 normalizeResult27_g267 = normalize( _WorldDickNormal );
				float3 normalizeResult31_g267 = normalize( _WorldDickBinormal );
				float3 normalizeResult29_g267 = normalize( cross( normalizeResult27_g267 , normalizeResult31_g267 ) );
				float4 appendResult26_g266 = (float4(1.0 , 0.0 , 0.0 , 0.0));
				float4 appendResult28_g266 = (float4(0.0 , 1.0 , 0.0 , 0.0));
				float4 appendResult31_g266 = (float4(0.0 , 0.0 , 1.0 , 0.0));
				float3 break27_g266 = -_WorldDickPosition;
				float4 appendResult29_g266 = (float4(break27_g266.x , break27_g266.y , break27_g266.z , 1.0));
				float4x4 temp_output_30_0_g266 = mul( transpose( float4x4( float4( normalizeResult27_g267 , 0.0 ).x,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).x,float4( normalizeResult29_g267 , 0.0 ).x,float4(0,0,0,1).x,float4( normalizeResult27_g267 , 0.0 ).y,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).y,float4( normalizeResult29_g267 , 0.0 ).y,float4(0,0,0,1).y,float4( normalizeResult27_g267 , 0.0 ).z,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).z,float4( normalizeResult29_g267 , 0.0 ).z,float4(0,0,0,1).z,float4( normalizeResult27_g267 , 0.0 ).w,float4( cross( normalizeResult29_g267 , normalizeResult27_g267 ) , 0.0 ).w,float4( normalizeResult29_g267 , 0.0 ).w,float4(0,0,0,1).w ) ), float4x4( appendResult26_g266.x,appendResult28_g266.x,appendResult31_g266.x,appendResult29_g266.x,appendResult26_g266.y,appendResult28_g266.y,appendResult31_g266.y,appendResult29_g266.y,appendResult26_g266.z,appendResult28_g266.z,appendResult31_g266.z,appendResult29_g266.z,appendResult26_g266.w,appendResult28_g266.w,appendResult31_g266.w,appendResult29_g266.w ) );
				float4x4 invertVal44_g266 = Inverse4x4( temp_output_30_0_g266 );
				float4 appendResult27_g265 = (float4(inputMesh.positionOS , 1.0));
				float3 pos32_g265 = mul( GetObjectToWorldMatrix(), appendResult27_g265 ).xyz;
				float3 localMyCustomExpression32_g265 = MyCustomExpression32_g265( pos32_g265 );
				float4 appendResult32_g266 = (float4(localMyCustomExpression32_g265 , 1.0));
				float4 break35_g266 = mul( temp_output_30_0_g266, appendResult32_g266 );
				float temp_output_124_0_g266 = _TipRadius;
				float2 appendResult36_g266 = (float2(break35_g266.y , break35_g266.z));
				float2 normalizeResult41_g266 = normalize( appendResult36_g266 );
				float temp_output_120_0_g266 = sqrt( max( break35_g266.x , 0.0 ) );
				float temp_output_48_0_g266 = tan( radians( _Angle ) );
				float temp_output_125_0_g266 = ( temp_output_124_0_g266 + ( temp_output_120_0_g266 * temp_output_48_0_g266 ) );
				float temp_output_37_0_g266 = length( appendResult36_g266 );
				float temp_output_114_0_g266 = ( ( temp_output_125_0_g266 - temp_output_37_0_g266 ) + 1.0 );
				float lerpResult102_g266 = lerp( temp_output_125_0_g266 , temp_output_37_0_g266 , saturate( temp_output_114_0_g266 ));
				float lerpResult130_g266 = lerp( 0.0 , lerpResult102_g266 , saturate( ( -( -temp_output_124_0_g266 - break35_g266.x ) / temp_output_124_0_g266 ) ));
				float2 break43_g266 = ( normalizeResult41_g266 * lerpResult130_g266 );
				float4 appendResult40_g266 = (float4(max( break35_g266.x , -temp_output_124_0_g266 ) , break43_g266.x , break43_g266.y , 1.0));
				float4 appendResult28_g265 = (float4(((mul( invertVal44_g266, appendResult40_g266 )).xyz).xyz , 1.0));
				float4 localWorldVar29_g265 = appendResult28_g265;
				(localWorldVar29_g265).xyz = GetCameraRelativePositionWS((localWorldVar29_g265).xyz);
				float4 transform29_g265 = mul(GetWorldToObjectMatrix(),localWorldVar29_g265);
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch13_g265 = (transform29_g265).xyz;
				#else
				float3 staticSwitch13_g265 = inputMesh.positionOS;
				#endif
				float3 temp_output_48_0 = staticSwitch13_g265;
				float localToCatmullRomSpace_float56_g1 = ( 0.0 );
				float3 penetratorRootWorld122_g1 = _PenetratorRootWorld;
				float3 worldPenetratorRootPos56_g1 = penetratorRootWorld122_g1;
				float3 penetratorRightWorld139_g1 = _PenetratorRightWorld;
				float3 right169_g1 = penetratorRightWorld139_g1;
				float3 penetratorUpWorld134_g1 = _PenetratorUpWorld;
				float3 up169_g1 = penetratorUpWorld134_g1;
				float3 penetratorForwardWorld126_g1 = _PenetratorForwardWorld;
				float3 forward169_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis169_g1 = ChangeOfBasis169_g1( right169_g1 , up169_g1 , forward169_g1 );
				float3 right9_g1 = penetratorRightWorld139_g1;
				float3 up9_g1 = penetratorUpWorld134_g1;
				float3 forward9_g1 = penetratorForwardWorld126_g1;
				float3x3 localChangeOfBasis9_g1 = ChangeOfBasis9_g1( right9_g1 , up9_g1 , forward9_g1 );
				float3 normalizeResult37 = normalize( _DickForward );
				float3 temp_output_36_0 = ( ( _BulgeProgress * normalizeResult37 ) + _DickOffset );
				float3 temp_output_3_0_g2 = temp_output_36_0;
				float3 normalizeResult6_g2 = normalize( ( temp_output_48_0 - temp_output_3_0_g2 ) );
				float3 temp_output_28_0 = ( temp_output_48_0 - temp_output_36_0 );
				float temp_output_41_0 = ( saturate( ( ( _BulgeRadius - length( temp_output_28_0 ) ) * 10.0 ) ) * inputMesh.ase_color.g * _BulgeBlend );
				float3 lerpResult33 = lerp( temp_output_48_0 , ( ( normalizeResult6_g2 * _BulgeRadius ) + temp_output_3_0_g2 ) , temp_output_41_0);
				float4 appendResult67_g1 = (float4(lerpResult33 , 1.0));
				float4 transform66_g1 = mul(GetObjectToWorldMatrix(),appendResult67_g1);
				transform66_g1.xyz = GetAbsolutePositionWS((transform66_g1).xyz);
				float3 localPenetratorSpaceVertexPosition142_g1 = ( (transform66_g1).xyz - ( _PenetratorStartWorld - penetratorRootWorld122_g1 ) );
				float3 temp_output_12_0_g1 = mul( localChangeOfBasis9_g1, ( localPenetratorSpaceVertexPosition142_g1 - penetratorRootWorld122_g1 ) );
				float3 break15_g1 = temp_output_12_0_g1;
				float temp_output_18_0_g1 = ( break15_g1.z * _SquashStretchCorrection );
				float3 appendResult26_g1 = (float3(break15_g1.x , break15_g1.y , temp_output_18_0_g1));
				float3 appendResult25_g1 = (float3(( break15_g1.x / _SquashStretchCorrection ) , ( break15_g1.y / _SquashStretchCorrection ) , temp_output_18_0_g1));
				float distanceToHole180_g1 = _DistanceToHole;
				float temp_output_17_0_g1 = ( distanceToHole180_g1 * 0.5 );
				float smoothstepResult23_g1 = smoothstep( 0.0 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float smoothstepResult22_g1 = smoothstep( distanceToHole180_g1 , temp_output_17_0_g1 , temp_output_18_0_g1);
				float3 lerpResult31_g1 = lerp( appendResult26_g1 , appendResult25_g1 , min( smoothstepResult23_g1 , smoothstepResult22_g1 ));
				float3 lerpResult32_g1 = lerp( lerpResult31_g1 , ( temp_output_12_0_g1 + ( ( distanceToHole180_g1 - ( ( distanceToHole180_g1 / ( _SquashStretchCorrection * _PenetratorWorldLength ) ) * _PenetratorWorldLength ) ) * float3(0,0,1) ) ) , step( distanceToHole180_g1 , temp_output_18_0_g1 ));
				float3 squashStretchedPosition44_g1 = lerpResult32_g1;
				float3 temp_output_150_0_g1 = ( float3(0,0,1) * _TruncateLength );
				float3 temp_output_149_0_g1 = ( squashStretchedPosition44_g1 - temp_output_150_0_g1 );
				float3 normalizeResult156_g1 = normalize( temp_output_149_0_g1 );
				float3 lerpResult152_g1 = lerp( temp_output_149_0_g1 , ( normalizeResult156_g1 * min( length( temp_output_149_0_g1 ) , _GirthRadius ) ) , saturate( ( temp_output_149_0_g1.z * ( 1.0 / _GirthRadius ) ) ));
				#ifdef _TRUNCATESPHERIZE_ON
				float3 staticSwitch116_g1 = ( lerpResult152_g1 + temp_output_150_0_g1 );
				#else
				float3 staticSwitch116_g1 = squashStretchedPosition44_g1;
				#endif
				float3 TruncatedPosition147_g1 = ( penetratorRootWorld122_g1 + mul( transpose( localChangeOfBasis169_g1 ), staticSwitch116_g1 ) );
				float3 worldPosition56_g1 = ( TruncatedPosition147_g1 + ( penetratorForwardWorld126_g1 * _PenetratorOffsetLength ) );
				float3 worldPenetratorForward56_g1 = penetratorForwardWorld126_g1;
				float3 worldPenetratorUp56_g1 = penetratorUpWorld134_g1;
				float3 worldPenetratorRight56_g1 = penetratorRightWorld139_g1;
				float3 temp_output_50_0_g265 = inputMesh.normalOS;
				float2 break146_g266 = normalizeResult41_g266;
				float4 appendResult139_g266 = (float4(temp_output_48_0_g266 , break146_g266.x , break146_g266.y , 0.0));
				float3 normalizeResult144_g266 = normalize( (mul( invertVal44_g266, appendResult139_g266 )).xyz );
				float3 lerpResult44_g265 = lerp( normalizeResult144_g266 , temp_output_50_0_g265 , saturate( sign( temp_output_114_0_g266 ) ));
				#ifdef _COCKVORESQUISHENABLED_ON
				float3 staticSwitch17_g265 = lerpResult44_g265;
				#else
				float3 staticSwitch17_g265 = temp_output_50_0_g265;
				#endif
				float3 temp_output_48_42 = staticSwitch17_g265;
				float3 normalizeResult38 = normalize( temp_output_28_0 );
				float dotResult50 = dot( temp_output_28_0 , normalizeResult37 );
				float3 lerpResult39 = lerp( temp_output_48_42 , normalizeResult38 , ( temp_output_41_0 * ( 1.0 - saturate( abs( dotResult50 ) ) ) ));
				float3 normalizeResult44 = normalize( lerpResult39 );
				float4 appendResult86_g1 = (float4(normalizeResult44 , 0.0));
				float3 normalizeResult87_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult86_g1 )).xyz );
				float3 worldNormal56_g1 = normalizeResult87_g1;
				float4 break93_g1 = inputMesh.tangentOS;
				float4 appendResult89_g1 = (float4(break93_g1.x , break93_g1.y , break93_g1.z , 0.0));
				float3 normalizeResult91_g1 = normalize( (mul( GetObjectToWorldMatrix(), appendResult89_g1 )).xyz );
				float4 appendResult94_g1 = (float4(normalizeResult91_g1 , break93_g1.w));
				float4 worldTangent56_g1 = appendResult94_g1;
				float3 worldPositionOUT56_g1 = float3( 0,0,0 );
				float3 worldNormalOUT56_g1 = float3( 0,0,0 );
				float4 worldTangentOUT56_g1 = float4( 0,0,0,0 );
				{
				ToCatmullRomSpace_float(worldPenetratorRootPos56_g1,worldPosition56_g1,worldPenetratorForward56_g1,worldPenetratorUp56_g1,worldPenetratorRight56_g1,worldNormal56_g1,worldTangent56_g1,worldPositionOUT56_g1,worldNormalOUT56_g1,worldTangentOUT56_g1);
				}
				float4 appendResult73_g1 = (float4(worldPositionOUT56_g1 , 1.0));
				float4 localWorldVar72_g1 = appendResult73_g1;
				(localWorldVar72_g1).xyz = GetCameraRelativePositionWS((localWorldVar72_g1).xyz);
				float4 transform72_g1 = mul(GetWorldToObjectMatrix(),localWorldVar72_g1);
				float3 lerpResult15 = lerp( temp_output_48_0 , (transform72_g1).xyz , inputMesh.ase_color.r);
				
				float4 appendResult75_g1 = (float4(worldNormalOUT56_g1 , 0.0));
				float3 normalizeResult76_g1 = normalize( (mul( GetWorldToObjectMatrix(), appendResult75_g1 )).xyz );
				float3 lerpResult17 = lerp( temp_output_48_42 , normalizeResult76_g1 , inputMesh.ase_color.r);
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  lerpResult15;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = lerpResult17;

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
				
				surfaceDescription.Alpha = 1;
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

			#pragma vertex Vert
			#pragma fragment Frag


			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

			#ifndef SHADER_UNLIT
			#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
			#define VARYINGS_NEED_CULLFACE
			#endif
			#endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
		    #define OUTPUT_SPLIT_LIGHTING
		    #endif

		    #if !( (SHADERPASS == SHADERPASS_FORWARD) || (SHADERPASS == SHADERPASS_LIGHT_TRANSPORT) \
               || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT)\
               || (SHADERPASS == SHADERPASS_PATH_TRACING) || (SHADERPASS == SHADERPASS_RAYTRACING_SUB_SURFACE) \
               || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) )

		    #define DISABLE_MODIFY_BAKED_DIFFUSE_LIGHTING
		    #endif

			#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
			#if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
				#define WRITE_NORMAL_BUFFER
			#endif
			#endif

			#ifndef DEBUG_DISPLAY
				#if !defined(_SURFACE_TYPE_TRANSPARENT)
					#if SHADERPASS == SHADERPASS_FORWARD
					#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
					#elif SHADERPASS == SHADERPASS_GBUFFER
					#define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
					#endif
				#endif
			#endif

			#if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define _DEFERRED_CAPABLE_MATERIAL
			#endif

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
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
				 float3 TangentSpaceNormal;
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
				float3 BentNormal;
				float Smoothness;
				float Occlusion;
				float3 NormalTS;
				float Metallic;
			};

			SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
			{
				SurfaceDescription surface = (SurfaceDescription)0;
				surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
				surface.Emission = float3(0, 0, 0);
				surface.Alpha = 1;
				surface.BentNormal = IN.TangentSpaceNormal;
				surface.Smoothness = 0.5;
				surface.Occlusion = 1;
				surface.NormalTS = IN.TangentSpaceNormal;
				surface.Metallic = 0;
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

			AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters  )
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
	
	CustomEditor "Rendering.HighDefinition.LightingShaderGraphGUI"
	
	Fallback Off
}
/*ASEBEGIN
Version=19201
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;0,0;Float;False;True;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;12;DickShader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;GBuffer;0;0;GBuffer;34;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;True;True;True;True;True;0;True;_LightLayersMaskBuffer4;False;False;False;False;False;False;False;True;True;0;True;_StencilRefGBuffer;255;False;;255;True;_StencilWriteMaskGBuffer;7;False;;3;False;;0;False;;0;False;;7;False;;3;False;;0;False;;0;False;;False;False;True;0;True;_ZTestGBuffer;False;True;1;LightMode=GBuffer;False;False;0;;0;0;Standard;39;Surface Type;0;0;  Rendering Pass;1;0;  Refraction Model;0;0;    Blending Mode;0;0;    Blend Preserves Specular;1;0;  Back Then Front Rendering;0;0;  Transparent Depth Prepass;0;0;  Transparent Depth Postpass;0;0;  ZWrite;0;0;  Z Test;4;0;Double-Sided;0;0;Alpha Clipping;0;0;  Use Shadow Threshold;0;0;Material Type,InvertActionOnDeselection;0;0;  Energy Conserving Specular;1;0;  Transmission,InvertActionOnDeselection;0;0;Forward Only;0;0;Receive Decals;1;0;Receives SSR;1;0;Receive SSR Transparent;0;0;Motion Vectors;1;0;  Add Precomputed Velocity;0;0;Specular AA;0;0;Specular Occlusion Mode;1;0;Override Baked GI;0;0;Depth Offset;0;0;DOTS Instancing;0;0;GPU Instancing;1;0;LOD CrossFade;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,;0;  Type;0;0;  Tess;16,False,;0;  Min;10,False,;0;  Max;25,False,;0;  Edge Length;16,False,;0;  Max Displacement;25,False,;0;Vertex Position;0;638235958401080136;0;11;True;True;True;True;True;True;False;False;False;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;0,0;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;META;0;1;META;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2;0,0;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=ShadowCaster;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3;0,0;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;SceneSelectionPass;0;3;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;4;0,0;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;DepthOnly;0;4;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;True;0;True;_StencilRefDepth;255;False;;255;True;_StencilWriteMaskDepth;7;False;;3;False;;0;False;;0;False;;7;False;;3;False;;0;False;;0;False;;False;True;1;False;;False;False;True;1;LightMode=DepthOnly;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;5;0,0;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;MotionVectors;0;5;MotionVectors;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;True;0;True;_StencilRefMV;255;False;;255;True;_StencilWriteMaskMV;7;False;;3;False;;0;False;;0;False;;7;False;;3;False;;0;False;;0;False;;False;True;1;False;;False;False;True;1;LightMode=MotionVectors;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;6;0,0;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;TransparentBackface;0;6;TransparentBackface;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;True;2;5;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;False;True;True;True;True;True;0;True;_ColorMaskTransparentVelOne;False;True;True;True;True;True;0;True;_ColorMaskTransparentVelTwo;False;False;False;False;False;True;0;True;_ZWrite;True;0;True;_ZTestTransparent;False;True;1;LightMode=TransparentBackface;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;7;0,0;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;TransparentDepthPrepass;0;7;TransparentDepthPrepass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;True;0;True;_StencilRefDepth;255;False;;255;True;_StencilWriteMaskDepth;7;False;;3;False;;0;False;;0;False;;7;False;;3;False;;0;False;;0;False;;False;True;1;False;;False;False;True;1;LightMode=TransparentDepthPrepass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;8;0,0;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;TransparentDepthPostpass;0;8;TransparentDepthPostpass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;True;1;LightMode=TransparentDepthPostpass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;9;0,0;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;Forward;0;9;Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;True;2;5;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;True;_CullModeForward;False;False;False;True;True;True;True;True;0;True;_ColorMaskTransparentVelOne;False;True;True;True;True;True;0;True;_ColorMaskTransparentVelTwo;False;False;False;True;True;0;True;_StencilRef;255;False;;255;True;_StencilWriteMask;7;False;;3;False;;0;False;;0;False;;7;False;;3;False;;0;False;;0;False;;False;True;0;True;_ZWrite;True;0;True;_ZTestDepthEqualForOpaque;False;True;1;LightMode=Forward;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;10;0,0;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;ScenePickingPass;0;10;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;True;3;False;;False;True;1;LightMode=Picking;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.LerpOp;15;-581.6802,751.9437;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;17;-584.7656,881.6729;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TangentVertexDataNode;21;-838.8391,1255.586;Inherit;False;1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;14;-1055.538,1190.004;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;20;-581.4393,1020.286;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;11;-1140.18,646.7589;Inherit;False;PenetratorDeformation;0;;1;ac383a8a454dc764caec4e7e5816beae;0;3;64;FLOAT3;0,0,0;False;69;FLOAT3;0,0,0;False;71;FLOAT4;0,0,0,0;False;4;FLOAT3;61;FLOAT3;62;FLOAT4;63;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-3108.154,743.9935;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;31;-2629.699,504.0775;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-3406.361,1016.452;Inherit;False;Property;_BulgeRadius;BulgeRadius;19;0;Create;True;0;0;0;False;0;False;0.58;0.58;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;25;-3407.154,829.9935;Inherit;False;Property;_DickForward;DickForward;27;0;Create;True;0;0;0;False;0;False;0,0,1;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;35;-3106.539,934.5379;Inherit;False;Property;_DickOffset;DickOffset;28;0;Create;True;0;0;0;False;0;False;0,0,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-2903.539,779.5379;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;37;-3245.539,848.5379;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-2420.54,505.538;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;30;-2771.7,472.0775;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;33;-1891.7,548.0775;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;32;-2266.7,334.0735;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;22;-2681.233,747.1965;Inherit;False;ProjectOnSphere;-1;;2;3366f7fa8574f0646a5b81b51f4db8d0;0;3;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-2089.259,324.2032;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;42;-2532.564,322.1983;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-2381.412,171.5769;Inherit;False;Property;_BulgeBlend;BulgeBlend;18;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;44;-1596.412,749.5769;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;45;-536.9493,-372.816;Inherit;True;Property;_BaseColorMap;BaseColorMap;15;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;46;-524.9493,65.18396;Inherit;True;Property;_MaskMap;MaskMap;16;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;47;-524.9493,-142.816;Inherit;True;Property;_NormalMap;NormalMap;17;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;-3011.288,535.5629;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;48;-4197.828,484.6135;Inherit;False;CockVoreSlurpFunction;20;;265;48b4b4d4c94c1d341abf875fe96b8fe0;0;2;49;FLOAT3;0,0,0;False;50;FLOAT3;0,0,0;False;2;FLOAT3;42;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-3437.509,725.9806;Inherit;False;Property;_BulgeProgress;BulgeProgress;29;0;Create;True;0;0;0;False;0;False;1;0;-1;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;38;-2263.696,867.1786;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;39;-1793.988,784.4831;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.AbsOpNode;51;-2464.911,1008.415;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;52;-2177.911,983.4149;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-2001.857,945.42;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;50;-2647.911,949.4149;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;55;-2344.857,1049.42;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
WireConnection;0;0;45;0
WireConnection;0;1;47;0
WireConnection;0;4;46;1
WireConnection;0;7;46;4
WireConnection;0;8;46;2
WireConnection;0;11;15;0
WireConnection;0;12;17;0
WireConnection;0;27;20;0
WireConnection;15;0;48;0
WireConnection;15;1;11;61
WireConnection;15;2;14;1
WireConnection;17;0;48;42
WireConnection;17;1;11;62
WireConnection;17;2;14;1
WireConnection;20;0;21;0
WireConnection;20;1;11;63
WireConnection;20;2;14;1
WireConnection;11;64;33;0
WireConnection;11;69;44;0
WireConnection;26;0;23;0
WireConnection;26;1;37;0
WireConnection;31;0;27;0
WireConnection;31;1;30;0
WireConnection;36;0;26;0
WireConnection;36;1;35;0
WireConnection;37;0;25;0
WireConnection;34;0;31;0
WireConnection;30;0;28;0
WireConnection;33;0;48;0
WireConnection;33;1;22;0
WireConnection;33;2;41;0
WireConnection;32;0;34;0
WireConnection;22;2;48;0
WireConnection;22;3;36;0
WireConnection;22;4;27;0
WireConnection;41;0;32;0
WireConnection;41;1;42;2
WireConnection;41;2;43;0
WireConnection;44;0;39;0
WireConnection;28;0;48;0
WireConnection;28;1;36;0
WireConnection;38;0;28;0
WireConnection;39;0;48;42
WireConnection;39;1;38;0
WireConnection;39;2;53;0
WireConnection;51;0;50;0
WireConnection;52;0;55;0
WireConnection;53;0;41;0
WireConnection;53;1;52;0
WireConnection;50;0;28;0
WireConnection;50;1;37;0
WireConnection;55;0;51;0
ASEEND*/
//CHKSM=4B709FC5247FAC501ACB022925FE2986E5B4854E