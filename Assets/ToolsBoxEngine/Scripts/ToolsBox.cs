﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolsBoxEngine {
    public enum Axis { X, Y, Z, W }

    #region Nullable vector
    // Nullable Vector
    //public class NVector2 {
    //    public Vector2? vector;

    //    //public static void operator =(NVector2 a, Vector2 b) => a.vector = b;
    //    public static implicit operator Vector2(NVector2 a) => (Vector2)a.vector;
    //    public static implicit operator NVector2(Vector2 a) => new NVector2((Vector2?)a);

    //    public NVector2() {
    //        vector = null;
    //    }

    //    public NVector2(Vector2? vector) {
    //        this.vector = vector;
    //    }

    //    public Vector2 Vector {
    //        get { return (Vector2)vector; }
    //        set { vector = value; }
    //    }

    //    public float x {
    //        get { return Vector.x; }
    //        set { Vector = new Vector2(value, Vector.y); }
    //    }

    //    public float y {
    //        get { return Vector.y; }
    //        set { Vector = new Vector2(Vector.x, value); }
    //    }
    //}
    #endregion

    #region Classes

    public class Nullable<T> where T : struct {
        public T? value;

        public static implicit operator T(Nullable<T> a) => (T)a.value;
        public static implicit operator Nullable<T>(T a) => new Nullable<T>((T?)a);

        public Nullable() {
            value = null;
        }

        public Nullable(T? value) {
            this.value = value;
        }

        public T Value {
            get { return (T)value; }
            set { this.value = value; }
        }
    }

    [Serializable]
    public class AmplitudeCurve {
        public AnimationCurve curve;
        public float duration;
        public float amplitude;
        [HideInInspector] public float timer;

        #region Constructeurs

        public AmplitudeCurve(AnimationCurve curve, float duration, float timer, float amplitude) {
            this.curve = curve;
            this.duration = duration;
            this.timer = timer;
            this.amplitude = amplitude;
        }

        public AmplitudeCurve(AnimationCurve curve) : this(curve, 1f, 0f, 1f) { }

        public AmplitudeCurve() : this(AnimationCurve.Linear(0f, 0f, 1f, 1f)) { }

        #endregion

        public float GetRatio() {
            float ratio = timer / duration;
            ratio = curve.Evaluate(ratio);
            return ratio;
        }

        public AmplitudeCurve Clone() {
            return new AmplitudeCurve(curve, duration, timer, amplitude);
        }
    }

    public class TimedEvent<T> {
        public bool ended;
        public event Tools.BasicDelegateTwoArgs<T> events;
        public T arg1, arg2;

        public TimedEvent(Tools.BasicDelegateTwoArgs<T> events, ref T arg1, T arg2) {
            ended = false;
            this.events += events;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }

        public bool IsEnded() {
            if (ended) {
                events(ref arg1, arg2);
                return true;
            }
            return false;
        }

        public IEnumerator Start(float time) {
            yield return new WaitForSeconds(time);
            ended = true;
        }
    }

    #endregion

    public static class Tools {

        #region Delegates

        public delegate void BasicDelegate();

        public delegate void BasicDelegate<T>(T arg);

        public delegate void BasicDelegateTwoArgs<T>(ref T arg1, T arg2);

        public delegate void BasicDelegate<T1, T2>(T1 arg1, T2 arg2);

        public delegate void BasicDelegate<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

        public delegate T BasicDelegateReturn<T>(T arg);

        public delegate T2 BasicDelegateReturn<T1, T2>(T1 arg);

        #endregion

        #region Extensions methods

        #region Vectors

        public static Vector2 To2D(this Vector3 vector, Axis axisToIgnore = Axis.Z) {
            switch (axisToIgnore) {
                case Axis.X:
                    return new Vector2(vector.y, vector.z);
                case Axis.Y:
                    return new Vector2(vector.x, vector.z);
                case Axis.Z:
                    return new Vector2(vector.x, vector.y);
                default:
                    return new Vector2(vector.x, vector.y);
            }
        }

        public static Vector2Int To2D(this Vector3Int vector, Axis axisToIgnore = Axis.Z) {
            switch (axisToIgnore) {
                case Axis.X:
                    return new Vector2Int(vector.y, vector.z);
                case Axis.Y:
                    return new Vector2Int(vector.x, vector.z);
                case Axis.Z:
                    return new Vector2Int(vector.x, vector.y);
                default:
                    return new Vector2Int(vector.x, vector.y);
            }
        }

        public static Vector3 To3D(this Vector2 vector, float value = 0f, Axis axis = Axis.Z) {
            switch (axis) {
                case Axis.X:
                    return new Vector3(value, vector.x, vector.y);
                case Axis.Y:
                    return new Vector3(vector.x, value, vector.y);
                case Axis.Z:
                    return new Vector3(vector.x, vector.y, value);
                default:
                    return new Vector3(vector.x, vector.y, value);
            }
        }

        public static Vector3Int To3D(this Vector2Int vector, int value = 0, Axis axis = Axis.Z) {
            switch (axis) {
                case Axis.X:
                    return new Vector3Int(value, vector.x, vector.y);
                case Axis.Y:
                    return new Vector3Int(vector.x, value, vector.y);
                case Axis.Z:
                    return new Vector3Int(vector.x, vector.y, value);
                default:
                    return new Vector3Int(vector.x, vector.y, value);
            }
        }

        public static Vector3 To3D(this Vector4 vector, Axis axisToIgnore = Axis.W) {
            switch (axisToIgnore) {
                case Axis.X:
                    return new Vector3(vector.y, vector.z, vector.w);
                case Axis.Y:
                    return new Vector3(vector.x, vector.z, vector.w);
                case Axis.Z:
                    return new Vector3(vector.x, vector.y, vector.w);
                case Axis.W:
                    return new Vector3(vector.x, vector.y, vector.z);
                default:
                    return new Vector3(vector.x, vector.y, vector.z);
            }
        }

        public static Vector3 Override(this Vector3 vector, float value, Axis axis = Axis.Y) {
            switch (axis) {
                case Axis.X:
                    vector.x = value;
                    break;
                case Axis.Y:
                    vector.y = value;
                    break;
                case Axis.Z:
                    vector.z = value;
                    break;
                default:
                    vector.y = value;
                    break;
            }

            return vector;
        }

        public static Vector2 Override(this Vector2 vector, float value, Axis axis = Axis.Y) {
            switch (axis) {
                case Axis.X:
                    vector.x = value;
                    break;
                case Axis.Y:
                    vector.y = value;
                    break;
                case Axis.Z:
                    Debug.LogWarning("Can't override Vector2 z axis, using default axis : y");
                    vector.y = value;
                    break;
                default:
                    vector.y = value;
                    break;
            }

            return vector;
        }

        public static Vector2Int FloorToInt(this Vector2 vector) {
            return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
        }
        
        public static Vector2 Abs(this Vector2 vector) {
            return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        }

        public static Vector3 Positive(this Vector3 vector) {
            return new Vector3(GetPositive(vector.x), GetPositive(vector.y), GetPositive(vector.z));
        }

        public static Vector3 Positive(this Vector3 vector, Axis axis) {
            switch (axis) {
                case Axis.X:
                    vector.x = GetPositive(vector.x);
                    break;
                case Axis.Y:
                    vector.y = GetPositive(vector.y);
                    break;
                case Axis.Z:
                    vector.z = GetPositive(vector.z);
                    break;
            }
            return vector;
        }

        public static Vector3 MultiplyIndividually(this Vector3 vector1, Vector3 vector2) {
            return new Vector3(vector1.x * vector2.x, vector1.y * vector2.y, vector1.z * vector2.z);
        }

        #endregion

        public static Vector2 Position2D(this Transform transform) {
            return transform.position.To2D();
        }

        public static int Find(this int[] array, int value) {
            for (int i = 0; i < array.Length; i++) {
                if (array[i] == value) {
                    return i;
                }
            }
            return -1;
        }

        public static void Print<T>(this Nullable<T>[] array) where T : struct {
            Debug.Log("----------------");
            for(int i = 0; i < array.Length; i++) {
                Debug.Log(i + ". " + array[i].Value);
            }
        }

        public static T[] ToArray<T>(this Nullable<T>[] array) where T : struct {
            T[] returnBack = new T[array.Length];
            for (int i = 0; i < array.Length; i++) {
                returnBack[i] = array[i].Value;
            }
            return returnBack;
        }

        public static Nullable<T>[] ToNullableArray<T>(this T[] array) where T : struct {
            Nullable<T>[] returnBack = new Nullable<T>[array.Length];
            for (int i = 0; i < array.Length; i++) {
                returnBack[i] = (Nullable<T>)array[i];
            }
            return returnBack;
        }

        public static List<T> ToList<T>(this List<Nullable<T>> list) where T : struct {
            List<T> returnBack = new List<T>();
            for (int i = 0; i < list.Count; i++) {
                returnBack.Add(list[i].Value);
            }
            return returnBack;
        }

        public static T2[] Individually<T1, T2>(this T1[] array, BasicDelegateReturn<T1, T2> function) { // Apply a function (with 1 argument) to an Array
            T2[] result = new T2[array.Length];
            for (int i = 0; i < array.Length; i++) {
                result[i] = function(array[i]);
            }
            return result;
        }

        public static Vector3 Redirect(this Vector3 vector, Vector3 direction) {
            float angle = Vector3.SignedAngle(vector, direction, Vector3.up);
            return Quaternion.AngleAxis(angle, Vector3.up) * vector;
        }

        public static Vector3 Redirect(this Vector3 vector, Vector3 firstDirection, Vector3 lastDirection) {
            float angle = Vector3.SignedAngle(firstDirection, lastDirection, Vector3.up);
            return Quaternion.AngleAxis(angle, Vector3.up) * vector;
        }

        public static bool Contains(this LayerMask layerMask, int layer) {
            return layerMask == (layerMask | (1 << layer));
        }

        public static bool IsInside(this float number, float min, float max) {
            if (number > min && number < max) {
                return true;
            }
            return false;
        }

        #endregion

        #region Utilities

        public static void DebugLog(string separator, params string[] strings) {
            string toPrint = "";
            for (int i = 0; i < strings.Length - 1; i++) {
                toPrint += strings[i] + separator;
            }
            toPrint += strings[strings.Length - 1];
            Debug.Log(toPrint);
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static Rect GetPlayerRect(int playerId, int maxPlayer) {
            switch (maxPlayer) {
                case 1:
                    return new Rect(0, 0, 1, 1);
                case 2:
                    switch (playerId) {
                        case 1:
                            return new Rect(0, 0.5f, 1, 0.5f);
                        case 2:
                            return new Rect(0, 0, 1, 0.5f);
                    }
                    break;
                case 3:
                    switch (playerId) {
                        case 1:
                            return new Rect(0, 0.5f, 0.5f, 0.5f);
                        case 2:
                            return new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                        case 3:
                            return new Rect(0, 0, 0.5f, 0.5f);
                    }
                    break;
                case 4:
                    switch (playerId) {
                        case 1:
                            return new Rect(0, 0.5f, 0.5f, 0.5f);
                        case 2:
                            return new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                        case 3:
                            return new Rect(0, 0, 0.5f, 0.5f);
                        case 4:
                            return new Rect(0.5f, 0, 0.5f, 0.5f);
                    }
                    break;
            }
            return new Rect(0, 0, 1, 1);
        }

        public static float AcuteAngle(float angle) {
            if (angle > 180f) {
                angle = angle - 360f;
            }

            return angle;
        }

        public static Vector3 AcuteAngle(Vector3 angle) {
            return new Vector3(AcuteAngle(angle.x), AcuteAngle(angle.y), AcuteAngle(angle.z));
        }

        public static int Ponder(params int[] weight) {
            int totWeight = 0;

            for (int i = 0; i < weight.Length; i++) {
                totWeight += weight[i];
            }

            int random = UnityEngine.Random.Range(0, totWeight);

            for (int i = 0; i < weight.Length; i++) {
                if (random < weight[i]) {
                    return i;
                }
                random -= weight[i];
            }

            return weight.Length - 1;
        }

        /// <summary>
        /// Return a random number from argument
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static float RandomFloat(params float[] numbers) {
            int rand = UnityEngine.Random.Range(0, numbers.Length);
            return numbers[rand];
        }

        #endregion

        public static IEnumerator Delay<T>(BasicDelegate<T> function, T arg, float time) {
            yield return new WaitForSeconds(time);
            function(arg);
        }

        public static IEnumerator Delay(BasicDelegate function, float time) {
            yield return new WaitForSeconds(time);
            function();
        }

        public static IEnumerator Wait(float time) {
            yield return new WaitForSeconds(time);
        }

        public static float Negative(float number) {
            if (number > 0) {
                number *= -1;
            }
            return number;
        }

        public static float Positive(float number) {
            if (number < 0) {
                number *= -1;
            }
            return number;
        }

        public static float GetPositive(float number) {
            if (number < 0) {
                number = 0;
            }
            return number;
        }
    }

    public class ReadOnlyAttribute : PropertyAttribute {

    }
}