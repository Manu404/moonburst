using System.IO;

namespace MoonBurst.Helper
{
    public class FileAssociationsHelper
    {
        public static string LayoutExtension => ".mblayout";
        public static string LayoutDescription => "MoonBurst Layout";
        public static string LayoutFilter => $"{LayoutDescription}|*{LayoutExtension}";
        
        public static string ConfigExtension => ".mbconfig";
        public static string ConfigDescription => "MoonBurst Config";
        public static string ConfigFilter => $"{ConfigDescription}|*{ConfigExtension}";

        public static void EnsureFileAssociation()
        {
            FileAssociations.EnsureAssociationsSet(
                new FileAssociations.FileAssociation()
                {
                    ExecutableFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location,
                    Extension = LayoutExtension,
                    FileTypeDescription = LayoutDescription,
                    ProgId = "MOONBURST",
                    DefaultIcon = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "img\\", "moonfile.ico")
                }, new FileAssociations.FileAssociation()
                {
                    ExecutableFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location,
                    Extension = ConfigExtension,
                    FileTypeDescription = ConfigDescription,
                    ProgId = "MOONBURST",
                    DefaultIcon = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "img\\", "moonfile.ico")
                });
        }
    }
}