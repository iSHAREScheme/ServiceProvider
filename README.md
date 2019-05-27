# iSHARE Service Provider

**iSHARE** is a collaborative effort to improve conditions for data-sharing for organisations involved in the logistics sector. The functional scope of the iSHARE Scheme focuses on topics of identification, authentication and authorization.

## iSHARE Service Provider

The **Service Provider**:

- provides a service, such as data for consumption by a Service Consumer

The **Service Provider** is a role for which iSHARE adherence (iSHARE) is REQUIRED.

## Installation process for API

### Prerequisites

- Install [.NET Core 2.1.4 Runtime](https://www.microsoft.com/net/download/dotnet-core/2.1) (or SDK 2.1.402 for development).

### Clone or download the Service Provider repository:

- `git clone https://github.com/iSHAREScheme/ServiceProvider.git` (or download zip)

### Setup the development environment

1. Create environment variable 'ENVIRONMENT' with the value 'Development'
2. Navigate to iSHARE.ServiceProvider.Api.Warehouse13 and create a new file named 'appsettings.Development.json'
3. Copy the content of 'appsettings.Development.json.template' into 'appsettings.Development.json' and complete all fields with the necessary information and save the changes
4. Into appsettings.Development.json file: 
    1. Change DigitalSigner -> PrivateKey value to the valid RSA private key value with the following format: "-----BEGIN RSA PRIVATE KEY-----\n...\n-----END RSA PRIVATE KEY-----". For this, you can use OpenSSL:
        1. Extract the private key from the certificate: `openssl pkcs12 -in "certificate.p12" -out "certificate.key.pem" -nodes -nocerts -password pass:your_password_here`
        2. Decrypt private key: `openssl rsa -in certificate.key.pem -out certificate.key.decr.pem`
        3. Extract the content from `certificate.key.decr.pem` and replace the endline characters with "\n"
    2. Change DigitalSigner -> RawPublicKey value to the valid public key value with the following format: "-----BEGIN CERTIFICATE-----...-----END CERTIFICATE-----". For this you can use OpenSSL:
        1. Extract .pem: `openssl pkcs12 -in certificate.p12 -clcerts -nokeys -out certificate.pem -password pass:your_password_here`
        2. Extract the content from `certificate.pem` and remove the endline characters
    3. Save changes
5. Go to Resources\Development
    1. Open certificate_authorities.json
    2. Add the necessary certificate authorities in the following format: "-----BEGIN CERTIFICATE-----...-----END CERTIFICATE-----" (this value can be obtained from a .pem certificate by extracting the content and removing the line separators/endlines)
    3. Save

## Build API

Navigate to the local Service Provider repository and run `dotnet build`

## Setup the database

Service Provider is using a SQL database that is created at runtime.
Various test records are inserted from JSON files present here

- `iSHARE.ServiceProvider.Api.Warehouse13\Seed\IdentityServer\Development`

## Run process

1. Navigate to the local Service Provider repository, into iSHARE.ServiceProvider.Api.Warehouse13 folder and run `dotnet run`
2. Open a browser tab and navigate to `localhost:8600/swagger`


### Certificate validation

Certificate validation related service can be found [here](iSHARE.IdentityServer\CertificateValidationService.cs)

### [Differences between the implementation and the official documentation](Differences.md)

### Note
The current implementation of the Service Provider validates the party only on the basis of the EORI number with the SO. The recommended validation should be done based on the EORI number and the certificate subject name pair.  
## API References

1. https://ishareworks.atlassian.net/wiki/spaces/IS/pages/70222191/iSHARE+Scheme
2. https://dev.ishareworks.org/
