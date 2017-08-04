using LockStepDemo;
using LockStepDemo.Protocol;
using LockStepDemo.Reflect;
using Protocol;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CSharp2Protocol
{
    class Program
    {
        static void Main(string[] args)
        {
            ProtocolTool.GenerateProtocolToCsharp();
        }
    }
}
