using System;
using System.Diagnostics.CodeAnalysis;

namespace AspNetMvcScaffolder
{
    public class FrameworkDependencyStatus
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "This type is immutable")]
        public static readonly FrameworkDependencyStatus InstallSuccessful = new FrameworkDependencyStatus()
        {
            IsNewDependencyInstall = true,
        };

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "This type is immutable")]
        public static readonly FrameworkDependencyStatus InstallNotNeeded = new FrameworkDependencyStatus()
        {
        };

        public static FrameworkDependencyStatus FromReadme(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return new FrameworkDependencyStatus()
            {
                IsNewDependencyInstall = true,
                IsReadmeRequired = true,
                ReadmeText = text,
            };
        }

        private FrameworkDependencyStatus()
        {
        }

        public bool IsNewDependencyInstall { get; private set; }

        public bool IsReadmeRequired { get; private set; }

        public string ReadmeText { get; private set; }
    }
}
