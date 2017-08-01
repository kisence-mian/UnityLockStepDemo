using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LockStepDemo.Reflect
{
    public class ReflectTool
    {
        public static Type[] GetTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes();
        }

        public static string GetProjectName()
        {
            return Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        }
    }
}
