
using UnityEngine;
using System.Collections.Generic;

static public class Toolkit
{
	static public bool InitSingleton<T>(T pCurrentInstance, ref T r_pInstanceObject) where T : MonoBehaviour
	{
		if (r_pInstanceObject != null)
		{
#if UNITY_EDITOR
			if (r_pInstanceObject != pCurrentInstance)
			{
				Debug.LogError("An instance of " + pCurrentInstance.GetType() + " already exists and yet another one is being created.");
				GameObject.Destroy(pCurrentInstance.gameObject);
			}
#endif
			return false;
		}

		r_pInstanceObject = pCurrentInstance;
		return true;
	}


	static public float Sqrd(this float fMe)
	{
		return fMe * fMe;
	}

	static public Color SetAlpha(this Color tColor, float fAlpha)
	{
		tColor.a = fAlpha;
		return tColor;
	}

	static public T Last<T>(this T[] pList)
	{
		return pList[pList.Length - 1];
	}

	static public List<T> Clone<T>(this List<T> tList)
	{
		List<T> tNewList = new List<T>(tList.Count);
		for (int i = 0; i < tList.Count; ++i)
			tNewList.Add(tList[i]);

		return tNewList;
	}

	static public Dictionary<T, U> Clone<T, U>(this Dictionary<T, U> tDictionary)
	{
		Dictionary<T, U> tNewDictionary = new Dictionary<T, U>(tDictionary.Count);
		foreach (KeyValuePair<T, U> pPair in tDictionary)
			tNewDictionary.Add(pPair.Key, pPair.Value);

		return tNewDictionary;
	}
}

static public class Vec2Extensions
{
	static public Vector3 xyz(this Vector2 tVec)
	{
		return tVec;    // Yep...
	}

	static public Vector3 xzy(this Vector2 tVec)
	{
		return new Vector3(tVec.x, 0.0f, tVec.y);
	}

	static public Vector2 ShiftX(this Vector2 tVec, float fOffsetX)
	{
		tVec.x += fOffsetX;
		return tVec;
	}

	static public Vector2 ShiftY(this Vector2 tVec, float fOffsetY)
	{
		tVec.x += fOffsetY;
		return tVec;
	}

	static public Vector2 ShiftXY(this Vector2 tVec, Vector2 tOffset)
	{
		tVec += tOffset;
		return tVec;
	}

	static public Vector2 SetX(this Vector2 tVec, float fX)
	{
		tVec.x = fX;
		return tVec;
	}

	static public Vector2 SetY(this Vector2 tVec, float fY)
	{
		tVec.y = fY;
		return tVec;
	}

	static public Vector2 Clamp(this Vector2 tVec, Vector2 tMin, Vector2 tMax)
	{
		tVec.x = Mathf.Clamp(tVec.x, tMin.x, tMax.x);
		tVec.y = Mathf.Clamp(tVec.y, tMin.y, tMax.y);
		return tVec;
	}
}

static public class Vec3Extensions
{
	static public Vector2 xy(this Vector3 tVec)
	{
		return tVec;
	}

	static public Vector3 xzy(this Vector3 tVec)
	{
		return new Vector3(tVec.x, tVec.z, tVec.y);
	}

	static public Vector3 xyz(this Vector3 tVec)
	{
		return new Vector3(tVec.x, tVec.y, tVec.z);
	}

	static public Vector3 x0z(this Vector3 tVec)
	{
		return new Vector3(tVec.x, 0.0f, tVec.z);
	}

	static public Vector3 xy0(this Vector3 tVec)
	{
		return new Vector3(tVec.x, tVec.y);
	}

	static public Vector3 _0yz(this Vector3 tVec)
	{
		return new Vector3(0.0f, tVec.y, tVec.z);
	}

	static public Vector3 ShiftX(this Vector3 tVec, float fOffsetX)
	{
		tVec.x += fOffsetX;
		return tVec;
	}

	static public Vector3 ShiftY(this Vector3 tVec, float fOffsetY)
	{
		tVec.x += fOffsetY;
		return tVec;
	}

	static public Vector3 ShiftZ(this Vector3 tVec, float fOffsetZ)
	{
		tVec.x += fOffsetZ;
		return tVec;
	}

	static public Vector3 ShiftXYZ(this Vector3 tVec, Vector3 tOffset)
	{
		tVec += tOffset;
		return tVec;
	}

	static public Vector3 SetX(this Vector3 tVec, float fX)
	{
		tVec.x = fX;
		return tVec;
	}

	static public Vector3 SetY(this Vector3 tVec, float fY)
	{
		tVec.y = fY;
		return tVec;
	}

	static public Vector3 SetZ(this Vector3 tVec, float fZ)
	{
		tVec.z = fZ;
		return tVec;
	}

	static public Vector3 Clamp(this Vector3 tVec, Vector3 tMin, Vector3 tMax)
	{
		tVec.x = Mathf.Clamp(tVec.x, tMin.x, tMax.x);
		tVec.y = Mathf.Clamp(tVec.y, tMin.y, tMax.y);
		tVec.z = Mathf.Clamp(tVec.z, tMin.z, tMax.z);
		return tVec;
	}
}
