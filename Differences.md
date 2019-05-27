# Differences between the implementation and the official documentation

## Introduction

The Service Provoder as provided is not intended to be a 'production-ready' system. The Service Provider is mainly for demonstrating that the iSHARE Authorization protocol functions properly and for pilots or proof of concepts, and by providing this open source code market parties can make a swift start in providing services themselves.

As a result of this intended use of this Service Provider, the source code is currently not aimed at complying with all different data specifications of iSHARE. Meaning, it does not pass all different 'unhappy flow' test cases that are encompassed by the iSHARE Conformance Test Tool. The iSHARE Maintenance organisation aims to correct these unexpected responses during 2019H1 and will publish a new version of the source code on this Github.

For the Service Provider API functions /token and /capabilities the following test cases are not responding as should be expected:

## Token endpoint:

#### ASSERTION INVALID IAT Client assertion JWT payload 'iat' field is after current time
status code was: 200, expected: 400

#### HTTP WRONG METHOD HTTP method is not POST
status code was: 400, expected: 405


## Capabilities endpoint

#### ACCESS TOKEN INVALID SIGNATURE Access token used is not signed correctly by the server exposing the capabilities endpoint (extract header+payload from access token and sign with different key)
status code was: 200, expected: 401

#### ACCESS TOKEN MISSING SIGNATURE Access token JWT signature missing
status code was: 200, expected: 401

#### AUTHORIZATION NOT BEARER Authorization' header is not 'Bearer' + 'value'
status code was: 200, expected: 401

#### HTTP WRONG METHOD HTTP method is not GET
status code was: 404, expected: 405
