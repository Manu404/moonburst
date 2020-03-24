using System.IO;

namespace MoonBurst.Helper
{
    public class FileAssociationsHelper
    {
        public static void EnsureFileAssociation()
        {
            FileAssociations.EnsureAssociationsSet(new[]{ new FileAssociations.FileAssociation()
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
        }
    }
}