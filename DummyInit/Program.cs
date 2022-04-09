using DummyInit.Classes2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DummyInit
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");


            Process(typeof(Root2));

            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        public static void Process(Type type)
        {
            var rc = new RecursionObj();
            rc.SB.Append($"var my{type.Name} = new {type.Name}() ");
            ProcessProps(type, rc);
            rc.SB.Append(";");
            var str = rc.SB.ToString();
            Console.WriteLine(str);
            System.Diagnostics.Debug.WriteLine(str);
        }

        public class RecursionObj
        {
            public RecursionObj()
            {
                SB = new StringBuilder();
            }
            public StringBuilder SB { get; set; }
            public string Tabs { get; set; }
            public string BlockLevel { get; set; }
            public int BlockCount { get; set; }

            public void IncrementBlock() { SB.AppendLine($"{Tabs}{{"); BlockCount++; Tabs += "  "; }
            public void DecrementBlock() { BlockCount--; Tabs = Tabs.Substring(2); SB.AppendLine($"{Tabs}}},"); }

            public int DefaultInt { get; set; } = 0;

            // returns true if object type
            public bool AddProperty(PropertyInfo pi) {
                //var str = $"public {pi.PropertyType.Name} {pi.Name} {{ get; set; }}";
                //var isObject = pi.IsOb
                //var str = $"{pi.Name} = new {pi.PropertyType.Name}()";
                var str = $"{pi.Name} = 0,";

                return false;
            }

            public void AddInteger(PropertyInfo pi) {
                SB.Append(Tabs);
                SB.AppendLine($"{pi.Name} = 0,");
            }

            internal void AddString(PropertyInfo pi) {
                SB.Append(Tabs);
                SB.AppendLine($"{pi.Name} = \"blaa\",");
            }

            public void AddObjectCreation(PropertyInfo pi) {
                SB.Append(Tabs);
                SB.AppendLine($"{pi.Name} = new {pi.PropertyType.Name}()");
            }

            public void AddNamelessListObjectCreation(Type type)
            {
                SB.Append(Tabs);
                SB.AppendLine($"new {type.Name}()");
            }

            public void AddNamelessListListCreation(Type type)
            {
                //SB.Append(Tabs);
                //SB.AppendLine($"new List<{type.Name}>()");
            }

            internal void AddListCreation(PropertyInfo pi, RecursionObj rObj)
            {
                Type itemType = pi.PropertyType.GetGenericArguments()[0];
                SB.Append(Tabs);
                SB.AppendLine($"{pi.Name} = new List<{itemType.Name}>()");
            }

            internal void AddListInteger(PropertyInfo pi)
            {
                SB.Append(Tabs);
                SB.AppendLine("1,");
            }
            internal void AddListString(PropertyInfo pi)
            {
                SB.Append(Tabs);
                SB.AppendLine("\"blaa\",");
            }
        }

        public static void ProcessProps(Type type, RecursionObj rObj) {
            rObj.IncrementBlock();
            var props = type.GetProperties();
            foreach (var pi in props)
            {
                var tc = Type.GetTypeCode(pi.PropertyType);

                if (IsNumericType(tc)) { rObj.AddInteger(pi); }

                else if (tc == TypeCode.String)
                {
                    rObj.AddString(pi);
                }
                else if (tc == TypeCode.Object)
                {
                    if (IsGenericList(pi.PropertyType))
                    {
                        rObj.AddListCreation(pi, rObj);
                        ProcessListObjects(pi, rObj);
                    }
                    else {
                        rObj.AddObjectCreation(pi);
                        ProcessProps(pi.PropertyType, rObj);
                    }
                }
                else
                {
                    Console.WriteLine("NOT SUPPORTED");
                    System.Diagnostics.Debug.WriteLine("NOT SUPPORTED");
                }
            }
            rObj.DecrementBlock();
        }

        private static void ProcessListObjects(PropertyInfo pi, RecursionObj rObj)
        {
            Type itemType = pi.PropertyType.GetGenericArguments()[0];
            var tc = Type.GetTypeCode(itemType);

            rObj.IncrementBlock();

            if (IsNumericType(tc)) {
                rObj.AddListInteger(pi);
            }

            else if (tc == TypeCode.String)
            {
                rObj.AddListString(pi);
            }
            else if (tc == TypeCode.Object)
            {
                if (IsGenericList(itemType)) // list of list objects
                {
                    rObj.AddNamelessListListCreation(itemType);
                    //ProcessListObjects(pi, rObj);
                }
                else
                {
                    rObj.AddNamelessListObjectCreation(itemType);
                    ProcessProps(itemType, rObj);
                }
            }

            rObj.DecrementBlock();
        }

        public static bool IsString(Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.String:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNumericType(TypeCode tc)
        {
            switch (tc)
            {
                //case TypeCode.Byte:
                //case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsGenericList(Type oType)
        {
            return (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(List<>)));
        }
    }
}
