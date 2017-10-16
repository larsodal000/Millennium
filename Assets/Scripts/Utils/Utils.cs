﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Utils {

    //Utility enums
    public enum EnumDirection
    {
        LEFT, RIGHT, UP, DOWN, UNKNOWN
    }

    //Enum utility methods
    public static EnumDirection getOpposite(this EnumDirection direction) {
        if (direction == EnumDirection.LEFT) {
            return EnumDirection.RIGHT;
        } else if (direction == EnumDirection.RIGHT) {
            return EnumDirection.LEFT;
        } else if (direction == EnumDirection.UP) {
            return EnumDirection.DOWN;
        } else if (direction == EnumDirection.DOWN) {
            return EnumDirection.UP;
        }
        return EnumDirection.UNKNOWN;
    }

    public static T NextEnum<T>(this T src) where T : struct {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    public static T PreviousEnum<T>(this T src) where T : struct {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) - 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    //Dictionary Utility

    public static T Get<T>(this Dictionary<string, object> instance, string name) {
        try {
            return (T)instance[name];
        }catch(Exception e){
            Debug.LogError("Failed to cast entry "+name+" to requested type: "+e.Message+e.StackTrace);
        }
        return default(T);
    }

    public static byte[] SerializeDict(this Dictionary<string, object> obj) {
        if (obj == null) {
            return null;
        }

        using (var memoryStream = new MemoryStream()) {
            var binaryFormatter = new BinaryFormatter();

            binaryFormatter.Serialize(memoryStream, obj);

            return memoryStream.ToArray();
        }
    }

    public static Dictionary<string, object> DeSerializeDict(this byte[] arrBytes) {
        using (var memoryStream = new MemoryStream()) {
            var binaryFormatter = new BinaryFormatter();

            memoryStream.Write(arrBytes, 0, arrBytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return (Dictionary<string, object>)binaryFormatter.Deserialize(memoryStream);
        }
    }

    //Utility methods
    public static string newLine(){
        return "\r\n";
    }

    public static Transform ClearChildren(this Transform transform) {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }

    //Save serialization utilities
    public static string Serialize(object value) {
        return JsonUtility.ToJson(value);
    }

    public static T Deserialize<T>(string json) {
        return JsonUtility.FromJson<T>(json);
    }

}