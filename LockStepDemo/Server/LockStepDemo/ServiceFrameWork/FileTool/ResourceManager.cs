using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ResourceManager
{
    public static string ResourcePath = "Map";
    public static string ReadTextFile(string name)
    {
        string path = Environment.CurrentDirectory + "/"+ ResourcePath + "/";

        return  FileTool.ReadStringByFile(path + name + ".txt");
    }
}
