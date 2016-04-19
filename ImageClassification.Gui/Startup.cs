using System;
using System.Collections.Generic;
using Wkiro.ImageClassification.Gui.Helpers;

namespace Wkiro.ImageClassification.Gui
{
    public class Startup
    {
        private static readonly Dictionary<string, string> KnownAssemblies;

        static Startup()
        {
            // Assemblies lack resolve
            KnownAssemblies = AssembliesHelpers.LoadKnownAssemblies();
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) => AssembliesHelpers.Resolve(KnownAssemblies, s, e);
        }

        [STAThread]
        public static void Main()
        {
            var application = new App();
            application.InitializeComponent();
            application.Run();
        }
    }
}
