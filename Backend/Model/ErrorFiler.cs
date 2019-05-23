using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace NAS.Model
{
    /// <summary>
    /// This class contains all the logic to log errors to a local text file for debugging.
    /// </summary>
    public class ErrorFiler
    {

        /// <summary>
        /// Store for the path property, which is relative to the root folder.
        /// </summary>
        readonly string path = HttpContext.Current.Server.MapPath(@"~\Content\ErrorLog.txt");

        /// <summary>
        /// Writes the error to the file with the type of error as well as when and where in the code it occured.
        /// </summary>
        /// <param name="error">The type of error that occurred.</param>
        /// <param name="errorText">Where the error occurred.</param>
        public void WriteError(string error, string errorText)
        {
            // Checks if there is a file
            if (File.Exists(path))
            {
                // Appends the text to the end of the file
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(DateTime.Now + "\t" + error + " -> " + errorText);
                }
            }
        }
    }
}
