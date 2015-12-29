using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace Common
{
    public class Functions
    {

        public static string pathToItems = "\\";//"\\Assets\\Resources\\";
        public static Dictionary<System.Type, string> paths = new Dictionary<System.Type, string>()
    {
        { typeof(Common.Module), "Items\\Modules\\" },
        { typeof(Consumable), "Items\\Consumables\\" }
    };

        public static bool Contains<T>(T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].ToString().ToLower() == value.ToString().ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public static T Clone<T>(T source)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, source);
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }

        public static void Serialize<T>(T subject, string filename)
        {
            FieldInfo[] fields = subject.GetType().GetFields();
            if (fields.Length > 0)
            {
                StreamWriter sw = new StreamWriter("Assets\\Resources\\" + paths[subject.GetType()] + filename, false);
                sw.WriteLine("<" + subject.GetType() + ">");
                for (int i = 0; i < fields.Length; i++)
                {
                    Serializable[] attributes = fields[i].GetCustomAttributes(typeof(Serializable), false) as Serializable[];
                    if (attributes.Length == 0)
                    {
                        sw.WriteLine(string.Format("\t<{0}>{1}</{0}>", fields[i].Name, fields[i].GetValue(subject)));
                    }
                }
                sw.WriteLine("</" + subject.GetType() + ">");
                sw.Flush();
                sw.Close();
            }
        }

        public static T Deserialize<T>(string filename)
        {
            StreamReader sr = new StreamReader(System.Environment.CurrentDirectory + pathToItems + paths[typeof(T)] + filename);
            Debug.Log(System.Environment.CurrentDirectory + pathToItems + paths[typeof(T)] + filename);
            string className = sr.ReadLine().Replace('<', ' ').Replace('>', ' ').Remove(0, 1);
            className = className.Remove(className.Length - 1, 1);
            var subject = System.Activator.CreateInstance(null, "Common." + className).Unwrap();
            Dictionary<string, string> values = new Dictionary<string, string>();
            string input = sr.ReadLine();
            while (input != "</" + className + ">")
            {
                string[] tmp, tmp2;
                tmp = input.Split('>');
                string name = tmp[0].Remove(0, 2).Split(' ')[0];
                tmp2 = tmp[1].Split('<');
                string value = tmp2[0];
                values.Add(name, value);
                input = sr.ReadLine();
            }
            FieldInfo[] fields = subject.GetType().GetFields();
            if (fields.Length > 0)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    try
                    {
                        Serializable[] attributes = fields[i].GetCustomAttributes(typeof(Serializable), false) as Serializable[];
                        if (attributes.Length == 0)
                        {
                            if (values[fields[i].Name] != null)
                            {
                                System.Type t = fields[i].FieldType;
                                var casted = Functions.Cast(values[fields[i].Name], fields[i].FieldType);
                                if (casted != null)
                                {
                                    fields[i].SetValue(subject, casted);
                                }
                            }

                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log( fields[i] + "\n" + e.InnerException + "\n" + e.StackTrace);
                    }
                }
            }
            sr.Close();
            return (T)subject;
        }

        public static bool BelongsToCircle(Vector2 point, Vector2 center, float r)
        {
            if (System.Math.Sqrt(System.Math.Pow((center.x - point.x), 2) + System.Math.Pow((center.y - point.y), 2)) <= r)
            {
                return true;
            }
            return false;
        }

        public static object Cast(object subject, System.Type type)
        {
            if (type.IsEnum)
            {
                return System.Enum.Parse(type, subject.ToString());
            }
            else
            {
                return System.Convert.ChangeType(subject, type);
            }
        }
    }
}
