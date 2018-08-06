using System;
using System.Threading.Tasks;
using AutoTexter.Models;
using AutoTexter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.AspNet.Core;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.Types;

namespace AutoTexter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeechController : TwilioController
    {
        private readonly ITextToSpeechService _textToSpeechService;
        private const string SiteUrl = "SITE_URL";

        public SpeechController(IOptions<TwilioAccount> account, ITextToSpeechService textToSpeechService)
        {
            _textToSpeechService = textToSpeechService ?? throw new ArgumentNullException(nameof(textToSpeechService));
            var acc = account.Value ?? throw new ArgumentNullException(nameof(account));

            TwilioClient.Init(
                acc.AccountSid,
                acc.AuthToken
            );
        }

        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            return Content("Hello");
        }
       
    }
}