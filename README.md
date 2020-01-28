# ERM
ERM : Entity Relationship Manager

Graph-based data model to manage entities (nodes) and their relationships (edges) with the following features:
* Support for different storage providers (e.g. elasticsearch)
* RESTful & GraphQL APIs
* Strongly typed field definitions and localizable property values.

## Setup

1. Run elasticsearch locally (using http://localhost:9200) or change Vouzamo.ERM.Api > Startup.js for alternative options / providers.
2. Build & run the Vouzamo.ERM.Api project.
3. Use Graph*i*QL (/graphiql) to interact with GraphQL.

## Getting Started

Lets use an example to explain how to produce a data model:

![alt text](docs/graph-concept.png)

### Creating the NodeType(s)

Using Graph*i*QL, run the following mutation:

```javascript
mutation CreateNodeTypes {
  types {
    human: create(name: "Human", scope: NODES) {
      id
    }
    dog: create(name: "Dog", scope: NODES) {
      id
    }
    cat: create(name: "Cat", scope: NODES) {
      id
    }
  }
}
```

This will return the id for each created type which you can use in the following mutations to add fields:

```javascript
mutation AddHumanFields($givenName: FieldInput!, $middleNames: FieldInput!, $familyName: FieldInput!, $description: FieldInput!) {
  types {
    givenName: addField(id: "{id}", field: $givenName)
    middleNames: addField(id: "{id}", field: $middleNames)
    familyName: addField(id: "{id}", field: $familyName)
    description: addField(id: "{id}", field: $description)
  }
}

//variables
{
  "givenName": {
    "type": "string",
    "key": "givenName",
    "name": "Given Name",
    "mandatory": true,
    "enumerable": false,
    "localizable": false
  },
  "middleNames": {
    "type": "string",
    "key": "middleNames",
    "name": "Middle Names",
    "mandatory": false,
    "enumerable": true,
    "localizable": false
  },
  "familyName": {
    "type": "string",
    "key": "familyName",
    "name": "Family Name",
    "mandatory": true,
    "enumerable": false,
    "localizable": false
  },
  "description": {
    "type": "string",
    "key": "description",
    "name": "Description",
    "mandatory": false,
    "enumerable": false,
    "localizable": true
  }
}
```

Note: Remember to replace `{id}` with the value returned by the previous mutation for the `Human` type.
