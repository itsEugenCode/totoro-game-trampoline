#!/usr/bin/env python3
"""Rewrite Assets/Scenes/Clearing.unity — vertical bounce + collectibles."""

from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
SCENE = ROOT / "Assets/Scenes/Clearing.unity"

GUID_GROUND = "816ddf4d0600f47d1b49c04f8e281f02"
GUID_TOTORO = "83c07bfd537124d0e87c3f282f296b24"
GUID_BOUNCE = "e4eaa412dfc224ddc85090df70720337"
GUID_CAMERA = "3b82fcc0d35ba4d8eb8a3a2c21b3b20c"
GUID_TALLY = "edbd175b3bf344e059becb00087895f6"
GUID_COLLECTIBLE = "e5ae9b79d049046cabfc192ea89cd67b"
GUID_PLAYER = "a70213b46ed3d4c5c8f3a8da60644edc"
GUID_LEAF = "dfbe12df5f8984141bee1f47d40a085d"
GUID_SEED = "67bd51d8a7b7b44a49fd9442ed9bc37e"
GUID_NUT = "e6c865ce2eac3415f885f26d05a5190b"
GUID_DUST = "ab8765ea805bb4bf78de85f2312525ff"
GUID_BUTTERFLY = "eff43948376a449a6af3b104a5d38d06"
GUID_RESPAWN = "8f3c2a1b9e0d4c6a8b7f5e4d3c2b1a0a"
GUID_FALL = "8f3c2a1b9e0d4c6a8b7f5e4d3c2b1a0b"
GUID_FINISH = "8f3c2a1b9e0d4c6a8b7f5e4d3c2b1a0c"

COL_GO = 1996327587914260411
COL_TR = 5757415073443756963
COL_SR = 7898758307700457499
PL_GO = 3102040573844858684
PL_TR = 2774529669002072750

H = 0.5


