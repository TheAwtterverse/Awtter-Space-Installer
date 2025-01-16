using System.Collections.Generic;
using System.Linq;

namespace AwtterSDK.Editor.Models.API
{
    public class ProductModel
    {
        private int? _id;

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
        public CategoryModel Category { get; set; }
        public string BaseName { get; set; }
        public bool IsBaseModel { get; set; }
        public string Icon { get; set; }

        public List<FileModel> Files { get; set; }

        public FileModel IsInstalled(bool findBase = false)
        {
            var vrcPackage = Files.FirstOrDefault(x => x.IsVrcUnitypackage);

            if (vrcPackage == null) return null;

            foreach (var file in Files)
            {
                var id = file.Path.Split('/')[5];

                if (!int.TryParse(id, out var fileId)) continue;

                if (AwtterSdkInstaller.InstalledPackages.BaseModel != null &&
                    AwtterSdkInstaller.InstalledPackages.BaseModel.Id == fileId && findBase)
                    return file;

                if (AwtterSdkInstaller.InstalledPackages.Dlcs.ContainsKey(fileId) && !findBase) return file;
            }

            return null;
        }
    }
}