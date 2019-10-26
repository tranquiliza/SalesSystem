using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IImageRepository
    {
        Task Save(Guid productId, byte[] fileData, string fileType, Guid imageId);
        Task<byte[]> GetImage(string imagePath);
    }
}