def platform(go_id, col_id, sr_id, tr_id, name, cx, top_y, width, color, children_yaml="[]"):
    py = top_y - H
    r, g, b = color
    return f"""--- !u!1 &{go_id}
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  serializedVersion: 6
  m_Component:
  - component: {{fileID: {tr_id}}}
  - component: {{fileID: {sr_id}}}
  - component: {{fileID: {col_id}}}
  m_Layer: 6
  m_Name: {name}
  m_TagString: Untagged
  m_Icon: {{fileID: 0}}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!61 &{col_id}
BoxCollider2D:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {go_id}}}
  m_Enabled: 1
  serializedVersion: 3
  m_Density: 1
  m_Material: {{fileID: 0}}
  m_IncludeLayers:
    serializedVersion: 2
    m_Bits: 0
  m_ExcludeLayers:
    serializedVersion: 2
    m_Bits: 0
  m_LayerOverridePriority: 0
  m_ForceSendLayers:
    serializedVersion: 2
    m_Bits: 4294967295
  m_ForceReceiveLayers:
    serializedVersion: 2
    m_Bits: 4294967295
  m_ContactCaptureLayers:
    serializedVersion: 2
    m_Bits: 4294967295
  m_CallbackLayers:
    serializedVersion: 2
    m_Bits: 4294967295
  m_IsTrigger: 0
  m_UsedByEffector: 0
  m_CompositeOperation: 0
  m_CompositeOrder: 0
  m_Offset: {{x: 0, y: {H * 0.5}}}
  m_SpriteTilingProperty:
    border: {{x: 0, y: 0, z: 0, w: 0}}
    pivot: {{x: 0.5, y: 0}}
    oldSize: {{x: 1, y: 0.5}}
    newSize: {{x: {width}, y: {H}}}
    adaptiveTilingThreshold: 0.5
    drawMode: 2
    adaptiveTiling: 0
  m_AutoTiling: 0
  m_Size: {{x: {width}, y: {H}}}
  m_EdgeRadius: 0
--- !u!212 &{sr_id}
SpriteRenderer:
  serializedVersion: 3
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {go_id}}}
  m_Enabled: 1
  m_CastShadows: 0
  m_ReceiveShadows: 0
  m_DynamicOccludee: 1
  m_StaticShadowCaster: 0
  m_MotionVectors: 1
  m_LightProbeUsage: 1
  m_ReflectionProbeUsage: 1
  m_RayTracingMode: 0
  m_RayTraceProcedural: 0
  m_RayTracingAccelStructBuildFlagsOverride: 0
  m_RayTracingAccelStructBuildFlags: 1
  m_SmallMeshCulling: 1
  m_ForceMeshLod: -1
  m_MeshLodSelectionBias: 0
  m_RenderingLayerMask: 1
  m_RendererPriority: 0
  m_Materials:
  - {{fileID: 10754, guid: 0000000000000000f000000000000000, type: 0}}
  m_StaticBatchInfo:
    firstSubMesh: 0
    subMeshCount: 0
  m_StaticBatchRoot: {{fileID: 0}}
  m_ProbeAnchor: {{fileID: 0}}
  m_LightProbeVolumeOverride: {{fileID: 0}}
  m_ScaleInLightmap: 1
  m_ReceiveGI: 1
  m_PreserveUVs: 0
  m_IgnoreNormalsForChartDetection: 0
  m_ImportantGI: 0
  m_StitchLightmapSeams: 1
  m_SelectedEditorRenderState: 0
  m_MinimumChartSize: 4
  m_AutoUVMaxDistance: 0.5
  m_AutoUVMaxAngle: 89
  m_LightmapParameters: {{fileID: 0}}
  m_GlobalIlluminationMeshLod: 0
  m_SortingLayerID: 0
  m_SortingLayer: 0
  m_SortingOrder: 0
  m_MaskInteraction: 0
  m_Sprite: {{fileID: 21300000, guid: {GUID_GROUND}, type: 3}}
  m_Color: {{r: {r}, g: {g}, b: {b}, a: 1}}
  m_FlipX: 0
  m_FlipY: 0
  m_DrawMode: 2
  m_Size: {{x: {width}, y: {H}}}
  m_AdaptiveModeThreshold: 0.5
  m_SpriteTileMode: 0
  m_WasSpriteAssigned: 1
  m_SpriteSortPoint: 0
  m_BlendShapeWeights: []
--- !u!4 &{tr_id}
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {go_id}}}
  serializedVersion: 2
  m_LocalRotation: {{x: 0, y: 0, z: 0, w: 1}}
  m_LocalPosition: {{x: {cx}, y: {py}, z: 0}}
  m_LocalScale: {{x: 1, y: 1, z: 1}}
  m_ConstrainProportionsScale: 0
  m_Children: {children_yaml}
  m_Father: {{fileID: 0}}
  m_LocalEulerAnglesHint: {{x: 0, y: 0, z: 0}}
"""


def collectible(inst_id, name, x, y, sprite_guid=None):
    sprite_mod = ""
    if sprite_guid:
        sprite_mod = f"""    - target: {{fileID: {COL_SR}, guid: {GUID_COLLECTIBLE}, type: 3}}
      propertyPath: m_Sprite
      value: 
      objectReference: {{fileID: 21300000, guid: {sprite_guid}, type: 3}}
"""
    return f"""--- !u!1001 &{inst_id}
PrefabInstance:
  m_ObjectHideFlags: 0
  serializedVersion: 2
  m_Modification:
    serializedVersion: 3
    m_TransformParent: {{fileID: 0}}
    m_Modifications:
    - target: {{fileID: {COL_GO}, guid: {GUID_COLLECTIBLE}, type: 3}}
      propertyPath: m_Name
      value: {name}
      objectReference: {{fileID: 0}}
    - target: {{fileID: {COL_TR}, guid: {GUID_COLLECTIBLE}, type: 3}}
      propertyPath: m_LocalPosition.x
      value: {x}
      objectReference: {{fileID: 0}}
    - target: {{fileID: {COL_TR}, guid: {GUID_COLLECTIBLE}, type: 3}}
      propertyPath: m_LocalPosition.y
      value: {y}
      objectReference: {{fileID: 0}}
    - target: {{fileID: {COL_TR}, guid: {GUID_COLLECTIBLE}, type: 3}}
      propertyPath: m_LocalPosition.z
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {COL_TR}, guid: {GUID_COLLECTIBLE}, type: 3}}
      propertyPath: m_LocalRotation.w
      value: 1
      objectReference: {{fileID: 0}}
    - target: {{fileID: {COL_TR}, guid: {GUID_COLLECTIBLE}, type: 3}}
      propertyPath: m_LocalRotation.x
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {COL_TR}, guid: {GUID_COLLECTIBLE}, type: 3}}
      propertyPath: m_LocalRotation.y
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {COL_TR}, guid: {GUID_COLLECTIBLE}, type: 3}}
      propertyPath: m_LocalRotation.z
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {COL_TR}, guid: {GUID_COLLECTIBLE}, type: 3}}
      propertyPath: m_LocalEulerAnglesHint.x
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {COL_TR}, guid: {GUID_COLLECTIBLE}, type: 3}}
      propertyPath: m_LocalEulerAnglesHint.y
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {COL_TR}, guid: {GUID_COLLECTIBLE}, type: 3}}
      propertyPath: m_LocalEulerAnglesHint.z
      value: 0
      objectReference: {{fileID: 0}}
{sprite_mod}    m_RemovedComponents: []
    m_RemovedGameObjects: []
    m_AddedGameObjects: []
    m_AddedComponents: []
  m_SourcePrefab: {{fileID: 100100000, guid: {GUID_COLLECTIBLE}, type: 3}}
"""


