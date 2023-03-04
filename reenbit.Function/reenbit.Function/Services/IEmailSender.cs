using System.Threading.Tasks;

namespace reenbit.Function.Services;

public interface IEmailSender
{
    Task SendAsync(string from, string fileName);
}
