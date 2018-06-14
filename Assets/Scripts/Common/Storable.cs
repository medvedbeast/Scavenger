using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Common
{
    [System.Serializable]
    public class Storable
    {
        public int slot;
        public float weight;
        public string name;
        public string description;
        public float price;
        public int maxStack;
        public int stack;

        public static string[] baseFields
        {
            get
            {
                System.Reflection.FieldInfo[] f = typeof(Storable).GetFields();
                List<string> l = new List<string>();
                for (int i = 0; i < f.Length; i++)
                {
                    l.Add(f[i].Name);
                }
                return l.ToArray();
            }
        }

        public override string ToString()
        {
            return string.Format("Storable: [name='{0}', slot='{1}'];", name, slot);
        }
    }
}