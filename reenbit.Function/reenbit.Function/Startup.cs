
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using reenbit.Function;
using reenbit.Function.Options;
using reenbit.Function.Services;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

[assembly:FunctionsStartup(typeof(Startup))]
namespace reenbit.Function;
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddOptions<EmailSendOptions>()
            .Configure<IConfiguration>((setting,conf) =>
            {
                conf.GetSection("EmailSendOptions").Bind(setting);
            });
       
        builder.Services.AddScoped<IEmailSender, EmailSender>();
    }
}