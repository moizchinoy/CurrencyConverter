# CurrencyConverter

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
2. Use configuartion for Url, cache expiration duration etc.
3. 400 and 500 are combined into 400

## Notes
1. Historical caching
2. Pagination
3. Polly
4. 90 days validation
5. Expiring historical data after 90 days
6. Expiring/Fetching latest rates at 16:00 CET
7. Currency validation
8. Readme file
9. Unit tests
10. Configuration
  a. Restricted currencies
  b. Remote api url
  c. Retries count
11. Pass CancellationToken
12. Use GetAwaiter false
13. Rename the methods to Async
14. 404 handling

Caching - Historical Data:
1. No caching
2. Exact matching
3. Subset matching
4. Partial matching