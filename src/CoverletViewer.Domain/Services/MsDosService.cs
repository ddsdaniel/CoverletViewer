using System.Diagnostics;

namespace CoverletViewer.Domain.Services
{
    public class MsDosService
    {
        public string Run(string commandLine)
        {
            var cmd = "/c " + commandLine;
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    Arguments = cmd,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                }
            };
            proc.Start();

            var output = proc.StandardOutput.ReadToEnd();

            return output;
        }
    }
}
