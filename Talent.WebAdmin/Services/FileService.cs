using Minio;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;
using System.Drawing;
using System.Drawing.Imaging;
using TAM.Talent.Commons.Core.Interfaces;
using TAM.Talent.Commons.Core.Models;
using Amazon.S3;
using Amazon.S3.Model;
using System.Net;
using Amazon;

namespace Talent.WebAdmin.Services
{
    /// <summary>
    /// File storage service class that using MinIO as the 3rd party storage service application.
    /// </summary>
    public class FileService : IFileStorageService
    {
        private MinioClient minioClient;
        private readonly TalentContext DB;

        //TODO: Change to AppSettigs

        private readonly string amazonAccessKeyVideo = "AKIA5QL72E6XCC7IYQTE";
        private readonly string amazonSecretKeyVideo = "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";
        private readonly string amazonAccessKey = "AKIA5QL72E6XABBNVKWP";
        private readonly string amazonSecretKey = "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";




        private readonly string bucketNameVideo = "tam-italent-vod-source71e471f1-1qaysmea2rzex";
        private readonly string bucketNameVideo2 = "tam-italent-vod-destination920a3c57-15e1rvg3y9yko";
        private readonly string bucketName = "tam-italent-web-production";

        private readonly string cloudfrontURL = "https://d3h6uowkn5vgr.cloudfront.net/";

        // private readonly Zipper

        public FileService(MinioClient mc, TalentContext talentContext)
        {
            minioClient = mc;
            this.DB = talentContext;
        }

        public Guid InsertBlob(string name, string mime)
        {
            var newGuid = Guid.NewGuid();

            var getName = name;

            if (name.Length > 64)
            {
                int lastIndex = name.LastIndexOf(".");
                if (lastIndex >= 0)
                {
                    getName = name.Substring(0, lastIndex);
                    getName = getName.Substring(0, 55);
                    getName = getName + '.' + mime;
                }
            }

            var createBlob = new Blobs
            {
                BlobId = newGuid,
                Name = getName,
                Mime = mime.ToLower(),
                CreatedAt = DateTime.Now,
            };

            this.DB.Blobs.Add(createBlob);
            //await this.DB.SaveChangesAsync();
            return newGuid;
        }

        public async Task UploadFile(string fileName, string fileExtension, Stream data)
        {
            //var bucketName = fileExtension.ToLower();
            //var location = "us-east-1";

            //// Make a bucket on the server, if not already present.
            //bool found = await this.minioClient.BucketExistsAsync(bucketName);
            //if (!found)
            //{
            //    await this.minioClient.MakeBucketAsync(bucketName, location);
            //}

            //await minioClient.PutObjectAsync(bucketName, $"{fileName}" + "." + $"{bucketName}", data, data.Length);


            //AWS S3
            var request = new PutObjectRequest();
            IAmazonS3 client;

            if (fileExtension.ToLower() == "mp4" || fileExtension.ToLower() == "wmv" || fileExtension.ToLower() == "vob" || fileExtension.ToLower() == "mkv" || fileExtension.ToLower() == "m4v" || fileExtension.ToLower() == "ifo")
            {
                request = new PutObjectRequest
                {
                    BucketName = bucketNameVideo,
                    Key = "assets01/" + fileName + "." + fileExtension.ToLower(),
                    InputStream = data

                };
                client = new AmazonS3Client(amazonAccessKeyVideo, amazonSecretKeyVideo, RegionEndpoint.APSoutheast1);
            }
            else
            {
                request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileExtension.ToLower() + "/" + fileName.ToLower() + "." + fileExtension.ToLower(),
                    InputStream = data

                };
                client = new AmazonS3Client(amazonAccessKey, amazonSecretKey, RegionEndpoint.APSoutheast1);
            }
            await client.PutObjectAsync(request);
        }

        public async Task<TAM.Talent.Commons.Core.Models.FileModel> GetFileDetailAsync(string blobId)
        {
            if (string.IsNullOrEmpty(blobId))
            {
                var empty = new FileModel
                {
                    Name = "",
                    Mime = "",
                    FileUrl = ""
                };
                return empty;
            }

            var parseId = Guid.Parse(blobId);

            var getData = DB.Blobs
            .Where(Q => Q.BlobId == parseId)
            .Select(Q => new FileModel
            {
                Name = Q.Name,
                Mime = Q.Mime,
                FileUrl = ""
            })
            .FirstOrDefault();

            getData.FileUrl = await GetFileAsync(blobId, getData.Mime);

            return getData;
        }

