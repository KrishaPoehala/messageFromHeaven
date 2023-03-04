using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using reenbit.Function.Services;

namespace reenbit.Function;

[StorageAccount("AzureWebJobsStorage")]
public class Function1
{
    private readonly IEmailSender _sender;
    public Function1(IEmailSender sender)
    {
        _sender = sender;
    }

    [FunctionName("Function1")]
    public async Task Run([BlobTrigger("reenbit-container/{name}")]
    Stream myBlob,IDictionary<string,string> metadata, string name, ILogger log)
    {
        log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        var from = metadata["email"];
        if (from is null)
        {
            log.LogError($"No email found in the metagata of a file {name}");
            return;
        }

        try
        {
            await _sender.SendAsync(from, name);
            log.LogInformation($"The notification was sent to the {from}");
        }
        catch (Exception ex)
        {
            log.LogError("The message could not be sent\n");
            log.LogError(ex.Message);
        }
    }
}
