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

        public ImageRepository(IConfigurationProvider configurationProvider)
        {
            _imageStoragePath = configurationProvider.ImageStoragePath;
        }

        public async Task Save(Guid productId, byte[] fileData, string fileType, Guid imageId)
        {
            var directoryPath = Path.Combine(_imageStoragePath, productId.ToString());
            var imagePath = Path.Combine(directoryPath, imageId.ToString() + fileType);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using var fileStream = new FileStream(imagePath, FileMode.Create);
            await fileStream.WriteAsync(fileData).ConfigureAwait(false);
        }

        public async Task<byte[]> GetImage(string imagePath)
        {
            var parts = imagePath.Split('_');
            if (parts.Length != 2)
                return null;

            var directoryPath = Path.Combine(_imageStoragePath, parts[0]);
            var image = Path.Combine(directoryPath, parts[1]);

            return await File.ReadAllBytesAsync(image).ConfigureAwait(false);
        }
    }
}
