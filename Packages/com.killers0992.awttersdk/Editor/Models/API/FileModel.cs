using System.Linq;
using Assets.Awtter_SDK.Editor.Models;

namespace AwtterSDK.Editor.Models.API
{
    public class FileModel
    {
        private int? _id;
        private string _simpleName;

        public int Id
        {
            get
            {
                if (!_id.HasValue)
                {
                    var splitId = Path.Split('/').Last();

                    if (int.TryParse(splitId, out var id))
                        _id = id;
                    else
                        _id = 0;
                }

                return _id.Value;
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
        public bool IsDLC => (IsVrcUnitypackage && IsMergerCompatible) || IsProp;
        public bool IsMarketPlace { get; set; } = false;

        public bool IsInstalled
        {
            get
            {
                if (AwtterSdkInstaller.InstalledPackages == null) return false;

                if (IsBaseModel)
                    return AwtterSdkInstaller.InstalledPackages?.BaseModel?.Id == Id;
                if (IsTool)
                    return AwtterSdkInstaller.InstalledPackages.Tools.ContainsKey(NameSimple);
                if (IsMarketPlace)
                    return AwtterSdkInstaller.InstalledPackages.Marketplace.ContainsKey(Id);
                return AwtterSdkInstaller.InstalledPackages.Dlcs.ContainsKey(Id);
            }
        }

        public InstalledPackageModel GetBase()
        {
            if (IsInstalled)
            {
                var baseModel = AwtterSdkInstaller.InstalledPackages?.BaseModel;
                if (baseModel == null) return null;

                AwtterSdkInstaller.InstalledPackages.BaseModel.File = this;
                return baseModel;
            }

            return null;
        }

        public InstalledPackageModel GetDLC()
        {
            if (IsInstalled)
            {
                var dlc = AwtterSdkInstaller.InstalledPackages.Dlcs[Id];
                if (dlc == null) return null;

                AwtterSdkInstaller.InstalledPackages.Dlcs[Id].File = this;
                return dlc;
            }

            return null;
        }

        public InstalledPackageModel GetMarketplace()
        {
            if (IsInstalled)
            {
                var marketplace = AwtterSdkInstaller.InstalledPackages.Marketplace[Id];
                if (marketplace == null) return null;

                AwtterSdkInstaller.InstalledPackages.Marketplace[Id].File = this;
                return marketplace;
            }

            return null;
        }

        public InstalledPackageModel GetTool()
        {
            if (IsInstalled)
            {
                var name = Name.ToLower().Replace(" ", "");

                var tool = AwtterSdkInstaller.InstalledPackages.Tools[name];
                if (tool == null) return null;

                AwtterSdkInstaller.InstalledPackages.Tools[name].Id = tool.Id;
                AwtterSdkInstaller.InstalledPackages.Tools[name].File = this;
                return tool;
            }

            return null;
        }

        public BaseView ToBaseView(ProductModel product, bool isPatreon = false, bool isMarketplace = false)
        {
            var ret = new BaseView
            {
                Id = Id,
                IsPatreon = isPatreon,
                IsMarketplace = isMarketplace,
                ProductId = product.Id,
                Icon = product.Icon,
                Version = Version,
                InstalledVersion = GetBase()?.Version,
                BaseName = product.BaseName,
                DownloadUrl = Path,
                Name = Name
            };

            if (isMarketplace) ret.Name = product.Name;

            return ret;
        }

        public DlcView ToDLCView(ProductModel product, bool isPatreon = false)
        {
            var ret = new DlcView
            {
                Id = Id,
                IsPatreon = isPatreon,
                Category = product.Category,
                ProductId = product.Id,
                Icon = product.Icon,
                Version = Version,
                InstalledVersion = GetDLC()?.Version,
                DownloadUrl = Path,
                IsProp = IsProp,
                IsDlc = IsDLC,
                Name = Name
            };

            return ret;
        }

        public MarketplaceView ToMarketplaceView(ProductModel product)
        {
            var ret = new MarketplaceView
            {
                Id = Id,
                IsMarketplace = true,
                Category = product.Category,
                ProductId = product.Id,
                Icon = product.Icon,
                Version = Version,
                InstalledVersion = GetMarketplace()?.Version,
                DownloadUrl = Path,
                Name = product.Name
            };

            return ret;
        }

        public ToolView ToToolView(ToolboxOkResponseModel product, bool isPatreon = false)
        {
            return new ToolView()
            {
                Id = Id,
                Icon = product.Icon,
                Version = Version,
                InstalledVersion = GetTool()?.Version,
                DownloadUrl = Path,
                Name = Name
            };
        }
    }
}