        public async Task<string> GetFileAsync(string fileName, string fileExtension)
        {
            //if (string.IsNullOrEmpty(fileName) == true)
            //{
            //    return null;
            //}

            //var bucketName = fileExtension.ToLower();
            //var newName = fileName + "." + bucketName;
            //var expiry = (int)TimeSpan.FromDays(7).TotalSeconds;


            //try
            //{
            //    // Check whether the object exists using statObject().
            //    // If the object is not found, statObject() throws an exception,
            //    // else it means that the object exists.
            //    // Execution is successful.
            //    await this.minioClient.StatObjectAsync(bucketName, newName);
            //    return await this.minioClient.PresignedGetObjectAsync(bucketName, newName, expiry);
            //}

            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //    return null;
            //}

            //AWS S3
            try
            {
                if (!String.IsNullOrEmpty(fileExtension))
                {
                    if (fileExtension.ToLower() == "mp4" || fileExtension.ToLower() == "wmv" || fileExtension.ToLower() == "vob" || fileExtension.ToLower() == "mkv" || fileExtension.ToLower() == "m4v" || fileExtension.ToLower() == "ifo")
                    {

                        string filekey = "italent-media/AppleHLS1/" + fileName.ToLower() + "." + "m3u8";

                        string url = string.Empty;

                        url = cloudfrontURL + filekey;
                        return url;
                    }
                    else
                    {
                        string filekey = fileExtension.ToLower() + "/" + fileName.ToLower() + "." + fileExtension.ToLower();
                        string url = string.Empty;
                        if (!string.IsNullOrEmpty(filekey) && !string.IsNullOrWhiteSpace(filekey))
                            using (IAmazonS3 client = new AmazonS3Client(amazonAccessKey, amazonSecretKey, RegionEndpoint.APSoutheast1))
                            {
                                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
                                request.BucketName = bucketName;
                                request.Key = filekey;
                                request.Verb = HttpVerb.GET;
                                //MAX EXPIRED TIME 6 JAM DARI AWS NYA
                                request.Expires = DateTime.UtcNow.AddMinutes(600);
                                url = client.GetPreSignedURL(request);

                            }

                        return url;
                    }
                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }

        public async Task<string> GetDownloadFileAsync(string fileName, string fileExtension)
        {
            //AWS S3
            try
            {
                if (!String.IsNullOrEmpty(fileExtension))
                {
                    if (fileExtension.ToLower() == "mp4" || fileExtension.ToLower() == "wmv" || fileExtension.ToLower() == "vob" || fileExtension.ToLower() == "mkv" || fileExtension.ToLower() == "m4v" || fileExtension.ToLower() == "ifo")
                    {

                        string filekey = "assets01/" + fileName.ToLower() + "." + "mp4";

                        string url = string.Empty;

                        if (!string.IsNullOrEmpty(filekey) && !string.IsNullOrWhiteSpace(filekey))
                            using (IAmazonS3 client = new AmazonS3Client(amazonAccessKeyVideo, amazonSecretKeyVideo, RegionEndpoint.APSoutheast1))
                            {
                                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
                                request.BucketName = bucketNameVideo;
                                request.Key = filekey;
                                request.Verb = HttpVerb.GET;
                                //MAX EXPIRED TIME 6 JAM DARI AWS NYA
                                request.Expires = DateTime.UtcNow.AddMinutes(600);
                                url = client.GetPreSignedURL(request);

                            }

                        return url;
                    }
                    else
                    {
                        string filekey = fileExtension.ToLower() + "/" + fileName.ToLower() + "." + fileExtension.ToLower();
                        string url = string.Empty;
                        if (!string.IsNullOrEmpty(filekey) && !string.IsNullOrWhiteSpace(filekey))
                            using (IAmazonS3 client = new AmazonS3Client(amazonAccessKey, amazonSecretKey, RegionEndpoint.APSoutheast1))
                            {
                                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
                                request.BucketName = bucketName;
                                request.Key = filekey;
                                request.Verb = HttpVerb.GET;
                                //MAX EXPIRED TIME 6 JAM DARI AWS NYA
                                request.Expires = DateTime.UtcNow.AddMinutes(600);
                                url = client.GetPreSignedURL(request);

                            }

                        return url;
                    }
                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }

        public async Task<long> GetFileSize(string fileName, string fileExtension)
        {

            string filekey = fileExtension.ToLower() + "/" + fileName.ToLower() + "." + fileExtension.ToLower();
            long fileSize = 0;
            try
            {
                using (IAmazonS3 client = new AmazonS3Client(amazonAccessKey, amazonSecretKey, RegionEndpoint.APSoutheast1))
                {
                    if (fileExtension.ToLower() == "mp4" || fileExtension.ToLower() == "wmv" || fileExtension.ToLower() == "vob" || fileExtension.ToLower() == "mkv" || fileExtension.ToLower() == "m4v" || fileExtension.ToLower() == "ifo")
                    {
                        filekey = "/assets01/" + fileName + "." + fileExtension.ToLower();

                        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
                        request.BucketName = bucketNameVideo2;
                        request.Key = filekey;
                        request.Verb = HttpVerb.GET;
                        //MAX EXPIRED TIME 6 JAM DARI AWS NYA
                        request.Expires = DateTime.UtcNow.AddMinutes(600);

                        var meta = await client.GetObjectMetadataAsync(bucketName, filekey);

                        fileSize = meta == null ? 0 : meta.Headers.ContentLength;
                    }
                    else
                    {
                        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
                        request.BucketName = bucketName;
                        request.Key = filekey;
                        request.Verb = HttpVerb.GET;
                        //MAX EXPIRED TIME 6 JAM DARI AWS NYA
                        request.Expires = DateTime.UtcNow.AddMinutes(600);

                        var meta = await client.GetObjectMetadataAsync(bucketName, filekey);

                        fileSize = meta == null ? 0 : meta.Headers.ContentLength;
                    }
                }
            }
            catch (Exception x)
            {
                fileSize = 0;
            }

            return fileSize;
        }


        public async Task<(byte[], string, string)> DownloadFileAsync(string blobId)
        {

            var getDetail = await GetFileDetailAsync(blobId);

            var blobBytes = new byte[0];

            //await this.minioClient.GetObjectAsync(getDetail.Mime, blobId + "." + getDetail.Mime, (stream) =>
            //   {
            //       using (var ms = new MemoryStream())
            //       {
            //           stream.CopyTo(ms);
            //           blobBytes = ms.ToArray();
            //       }
            //   });


            //AWS S3
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = getDetail.Mime.ToLower() + "/" + blobId + "." + getDetail.Mime.ToLower(),
            };

            IAmazonS3 client = new AmazonS3Client(amazonAccessKey, amazonSecretKey, RegionEndpoint.APSoutheast1);
            GetObjectResponse response = await client.GetObjectAsync(request);
            MemoryStream memoryStream = new MemoryStream();
            using (Stream responseStream = response.ResponseStream)
            {
                responseStream.CopyTo(memoryStream);
                blobBytes = memoryStream.ToArray();
            }


            //var dataZip = await ZipService.Compress(filesForZip);
            //var contentType = "application/zip";

            return (blobBytes, getDetail.Name, getDetail.Mime);
        }

        public async Task DeleteFileAsync(Guid fileName, string fileExtension)
        {
            //var bucketName = fileExtension;
            //var newName = fileName + "." + bucketName;
            //await this.minioClient.RemoveObjectAsync(bucketName, newName);

            //S3
            IAmazonS3 client = new AmazonS3Client(amazonAccessKey, amazonSecretKey, RegionEndpoint.APSoutheast1);
            await client.DeleteObjectAsync(bucketName, fileExtension.ToLower() + "/" + fileName + "." + fileExtension.ToLower());
        }

        public async Task<Guid> UploadFileFromBase64(TAM.Talent.Commons.Core.Models.FileContent fileContent)
        {
            Byte[] bytes = Convert.FromBase64String(fileContent.Base64);
            Stream data = new MemoryStream(bytes);

            var blobId = InsertBlob(fileContent.FileName, fileContent.ContentType);

            var checkFileType = GetFileExtension(fileContent.Base64);
            if (checkFileType.ToLower() == "png" || checkFileType.ToLower() == "jpg")
            {
                using (MemoryStream mem = new MemoryStream(bytes))
                {
                    using (Bitmap bmp1 = (Bitmap)Image.FromStream(mem))
                    {

                        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
                        ImageCodecInfo typeEncoder = codecs[1];
                        if (checkFileType.ToLower() == "png") typeEncoder = codecs[6];

                        Encoder myEncoder = Encoder.Quality;

                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, 40L);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bmp1.Save(ms, typeEncoder, myEncoderParameters);
                            Image imgImage = Image.FromStream(ms);

                            Byte[] outputBytes;
                            outputBytes = ms.ToArray();
                            Stream resultData = new MemoryStream(outputBytes);

                            // TODO: argument fileExtension must be passed with file extension.
                            await UploadFile(blobId.ToString(), fileContent.ContentType, resultData);
                            return blobId;
                        }
                    }
                }
            }

            await UploadFile(blobId.ToString(), fileContent.ContentType, data);
            return blobId;
        }

        public static string GetFileExtension(string base64String)
        {
            var data = "";
            if (!string.IsNullOrEmpty(base64String))
            {
                data = base64String.Substring(0, 5);
            }

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }


        }

