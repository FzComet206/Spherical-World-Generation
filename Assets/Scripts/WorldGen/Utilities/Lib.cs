using System;
using System.IO;
using UnityEngine;
using static UnityEngine.Mathf;
using Object = UnityEngine.Object;

public static class Lib
{
    public static float remap(float v, float minOld, float maxOld, float minNew, float maxNew) {
        return Clamp01(minNew + (v-minOld) * (maxNew - minNew) / (maxOld-minOld));
    }
    public static Vector3 PointOnCubeToPointOnSphere(Vector3 p)
    {
        float x2 = p.x * p.x / 2;
        float y2 = p.y * p.y / 2;
        float z2 = p.z * p.z / 2;
        float x = p.x * Sqrt(1 - y2 - z2 + (p.y * p.y * p.z * p.z) / 3);
        float y = p.y * Sqrt(1 - z2 - x2 + (p.x * p.x * p.z * p.z) / 3);
        float z = p.z * Sqrt(1 - x2 - y2 + (p.x * p.x * p.y * p.y) / 3);
        return new Vector3(x, y, z);
    }
    
    public static Coordinate PointToCoordinate(Vector3 pointOnUnitSphere)
    {
        float latitude = Asin(pointOnUnitSphere.y);
        float a = pointOnUnitSphere.x;
        float b = -pointOnUnitSphere.z;

        float longitude = Atan2(a, b);
        return new Coordinate(longitude, latitude);
    }

    // Calculate point on sphere given longitude and latitude (in radians), and the radius of the sphere
    public static Vector3 CoordinateToPoint(Coordinate coordinate, float radius)
    {
        float y = Sin(coordinate.latitude);
        float r = Cos(coordinate.latitude); // radius of 2d circle cut through sphere at 'y'
        float x = Sin(coordinate.longitude) * r;
        float z = Cos(coordinate.longitude) * r;

        return new Vector3(x, y, z) * radius;
    }

    public static Vector3 TraverseOnSphere(Vector3 start, float thetaDegree, float phiDegree)
    {
        float r = Sqrt(start.x * start.x + start.y * start.y + start.z * start.z);
        float theta = Acos(start.z / r);
        float phi = Atan2(start.y, start.x);

        phi += Deg2Rad * phiDegree;
        theta += Deg2Rad * thetaDegree;
        
        return new Vector3(
            r * Cos(phi) * Sin(theta),
            r * Sin(phi) * Sin(theta),
            r * Cos(theta)
        );
    }

    public static float DistanceBetweenPointsOnSphere(Vector3 a, Vector3 b, float radius)
    {
        return radius * DistanceBetweenPointsOnUnitSphere(a / radius, b / radius);
    }

    public static float DistanceBetweenPointsOnUnitSphere(Vector3 a, Vector3 b)
    {
        return Acos(Vector3.Dot(a, b));
    }
    
    [Serializable]
    public struct Coordinate
    {
        // Longitude/latitude in radians
        [Range(-PI, PI)]
        public float longitude;
        [Range(-PI / 2, PI / 2)]
        public float latitude;

        public Coordinate(float longitude, float latitude)
        {
            this.longitude = longitude;
            this.latitude = latitude;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(longitude, latitude);
        }

        public Vector2 ToUV()
        {
            return new Vector2((longitude + PI) / (2f * PI), (latitude + PI / 2f) / PI);
        }
    }
    
    public static void DumpRenderTexture (RenderTexture rt, string pngOutPath, TextureFormat texFormat)
    {
        var oldRT = RenderTexture.active;
        var tex = new Texture2D (rt.width, rt.height, texFormat, false, false);
        RenderTexture.active = rt;
        
        tex.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);
        tex.Apply ();
        
        
        File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());
        RenderTexture.active = oldRT;
        Object.Destroy(tex);
    }

    public static Texture2D ReadFromPng(string path)
    {
        byte[] data =  File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(1, 1, TextureFormat.R16, false);
        tex.LoadImage(data);
        tex.Apply();
        return tex;
    }
    
    public static ComputeBuffer SampleCurveToBuffer(AnimationCurve curve, int resolution)
    {
        float[] curvePreSample = new float[resolution];
        for (int i = 0; i < resolution; i++)
        {
            curvePreSample[i] = curve.Evaluate(i / (float) resolution);
        }
        ComputeBuffer cp = new ComputeBuffer(curvePreSample.Length, sizeof(float));
        cp.SetData(curvePreSample);
        return cp;
    }
    
    public static float[] SampleCurveToArray(AnimationCurve curve, int resolution)
    {
        float[] curvePreSample = new float[resolution];
        for (int i = 0; i < resolution; i++)
        {
            curvePreSample[i] = curve.Evaluate(i / (float) resolution);
        }
        return curvePreSample;
    }
}
