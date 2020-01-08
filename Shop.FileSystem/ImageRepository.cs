using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core;
using Tranquiliza.Shop.Core.Application;

namespace Tranquiliza.Shop.FileSystem
{
    public class ImageRepository : IImageRepository
    {
        private readonly string _imageStoragePath;
        private readonly IApplicationLogger _log;

        public ImageRepository(IApplicationConfigurationProvider configurationProvider, IApplicationLogger log)
        {
            _imageStoragePath = configurationProvider.ImageStoragePath;
            _log = log;
        }

        public async Task Save(byte[] fileData, string fileType, Guid imageId)
        {
            var directoryPath = Path.Combine(_imageStoragePath);
            var imagePath = Path.Combine(directoryPath, imageId.ToString() + fileType);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using var fileStream = new FileStream(imagePath, FileMode.Create);
            await fileStream.WriteAsync(fileData).ConfigureAwait(false);
        }

        public async Task<byte[]> GetImage(string imagePath)
        {
            var image = Path.Combine(_imageStoragePath, imagePath);

            return await File.ReadAllBytesAsync(image).ConfigureAwait(false);
        }

        public Task Delete(string imageName)
        {
            var image = Path.Combine(_imageStoragePath, imageName);
            try
            {
                File.Delete(image);
            }
            catch (Exception ex)
            {
                _log.Warning($"Unable to delete file {imageName} from {image}", ex);
            }

            return Task.CompletedTask;
        }
    }
}
