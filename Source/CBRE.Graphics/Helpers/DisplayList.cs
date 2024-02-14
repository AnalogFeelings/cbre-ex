using CBRE.Localization;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace CBRE.Graphics.Helpers
{
    public class DisplayList : IDisposable
    {
        private static Dictionary<string, int> Lists;
        private static string CurrentList;
        public static object ListLock { get; private set; }

        static DisplayList()
        {
            Lists = new Dictionary<string, int>();
            CurrentList = null;
            ListLock = new object();
        }

        public static bool Exists(string name)
        {
            return Lists.ContainsKey(name);
        }

        public static void Create(string name)
        {
            if (Lists.ContainsKey(name))
            {
                Delete(name);
            }
            int num = GL.GenLists(1);
            Lists.Add(name, num);
        }

        public static void Begin(string name)
        {
            if (!Lists.ContainsKey(name))
            {
                throw new Exception(Local.LocalString("exception.list_not_exist"));
            }
            else if (CurrentList != null)
            {
                throw new Exception(Local.LocalString("exception.another_list_in_progress", CurrentList));
            }
            GL.NewList(Lists[name], ListMode.Compile);
            CurrentList = name;
        }

        public static void End(string name)
        {
            if (CurrentList == null)
            {
                throw new Exception(Local.LocalString("exception.no_list_in_progress"));
            }
            else if (CurrentList != name)
            {
                throw new Exception(Local.LocalString("exception.cannot_end_current_list", name, CurrentList));
            }
            GL.EndList();
            CurrentList = null;
        }

        public static void Call(string name)
        {
            if (!Lists.ContainsKey(name))
            {
                throw new Exception(Local.LocalString("exception.list_not_exist"));
            }
            GL.CallList(Lists[name]);
        }

        public static void Delete(string name)
        {
            if (Lists.ContainsKey(name))
            {
                GL.DeleteLists(Lists[name], 1);
                Lists.Remove(name);
            }
        }

        public static void DeleteAll()
        {
            foreach (KeyValuePair<string, int> e in Lists)
            {
                GL.DeleteLists(e.Value, 1);
            }
            Lists.Clear();
        }

        public static DisplayList Using(string name)
        {
            return new DisplayList(name);
        }

        private string Name;

        public DisplayList(string name)
        {
            Name = name;
            Create(Name);
            Begin(Name);
        }

        public void Dispose()
        {
            End(Name);
        }
    }
}
