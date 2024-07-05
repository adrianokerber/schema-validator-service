# schema-validator-service
A sample of JSON schema validation as a service. This service could be used as a barrier between systems where a message could be validated based on its requirements and specific types.
On this initial sample we didn't extend the ValidationAttribute's behaviour for new validations but it would make sense in this "validation as a service" solution.

## Roadmap

Next steps

- Revamp SchemaFactory. We should register our schemas so we could search for the right one instead of hardcoded options