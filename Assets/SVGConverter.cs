using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VectorGraphics;
using Unity.VectorGraphics.Editor;
using UnityEngine;

public class SVGConverter : MonoBehaviour
{
    public TextAsset svgFolder;
    static readonly char[] pathSeperators = { 'M', 'L', 'Z', 'H', 'V', 'C', 'S', 'Q', 'T', 'A' };
    const string folderPath = "SVG/";
    void Start()
    {
        Debug.Log(svgFolder.text);
        string pattern = @"path\s+d=\"".*\""\s";//Getting path attribute from SVG
        string pattern2 = @"\"".*\""\s";//Getting path attribute's value.
        string svgPath = Regex.Match(svgFolder.text, pattern).Value;
        svgPath = Regex.Match(svgPath, pattern2).Value;
        svgPath = svgPath.Replace("\"", "");//Deleting unnecessary quotes
        svgPath = putSeperatorPrefix(svgPath);
        Debug.Log(svgPath);
        getCoordinates(svgPath);//For keeping first letters in pattern.
    }
    public static string[] getCoordinates(string pathString)
    {
        string[] lines = pathString.Split('#');
        foreach (string line in lines)
        {
            Debug.Log(line);
        }
        return lines;
    }

    private static string putSeperatorPrefix(string str)
    {
        for (int i = 0; i < pathSeperators.Length; i++)
        {
            string seperator = pathSeperators[i].ToString();
            str = str.Replace(seperator, "#" + seperator);
        }
        return str;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
