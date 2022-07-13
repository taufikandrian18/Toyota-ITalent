using System;
using System.IO;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace TAM.Talent.Commons.Core.Interfaces
{
    /// <summary>
    /// Interface for providing blueprint service class definitions for storing blob file into storage service such as MinIO and Azure Storage Container.
    /// </summary>
    public interface IFileStorageService
    {
        // TODO: Add methods' comments.
        Guid InsertBlob(string name, string mime);

        Task UploadFile(string fileName, string fileExtension, Stream data);

        Task<FileModel> GetFileDetailAsync(string blobId);

        Task<string> GetFileAsync(string fileName, string fileExtension);

        Task<string> GetDownloadFileAsync(string fileName, string fileExtension);

        Task DeleteFileAsync(Guid fileName, string fileExtension);

        Task<(byte[], string, string)> DownloadFileAsync(string blobId);

        Task<Guid> UploadFileFromBase64(FileContent fileContent);
        Task<Guid> UploadCertificateFile(Stream data, FileContent fileContent);

        Task<long> GetFileSize(string fileName, string fileExtension);
    }
}
