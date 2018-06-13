using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace SysWork.IO.GoogleDriveManager
{
    public enum EGoogleStatusNotifierMessageType
    {
        Uploading,
        Downloading,
        Completed,
        Failed,
        Message
    }
    public interface IGoogleStatusNotifier
    {
        void UpdateStatus(EGoogleStatusNotifierMessageType messateType, long bytes);
    }

    public class GoogleDriveFile
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string LastModified { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public string Hash { get; set; }
        public string WebContentLink { get; set; }

        public GoogleDriveFile(string name, string size, string lastModified, string type, string id, string hash, string webContentLink)
        {
            this.Name = name;
            this.Size = size;
            this.LastModified = lastModified;
            this.Type = type;
            this.Id = id;
            this.Hash = hash;
            this.WebContentLink = webContentLink;
        }
    }

    public static class GoogleDriveAPIV3
    {
        private static string[] _Scopes = { DriveService.Scope.Drive };

        private static UserCredential _credential;
        private static DriveService _driveService;

        private static string _appCredentialStore = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\.credentials";

        private static string _applicationName = "GoogleDriveManager";
        
        private static readonly string[] _SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static bool GoogleDriveConnection(string jsonSecretFile, string profileName)
        {

            return (GetCredential(jsonSecretFile, profileName) && CreateDriveService());

        }

        public static List<GoogleDriveFile> ListDriveFiles(string fileName = null, string fileType = null)
        {
            List<GoogleDriveFile> filesList = new List<GoogleDriveFile>();

            if(fileName == null && fileType == null)
            {
                FilesResource.ListRequest listRequest = _driveService.Files.List();
                listRequest.PageSize = 1000;
                listRequest.Fields = "nextPageToken, files(mimeType, id, name, parents, size, modifiedTime, md5Checksum, webViewLink)";
                //listRequest.OrderBy = "mimeType";
                // List files.
                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
                filesList.Clear();
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                            
                        filesList.Add(new GoogleDriveFile(
                        file.Name,
                        SizeFix(file.Size.ToString(), file.MimeType),
                        file.ModifiedTime.ToString(),
                        file.MimeType,
                        file.Id, file.Md5Checksum,
                        file.WebViewLink));
                        System.Diagnostics.Debug.WriteLine("{0} {1} {2} {3}",
                            file.Name, file.Id, file.MimeType, file.Size.ToString());
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No files found.");
                }
            }
            else
            {
                string pageToken = null;
                do
                {
                    FilesResource.ListRequest request = _driveService.Files.List();
                    request.PageSize = 1000;
                    //request.Q = "mimeType='image/jpeg'";
                    request.Q = "name contains '" + fileName + "'";
                    if (fileType != null)
                    {
                        request.Q += "and (mimeType contains '" + fileType + "')";
                    }
                    request.Spaces = "drive";
                    request.Fields = "nextPageToken, files(mimeType, id, name, parents, size, modifiedTime, md5Checksum, webViewLink)";
                    request.PageToken = pageToken;
                    var result = request.Execute();
                    foreach (var file in result.Files)
                    {
                        filesList.Add(new GoogleDriveFile(
                            file.Name,
                            SizeFix(file.Size.ToString(), file.MimeType),
                            file.ModifiedTime.ToString(),
                            file.MimeType,
                            file.Id, file.Md5Checksum,
                            file.WebViewLink));
                    }
                    pageToken = result.NextPageToken;
                } while (pageToken != null);
            }

            return filesList;
        }

        
        private static string SizeFix(string bytesString, string type, int decimalPlaces = 1)
        {
            long value;
            if (long.TryParse(bytesString, out value))
            {
                if (value < 0) { return "-" + SizeFix((-value).ToString(), type); }

                int i = 0;
                decimal dValue = (decimal)value;
                while (Math.Round(dValue, decimalPlaces) >= 1000)
                {
                    dValue /= 1024;
                    i++;
                }
                return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, _SizeSuffixes[i]);
            }
            else
            {
                return type.Split('.').Last();
            }
            
        }

        private static bool GetCredential(string clientSecretFile, string profileName)
        {
            string storeSecretFile = Path.Combine(_appCredentialStore , "-" + profileName + "-" + Path.GetFileName(clientSecretFile));

            using (var stream = new FileStream(clientSecretFile, FileMode.Open, FileAccess.Read))
            {

                _credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _Scopes,
                    Path.GetFileName(storeSecretFile),
                    CancellationToken.None,
                    new FileDataStore(_appCredentialStore, true)).Result;
            }

            return true;
        }

        private static bool CreateDriveService()
        {
            // Create Drive API service.
            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = _applicationName,
            });
            return true;

        }

        
        private static bool UploadFileToDrive(string folderId, string fileName, string filePath, string fileType, IGoogleStatusNotifier statusNotifier)
        {

            statusNotifier.UpdateStatus(EGoogleStatusNotifierMessageType.Uploading,0);
            long totalSize = 100000;
            FileInfo fi = new FileInfo(filePath);
            totalSize = fi.Length;

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = fileName
            };
            if (folderId != null)
            {
                fileMetadata.Parents = new List<string>
                {
                    folderId
                };
            }

            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
            {
                request = _driveService.Files.Create(fileMetadata, stream, fileType);
                request.ChunkSize = FilesResource.CreateMediaUpload.MinimumChunkSize;
                request.ProgressChanged += (IUploadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case UploadStatus.Uploading:
                            {
                                statusNotifier.UpdateStatus(EGoogleStatusNotifierMessageType.Uploading, ((progress.BytesSent * 100) / totalSize));
                                System.Diagnostics.Debug.WriteLine(progress.BytesSent);
                                break;
                            }
                        case UploadStatus.Completed:
                            {

                                statusNotifier.UpdateStatus(EGoogleStatusNotifierMessageType.Completed, 100);
                                System.Diagnostics.Debug.WriteLine("Upload complete.");
                                break;
                            }
                        case UploadStatus.Failed:
                            {
                                statusNotifier.UpdateStatus(EGoogleStatusNotifierMessageType.Failed, 0);
                                System.Diagnostics.Debug.WriteLine("Upload failed.");
                                break;
                            }
                    }
                };

                request.Fields = "id";
                request.Upload();
            }
            var file = request.ResponseBody;
            System.Diagnostics.Debug.WriteLine("File ID:{0} \n FileName {1} ", file.Id, file.Name);
            return true;
        }

        private static bool UploadFileToDrive(string folderId, string fileName, string filePath, string fileType, bool onlyNew, IGoogleStatusNotifier statusNotifier)
        {
            if (onlyNew)
            {
                if (!CompareHash(Gtools.HashGenerator(filePath)))
                {
                    UploadFileToDrive(folderId, fileName, filePath, fileType, statusNotifier);
                    return true;
                }
                else return false;

            }
            else
            {
                UploadFileToDrive(folderId, fileName, filePath, fileType, statusNotifier);
                return true;
            }
                
        }


        public static bool CompareHash(string hashToCompare)
        {
            foreach (GoogleDriveFile file in GoogleDriveAPIV3.ListDriveFiles())
            {
                if (file.Hash == hashToCompare) return true;
            }
            return false;
        }

        public static string CreateFolderToDrive(string folderName, string parentId)
        {
                
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = folderName,
                    MimeType = "application/vnd.google-apps.folder",
                    
                };
                if(parentId != null)
                {
                    fileMetadata.Parents = new List<string>
                    {
                        parentId
                    };
                }
                
                var request = _driveService.Files.Create(fileMetadata);
                request.Fields = "id";
                var file = request.Execute();
                System.Diagnostics.Debug.WriteLine("{0} {1}",file.Name, file.Id);
                return file.Id;
        }


        public static void UploadToDrive(string path, string name, string parentId, bool onlyNew, IGoogleStatusNotifier statusNotifier)
        {
            if (Path.HasExtension(path))
            {
                UploadFileToDrive(
                    parentId,
                    name,
                    path,
                    GetMimeType(Path.GetFileName(path)),
                    onlyNew, statusNotifier);
            }
            else
            {
                DirectoryUpload(path, parentId, onlyNew, statusNotifier);
            }
        }

        public static void DirectoryUpload(string path, string parentId, bool onlyNew, IGoogleStatusNotifier statusNotifier)
        {
                // Get the subdirectories for the specified directory.
            string folderId = CreateFolderToDrive(
                    Path.GetFileName(path),
                    parentId);

            System.Diagnostics.Debug.WriteLine(folderId);

            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + path);
            }
                
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                UploadFileToDrive(
                    folderId, file.Name,
                    Path.Combine(path, file.Name),
                    GetMimeType(file.Name), onlyNew, statusNotifier);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo subdir in dirs)
            {
                DirectoryUpload(subdir.FullName,  folderId, onlyNew, statusNotifier);
            }
        }

        private static void ConvertMemoryStreamToFileStream(MemoryStream stream, string savePath)
        {
            FileStream fileStream;
            using (fileStream = new System.IO.FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                // System.IO.File.Create(saveFile)
                stream.WriteTo(fileStream);
                fileStream.Close();
            }
        }

        public static void RemoveFile(string fileID)
        {
            var request = _driveService.Files.Delete(fileID);
            request.Execute();
        }

        public static void DownloadFromDrive(string filename, string fileId, string savePath, string mimeType, IGoogleStatusNotifier statusNotifier)
        {
            long totalSize = 100000;
            statusNotifier.UpdateStatus(EGoogleStatusNotifierMessageType.Downloading, 0);
            if (Path.HasExtension(filename))
            {
                var request = _driveService.Files.Get(fileId);
                    
                var stream = new System.IO.MemoryStream();
                System.Diagnostics.Debug.WriteLine(fileId);
                // Add a handler which will be notified on progress changes.
                // It will notify on each chunk download and when the
                // download is completed or failed.
                request.MediaDownloader.ProgressChanged +=
                    (IDownloadProgress progress) =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                {
                                    System.Diagnostics.Debug.WriteLine(progress.BytesDownloaded);
                                    statusNotifier.UpdateStatus(EGoogleStatusNotifierMessageType.Downloading, ((progress.BytesDownloaded * 100) / totalSize));
                                    break;
                                }
                            case DownloadStatus.Completed:
                                {

                                    statusNotifier.UpdateStatus(EGoogleStatusNotifierMessageType.Completed, 100);
                                    System.Diagnostics.Debug.WriteLine("Download complete.");
                                    break;
                                }
                            case DownloadStatus.Failed:
                                {

                                    statusNotifier.UpdateStatus(EGoogleStatusNotifierMessageType.Failed, 0);
                                    System.Diagnostics.Debug.WriteLine("Download failed.");
                                    break;
                                }
                        }
                    };
                request.Download(stream);
                ConvertMemoryStreamToFileStream(stream, savePath + @"\" + @filename);
                stream.Dispose();
            }
            else
            {
                string extension = "", converter = "";
                foreach(MimeTypeConvert obj in MimeConverter.mimeList())
                {
                    if (mimeType == obj.MimeType)
                    {
                        extension = obj.Extension;
                        converter = obj.ConverterType;
                    }
                }
                System.Diagnostics.Debug.WriteLine("{0} {1} {2}", fileId, extension, mimeType);
                var request = _driveService.Files.Export(fileId, converter);
                var stream = new System.IO.MemoryStream();
                // Add a handler which will be notified on progress changes.
                // It will notify on each chunk download and when the
                // download is completed or failed.
                request.MediaDownloader.ProgressChanged +=
                        (IDownloadProgress progress) =>
                        {
                            switch (progress.Status)
                            {
                                case DownloadStatus.Downloading:
                                    {
                                        statusNotifier.UpdateStatus(EGoogleStatusNotifierMessageType.Downloading, ((progress.BytesDownloaded * 100) / totalSize));
                                        Console.WriteLine(progress.BytesDownloaded);
                                        break;
                                    }
                                case DownloadStatus.Completed:
                                    {
                                        statusNotifier.UpdateStatus(EGoogleStatusNotifierMessageType.Completed, 100);
                                        break;
                                    }
                                case DownloadStatus.Failed:
                                    {
                                        statusNotifier.UpdateStatus(EGoogleStatusNotifierMessageType.Failed, ((progress.BytesDownloaded * 100) / totalSize));
                                        break;
                                    }
                            }
                        };
                request.Download(stream);
                ConvertMemoryStreamToFileStream(stream, savePath + @"\" + @filename + extension);
                stream.Dispose();
            }
        }

        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            System.Diagnostics.Debug.WriteLine(mimeType);
            return mimeType;
        }

    }
}