def main():
    text = SCENE.read_text()
    header = text[: text.index("--- !u!1001 &")]

    GROUND = (1114250068, 501137174, 1143470402, 997451040)
    SP_GO, SP_TR, SP_MB = 1368766608, 1808895204, 123240560
    FA_GO, FA_COL, FA_TR, FA_MB = 186394312, 544602212, 583342446, 1971826902
    TO_GO, TO_MB, TO_COL, TO_SR, TO_TR = 1042525187, 1042525188, 1042525189, 1042525190, 1042525191
    CAM_GO, CAM_MB, CAM_AL, CAM_CAM, CAM_TR = 1140395624, 1140395625, 1140395626, 1140395627, 1140395628
    TALLY_GO, TALLY_MB, TALLY_TR = 1966881505, 1966881506, 1966881507
    PLAYER_INST = 6707511494176892138
    PLAYER_STRIPPED_TR = 6707511494176892139
    COL = {
        "Leaf": 506663854,
        "Butterfly": 448943993,
        "Seed": 373487488,
        "Dust": 965032689,
        "Nut": 1726465941,
    }

    parts = [header]
    parts.append(collectible(COL["Leaf"], "Collectible_Leaf", 2.2, 2.2, GUID_LEAF))
    parts.append(collectible(COL["Seed"], "Collectible_Seed", 3.2, 5.8, GUID_SEED))
    parts.append(collectible(COL["Nut"], "Collectible_Nut", 4.2, 7.9, GUID_NUT))
    parts.append(collectible(COL["Dust"], "Collectible_Dust", 2.6, 9.0, GUID_DUST))
    parts.append(collectible(COL["Butterfly"], "Collectible_Butterfly", 4.6, 10.4, GUID_BUTTERFLY))

    parts.append(platform(*GROUND, "ClearingGround", 3.5, 0, 18, (0.45, 0.62, 0.38)))

    # Totoro
    parts.append(
        f"""--- !u!1 &{TO_GO}
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  serializedVersion: 6
  m_Component:
  - component: {{fileID: {TO_TR}}}
  - component: {{fileID: {TO_SR}}}
  - component: {{fileID: {TO_COL}}}
  - component: {{fileID: {TO_MB}}}
  m_Layer: 0
  m_Name: Totoro
  m_TagString: Untagged
  m_Icon: {{fileID: 0}}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!114 &{TO_MB}
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {TO_GO}}}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {{fileID: 11500000, guid: {GUID_BOUNCE}, type: 3}}
  m_Name: 
  m_EditorClassIdentifier: Assembly-CSharp::TotoroBounce
  upwardSpeed: 24
  feetGroundY: 0
--- !u!61 &{TO_COL}
BoxCollider2D:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {TO_GO}}}
  m_Enabled: 1
  serializedVersion: 3
  m_Density: 1
  m_Material: {{fileID: 0}}
  m_IncludeLayers:
    serializedVersion: 2
    m_Bits: 0
  m_ExcludeLayers:
    serializedVersion: 2
    m_Bits: 0
  m_LayerOverridePriority: 0
  m_ForceSendLayers:
    serializedVersion: 2
    m_Bits: 4294967295
  m_ForceReceiveLayers:
    serializedVersion: 2
    m_Bits: 4294967295
  m_ContactCaptureLayers:
    serializedVersion: 2
    m_Bits: 4294967295
  m_CallbackLayers:
    serializedVersion: 2
    m_Bits: 4294967295
  m_IsTrigger: 1
  m_UsedByEffector: 0
  m_CompositeOperation: 0
  m_CompositeOrder: 0
  m_Offset: {{x: 0.5, y: 2.25}}
  m_SpriteTilingProperty:
    border: {{x: 0, y: 0, z: 0, w: 0}}
    pivot: {{x: 0.5, y: 0}}
    oldSize: {{x: 6, y: 3}}
    newSize: {{x: 6, y: 3}}
    adaptiveTilingThreshold: 0.5
    drawMode: 0
    adaptiveTiling: 0
  m_AutoTiling: 0
  m_Size: {{x: 3, y: 1.5}}
  m_EdgeRadius: 0
--- !u!212 &{TO_SR}
SpriteRenderer:
  serializedVersion: 3
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {TO_GO}}}
  m_Enabled: 1
  m_CastShadows: 0
  m_ReceiveShadows: 0
  m_DynamicOccludee: 1
  m_StaticShadowCaster: 0
  m_MotionVectors: 1
  m_LightProbeUsage: 1
  m_ReflectionProbeUsage: 1
  m_RayTracingMode: 0
  m_RayTraceProcedural: 0
  m_RayTracingAccelStructBuildFlagsOverride: 0
  m_RayTracingAccelStructBuildFlags: 1
  m_SmallMeshCulling: 1
  m_ForceMeshLod: -1
  m_MeshLodSelectionBias: 0
  m_RenderingLayerMask: 1
  m_RendererPriority: 0
  m_Materials:
  - {{fileID: 10754, guid: 0000000000000000f000000000000000, type: 0}}
  m_StaticBatchInfo:
    firstSubMesh: 0
    subMeshCount: 0
  m_StaticBatchRoot: {{fileID: 0}}
  m_ProbeAnchor: {{fileID: 0}}
  m_LightProbeVolumeOverride: {{fileID: 0}}
  m_ScaleInLightmap: 1
  m_ReceiveGI: 1
  m_PreserveUVs: 0
  m_IgnoreNormalsForChartDetection: 0
  m_ImportantGI: 0
  m_StitchLightmapSeams: 1
  m_SelectedEditorRenderState: 0
  m_MinimumChartSize: 4
  m_AutoUVMaxDistance: 0.5
  m_AutoUVMaxAngle: 89
  m_LightmapParameters: {{fileID: 0}}
  m_GlobalIlluminationMeshLod: 0
  m_SortingLayerID: 0
  m_SortingLayer: 0
  m_SortingOrder: 0
  m_MaskInteraction: 0
  m_Sprite: {{fileID: 21300000, guid: {GUID_TOTORO}, type: 3}}
  m_Color: {{r: 1, g: 1, b: 1, a: 1}}
  m_FlipX: 0
  m_FlipY: 0
  m_DrawMode: 0
  m_Size: {{x: 6, y: 3}}
  m_AdaptiveModeThreshold: 0.5
  m_SpriteTileMode: 0
  m_WasSpriteAssigned: 1
  m_SpriteSortPoint: 0
  m_BlendShapeWeights: []
--- !u!4 &{TO_TR}
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {TO_GO}}}
  serializedVersion: 2
  m_LocalRotation: {{x: 0, y: 0, z: 0, w: 1}}
  m_LocalPosition: {{x: 3.5, y: 0, z: 0}}
  m_LocalScale: {{x: 1, y: 1, z: 1}}
  m_ConstrainProportionsScale: 0
  m_Children: []
  m_Father: {{fileID: 0}}
  m_LocalEulerAnglesHint: {{x: 0, y: 0, z: 0}}
"""
    )

    # Camera
    parts.append(
        f"""--- !u!1 &{CAM_GO}
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  serializedVersion: 6
  m_Component:
  - component: {{fileID: {CAM_TR}}}
  - component: {{fileID: {CAM_CAM}}}
  - component: {{fileID: {CAM_AL}}}
  - component: {{fileID: {CAM_MB}}}
  m_Layer: 0
  m_Name: Main Camera
  m_TagString: MainCamera
  m_Icon: {{fileID: 0}}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!114 &{CAM_MB}
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {CAM_GO}}}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {{fileID: 11500000, guid: {GUID_CAMERA}, type: 3}}
  m_Name: 
  m_EditorClassIdentifier: Assembly-CSharp::CameraController
  target: {{fileID: {PLAYER_STRIPPED_TR}}}
  offset: {{x: 0, y: 1, z: -10}}
  smoothTimeX: 0.15
  smoothTimeY: 0.25
  verticalDeadZone: 0.9
  highJumpMinSpeed: 14
  lowJumpFollowDelayHeight: 0.85
  lowJumpSmoothTimeY: 0.45
  lookAheadUp: 2.5
  lookAheadDown: 2.2
  lookAheadVelocityThreshold: 8
  highJumpSmoothTimeY: 0.12
  fallSmoothTimeY: 0.1
  apexHoldVy: 4
  apexHoldSmoothTimeY: 0.08
  useBounds: 1
  boundsMin: {{x: -4, y: 0.5}}
  boundsMax: {{x: 10, y: 16}}
--- !u!81 &{CAM_AL}
AudioListener:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {CAM_GO}}}
  m_Enabled: 1
--- !u!20 &{CAM_CAM}
Camera:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {CAM_GO}}}
  m_Enabled: 1
  serializedVersion: 2
  m_ClearFlags: 2
  m_BackGroundColor: {{r: 0.64, g: 0.78, b: 0.72, a: 1}}
  m_projectionMatrixMode: 1
  m_GateFitMode: 2
  m_FOVAxisMode: 0
  m_Iso: 200
  m_ShutterSpeed: 0.005
  m_Aperture: 16
  m_FocusDistance: 10
  m_FocalLength: 50
  m_BladeCount: 5
  m_Curvature: {{x: 2, y: 11}}
  m_BarrelClipping: 0.25
  m_Anamorphism: 0
  m_SensorSize: {{x: 36, y: 24}}
  m_LensShift: {{x: 0, y: 0}}
  m_NormalizedViewPortRect:
    serializedVersion: 2
    x: 0
    y: 0
    width: 1
    height: 1
  near clip plane: 0.3
  far clip plane: 1000
  field of view: 60
  orthographic: 1
  orthographic size: 6
  m_Depth: 0
  m_CullingMask:
    serializedVersion: 2
    m_Bits: 4294967295
  m_RenderingPath: -1
  m_TargetTexture: {{fileID: 0}}
  m_TargetDisplay: 0
  m_TargetEye: 3
  m_HDR: 1
  m_AllowMSAA: 1
  m_AllowDynamicResolution: 0
  m_ForceIntoRT: 0
  m_OcclusionCulling: 1
  m_StereoConvergence: 10
  m_StereoSeparation: 0.022
--- !u!4 &{CAM_TR}
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {CAM_GO}}}
  serializedVersion: 2
  m_LocalRotation: {{x: 0, y: 0, z: 0, w: 1}}
  m_LocalPosition: {{x: 1.5, y: 1, z: -10}}
  m_LocalScale: {{x: 1, y: 1, z: 1}}
  m_ConstrainProportionsScale: 0
  m_Children: []
  m_Father: {{fileID: 0}}
  m_LocalEulerAnglesHint: {{x: 0, y: 0, z: 0}}
"""
    )

    # CollectibleTally
    parts.append(
        f"""--- !u!1 &{TALLY_GO}
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  serializedVersion: 6
  m_Component:
  - component: {{fileID: {TALLY_TR}}}
  - component: {{fileID: {TALLY_MB}}}
  m_Layer: 0
  m_Name: CollectibleTally
  m_TagString: Untagged
  m_Icon: {{fileID: 0}}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!114 &{TALLY_MB}
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {TALLY_GO}}}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {{fileID: 11500000, guid: {GUID_TALLY}, type: 3}}
  m_Name: 
  m_EditorClassIdentifier: Assembly-CSharp::CollectibleTally
  collectedCount: 0
  logToConsole: 1
--- !u!4 &{TALLY_TR}
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {TALLY_GO}}}
  serializedVersion: 2
  m_LocalRotation: {{x: 0, y: 0, z: 0, w: 1}}
  m_LocalPosition: {{x: 0, y: 0, z: 0}}
  m_LocalScale: {{x: 1, y: 1, z: 1}}
  m_ConstrainProportionsScale: 0
  m_Children: []
  m_Father: {{fileID: 0}}
  m_LocalEulerAnglesHint: {{x: 0, y: 0, z: 0}}
"""
    )

    # PlayerSpawn
    parts.append(
        f"""--- !u!1 &{SP_GO}
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  serializedVersion: 6
  m_Component:
  - component: {{fileID: {SP_TR}}}
  - component: {{fileID: {SP_MB}}}
  m_Layer: 0
  m_Name: PlayerSpawn
  m_TagString: Untagged
  m_Icon: {{fileID: 0}}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!114 &{SP_MB}
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {SP_GO}}}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {{fileID: 11500000, guid: {GUID_RESPAWN}, type: 3}}
  m_Name: 
  m_EditorClassIdentifier: Assembly-CSharp::RespawnPoint
  activateOnAwake: 1
--- !u!4 &{SP_TR}
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {SP_GO}}}
  serializedVersion: 2
  m_LocalRotation: {{x: 0, y: 0, z: 0, w: 1}}
  m_LocalPosition: {{x: 1.5, y: 0, z: 0}}
  m_LocalScale: {{x: 1, y: 1, z: 1}}
  m_ConstrainProportionsScale: 0
  m_Children: []
  m_Father: {{fileID: 0}}
  m_LocalEulerAnglesHint: {{x: 0, y: 0, z: 0}}
"""
    )

    # FallRespawnZone
    parts.append(
        f"""--- !u!1 &{FA_GO}
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  serializedVersion: 6
  m_Component:
  - component: {{fileID: {FA_TR}}}
  - component: {{fileID: {FA_COL}}}
  - component: {{fileID: {FA_MB}}}
  m_Layer: 0
  m_Name: FallRespawnZone
  m_TagString: Untagged
  m_Icon: {{fileID: 0}}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!61 &{FA_COL}
BoxCollider2D:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {FA_GO}}}
  m_Enabled: 1
  serializedVersion: 3
  m_Density: 1
  m_Material: {{fileID: 0}}
  m_IncludeLayers:
    serializedVersion: 2
    m_Bits: 0
  m_ExcludeLayers:
    serializedVersion: 2
    m_Bits: 0
  m_LayerOverridePriority: 0
  m_ForceSendLayers:
    serializedVersion: 2
    m_Bits: 4294967295
  m_ForceReceiveLayers:
    serializedVersion: 2
    m_Bits: 4294967295
  m_ContactCaptureLayers:
    serializedVersion: 2
    m_Bits: 4294967295
  m_CallbackLayers:
    serializedVersion: 2
    m_Bits: 4294967295
  m_IsTrigger: 1
  m_UsedByEffector: 0
  m_CompositeOperation: 0
  m_CompositeOrder: 0
  m_Offset: {{x: 0, y: 0}}
  m_SpriteTilingProperty:
    border: {{x: 0, y: 0, z: 0, w: 0}}
    pivot: {{x: 0.5, y: 0.5}}
    oldSize: {{x: 1, y: 1}}
    newSize: {{x: 1, y: 1}}
    adaptiveTilingThreshold: 0.5
    drawMode: 0
    adaptiveTiling: 0
  m_AutoTiling: 0
  m_Size: {{x: 22, y: 4}}
  m_EdgeRadius: 0
--- !u!114 &{FA_MB}
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {FA_GO}}}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {{fileID: 11500000, guid: {GUID_FALL}, type: 3}}
  m_Name: 
  m_EditorClassIdentifier: Assembly-CSharp::FallRespawnZone
--- !u!4 &{FA_TR}
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: {FA_GO}}}
  serializedVersion: 2
  m_LocalRotation: {{x: 0, y: 0, z: 0, w: 1}}
  m_LocalPosition: {{x: 3.5, y: -5, z: 0}}
  m_LocalScale: {{x: 1, y: 1, z: 1}}
  m_ConstrainProportionsScale: 0
  m_Children: []
  m_Father: {{fileID: 0}}
  m_LocalEulerAnglesHint: {{x: 0, y: 0, z: 0}}
"""
    )

    # Player
    parts.append(
        f"""--- !u!1001 &{PLAYER_INST}
PrefabInstance:
  m_ObjectHideFlags: 0
  serializedVersion: 2
  m_Modification:
    serializedVersion: 3
    m_TransformParent: {{fileID: 0}}
    m_Modifications:
    - target: {{fileID: {PL_TR}, guid: {GUID_PLAYER}, type: 3}}
      propertyPath: m_LocalPosition.x
      value: 1.5
      objectReference: {{fileID: 0}}
    - target: {{fileID: {PL_TR}, guid: {GUID_PLAYER}, type: 3}}
      propertyPath: m_LocalPosition.y
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {PL_TR}, guid: {GUID_PLAYER}, type: 3}}
      propertyPath: m_LocalPosition.z
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {PL_TR}, guid: {GUID_PLAYER}, type: 3}}
      propertyPath: m_LocalRotation.w
      value: 1
      objectReference: {{fileID: 0}}
    - target: {{fileID: {PL_TR}, guid: {GUID_PLAYER}, type: 3}}
      propertyPath: m_LocalRotation.x
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {PL_TR}, guid: {GUID_PLAYER}, type: 3}}
      propertyPath: m_LocalRotation.y
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {PL_TR}, guid: {GUID_PLAYER}, type: 3}}
      propertyPath: m_LocalRotation.z
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {PL_TR}, guid: {GUID_PLAYER}, type: 3}}
      propertyPath: m_LocalEulerAnglesHint.x
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {PL_TR}, guid: {GUID_PLAYER}, type: 3}}
      propertyPath: m_LocalEulerAnglesHint.y
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {PL_TR}, guid: {GUID_PLAYER}, type: 3}}
      propertyPath: m_LocalEulerAnglesHint.z
      value: 0
      objectReference: {{fileID: 0}}
    - target: {{fileID: {PL_GO}, guid: {GUID_PLAYER}, type: 3}}
      propertyPath: m_Name
      value: Player
      objectReference: {{fileID: 0}}
    m_RemovedComponents: []
    m_RemovedGameObjects: []
    m_AddedGameObjects: []
    m_AddedComponents: []
  m_SourcePrefab: {{fileID: 100100000, guid: {GUID_PLAYER}, type: 3}}
--- !u!4 &{PLAYER_STRIPPED_TR} stripped
Transform:
  m_CorrespondingSourceObject: {{fileID: {PL_TR}, guid: {GUID_PLAYER}, type: 3}}
  m_PrefabInstance: {{fileID: {PLAYER_INST}}}
  m_PrefabAsset: {{fileID: 0}}
"""
    )

    roots = [
        GROUND[3],
        TO_TR,
        CAM_TR,
        TALLY_TR,
        SP_TR,
        FA_TR,
        PLAYER_INST,
        COL["Leaf"],
        COL["Seed"],
        COL["Nut"],
        COL["Dust"],
        COL["Butterfly"],
    ]
    roots_yaml = "\n".join(f"  - {{fileID: {r}}}" for r in roots)
    parts.append(
        f"""--- !u!1660057539 &9223372036854775807
SceneRoots:
  m_ObjectHideFlags: 0
  m_Roots:
{roots_yaml}
"""
    )

    SCENE.write_text("".join(parts))
    print(f"Wrote {SCENE}")


if __name__ == "__main__":
    main()
