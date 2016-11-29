[![Build Status](https://travis-ci.org/MewsSystems/eet.svg?branch=master)](https://travis-ci.org/MewsSystems/eet)

# EET
EET stands for Elektronická Evidence Tržeb, which is Czech version of Fiscal Printers.
It's an online API provided by the Ministry of Finance in a form of a SOAP Web Service.

## Key features
- No Czech abbreviations.
- Early data validation.
- Intuitive immutable DTOs.
- SOAP communication (including WS-Security signing).
- PKP and BKP security code computation.
- Support for parallel async requests.

## Known issues
- [8](https://github.com/MewsSystems/eet/issues/8): As the communication is done fully via HTTPS, we postponed the implementation of response signature verification. It's a potential security risk that will be addressed in upcoming releases.

## Usage
We tend to use immutable DTOs wherever possible, especially to ensure data validity.
We want the library to throw an error as soon as possible, i.e. when constructing corresponding data structures.
That is why we even introduce wrappers for simple datatypes.
Various usages are demonstrated in our test cases.

### Simplest usage example
```csharp
var certificate = new Certificate(
    password: "certificatePassword",
    data: certificateContentsByteArray
);

var record = new RevenueRecord(
    identification: new Identification(
        taxPayerIdentifier: new TaxIdentifier("CZ1234567890"),
        registryIdentifier: new RegistryIdentifier("01"),
        premisesIdentifier: new PremisesIdentifier(1),
        certificate: certificate
    ),
    revenue: new Revenue(
        gross: new CurrencyValue(1234.00m)
    ),
    billNumber: new BillNumber("2016-321")
);

var client = new EetClient(certificate);
var response = await client.SendRevenueAsync(record);
if (response.IsSuccess)
{
    var fiscalCode = response.Success.FiscalCode;
}
```

### Verification mode
```csharp
var response = await client.SendRevenueAsync(record, EetMode.Verification);
```

### Using playground
```csharp
var client = new EetClient(certificate, EetEnvironment.Playground);
```

# Tests
Test are not currently running on Travis CI, as their network infrastructure is blocking HTTPS traffic directed into Europe, where the EET servers are located.

Otherwise, a xUnit test suite is a part of the library, it contains end-to-end tests that communicates with Playground EET servers to verify the lib is really working against the current EET version.

# NuGet

We have published the library as [Mews.Eet](https://www.nuget.org/packages/Mews.Eet/).

Alternatively, the NuGet package can be generated by running the following command:

```
src\Mews.Eet> nuget pack .\Mews.Eet.csproj -IncludeReferencedProjects
```

# Authors
Development: [@jirihelmich](https://github.com/jirihelmich)

Code review: [@siroky](https://github.com/siroky), [@onashackem](https://github.com/onashackem)

# Participants:
- [@tomasdeml](https://github.com/tomasdeml): [PR#3](https://github.com/MewsSystems/eet/pull/3/files)

The time to implement this was kindly provided by [Mews Systems](http://mewssystems.com).

# Related projects
- [eet-client](https://github.com/todvora/eet-client) by [@todvora](https://github.com/todvora): Java, MIT
- [SwiftEET](https://github.com/charlieMonroe/SwiftEET) by [@charlieMonroe](https://github.com/charlieMonroe): Swift, GPL-3.0
- [http://hlidaceet.cz/](http://hlidaceet.cz/): A project that uses our library to monitor the EET endpoint.

# Credits
- [Komodosoft](http://www.komodosoft.net) For publishing [the post](http://www.komodosoft.net/post/2016/03/24/sign-a-soap-message-using-x-509-certificate.aspx) about signing a SOAP message without using WCF.
