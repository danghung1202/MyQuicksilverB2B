﻿using EPiServer.Reference.Commerce.Site.B2B.Services;
using Mediachase.Commerce;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Markets;

namespace EPiServer.Reference.Commerce.Site.Features.Market.Services
{
    public class CurrentMarket : ICurrentMarket
    {
        private const string MarketCookie = "MarketId";
        private static readonly MarketId DefaultMarketId = new MarketId("US");
        private readonly IMarketService _marketService;
        private readonly CookieService _cookieService;

        public CurrentMarket(IMarketService marketService, CookieService cookieService)
        {
            _marketService = marketService;
            _cookieService = cookieService;
        }

        public IMarket GetCurrentMarket()
        {
            var _currentMarket = _cookieService.Get(MarketCookie);
            if (string.IsNullOrEmpty(_currentMarket))
            {
                _currentMarket = DefaultMarketId.Value;
            }
            return GetMarket(new MarketId(_currentMarket));
        }

        public void SetCurrentMarket(MarketId marketId)
        {
            var market = GetMarket(marketId);
            SiteContext.Current.Currency = market.DefaultCurrency;
            _cookieService.Set(MarketCookie, marketId.Value);
        }

        private IMarket GetMarket(MarketId marketId)
        {
            return _marketService.GetMarket(marketId) ?? _marketService.GetMarket(DefaultMarketId);
        }
    }
}