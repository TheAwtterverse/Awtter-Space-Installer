﻿using Assets.Awtter_SDK.Editor.Models;
using System.Linq;
using UnityEngine;
using UnityEngine.WSA;
using VRC.PackageManagement.Core.Types.Packages;

namespace AwtterSDK.Editor.Models.API
{
    public class FileModel
    {
        private string _simpleName;
        private int? _id;
        public int Id
        {
            get
            {
                if (!_id.HasValue)
                {
                    string splitId = Path.Split('/').Last();

                    if (int.TryParse(splitId, out int id))
                        _id = id;
                }

                if (_id != null) return _id.Value;
                return -1;
            }
        }


        public string Path { get; set; }
        public string Name { get; set; }

        public string NameSimple
        {
            get
            {
                if (_simpleName == null)
                    _simpleName = Name.ToLower().Replace(" ", "");

                return _simpleName;
            }
        }

        public string Version { get; set; }
        public bool IsTool { get; set; }
        public bool IsProp { get; set; }
        public bool IsVrcUnitypackage { get; set; }
        public bool IsMergerCompatible { get; set; }
        public bool IsBaseModel => IsVrcUnitypackage && !IsMergerCompatible;
        public bool IsDLC => IsVrcUnitypackage && IsMergerCompatible || IsProp;

        public bool IsInstalled
        {
            get
            {
                if (AwtterSpaceInstaller.InstalledPackages == null) return false;
                
                if (IsBaseModel)
                    return AwtterSpaceInstaller.InstalledPackages?.BaseModel?.Id == Id;
                
                else if (IsTool)
                    return AwtterSpaceInstaller.InstalledPackages.Tools.ContainsKey(NameSimple);
                else
                    return AwtterSpaceInstaller.InstalledPackages.Dlcs.ContainsKey(Id);
            }
        }

        public InstalledPackageModel GetBase()
        {
            if (IsInstalled)
            {
                var baseModel = AwtterSpaceInstaller.InstalledPackages?.BaseModel;
                if (baseModel == null) return null;

                AwtterSpaceInstaller.InstalledPackages.BaseModel.File = this;
                return baseModel;
            }
            return null;
        }

        public InstalledPackageModel GetDLC()
        {
            if (IsInstalled)
            {
                var dlc = AwtterSpaceInstaller.InstalledPackages.Dlcs[Id];
                if (dlc == null) return null;

                AwtterSpaceInstaller.InstalledPackages.Dlcs[Id].File = this;
                return dlc;
            }
            return null;
        }

        public InstalledPackageModel GetTool()
        {
            if (IsInstalled)
            {
                var name = Name.ToLower().Replace(" ", "");
                
                AwtterSpaceInstaller.InstalledPackages.Tools.TryGetValue(name, out InstalledPackageModel tool);
                if (tool == null) return null;
                
                AwtterSpaceInstaller.InstalledPackages.Tools[name].Id = tool.Id;
                AwtterSpaceInstaller.InstalledPackages.Tools[name].File = this;
                return tool;
            }
            return null;
        }


        public BaseView ToBaseView(ProductModel product, bool isPatreon = false) => new BaseView()
        {
            Id = Id,
            IsPatreon = isPatreon,
            ProductId = product.Id,
            Icon = product.Icon,
            Version = Version,
            InstalledVersion = GetBase()?.Version,
            BaseName = product.BaseName,
            DownloadUrl = Path,
            Name = Name,
        };

        public DlcView ToDLCView(ProductModel product, bool isPatreon = false) => new DlcView()
        {
            Id = Id,
            IsPatreon = isPatreon,
            ProductId = product.Id,
            Icon = product.Icon,
            Version = Version,
            InstalledVersion = GetDLC()?.Version,
            DownloadUrl = Path,
            IsProp = IsProp,
            IsDlc = IsDLC,
            Name = Name,
        };

        public ToolView ToToolView(ToolboxOkResponseModel product, bool isPatreon = false) => new ToolView()
        {
            Id = Id,
            Icon = product.Icon,
            Version = Version,
            InstalledVersion = GetTool()?.Version,
            DownloadUrl = Path,
            Name = Name,
        };
    }
}
