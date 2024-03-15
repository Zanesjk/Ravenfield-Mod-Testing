using UnityEngine;
using System.Collections.Generic;

public static class SMath {

	public static Vector3 LineVsPointClosest(Vector3 origin, Vector3 direction, Vector3 point) {
		Vector3 deltaPoint = point-origin;
		return Vector3.Project(deltaPoint, direction)+origin;
	}

	public static float LineVsPointClosestT(Vector3 origin, Vector3 direction, Vector3 point) {
		return ProjectScalar(point-origin, direction);
	}
		
	public static Vector3 LineSegmentVsPointClosest(Vector3 a, Vector3 b, Vector3 point) {
		Vector3 delta = b-a;
		//Debug.Log(LineVsPointClosestT(a, delta, point))
		return a+Mathf.Clamp01(LineVsPointClosestT(a, delta, point))*delta;
	}

	public static float LineSegmentVsPointClosestT(Vector3 a, Vector3 b, Vector3 point) {
		return Mathf.Clamp01(LineVsPointClosestT(a, b-a, point));
	}

	public static float ProjectScalar(Vector3 a, Vector3 onto) {
		return Vector3.Dot(a, onto)/onto.sqrMagnitude;
	}

	public static float BearingRadian(Vector3 delta) {
		return Mathf.Atan2(delta.z, delta.x);
	}

	public static float Bearing(Vector3 delta) {
		return BearingRadian(delta)*Mathf.Rad2Deg;
	}

    public static Quaternion LookRotationRespectUp(Vector3 forward, Vector3 up) {
        return Quaternion.LookRotation(up, forward) * Quaternion.Euler(0f, 0f, 180f) * Quaternion.Euler(90f, 0f, 0f);
    }

    public static float Damp(float source, float target, float smoothing, float dt) {
        return Mathf.Lerp(source, target, 1 - Mathf.Pow(smoothing, dt));
    }

    public static Vector3 Damp(Vector3 source, Vector3 target, float smoothing, float dt) {
        return Vector3.Lerp(source, target, 1 - Mathf.Pow(smoothing, dt));
    }

    public static Quaternion DampLerp(Quaternion source, Quaternion target, float smoothing, float dt) {
        return Quaternion.Lerp(source, target, 1 - Mathf.Pow(smoothing, dt));
    }

    public static Quaternion DampSlerp(Quaternion source, Quaternion target, float smoothing, float dt) {
        return Quaternion.Slerp(source, target, 1 - Mathf.Pow(smoothing, dt));
    }

    public static Quaternion DeltaRotation(Quaternion from, Quaternion to) {
        return to * Quaternion.Inverse(from);
    }

    public static Vector3 Median(List<Vector3> points) {
        List<float> x = new List<float>();
        List<float> y = new List<float>();
        List<float> z = new List<float>();

        foreach(var p in points) {
            x.Add(p.x);
            y.Add(p.y);
            z.Add(p.z);
        }

        x.Sort();
        y.Sort();
        z.Sort();

        int center = points.Count / 2;
        return new Vector3(x[center], y[center], z[center]);
    }

    public static float SignedAngleBetween(Vector3 v1, Vector3 v2, Vector3 axis) {
        return Mathf.Atan2(
            Vector3.Dot(axis, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

	public static Bounds TransformBounds(Bounds local, Matrix4x4 localToWorld) {

		Bounds bounds = new Bounds(localToWorld.MultiplyPoint(local.min), Vector3.zero);
		bounds.Encapsulate(localToWorld.MultiplyPoint(new Vector3(local.min.x, local.min.y, local.max.z)));
		bounds.Encapsulate(localToWorld.MultiplyPoint(new Vector3(local.max.x, local.min.y, local.max.z)));
		bounds.Encapsulate(localToWorld.MultiplyPoint(new Vector3(local.max.x, local.min.y, local.min.z)));

		bounds.Encapsulate(localToWorld.MultiplyPoint(new Vector3(local.min.x, local.max.y, local.min.z)));
		bounds.Encapsulate(localToWorld.MultiplyPoint(new Vector3(local.min.x, local.max.y, local.max.z)));
		bounds.Encapsulate(localToWorld.MultiplyPoint(local.max));
		bounds.Encapsulate(localToWorld.MultiplyPoint(new Vector3(local.max.x, local.max.y, local.min.z)));

		return bounds;
	}

    public static class V2D {

		public static bool LineSegementsIntersect(Vector2 p, Vector2 p2, Vector2 q, Vector2 q2, 
			out Vector2 intersection)//, bool considerCollinearOverlapAsIntersect = false)
		{
			intersection = new Vector2();

			var r = p2 - p;
			var s = q2 - q;
			var rxs = r.Cross(s);
			var qpxr = (q - p).Cross(r);

			// If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
			if (Mathf.Approximately(rxs, 0f) && Mathf.Approximately(qpxr, 0f))
			{
				// 1. If either  0 <= (q - p) * r <= r * r or 0 <= (p - q) * s <= * s
				// then the two lines are overlapping,
				/*if (considerCollinearOverlapAsIntersect)
					if ((0 <= (q - p)*r && (q - p)*r <= r*r) || (0 <= (p - q)*s && (p - q)*s <= s*s))
						return true;*/

				// 2. If neither 0 <= (q - p) * r = r * r nor 0 <= (p - q) * s <= s * s
				// then the two lines are collinear but disjoint.
				// No need to implement this expression, as it follows from the expression above.
				return false;
			}

			// 3. If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
			if (Mathf.Approximately(rxs, 0f) && !Mathf.Approximately(qpxr, 0f))
				return false;

			// t = (q - p) x s / (r x s)
			var t = (q - p).Cross(s)/rxs;

			// u = (q - p) x r / (r x s)

			var u = (q - p).Cross(r)/rxs;

			// 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1
			// the two line segments meet at the point p + t r = q + u s.
			if (!Mathf.Approximately(rxs, 0f) && (0 <= t && t <= 1) && (0 <= u && u <= 1))
			{
				// We can calculate the intersection point using either t or u.
				intersection = p + t*r;

				// An intersection was found.
				return true;
			}

			// 5. Otherwise, the two line segments are not parallel but do not intersect.
			return false;
		}

		public static float RadiansFromTo(float origin, float target) {
			float delta = Mathf.DeltaAngle(origin, target);
			if(delta < 0f) delta += 2*Mathf.PI;
			return delta;
		}
	}
}