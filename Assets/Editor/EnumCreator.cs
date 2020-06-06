using System;
using System.IO;

public static class EnumCreator
{
    /// <summary>
    /// 指定されたEnumの要素でファイルを出力する
    /// </summary>
    /// <param name="elements"></param>
    /// <param name="path"></param>
    public static void Create(string[] elements, string path, string fileName)
    {
        string code = "public enum " + fileName + Environment.NewLine + "{" + Environment.NewLine;

        foreach(var element in elements)
        {
            code += element + "," + Environment.NewLine;
        }

        code += "}";

        File.WriteAllText(Path.GetFullPath(path), code);
    }
}
