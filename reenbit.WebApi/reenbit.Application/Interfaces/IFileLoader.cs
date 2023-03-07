using Microsoft.AspNetCore.Http;

namespace reenbit.Application.Interfaces;

public interface IFileLoader
{
    Task LoadFile(string email, IFormFile file,CancellationToken cancellationToken = default);
}
