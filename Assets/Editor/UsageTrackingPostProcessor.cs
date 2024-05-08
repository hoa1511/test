using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;

public class UsageTrackingPostProcessor {

    private const string DESCRIPTION = "We will utilize your usage data to enhance the performance of the game and offer you a more customized and enjoyable advertising experience.";

    [PostProcessBuild(0)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToXcode) {
        if (buildTarget == BuildTarget.iOS) {
            AddPListValues(pathToXcode);
        }
    }

    // Implement a function to read and write values to the plist file:
    private static void AddPListValues(string pathToXcode) {
        // Retrieve the plist file from the Xcode project directory:
        string plistPath = Path.Combine(pathToXcode, "Info.plist");
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        plist.root.SetString("NSUserTrackingUsageDescription", DESCRIPTION);

        // Save changes to the plist:
        plist.WriteToFile(plistPath);
    }
}
#endif
