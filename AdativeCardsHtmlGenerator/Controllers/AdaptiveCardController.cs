using AdaptiveCards;
using AdaptiveCards.Rendering;
using AdaptiveCards.Rendering.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdativeCardsHtmlGenerator.Controllers
{
    [Route("api/GetCardHtml")]
    [ApiController]
    public class AdaptiveCardController : ControllerBase
    {
        private ILogger<WeatherForecastController> _logger;

        public AdaptiveCardController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public string Get()
        {
           

            // string cardJson = "{\"type\": \"AdaptiveCard\",\"$data\": {\"author\": {\"name\": \"Ernest Hemingway\"}},\"body\": [{\"type\": \"TextBlock\",\"size\": \"Medium\",\"weight\": \"Bolder\",\"text\": \"Adaptive Card Sample\"},{\"type\": \"ColumnSet\",\"columns\": [{\"type\": \"Column\",\"items\": [{\"type\": \"TextBlock\",\"weight\": \"Bolder\",\"text\": \"Hello from, old syntax {author.name}, new syntax ${author.name}\",\"wrap\": true}],\"width\": \"stretch\"}]}],\"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\",\"version\": \"1.2\"}";
            string cardJson = "{\"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\",\"type\": \"AdaptiveCard\",\"version\": \"1.2\",\"body\": [{\"type\": \"Container\",\"items\": [{\"type\": \"TextBlock\",\"text\": \"${companyName}\",\"size\": \"Medium\",\"wrap\": true,\"style\": \"heading\"},{\"type\": \"TextBlock\",\"text\": \"${primaryExchange}: ${symbol}\",\"isSubtle\": true,\"spacing\": \"None\",\"wrap\": true},{\"type\": \"TextBlock\",\"text\": \"{{DATE(${formatEpoch(latestUpdate, 'yyyy-MM-ddTHH:mm:ssZ')}, SHORT)}} {{TIME(${formatEpoch(latestUpdate, 'yyyy-MM-ddTHH:mm:ssZ')})}}\",\"wrap\": true}]},{\"type\": \"Container\",\"spacing\": \"None\",\"items\": [{\"type\": \"ColumnSet\",\"columns\": [{\"type\": \"Column\",\"width\": \"stretch\",\"items\": [{\"type\": \"TextBlock\",\"text\": \"${formatNumber(latestPrice, 2)} \",\"size\": \"ExtraLarge\",\"wrap\": true},{\"type\": \"TextBlock\",\"text\": \"${if(change >= 0, '▲', '▼')} ${formatNumber(change,2)} USD (${formatNumber(changePercent * 100, 2)}%)\",\"color\": \"${if(change >= 0, 'good', 'attention')}\",\"spacing\": \"None\",\"wrap\": true}]},{\"type\": \"Column\",\"width\": \"auto\",\"items\": [{\"type\": \"FactSet\",\"facts\": [{\"title\": \"Open\",\"value\": \"${open} \"},{\"title\": \"High\",\"value\": \"${high} \"},{\"title\": \"Low\",\"value\": \"${low} \"}]}]}]}]}]}";

            //
            //var template = new AdaptiveCards.Templating.AdaptiveCardTemplate(cardJson);
            //var cardPayload = template.Expand(new { author = new { name = "Noel" } });
            //var card = AdaptiveCard.FromJson(cardPayload);
            //AdaptiveCardRenderer renderer = new AdaptiveCardRenderer();
            //RenderedAdaptiveCard renderedCard = renderer.RenderCard(card.Card);
            //HtmlTag html = renderedCard.Html;

            //    return html.ToString(); ;

            var values = "{\"symbol\": \"MSFT\",\"companyName\": \"Microsoft Corporation\",\"primaryExchange\": \"Nasdaq Global Select\",\"sector\": \"Technology\",\"calculationPrice\": \"close\",\"open\": 127.42,\"openTime\": 1556890200,\"close\": 128.9,\"closeTime\": 1556913600,\"high\": 129.43,\"low\": 127.25,\"latestPrice\": 128.9,\"latestSource\": \"Close\",\"latestTime\": \"May 3, 2019\",\"latestUpdate\": 1556913600,\"latestVolume\": 24835154,\"iexRealtimePrice\": null,\"iexRealtimeSize\": null,\"iexLastUpdated\": null,\"delayedPrice\": 128.9,\"delayedPriceTime\": 1556913600,\"extendedPrice\": 129.04,\"extendedChange\": 0.14,\"extendedChangePercent\": 0.00109,\"extendedPriceTime\": 1556917190,\"previousClose\": 126.21,\"change\": 2.69,\"changePercent\": 0.02131,\"iexMarketPercent\": null,\"iexVolume\": null,\"avgTotalVolume\": 22183270,\"iexBidPrice\": null,\"iexBidSize\": null,\"iexAskPrice\": null,\"iexAskSize\": null,\"marketCap\": 987737229888,\"peRatio\": 30.84,\"week52High\": 131.37,\"week52Low\": 93.96,\"ytdChange\": 0.30147812013916003}";

            var json1 = JsonConvert.DeserializeObject(values);
            JObject jsonObj = JObject.Parse(values);
            Dictionary<string, string> dictObj = jsonObj.ToObject<Dictionary<string, string>>();
            AdaptiveCardParseResult result = AdaptiveCard.FromJson(cardJson);
            

            
            IList<AdaptiveWarning> warnings = result.Warnings;
            Console.WriteLine("Count =>" + warnings.Count);
            // Get card from result
            AdaptiveCard card = result.Card;
            ReplaceDataInCard(card.Body, dictObj);
            AdaptiveCardRenderer renderer = new AdaptiveCardRenderer();
            RenderedAdaptiveCard renderedCard = renderer.RenderCard(card);
            HtmlTag html = renderedCard.Html;
            string restul1 = html.ToString();
            //foreach (var item in dictObj.Keys)
            //{
            //    restul1 = restul1.Replace("${" + item + "}", dictObj[item]);
            //    Console.WriteLine(item);
            //}

            return restul1;
        }

        static void ReplaceDataInCard(List<AdaptiveElement> elements, Dictionary<string, string> data)
        {
            foreach (var element in elements)
            {
                if (element is AdaptiveTextBlock textBlock)
                {
                    foreach (var item in data)
                    {
                        textBlock.Text = textBlock.Text.Replace("${" + item.Key + "}", item.Value);
                    }
                }
                else if (element is AdaptiveContainer container)
                {
                    ReplaceDataInCard(container.Items, data);
                }
                // Add other element types if needed
            }
        }
    }
}
