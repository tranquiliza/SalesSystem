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

        public ImageRepository(IApplicationConfigurationProvider configurationProvider)
        {
            _imageStoragePath = configurationProvider.ImageStoragePath;
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
            var directoryPath = Path.Combine(_imageStoragePath);
            var image = Path.Combine(directoryPath, imagePath);

            return await File.ReadAllBytesAsync(image).ConfigureAwait(false);
        }
    }
}
