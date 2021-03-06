﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.ProjectModel;
using OmniSharp.DotNet.Cache;

namespace OmniSharp.DotNet.Models
{
    public class DotNetProjectInfo
    {
        public DotNetProjectInfo(ProjectEntry entry, bool includeSourceFiles = false)
        {
            this.Path = entry.ProjectDirectory;

            var baseContext = entry.ProjectStates.FirstOrDefault()?.ProjectContext;
            this.Name = baseContext?.ProjectFile?.Name;
            this.Frameworks = entry.ProjectStates
                .Select(state => new DotNetFramework(state.ProjectContext.TargetFramework))
                .ToList();
            this.ProjectSearchPaths = baseContext?.GlobalSettings?.ProjectSearchPaths ?? new List<string>();
            this.Configurations = baseContext?.RootProject?.Project?.GetConfigurations()
                ?.Select(x => new DotNetConfiguration(baseContext, x))
                ?.ToList() ?? new List<DotNetConfiguration>();

            var sourceFiles = new List<string>();
            if (includeSourceFiles)
            {
                sourceFiles.AddRange(entry.ProjectStates
                    .SelectMany(x => x.ProjectContext.ProjectFile.Files.SourceFiles)
                    .Distinct());
            }

            this.SourceFiles = sourceFiles;
        }

        public string Path { get; }
        public string Name { get; }
        public IList<string> ProjectSearchPaths { get; set; }
        public IList<DotNetConfiguration> Configurations { get; set; }
        public IList<DotNetFramework> Frameworks { get; set; }
        public IList<string> SourceFiles { get; set; }
    }
}
