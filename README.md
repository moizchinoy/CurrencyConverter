# CurrencyConverter

## Clone Repository
```
git clone https://github.com/moizchinoy/CurrencyConverter.git
```

## Build
```
dotnet build
```

## Run
```
dotnet run --project .\src\Api\
```

## Endpoints

### Latest Rates
```
GET /Currency/latest?currency=usd HTTP/1.1
Host: localhost:5157
```

### Convert Amount
```
GET /Currency/convert?currency=usd&amount=128 HTTP/1.1
Host: localhost:5157
```

### Historical Rates
```
GET /Currency/history?currency=usd&fromDate=2024-4-10&toDate=2024-5-17&page=1&size=50 HTTP/1.1
Host: localhost:5157
```

## Improvements
Following improvements can be done:
1. More unit tests
2. Use configuration for Url, cache expiration duration etc.
3. 404 and 400 and 500 are combined into 400
4. Expiring latest rates cache at 16:00 CET
5. Currency validation
6. Cache expiry configurable

## Caching - Latest Rates:
Latest rates are cached for 5 mins now. We can improve it to use the fact that rates are refreshed every day at 16:00 CET

## Caching - Historical Rates:
Historical rates are cached in 5 days slots. For e.g.
- If user requested for 01-02-2024, data is fetched for 01-01-2024 - 05-01-2024
- If user requested for 02-02-2024, data is fetched for 01-01-2024 - 05-01-2024
- If user requested for 01-02-2024 to 07-02-2024, data is fetched for 01-01-2024 - 05-01-2024 and 06-02-2024 - 10-02-2024
