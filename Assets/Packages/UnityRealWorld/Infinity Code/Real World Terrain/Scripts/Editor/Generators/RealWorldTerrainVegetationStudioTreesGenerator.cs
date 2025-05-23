﻿/*         INFINITY CODE         */
/*   https://infinity-code.com   */

using System.Collections.Generic;
using InfinityCode.RealWorldTerrain.OSM;
using InfinityCode.RealWorldTerrain.Phases;
using InfinityCode.RealWorldTerrain.Windows;
using UnityEngine;

#if VEGETATION_STUDIO || VEGETATION_STUDIO_PRO
using AwesomeTechnologies;
using AwesomeTechnologies.Common;
using AwesomeTechnologies.VegetationStudio;

#if VEGETATION_STUDIO_PRO
using AwesomeTechnologies.VegetationSystem;
#endif

#endif

namespace InfinityCode.RealWorldTerrain.Generators
{
    public class RealWorldTerrainVegetationStudioTreesGenerator
    {
        public static Dictionary<string, RealWorldTerrainOSMNode> nodes;
        public static Dictionary<string, RealWorldTerrainOSMWay> ways;
        public static List<RealWorldTerrainOSMRelation> relations;

        public static void Generate(RealWorldTerrainContainer container)
        {
#if VEGETATION_STUDIO || VEGETATION_STUDIO_PRO
            RealWorldTerrainOSMUtils.LoadOSM(RealWorldTerrainTreesGenerator.compressedFilename, out nodes, out ways, out relations);
            container.generateTrees = true;

            VegetationStudioManager vsm = RealWorldTerrainUtils.FindObjectOfType<VegetationStudioManager>();
            if (vsm == null) VegetationStudioManagerEditor.AddVegetationStudioManager();

            RealWorldTerrainPrefs prefs = RealWorldTerrainWindow.prefs;

#if !VEGETATION_STUDIO_PRO
            VegetationSystem[] vsl = Object.FindObjectsOfType<VegetationSystem>();
            if (vsl.Length > prefs.terrainCount.count)
            {
                for (int i = prefs.terrainCount.count; i < vsl.Length; i++) Object.DestroyImmediate(vsl[i].gameObject);
                vsl = Object.FindObjectsOfType<VegetationSystem>();
            }
            else if (vsl.Length < prefs.terrainCount.count)
            {
                for (int i = vsl.Length; i < prefs.terrainCount.count; i++) VegetationStudioManagerEditor.AddVegetationSystem();
                vsl = Object.FindObjectsOfType<VegetationSystem>();
            }

            for (int i = 0; i < vsl.Length; i++)
            {
                VegetationSystem vs = vsl[i];
                vs.AutoselectTerrain = false;
                vs.currentTerrain = container.terrains[i].terrain;
                if (vs.CurrentVegetationPackage != prefs.vegetationStudioPackage) vs.AddVegetationPackage(prefs.vegetationStudioPackage, true);
            }
#else
            VegetationSystemPro vsl = RealWorldTerrainUtils.FindObjectOfType<VegetationSystemPro>();
            if (vsl == null)
            {
                VegetationStudioManagerEditor.AddVegetationStudioManager();
                vsl = RealWorldTerrainUtils.FindObjectOfType<VegetationSystemPro>();
            }

            for (int i = 0; i < container.terrains.Length; i++)
            {
                GameObject go = container.terrains[i].gameObject;
                if (go.GetComponent<UnityTerrain>() == null) go.AddComponent<UnityTerrain>();
                vsl.AddTerrain(go);
            }
            vsl.AddVegetationPackage(prefs.vegetationStudioPackage);
            vsl.CalculateVegetationSystemBounds();
#endif

            VegetationMaskArea[] oldMasks = container.gameObject.GetComponentsInChildren<VegetationMaskArea>();
            foreach (VegetationMaskArea oldMask in oldMasks)
            {
                if (oldMask.gameObject.name.StartsWith("Tree Mask")) Object.DestroyImmediate(oldMask.gameObject);
            }

            foreach (var pair in ways)
            {
                RealWorldTerrainOSMWay way = pair.Value;

                List<Vector3> points = RealWorldTerrainOSMUtils.GetGlobalPointsFromWay(way, nodes);
                if (points.Count < 3) continue;

                GameObject go = new GameObject("Tree Mask " + way.id);
                go.transform.SetParent(container.transform, false);
                VegetationMaskArea mask = go.AddComponent<VegetationMaskArea>();
                mask.RemoveGrass = false;
                mask.RemoveLargeObjects = false;
                mask.RemoveObjects = false;
                mask.RemovePlants = false;
                mask.RemoveTrees = false;
                mask.IncludeVegetationType = true;

                for (int i = 0; i < prefs.vegetationStudioGrassTypes.Count; i++)
                {
                    VegetationTypeSettings vts = new VegetationTypeSettings();
                    mask.VegetationTypeList.Add(vts);
                    vts.Index = (VegetationTypeIndex)prefs.vegetationStudioTreeTypes[i];
                }

                List<Node> vnodes = new List<Node>();
                Vector3 firstPos = Vector3.zero;
                for (int i = 0; i < points.Count; i++)
                {
                    Vector3 p = RealWorldTerrainEditorUtils.CoordsToWorldWithElevation(points[i], container);

                    if (i == 0) firstPos = p;
                    p -= firstPos;

                    vnodes.Add(new Node
                    {
                        Position = p
                    });
                }

                go.transform.position = firstPos;

                mask.Nodes = vnodes;
            }
#endif

            nodes = null;
            ways = null;
            relations = null;

            RealWorldTerrainPhase.phaseComplete = true;
        }
    }
}