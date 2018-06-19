using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
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
        void GoogleDriveUpdateStatus(EGoogleStatusNotifierMessageType messageType, long percent);
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

        public static List<GoogleDriveFile> ListDriveFiles(string fileName = null, string fileType = null, bool includeTrashed = false, bool includeTeamDriveItems = false)
        {
            List<GoogleDriveFile> filesList = new List<GoogleDriveFile>();

            if(fileName == null && fileType == null)
            {
                FilesResource.ListRequest listRequest = _driveService.Files.List();
                listRequest.PageSize = 1000;
                listRequest.Fields = "nextPageToken, files(mimeType, id, name, parents, size, modifiedTime, md5Checksum, webViewLink, trashed)";
                listRequest.IncludeTeamDriveItems = includeTeamDriveItems;
                // List files.
                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
                filesList.Clear();
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        bool isTrashed = file.Trashed ?? false;
                        bool includeInList = (!isTrashed) || includeTrashed;

                        if (includeInList)
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
                    request.Fields = "nextPageToken, files(mimeType, id, name, parents, size, modifiedTime, md5Checksum, webViewLink, trashed)";
                    request.PageToken = pageToken;
                    var result = request.Execute();
                    foreach (var file in result.Files)
                    {
                        bool isTrashed = file.Trashed ?? false;
                        bool includeInList = (!isTrashed) || includeTrashed;

                        if (includeInList)
                        {
                            filesList.Add(new GoogleDriveFile(
                                file.Name,
                                SizeFix(file.Size.ToString(), file.MimeType),
                                file.ModifiedTime.ToString(),
                                file.MimeType,
                                file.Id, file.Md5Checksum,
                                file.WebViewLink));

                        }
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

        private static bool UploadFileToDrive(IGoogleStatusNotifier statusNotifier,string folderId, string fileName, string filePath, string fileType)
        {
            return (UploadFileToDrive(folderId, fileName, filePath, fileType, statusNotifier) !=null);
        }

        private static GoogleDriveFile UploadFileToDrive(string folderId, string fileName, string filePath, string fileType, IGoogleStatusNotifier statusNotifier)
        {
            statusNotifier.GoogleDriveUpdateStatus(EGoogleStatusNotifierMessageType.Uploading, 0);
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
                                statusNotifier.GoogleDriveUpdateStatus(EGoogleStatusNotifierMessageType.Uploading, ((progress.BytesSent * 100) / totalSize));
                                System.Diagnostics.Debug.WriteLine(progress.BytesSent);
                                break;
                            }
                        case UploadStatus.Completed:
                            {

                                statusNotifier.GoogleDriveUpdateStatus(EGoogleStatusNotifierMessageType.Completed, 100);
                                System.Diagnostics.Debug.WriteLine("Upload complete.");
                                break;
                            }
                        case UploadStatus.Failed:
                            {
                                statusNotifier.GoogleDriveUpdateStatus(EGoogleStatusNotifierMessageType.Failed, 0);
                                System.Diagnostics.Debug.WriteLine("Upload failed.");
                                break;
                            }
                    }
                };
                
                request.Fields = "mimeType, id, name, parents, size, modifiedTime, md5Checksum, webViewLink";
                request.Upload();
            }
            var file = request.ResponseBody;
            GoogleDriveFile googleDriveFile = null;
            if (file != null)
                googleDriveFile = new GoogleDriveFile(file.Name, SizeFix(file.Size.ToString(), file.MimeType), file.ModifiedTime.ToString(), file.MimeType,file.Id,file.Md5Checksum,file.WebViewLink);

            return googleDriveFile;
        }
        private static bool UploadFileToDrive(IGoogleStatusNotifier statusNotifier, string folderId, string fileName, string filePath, string fileType, bool onlyNew)
        {
            return (UploadFileToDrive(folderId, fileName, filePath, fileType, onlyNew, statusNotifier) != null);
        }
        private static GoogleDriveFile UploadFileToDrive(string folderId, string fileName, string filePath, string fileType, bool onlyNew, IGoogleStatusNotifier statusNotifier)
        {
            if (onlyNew)
            {
                if (!CompareHash(Gtools.HashGenerator(filePath)))
                {
                    return UploadFileToDrive(folderId, fileName, filePath, fileType, statusNotifier);
                }
                else return null;

            }
            else
            {
                return UploadFileToDrive(folderId, fileName, filePath, fileType, statusNotifier);
            }
                
        }

        public static bool CompareHash(string hashToCompare)
        {
            return CompareHash(hashToCompare, out GoogleDriveFile googleDriveFile);
        }

        public static bool CompareHash(string hashToCompare, out GoogleDriveFile googleDriveFile)
        {
            googleDriveFile = null;
            foreach (GoogleDriveFile file in GoogleDriveAPIV3.ListDriveFiles())
            {
                if (file.Hash == hashToCompare)
                {
                    googleDriveFile = file;
                    return true;
                }
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
        public static void UploadToDrive(IGoogleStatusNotifier statusNotifier,string path, string name, string parentId, bool onlyNew)
        {
            UploadToDrive(path, name, parentId, onlyNew, statusNotifier);
        }
        public static GoogleDriveFile UploadToDrive(string path, string name, string parentId, bool onlyNew, IGoogleStatusNotifier statusNotifier)
        {
            if (Path.HasExtension(path))
            {
                return UploadFileToDrive(
                    parentId,
                    name,
                    path,
                    GetMimeType(Path.GetFileName(path)),
                    onlyNew, statusNotifier);
            }
            else
            {
                DirectoryUpload(path, parentId, onlyNew, statusNotifier);
                return null;
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
            statusNotifier.GoogleDriveUpdateStatus(EGoogleStatusNotifierMessageType.Downloading, 0);
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
                                    statusNotifier.GoogleDriveUpdateStatus(EGoogleStatusNotifierMessageType.Downloading, ((progress.BytesDownloaded * 100) / totalSize));
                                    break;
                                }
                            case DownloadStatus.Completed:
                                {

                                    statusNotifier.GoogleDriveUpdateStatus(EGoogleStatusNotifierMessageType.Completed, 100);
                                    System.Diagnostics.Debug.WriteLine("Download complete.");
                                    break;
                                }
                            case DownloadStatus.Failed:
                                {

                                    statusNotifier.GoogleDriveUpdateStatus(EGoogleStatusNotifierMessageType.Failed, 0);
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
                                        statusNotifier.GoogleDriveUpdateStatus(EGoogleStatusNotifierMessageType.Downloading, ((progress.BytesDownloaded * 100) / totalSize));
                                        Console.WriteLine(progress.BytesDownloaded);
                                        break;
                                    }
                                case DownloadStatus.Completed:
                                    {
                                        statusNotifier.GoogleDriveUpdateStatus(EGoogleStatusNotifierMessageType.Completed, 100);
                                        break;
                                    }
                                case DownloadStatus.Failed:
                                    {
                                        statusNotifier.GoogleDriveUpdateStatus(EGoogleStatusNotifierMessageType.Failed, ((progress.BytesDownloaded * 100) / totalSize));
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

        /// <summary>
        /// Insert a new permission.
        /// </summary>
        /// <param name="service">Drive API service instance.</param>
        /// <param name="fileId">ID of the file to insert permission for.</param>
        /// <param name="who">
        /// User or group e-mail address, domain name or null for "default" type.
        /// </param>
        /// <param name="type">The value "user", "group", "domain" or "default".</param>
        /// <param name="role">The value "owner", "writer" or "reader".</param>
        /// <returns>The inserted permission, null is returned if an API error occurred</returns>
        public static Permission InsertPermission(DriveService service, String fileId, String who, String type, String role)
        {
            Permission newPermission = new Permission();

            newPermission.Type = type;
            newPermission.Role = role;
            newPermission.EmailAddress = who;

            try
            {
                return service.Permissions.Create(newPermission, fileId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return null;
        }

        public static Google.Apis.Drive.v3.Data.User GetUserData()
        {
            /*
            AboutResource.GetRequest ag = new AboutResource.GetRequest(_driveService);
            ag.Fields = "user,storageQuota";
            var response = ag.Execute();
            if (response.StorageQuota.Usage.HasValue)
            {
                var xx = response.StorageQuota.Usage.Value;
            }
            */

            var request = _driveService.About.Get();
            request.Fields = "user";
            var response = request.Execute();

            return response.User;
        }

        public static long GetDriveSpaceUsage()
        {
            try
            {
                AboutResource.GetRequest ag = new AboutResource.GetRequest(_driveService);
                ag.Fields = "storageQuota";
                var response = ag.Execute();
                if (response.StorageQuota.Usage.HasValue)
                    return response.StorageQuota.Usage.Value;
                else
                    return -1;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return -1;
            }
        }


        public static long GetDriveSpaceLimit()
        {
            try
            {
                AboutResource.GetRequest ag = new AboutResource.GetRequest(_driveService);
                ag.Fields = "storageQuota";
                var response = ag.Execute();
                if (response.StorageQuota.Limit.HasValue)
                    return response.StorageQuota.Limit.Value;
                else
                    return -1;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return -1;
            }
        }
    }
}
