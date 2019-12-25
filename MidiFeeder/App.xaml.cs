using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MoonBurst.ViewModel;

namespace MoonBurst
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            FileAssociations.EnsureAssociationsSet(new []{ new FileAssociations.FileAssociation()
            {
                ExecutableFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location,
                Extension = ".mblayout",
                FileTypeDescription = "MoonBurst Layout",
                ProgId = "MOONBURST",
                DefaultIcon = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "img\\", "moonfile.ico")
            },
                new FileAssociations.FileAssociation()
                {
                    ExecutableFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location,
                    Extension = ".mbconfig",
                    FileTypeDescription = "MoonBurst Config",
                    ProgId = "MOONBURST",
                    DefaultIcon = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "img\\", "moonfile.ico")
                }
            });
            base.OnStartup(e);
        }
    }
}
