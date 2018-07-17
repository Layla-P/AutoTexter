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

        [HttpPost]
        [Route("voice")]
        public async Task<IActionResult> Voice([FromForm]TwilioResponse twilioResponse)
        {
            await CallResource.CreateAsync(to: new PhoneNumber("SITE_URL"), from: "SITE_URL",
                url: new Uri($"{SiteUrl}/api/speech/call/{twilioResponse.MessageSid}"), method: "GET");
            return Content("");
        }

        [HttpGet]
        [Route("call/{messageSid}")]
        public async Task<TwiMLResult> Call([FromRoute]string messageSid)
        {
            var message = await MessageResource.FetchAsync(pathSid: messageSid);
            var response = await _textToSpeechService
                .GetSpeech(message.Body, message.From.ToString());
            var twiml = new VoiceResponse();
            twiml.Play(new Uri(response.Path));
            return TwiML(twiml);
        }
    }
}