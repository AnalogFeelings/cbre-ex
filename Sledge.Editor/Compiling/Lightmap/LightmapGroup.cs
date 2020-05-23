﻿using Sledge.DataStructures.Geometric;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sledge.Editor.Compiling.Lightmap
{
    public class LightmapGroup
    {
        public PlaneF Plane;
        public BoxF BoundingBox;
        public List<LMFace> Faces;

        public CoordinateF uAxis;
        public CoordinateF vAxis;
        public float? minTotalX;
        public float? minTotalY;
        public float? maxTotalX;
        public float? maxTotalY;
        public int writeX;
        public int writeY;

        private void CalculateInitialUV()
        {
            if (uAxis == null || vAxis == null)
            {
                var direction = Plane.GetClosestAxisToNormal();
                var tempV = direction == CoordinateF.UnitZ ? -CoordinateF.UnitY : -CoordinateF.UnitZ;
                uAxis = Plane.Normal.Cross(tempV).Normalise();
                vAxis = uAxis.Cross(Plane.Normal).Normalise();
            }

            if (minTotalX == null || minTotalY == null || maxTotalX == null || maxTotalY == null)
            {
                foreach (LMFace face in Faces)
                {
                    foreach (CoordinateF coord in face.Vertices.Select(x => x.Location))
                    {
                        float x = coord.Dot(uAxis);
                        float y = coord.Dot(vAxis);

                        if (minTotalX == null || x < minTotalX) minTotalX = x;
                        if (minTotalY == null || y < minTotalY) minTotalY = y;
                        if (maxTotalX == null || x > maxTotalX) maxTotalX = x;
                        if (maxTotalY == null || y > maxTotalY) maxTotalY = y;
                    }
                }

                minTotalX -= Config.DownscaleFactor; minTotalY -= Config.DownscaleFactor;
                maxTotalX += Config.DownscaleFactor; maxTotalY += Config.DownscaleFactor;

                minTotalX /= Config.DownscaleFactor; minTotalX = (float)Math.Ceiling(minTotalX.Value); minTotalX *= Config.DownscaleFactor;
                minTotalY /= Config.DownscaleFactor; minTotalY = (float)Math.Ceiling(minTotalY.Value); minTotalY *= Config.DownscaleFactor;
                maxTotalX /= Config.DownscaleFactor; maxTotalX = (float)Math.Ceiling(maxTotalX.Value); maxTotalX *= Config.DownscaleFactor;
                maxTotalY /= Config.DownscaleFactor; maxTotalY = (float)Math.Ceiling(maxTotalY.Value); maxTotalY *= Config.DownscaleFactor;

                if ((maxTotalX - minTotalX) > (maxTotalY - minTotalY))
                {
                    SwapUV();
                }
            }
        }

        public float Width
        {
            get
            {
                CalculateInitialUV();
                return (maxTotalX - minTotalX).Value;
            }
        }

        public float Height
        {
            get
            {
                CalculateInitialUV();
                return (maxTotalY - minTotalY).Value;
            }
        }

        public void SwapUV()
        {
            float maxSwap = maxTotalX.Value; float minSwap = minTotalX.Value;
            maxTotalX = maxTotalY; minTotalX = minTotalY;
            maxTotalY = maxSwap; minTotalY = minSwap;

            CoordinateF swapAxis = uAxis;
            uAxis = vAxis;
            vAxis = swapAxis;
        }

        public static LightmapGroup FindCoplanar(List<LightmapGroup> lmGroups, LMFace otherFace)
        {
            foreach (LightmapGroup group in lmGroups)
            {
                if ((group.Plane.Normal - otherFace.Plane.Normal).LengthSquared() < 0.1f)
                {
                    PlaneF plane2 = new PlaneF(otherFace.Plane.Normal, otherFace.Vertices[0].Location);
                    if (Math.Abs(plane2.EvalAtPoint((group.Plane.PointOnPlane))) > 4.0f) continue;
                    BoxF faceBox = new BoxF(otherFace.BoundingBox.Start - new CoordinateF(3.0f, 3.0f, 3.0f), otherFace.BoundingBox.End + new CoordinateF(3.0f, 3.0f, 3.0f));
                    if (faceBox.IntersectsWith(group.BoundingBox)) return group;
                }
            }
            return null;
        }
    }
}