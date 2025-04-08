# schema-validator-service
A sample of JSON schema validation as a service. This service could be used as a barrier between systems where a message could be validated based on its requirements and specific types.
On this initial sample we didn't extend the ValidationAttribute's behaviour for new validations but it would make sense in this "validation as a service" solution.

## Run application

Just use `docker compose up -d` and `docker compose down` for start or shutdown the application.

Use the following cURL to execute the request for the application
```curl
curl --location 'http://localhost:5286/validate-json' \
--header 'accept: */*' \
--header 'Content-Type: application/json' \
--data '{
    "Json": "{ \"FirstName\": \"Name\", \"LastName\": \"\", \"Document\": \"12345678\", \"ExtraField\": \"nothing\" }",
    "Metadata": {
        "Entity": "Person"
    }
}'
```

## Roadmap

Next steps

- Revamp SchemaFactory. We should register our schemas so we could search for the right one instead of hardcoded options