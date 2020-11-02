#pragma warning disable 1591
using System;
using System.IO;

public class XML
{
    public static string path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
    public static string actualPath = path.Substring(0, path.LastIndexOf("bin"));
    public static string projectPath = new Uri(actualPath).LocalPath;
    public static string ResultsPath = projectPath + "Result";     //This may not exist on a clean bootstrapped system....
    public static string date = DateTime.Now.ToString("yyyyMMddHHmmss");
    public static string FailResultsFilePath = ResultsPath+"\\FailResults.txt";
    public static string FailResultsFilePath1 = ResultsPath +"\\FailResultslist"+"\\FailResults"+date+".txt";
    public static string FailurePlaylistFilePath = ResultsPath +"\\Playlists"+"\\FailurePlaylist"+date+".playlist";

   
    public void FailurePlaylist(string TestcaseName)
    {

        var line = Environment.NewLine;
        
        try
        {
            if (!File.Exists(FailurePlaylistFilePath))
            {
                File.Create(FailurePlaylistFilePath).Dispose();
                using (TextWriter tw = new StreamWriter(FailurePlaylistFilePath))
                {
                    tw.WriteLine("<Playlist Version =" + "\"" + "1.0" + "\"" + ">" + "<Add Test=" + "\"" + TestcaseName + "\"" + "/>");
                }
            }
            else if (File.Exists(FailurePlaylistFilePath))
            {
                using (TextWriter tw = new StreamWriter(FailurePlaylistFilePath, true))
                    tw.WriteLine("<Add Test=" + "\"" + TestcaseName + "\"" + "/>");
            }
        }
        catch (Exception)
        {

        }

    }

    public static void deletefile()
    {
        if (File.Exists(FailResultsFilePath))
        {
            File.Copy(FailResultsFilePath, FailResultsFilePath1, true);
            File.Delete(FailResultsFilePath);
        }
    }


    public void FailureTests(string TestcaseName)
    {
        var line = Environment.NewLine;
        
        try
        {
            if (!File.Exists(FailResultsFilePath))
            {
                File.Create(FailResultsFilePath).Dispose();
                using (TextWriter tw = new StreamWriter(FailResultsFilePath))
                {
                    tw.WriteLine(TestcaseName);
                }
            }
            else if (File.Exists(FailResultsFilePath))
            {
                using (TextWriter tw = new StreamWriter(FailResultsFilePath, true))
                    tw.WriteLine(TestcaseName);
            }
        }
        catch(Exception)
        {

        }

    }
    public static void append()
    {
        if (File.Exists(FailurePlaylistFilePath))
        {
            using (TextWriter tw = new StreamWriter(FailurePlaylistFilePath, true))
                tw.WriteLine("</Playlist>");
        }

    }
}
#pragma warning restore 1591