        public async Task<Guid> UploadCertificateFile(Stream data, FileContent fileContent)
        {
            var blobId = InsertBlob(fileContent.FileName, fileContent.ContentType);
            await UploadFile(blobId.ToString(), fileContent.ContentType, data);
            return blobId;
        }

        /// <summary>
        ///     Get a file from S3
        /// </summary>
        /// <param name="bucketName">Bucket where the file is stored</param>
        /// <param name="keyName">Key name of the bucket (File Name)</param>
        /// <returns></returns>
        //public string GetObjectFromS3Async(string bucketName, string keyName)
        //{
        //    string filekey = "jpeg/1.jpeg";
        //    string amazonAccessKey = "AKIAZIJHHLNMOGMDUSOZ";
        //    string amazonSecretKey = "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";

        //    string url = string.Empty;
        //    if (!string.IsNullOrEmpty(filekey) && !string.IsNullOrWhiteSpace(filekey))
        //        using (IAmazonS3 client = new AmazonS3Client(amazonAccessKey, amazonSecretKey, RegionEndpoint.APSoutheast1))
        //        {
        //            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
        //            request.BucketName = "s3-italent-001";
        //            request.Key = filekey;
        //            request.Verb = HttpVerb.GET;
        //            request.Expires = DateTime.UtcNow.AddMinutes(10);
        //            url = client.GetPreSignedURL(request);
        //        }
        //    return url;
        //}

    }
}