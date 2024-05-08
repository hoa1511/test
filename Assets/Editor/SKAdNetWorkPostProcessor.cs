using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class SKAdNetWorkPostProcessor
{

    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToXcode)
    {
        if (target == BuildTarget.iOS)
        {
            AddSkNetworkItems(pathToXcode);
        }
    }

    private static void AddSkNetworkItems(string pathToXcode)
    {
        // Load the Info.plist file
        string plistPath = Path.Combine(pathToXcode, "Info.plist");
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        // Read SKAdNetworkIdentifiers from file
        string identifiersFilePath = Path.Combine(Application.dataPath, "Editor", "SKAdNetworkIdentifiers.txt");
        string[] skAdNetworkIdentifiers = File.ReadAllLines(identifiersFilePath);

        // Add SKAdNetworkItems
        var skAdNetworkItems = plist.root.CreateArray("SKAdNetworkItems");
        foreach (string identifier in skAdNetworkIdentifiers)
        {
            var dict = skAdNetworkItems.AddDict();
            dict.SetString("SKAdNetworkIdentifier", identifier.Trim());
        }

        // Save changes to the Info.plist file
        plist.WriteToFile(plistPath);
    }
}
#endif
