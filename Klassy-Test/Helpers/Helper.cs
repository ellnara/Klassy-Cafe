using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Klassy_Test.Helpers
{
    public static class Helper
    {
        public static string GetPath(string root,params string[] folders)
        {
            string resultPath = root;
            foreach (var folder in folders)
            {
                resultPath = Path.Combine(resultPath,folder);
            }
            return resultPath;
        }
    }
}
