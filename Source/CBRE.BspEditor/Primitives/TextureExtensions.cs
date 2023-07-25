using System;
using System.Linq;
using System.Numerics;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.Common;
using CBRE.DataStructures.Geometric;
using Plane = CBRE.DataStructures.Geometric.Plane;

namespace CBRE.BspEditor.Primitives
{
    public static class TextureExtensions
    {
        public static void AlignToNormal(this Texture tex, Vector3 normal)
        {
            // Get the closest axis for this normal
            Vector3 axis = normal.ClosestAxis();

            Vector3 tempV = axis == Vector3.UnitZ ? -Vector3.UnitY : -Vector3.UnitZ;
            tex.UAxis = normal.Cross(tempV).Normalise();
            tex.VAxis = tex.UAxis.Cross(normal).Normalise();
            tex.Rotation = 0;
        }

        public static bool IsAlignedToNormal(this Texture tex, Vector3 normal)
        {
            Vector3 cp = tex.UAxis.Cross(tex.VAxis).Normalise();
            return cp.EquivalentTo(normal, 0.01f) || cp.EquivalentTo(-normal, 0.01f);
        }

        public static void AlignWithTexture(this Texture tex, Plane currentPlane, Plane alignToPlane, Texture alignToTexture)
        {
            // Get reference values for the axes
            Vector3 refU = alignToTexture.UAxis;
            Vector3 refV = alignToTexture.VAxis;
            // Reference points in the texture plane to use for shifting later on
            Vector3 refX = alignToTexture.UAxis * alignToTexture.XShift * alignToTexture.XScale;
            Vector3 refY = alignToTexture.VAxis * alignToTexture.YShift * alignToTexture.YScale;

            // Two non-parallel planes intersect at an edge. We want the textures on this face
            // to line up with the textures on the provided face. To do this, we rotate the texture 
            // normal on the provided face around the intersection edge to get the new texture axes.
            // Then we rotate the texture reference point around this edge as well to get the new shift values.
            // The scale values on both faces will always end up being the same value.

            // Find the intersection edge vector
            Vector3 intersectionEdge = alignToPlane.Normal.Cross(currentPlane.Normal);

            // If the planes are parallel, the texture doesn't need any rotation - just different shift values.
            if (Math.Abs(intersectionEdge.Length()) > 0.01f)
            {
                // Create a plane using the intersection edge as the normal
                Plane intersectionPlane = new Plane(intersectionEdge, 0);

                Vector3? intersect = Plane.Intersect(alignToPlane, currentPlane, intersectionPlane);
                if (intersect != null)
                {
                    // Since the intersection plane is perpendicular to both face planes, we can find the angle
                    // between the two planes (the align plane and the plane of this face) by projecting
                    // the normals of the planes onto the perpendicular plane and taking the cross product.

                    // Project the two normals onto the perpendicular plane
                    Vector3 apNormal = intersectionPlane.Project(alignToPlane.Normal).Normalise();
                    Vector3 cpNormal = intersectionPlane.Project(currentPlane.Normal).Normalise();

                    // Get the angle between the projected normals
                    double dot = Math.Round(apNormal.Dot(cpNormal), 4);
                    float angle = (float) Math.Acos(dot); // A.B = cos(angle)

                    // Rotate the texture axis by the angle around the intersection edge
                    Matrix4x4 transform = Matrix4x4.CreateFromAxisAngle(intersectionEdge.Normalise(), angle);
                    refU = transform.Transform(refU);
                    refV = transform.Transform(refV);

                    // Rotate the texture reference points as well, but around the intersection line, not the origin
                    refX = transform.Transform(refX + intersect.Value) - intersect.Value;
                    refY = transform.Transform(refY + intersect.Value) - intersect.Value;
                }
            }

            // Convert the reference points back to get the final values
            tex.Rotation = 0;
            tex.UAxis = refU;
            tex.VAxis = refV;
            tex.XShift = refU.Dot(refX) / alignToTexture.XScale;
            tex.YShift = refV.Dot(refY) / alignToTexture.YScale;
            tex.XScale = alignToTexture.XScale;
            tex.YScale = alignToTexture.YScale;
        }

        public static void MinimiseTextureShiftValues(this Texture tex, int width, int height)
        {
            if (width <= 0 || height <= 0) return;

            // Keep the shift values to a minimum
            tex.XShift = tex.XShift % width;
            tex.YShift = tex.YShift % height;
            if (tex.XShift < -width / 2f) tex.XShift += width;
            if (tex.YShift < -height / 2f) tex.YShift += height;
        }

        public static void FitToPointCloud(this Texture tex, int width, int height, Cloud cloud, int tileX, int tileY)
        {
            if (width <= 0 || height <= 0) return;
            if (tileX <= 0) tileX = 1;
            if (tileY <= 0) tileY = 1;

            // Scale will change, no need to use it in the calculations
            System.Collections.Generic.List<float> xvals = cloud.GetExtents().Select(x => x.Dot(tex.UAxis)).ToList();
            System.Collections.Generic.List<float> yvals = cloud.GetExtents().Select(x => x.Dot(tex.VAxis)).ToList();

            float minU = xvals.Min();
            float minV = yvals.Min();
            float maxU = xvals.Max();
            float maxV = yvals.Max();

            tex.XScale = (maxU - minU) / (width * tileX);
            tex.YScale = (maxV - minV) / (height * tileY);
            tex.XShift = -minU / tex.XScale;
            tex.YShift = -minV / tex.YScale;
        }

        public static void AlignWithPointCloud(this Texture tex, int width, int height, Cloud cloud, BoxAlignMode mode)
        {
            if (width <= 0 || height <= 0) return;

            System.Collections.Generic.List<float> xvals = cloud.GetExtents().Select(x => x.Dot(tex.UAxis) / tex.XScale).ToList();
            System.Collections.Generic.List<float> yvals = cloud.GetExtents().Select(x => x.Dot(tex.VAxis) / tex.YScale).ToList();

            float minU = xvals.Min();
            float minV = yvals.Min();
            float maxU = xvals.Max();
            float maxV = yvals.Max();

            switch (mode)
            {
                case BoxAlignMode.Left:
                    tex.XShift = -minU;
                    break;
                case BoxAlignMode.Right:
                    tex.XShift = -maxU + width;
                    break;
                case BoxAlignMode.Center:
                    float avgU = (minU + maxU) / 2;
                    float avgV = (minV + maxV) / 2;
                    tex.XShift = -avgU + width / 2f;
                    tex.YShift = -avgV + height / 2f;
                    break;
                case BoxAlignMode.Top:
                    tex.YShift = -minV;
                    break;
                case BoxAlignMode.Bottom:
                    tex.YShift = -maxV + height;
                    break;
            }
        }
        
        public static void SetRotation(this Texture tex, float rotate)
        {
            float rads = (float) MathHelper.DegreesToRadians(tex.Rotation - rotate);

            // Rotate around the texture normal
            Vector3 texNorm = tex.VAxis.Cross(tex.UAxis).Normalise();

            Matrix4x4 transform = Matrix4x4.CreateFromAxisAngle(texNorm, rads);
            tex.UAxis = transform.Transform(tex.UAxis);
            tex.VAxis = transform.Transform(tex.VAxis);
            tex.Rotation = rotate;
        }
    }
}